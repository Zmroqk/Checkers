using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine.Heuristics
{
    public class AreaHeurestic : IHeuristic
    {
        Board Board { get; set; }

        public AreaHeurestic(Board board)
        {
            Board = board;
        }

        public HeuristicEvaluationResult Evaluate()
        {
            HeuristicEvaluationResult result = new HeuristicEvaluationResult();
            double whitePoints = 0;
            foreach (Piece piece in Board.WhitePieces)
            {
                if (piece.IsQueen)
                {
                    whitePoints += 5;
                }
                else
                {
                    double points = Math.Abs(piece.Field.Row - 8d) / 4d;
                    whitePoints += points;
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
                    double points = (piece.Field.Row + 1) / 4d;
                    blackPoints += points;
                }
            }
            result.BlackPlayerScore = blackPoints;
            result.WhitePlayerScore = whitePoints;
            return result;
        }
    }
}
