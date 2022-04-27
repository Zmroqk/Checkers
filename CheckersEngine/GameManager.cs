using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckersEngine.Players;


namespace CheckersEngine
{
    public class GameManager
    {
        public Board Board { get; set; }

        private IPlayer? WhitePlayer { get; set; }
        private IPlayer? BlackPlayer { get; set; }

        public GameManager()
        {
            Board = new Board();
            Board.InitBoard();
            WhitePlayer = null;
            BlackPlayer = null;
            Board.OnPlayerTurnChange += Board_OnPlayerTurnChange;
            Board.OnWinnerAnnouncement += Board_OnWinnerAnnouncement;
        }

        private void Board_OnWinnerAnnouncement(object? sender, IPlayer e)
        {
            Board.OnPlayerTurnChange -= Board_OnPlayerTurnChange;
        }

        private void Board_OnPlayerTurnChange(object? sender, IPlayer e)
        {
            Paths offers = Board.FindPossibleMoves(Board.ActivePlayer);
            if(offers.FoundPaths.Keys.Count == 0)
            {
                Board.ChangePlayer();
            }
            else
            {
                Stack<Move> selected;
                if (e == WhitePlayer)
                {
                    selected = WhitePlayer.MakeMove(offers);
                }
                else
                {
                    selected = BlackPlayer.MakeMove(offers);
                }
                Board.MovePiece(selected.Peek().StartField.Piece, selected);
            }      
            
        }

        public void EndGame()
        {
            Board.OnPlayerTurnChange -= Board_OnPlayerTurnChange;
            Board.Dispose();
        }

        public void StartGame()
        {
            Board.StartGame();
        }

        public void SetPlayerWhite(IPlayer player)
        {
            WhitePlayer = player;
            WhitePlayer.Color = CheckersColor.White;
            if (BlackPlayer != null)
                Board.InitPlayers(WhitePlayer, BlackPlayer);
        }

        public void SetPlayerBlack(IPlayer player)
        {
            BlackPlayer = player;
            BlackPlayer.Color = CheckersColor.Black;
            if (WhitePlayer != null)
                Board.InitPlayers(WhitePlayer, BlackPlayer);
        }
    }
}
