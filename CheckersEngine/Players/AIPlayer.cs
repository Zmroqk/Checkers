using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine.Players
{
    public class AIPlayer : IPlayer
    {
        public short PiecesLeft { 
            get {
                return _piecesLeft;
            } 
            set {
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
        Board Board { get; set; }

        IAIAlgorithm AIAlgorithm { get; set; }

        public AIPlayer(IAIAlgorithm aiAlgorithm, Board board)
        {
            AIAlgorithm = aiAlgorithm;
            Board = board;
        }

        public Stack<Move> MakeMove(Paths offers)
        {
            Board.InformPlayersAboutChange = false;
            Stack<Move> selected = AIAlgorithm.FindBestMove(offers);
            Board.InformPlayersAboutChange = true;
            Thread.Sleep(new Random().Next(800, 1000));
            return selected;
        }
    }
}
