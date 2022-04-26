﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckersEngine.Players;

namespace CheckersEngine
{
    public class Board : IDisposable
    {
        public const int BoardSize = 8;

        public Field[][] Fields;
        public List<Piece> BlackPieces;
        public List<Piece> WhitePieces;
        public IPlayer WhitePlayer;
        public IPlayer BlackPlayer;
        public IPlayer? ActivePlayer;

        public IPlayer? Winner;
        public event EventHandler<IPlayer> OnWinnerAnnouncement;
        public event EventHandler<IPlayer> OnPlayerTurnChange;

        public Paths ActivePlayerMoves;

        private Stack<Move> MoveHistory;

        private bool IsBoardInitialized;
        private bool ArePlayersInitialized;

        public bool InformPlayersAboutChange;

        public Board()
        {            
            BlackPieces = new List<Piece>();
            WhitePieces = new List<Piece>();
            MoveHistory = new Stack<Move>();
            IsBoardInitialized = false;
            ArePlayersInitialized = false;
            InformPlayersAboutChange = true;
        }

        private void OnNoPiecesLeft(object? sender, EventArgs e)
        {
            if (sender == WhitePlayer)
            {
                ActivePlayer = null;
                OnWinnerAnnouncement?.Invoke(this, BlackPlayer);
                Dispose();
            }
            else if (sender == BlackPlayer)
            {
                ActivePlayer = null;
                OnWinnerAnnouncement?.Invoke(this, WhitePlayer);
                Dispose();
            }
        }

        public void InitPlayers(IPlayer first, IPlayer second)
        {
            if (first == null || second == null)
            {
                throw new ArgumentNullException();
            }
            WhitePlayer = first;
            BlackPlayer = second;
            ActivePlayer = first;

            WhitePlayer.NoPiecesLeft += OnNoPiecesLeft;
            BlackPlayer.NoPiecesLeft += OnNoPiecesLeft;
            ArePlayersInitialized = true;
            OnPlayerTurnChange?.Invoke(this, WhitePlayer);
        }
        public void InitBoard()
        {
            Fields = new Field[BoardSize][];
            int counter = 0;
            for (int i = 0; i < BoardSize; i++)
            {
                Fields[i] = new Field[BoardSize];
                CheckersColor currentColor;
                if (i % 2 == 0)
                    currentColor = CheckersColor.White;
                else
                    currentColor = CheckersColor.Black;
                for (int j = 0; j < BoardSize; j++)
                {
                    Fields[i][j] = new Field(i, j, this, currentColor);
                    if (currentColor == CheckersColor.White)
                        currentColor = CheckersColor.Black;
                    else
                    {
                        currentColor = CheckersColor.White;
                        if ((i == 0 && j % 2 == 1) || (i == 1 && j % 2 == 0))
                        {
                            Fields[i][j].Piece = new Piece(Fields[i][j], CheckersColor.Black, BlackPlayer, counter++);
                            Fields[i][j].Piece.OnDestroy += Piece_OnDestroy;
                            BlackPieces.Add(Fields[i][j].Piece);
                        }
                        else if ((i == BoardSize - 1 && j % 2 == 0) || (i == BoardSize - 2 && j % 2 == 1))
                        {
                            Fields[i][j].Piece = new Piece(Fields[i][j], CheckersColor.White, WhitePlayer, counter++);
                            Fields[i][j].Piece.OnDestroy += Piece_OnDestroy;
                            WhitePieces.Add(Fields[i][j].Piece);
                        }
                    }
                }
            }
            IsBoardInitialized = true;
        }

        private void Piece_OnDestroy(object? sender, Piece e)
        {
            if(e.Color == CheckersColor.White)
            {
                WhitePieces.Remove(e);
                WhitePlayer.PiecesLeft--;
            }         
            else if(e.Color == CheckersColor.Black)
            {
                BlackPieces.Remove(e);
                BlackPlayer.PiecesLeft--;
            }     
        }

        public void MovePiece(Piece piece, Stack<Move> moves)
        {
            if (!IsBoardInitialized && !ArePlayersInitialized)
                throw new ArgumentNullException();
            List<Stack<Move>> legalMoves = ActivePlayerMoves.FoundPaths[piece]; //TODO EXTEND MOVE SECURITY
            if (legalMoves.Contains(moves))
            {
                while (moves.Count > 0)
                {
                    Move poppedMove = moves.Pop();
                    MoveHistory.Push(poppedMove);
                    if (!InformPlayersAboutChange)
                    {
                        poppedMove.StartField.InformPieceChanged = false;
                        poppedMove.DestinationField.InformPieceChanged = false;
                        if(poppedMove.AttackedField != null)
                            poppedMove.AttackedField.InformPieceChanged = false;
                    }
                    else
                    {
                        poppedMove.StartField.InformPieceChanged = true;
                        poppedMove.DestinationField.InformPieceChanged = true;
                        if (poppedMove.AttackedField != null)
                            poppedMove.AttackedField.InformPieceChanged = true;
                    }
                    poppedMove.DestinationField.Piece = poppedMove.StartField.Piece;
                    poppedMove.StartField.Piece = null;                 
                    if (poppedMove.AttackedField != null)
                    {
                        if (poppedMove.AttackedPiece.Color == CheckersColor.Black)
                            BlackPieces.Remove(poppedMove.AttackedPiece);
                        else
                            WhitePieces.Remove(poppedMove.AttackedPiece);
                        poppedMove.AttackedField.Piece = null;
                    }
                    poppedMove.DestinationField.Piece.Field = poppedMove.DestinationField;
                    if (moves.Count == 0)
                    {
                        if (poppedMove.DestinationField.Piece.Color == CheckersColor.Black 
                            && poppedMove.DestinationField.Row == BoardSize-1 
                            && !poppedMove.DestinationField.Piece.IsQueen)
                        {
                            poppedMove.DestinationField.Piece.IsQueen = true;
                            poppedMove.ChangedToQueen = true;
                        }
                        else if(poppedMove.DestinationField.Piece.Color == CheckersColor.White 
                            && poppedMove.DestinationField.Row == 0
                            && !poppedMove.DestinationField.Piece.IsQueen)
                        {
                            poppedMove.DestinationField.Piece.IsQueen = true;
                            poppedMove.ChangedToQueen = true;
                        }
                    }
                }
                if(ActivePlayer == WhitePlayer)
                {
                    ActivePlayer = BlackPlayer;
                }
                else
                {
                    ActivePlayer = WhitePlayer;
                }
                if (InformPlayersAboutChange)
                {
                    OnPlayerTurnChange?.Invoke(this, ActivePlayer);
                }
            }      
        }

        public void UndoMoves(int count = 1)
        {
            if (!IsBoardInitialized && !ArePlayersInitialized)
                throw new ArgumentNullException();
            if (count > MoveHistory.Count)
                throw new ArgumentException();
            for (int i = 0; i < count; i++) {
                Move poppedMove = MoveHistory.Pop();
                if (!InformPlayersAboutChange)
                {
                    poppedMove.StartField.InformPieceChanged = false;
                    poppedMove.DestinationField.InformPieceChanged = false;
                    if (poppedMove.AttackedField != null)
                        poppedMove.AttackedField.InformPieceChanged = false;
                }
                else
                {
                    poppedMove.StartField.InformPieceChanged = true;
                    poppedMove.DestinationField.InformPieceChanged = true;
                    if (poppedMove.AttackedField != null)
                        poppedMove.AttackedField.InformPieceChanged = true;
                }
                poppedMove.StartField.Piece = poppedMove.DestinationField.Piece;
                poppedMove.DestinationField.Piece = null;
                if (poppedMove.AttackedField != null)
                {
                    poppedMove.AttackedField.Piece = poppedMove.AttackedPiece;
                    if (poppedMove.AttackedPiece.Color == CheckersColor.Black)
                        BlackPieces.Add(poppedMove.AttackedPiece);
                    else
                        WhitePieces.Add(poppedMove.AttackedPiece);
                }
                poppedMove.StartField.Piece.Field = poppedMove.StartField;
                if (poppedMove.ChangedToQueen)
                {
                    poppedMove.StartField.Piece.IsQueen = false;
                }
            }
            if (ActivePlayer == WhitePlayer)
            {
                ActivePlayer = BlackPlayer;
            }
            else
            {
                ActivePlayer = WhitePlayer;
            }
        }

        public Paths FindPossibleMoves(IPlayer player)
        {
            if (!IsBoardInitialized && !ArePlayersInitialized)
                throw new ArgumentNullException();
            List<Piece> checkedList;
            if(player.Color == CheckersColor.White)
            {
                checkedList = WhitePieces;
            }
            else
            {
                checkedList = BlackPieces;
            }
            Paths paths = new Paths();
            bool foundAttack = false;
            foreach(Piece piece in checkedList)
            {
                List<Stack<Move>> possibleAttacks = piece.FindAttacks();
                if (possibleAttacks != null)
                {
                    piece.MustAttack = true;
                    paths.FoundPaths.Add(piece, possibleAttacks);
                    foundAttack = true;
                }
                else if(!foundAttack)
                {
                    piece.MustAttack = false;
                    List<Stack<Move>> possibleMoves = piece.FindMoves();
                    if(possibleMoves.Count > 0)
                        paths.FoundPaths.Add(piece, possibleMoves);
                }
            }
            if (foundAttack)
            {
                Piece[] pieces = paths.FoundPaths.Keys.ToArray();
                for(int i = 0; i < pieces.Length; i++)
                {
                    if (!pieces[i].MustAttack)
                    {
                        paths.FoundPaths.Remove(pieces[i]);
                    }
                }
            }
            ActivePlayerMoves = paths;
            return paths;
        }

        public Field this[int index, int secondIndex] {
            get
            {
                if (!IsBoardInitialized)
                    throw new ArgumentNullException();
                return Fields[index][secondIndex];
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new String('-', 4 * BoardSize + 1));
            sb.AppendLine();
            for (int i = 0; i < BoardSize; i++)
            {
                sb.Append("|");
                for (int j = 0; j < BoardSize; j++)
                {
                    if (Fields[i][j].Piece == null)
                        sb.Append(" _ |");
                    else if (Fields[i][j].Piece.Color == CheckersColor.White)
                        sb.Append(" W |");
                    else
                        sb.Append(" B |");
                }
                sb.AppendLine();
            }
            sb.Append(new String('-', 4 * BoardSize + 1));
            sb.AppendLine();
            return sb.ToString();
        }

        public void Dispose()
        {
            IsBoardInitialized = false;
            ArePlayersInitialized = false;
            Fields = null;
            ActivePlayerMoves = null;
            WhitePlayer = null;
            BlackPlayer = null;
            MoveHistory = null;
            ActivePlayer = null;
        }
    }
}
