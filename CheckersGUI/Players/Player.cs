using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CheckersEngine;
using CheckersEngine.Players;
using CheckersGUI.Components;

namespace CheckersGUI.Players
{
    public class Player : IPlayer
    {
        public short PiecesLeft
        {
            get
            {
                return _piecesLeft;
            }
            set
            {
                if (value == 0)
                {
                    _piecesLeft = 0;
                    NoPiecesLeft?.Invoke(this, null);
                }
                else
                {
                    _piecesLeft = value;
                }
            }
        }
        public double Score { get; set; }
        public CheckersColor Color { get; set; }

        public event EventHandler NoPiecesLeft;

        short _piecesLeft = 12;

        FieldGUI[][] GUIFields;

        List<(int index, Stack<Move> stack)> Moves;
        TaskCompletionSource<Stack<Move>> TaskCompletionSource;
        Piece SelectedPiece;
        Paths CurrentOffers;

        public Player(FieldGUI[][] guiFields)
        {
            GUIFields = guiFields;
        }

        public Stack<Move> MakeMove(Paths offers)
        {
            TaskCompletionSource = new TaskCompletionSource<Stack<Move>>();
            CurrentOffers = offers;
            Task.Run(async () =>
            {
                Task task = TaskCompletionSource.Task;
                foreach (var offer in offers.FoundPaths)
                {
                    Field field = offer.Key.Field;
                    GUIFields[field.Row][field.Column].HighlightThinkness = 3;
                    GUIFields[field.Row][field.Column].ButtonClicked += OnGUIFieldClick;
                }
                await task;
            }).Wait();
            return TaskCompletionSource.Task.Result;
        }

        private void OnGUIFieldClick(object sender, FieldGUI field)
        {
            for (int i = 0; i < GUIFields.Length; i++)
            {
                for (int j = 0; j < GUIFields[i].Length; j++)
                {
                    GUIFields[i][j].HighlightThinkness = 0;
                    GUIFields[i][j].ButtonClicked -= OnGUIFieldClick;
                }
            }
            foreach (var offer in CurrentOffers.FoundPaths)
            {
                Field fieldRef = offer.Key.Field;
                GUIFields[fieldRef.Row][fieldRef.Column].HighlightThinkness = 0;
            }
            field.HighlightThinkness = 3;
            Moves = CurrentOffers.FoundPaths[field.FieldRef.Piece].Select((s, i) => (i, new Stack<Move>(s.Reverse()))).ToList();
            foreach (var move in Moves)
            {
                Move currentMove = move.stack.Pop();
                GUIFields[currentMove.DestinationField.Row][currentMove.DestinationField.Column].HighlightThinkness = 3;
                GUIFields[currentMove.DestinationField.Row][currentMove.DestinationField.Column].CurrentMove = currentMove;
                GUIFields[currentMove.DestinationField.Row][currentMove.DestinationField.Column].MoveIndex = move.index;
                GUIFields[currentMove.DestinationField.Row][currentMove.DestinationField.Column].ButtonClicked += OnGUIMoveClick;
            }
        }

        private void OnGUIMoveClick(object sender, FieldGUI field)
        {
            for(int i = 0; i < GUIFields.Length; i++)
            {
                for(int j = 0; j < GUIFields[i].Length; j++)
                {
                    GUIFields[i][j].HighlightThinkness = 0;
                    GUIFields[i][j].ButtonClicked -= OnGUIMoveClick;
                }
            }
            if(SelectedPiece == null)
                SelectedPiece = field.CurrentMove.StartField.Piece;
            field.SetColor(Color, SelectedPiece.IsQueen);
            GUIFields[field.CurrentMove.StartField.Row][field.CurrentMove.StartField.Column].SetTransparent();
            if (field.CurrentMove.AttackedField != null)
                GUIFields[field.CurrentMove.AttackedField.Row][field.CurrentMove.AttackedField.Column].SetTransparent();
            Moves = Moves.Where(m => { if (m.stack.Count > 0) return m.stack.Peek().StartField == field.FieldRef; else return false; }).ToList();
            if (Moves.Count == 0)
            {
                TaskCompletionSource.SetResult(CurrentOffers.FoundPaths[SelectedPiece][field.MoveIndex]);
                SelectedPiece = null;
                Moves = null;
                CurrentOffers = null;
                return;
            }
            foreach (var move in Moves)
            {
                Move currentMove = move.stack.Pop();
                GUIFields[currentMove.DestinationField.Row][currentMove.DestinationField.Column].HighlightThinkness = 3;
                GUIFields[currentMove.DestinationField.Row][currentMove.DestinationField.Column].CurrentMove = currentMove;
                GUIFields[currentMove.DestinationField.Row][currentMove.DestinationField.Column].ButtonClicked += OnGUIMoveClick;
            }
        }
    }
}
