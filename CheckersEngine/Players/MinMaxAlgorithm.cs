using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckersEngine.Heuristics;

namespace CheckersEngine.Players
{
    public class MinMaxAlgorithm : IAIAlgorithm
    {
        Board Board { get; set; }
        IHeuristic Heuristic { get; set; }

        short Level { get; set; }

        public MinMaxAlgorithm(short level, Board board, IHeuristic heuristic)
        {
            Board = board;
            Heuristic = heuristic;
            Level = level;
        }

        public Stack<Move> FindBestMove(Paths offers)
        {
            double max = double.MinValue;
            Stack<Move> bestMoves = null;
            Piece[] pieces = offers.FoundPaths.Keys.ToArray();
            for(int i = 0; i < pieces.Length; i++)
            {
                List<Stack<Move>> moves = offers.FoundPaths[pieces[i]];
                for(int j = 0; j < moves.Count; j++)
                {
                    Stack<Move> move = moves[j];
                    Stack<Move> moveCopy = new Stack<Move>(move);
                    int count = move.Count;
                    Board.MovePiece(pieces[i], move);
                    double score = mini(Level);
                    Board.UndoMoves(count);
                    if (score > max)
                    {
                        max = score;
                        bestMoves = moveCopy;
                    }
                }
            }
            return bestMoves;
        }

        public double maxi(int depth)
        {
            HeuristicEvaluationResult result = Heuristic.Evaluate();
            CheckersColor activeColor = Board.ActivePlayer.Color;
            if (depth == 0) {
                if(activeColor == CheckersColor.White)
                    return result.WhitePlayerScore - result.BlackPlayerScore;
                else
                    return result.BlackPlayerScore - result.WhitePlayerScore;
            }
            double max = double.MinValue;
            Paths newOffers = Board.FindPossibleMoves(Board.ActivePlayer);
            Piece[] pieces = newOffers.FoundPaths.Keys.ToArray();
            for (int i = 0; i < pieces.Length; i++)
            {
                List<Stack<Move>> moves = newOffers.FoundPaths[pieces[i]];
                for (int j = 0; j < moves.Count; j++)
                {
                    Stack<Move> move = moves[j];
                    int count = move.Count;
                    Board.MovePiece(pieces[i], move);
                    double score = mini(depth - 1);
                    if (score > max)
                        max = score;
                    Board.UndoMoves(count);
                }
            }
            return max;
        }

        public double mini(int depth)
        {
            HeuristicEvaluationResult result = Heuristic.Evaluate();
            CheckersColor activeColor = Board.ActivePlayer.Color;
            if (depth == 0)
            {
                if (activeColor == CheckersColor.White)
                    return result.BlackPlayerScore - result.WhitePlayerScore;
                else
                    return result.WhitePlayerScore - result.BlackPlayerScore;
            }
            double min = double.MaxValue;
            Paths newOffers = Board.FindPossibleMoves(Board.ActivePlayer);
            Piece[] pieces = newOffers.FoundPaths.Keys.ToArray();
            for (int i = 0; i < pieces.Length; i++)
            {
                List<Stack<Move>> moves = newOffers.FoundPaths[pieces[i]];
                for (int j = 0; j < moves.Count; j++)
                {
                    Stack<Move> move = moves[j];
                    int count = move.Count;
                    Board.MovePiece(pieces[i], move);
                    double score = maxi(depth - 1);
                    if (score < min)
                        min = score;
                    Board.UndoMoves(count);
                }
            }
            return min;
        }
    }
}
