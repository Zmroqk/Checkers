using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckersEngine.Analytics;
using CheckersEngine.Heuristics;

namespace CheckersEngine.Players
{
    public class MinMaxAlgorithm : IAIAlgorithm
    {
        Board Board { get; set; }
        IHeuristic Heuristic { get; set; }
        IAnalytics? Analytics { get; set; }

        short Level { get; set; }

        Random Random { get; set; }

        public MinMaxAlgorithm(short level, Board board, IHeuristic heuristic, IAnalytics? analytics)
        {
            Board = board;
            Heuristic = heuristic;
            Level = level;
            Random = new Random();
            Analytics = analytics;
        }

        public Stack<Move> FindBestMove(Paths offers)
        {
            if(Analytics != null)
                Analytics.Count++;
            DateTime start = DateTime.Now;
            double max = double.MinValue;
            List<Stack<Move>> bestMoves = new List<Stack<Move>>();
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
                    double score = mini(Level - 1);
                    Board.UndoMoves(count);
                    while(moveCopy.Count > 0)
                    {
                        move.Push(moveCopy.Pop());
                    }
                    if (score > max)
                    {
                        max = score;
                        bestMoves.Clear();
                        bestMoves.Add(move);
                    }
                    else if(score == max)
                    {
                        bestMoves.Add(move);
                    }
                    Board.ActivePlayerMoves = offers;
                }
            }
            DateTime end = DateTime.Now;
            if (Analytics != null)
                Analytics.Time += (end - start).TotalMilliseconds;
            return bestMoves[Random.Next(0, bestMoves.Count)];
        }

        public double maxi(int depth)
        {
            if (Analytics != null)
                Analytics.VisitedNodes++;
            HeuristicEvaluationResult result = Heuristic.Evaluate();
            CheckersColor activeColor = Board.ActivePlayer.Color;
            if (depth == 0 || Board.BlackPieces.Count == 0 || Board.WhitePieces.Count == 0) {
                if(activeColor == CheckersColor.White)
                    return result.WhitePlayerScore - result.BlackPlayerScore;
                else
                    return result.BlackPlayerScore - result.WhitePlayerScore;
            }
            double max = double.MinValue;
            Paths newOffers = Board.FindPossibleMoves(Board.ActivePlayer);
            Piece[] pieces = newOffers.FoundPaths.Keys.ToArray();
            if (pieces.Length > 0)
            {
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
                        Board.ActivePlayerMoves = newOffers;
                    }
                }
            }
            else
            {
                Board.ChangePlayer();
                mini(depth - 1);
                Board.ChangePlayer();
            }
            return max;
        }
        public double mini(int depth)
        {
            if (Analytics != null)
                Analytics.VisitedNodes++;
            HeuristicEvaluationResult result = Heuristic.Evaluate();
            CheckersColor activeColor = Board.ActivePlayer.Color;
            if (depth == 0 || Board.BlackPieces.Count == 0 || Board.WhitePieces.Count == 0)
            {
                if (activeColor == CheckersColor.White)
                    return result.BlackPlayerScore - result.WhitePlayerScore;
                else
                    return result.WhitePlayerScore - result.BlackPlayerScore;
            }
            double min = double.MaxValue;
            Paths newOffers = Board.FindPossibleMoves(Board.ActivePlayer);
            Piece[] pieces = newOffers.FoundPaths.Keys.ToArray();
            if (pieces.Length > 0)
            {
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
                        Board.ActivePlayerMoves = newOffers;
                    }
                }
            }
            else
            {
                Board.ChangePlayer();
                maxi(depth - 1);
                Board.ChangePlayer();
            }
            return min;
        }
    }
}
