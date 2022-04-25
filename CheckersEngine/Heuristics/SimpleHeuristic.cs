using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine.Heuristics
{
    public class SimpleHeuristic : IHeuristic
    {
        Board Board { get; set; }

        public SimpleHeuristic(Board board)
        {
            Board = board;
        }

        public HeuristicEvaluationResult Evaluate()
        {
            HeuristicEvaluationResult result = new HeuristicEvaluationResult();
            double whitePoints = 0;
            foreach(Piece piece in Board.WhitePieces)
            {
                if (piece.IsQueen)
                {
                    whitePoints += 5;
                }
                else
                {
                    whitePoints++;
                }
            }
            double blackPoints = 0;
            foreach (Piece piece in Board.BlackPieces)
            {
                if (piece.IsQueen)
                {
                    blackPoints += 5;
                }
                else
                {
                    blackPoints++;
                }
            }
            result.BlackPlayerScore = blackPoints;
            result.WhitePlayerScore = whitePoints;
            return result;
        }
    }
}
