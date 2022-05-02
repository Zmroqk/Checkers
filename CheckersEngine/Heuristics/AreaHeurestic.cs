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
            double blackPoints = 0;
            foreach (Piece piece in Board.WhitePieces)
            {
                if (Board.BlackPieces.Count == 0)
                {
                    whitePoints += 100;                
                }
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
            foreach (Piece piece in Board.BlackPieces)
            {
                if (Board.WhitePieces.Count == 0)
                {
                    blackPoints += 100;
                }
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
            if (Board.DrawCounter == Board.DrawMaxMoves)
            {
                whitePoints = -100;
                blackPoints = -100;
            }
            else
            {
                whitePoints *= 1 - Board.DrawCounter / (double)Board.DrawMaxMoves;
                blackPoints *= 1 - Board.DrawCounter / (double)Board.DrawMaxMoves;
            }
            result.BlackPlayerScore = blackPoints;
            result.WhitePlayerScore = whitePoints;
            return result;
        }
    }
}
