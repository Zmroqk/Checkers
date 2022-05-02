using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckersEngine.Analytics;
using CheckersEngine.Heuristics;

namespace CheckersEngine.Players
{
    public class AlfaBetaPruningAlgorithm : IAIAlgorithm
    {
        Board Board { get; set; }
        IHeuristic Heuristic { get; set; }
        IAnalytics? Analytics { get; set; }

        short Level { get; set; }

        Random Random { get; set; }

        public AlfaBetaPruningAlgorithm(short level, Board board, IHeuristic heuristic, IAnalytics? analytics)
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
            for (int i = 0; i < pieces.Length; i++)
            {
                List<Stack<Move>> moves = offers.FoundPaths[pieces[i]];
                for (int j = 0; j < moves.Count; j++)
                {
                    Stack<Move> move = moves[j];
                    Stack<Move> moveCopy = new Stack<Move>(move);
                    int count = move.Count;
                    Board.MovePiece(pieces[i], move);
                    double score = AlphaBetaMini(double.MinValue, double.MaxValue, Level - 1);
                    Board.UndoMoves(count);
                    while (moveCopy.Count > 0)
                    {
                        move.Push(moveCopy.Pop());
                    }
                    if (score > max)
                    {
                        max = score;
                        bestMoves.Clear();
                        bestMoves.Add(move);
                    }
                    else if (score == max)
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

        public double AlphaBetaMax(double alpha, double beta, int depth)
        {
            if(Analytics != null)
                Analytics.VisitedNodes++;
            HeuristicEvaluationResult result = Heuristic.Evaluate();
            CheckersColor activeColor = Board.ActivePlayer.Color;
            if (depth == 0 || Board.BlackPieces.Count == 0 || Board.WhitePieces.Count == 0) {
                if(activeColor == CheckersColor.White)
                    return result.WhitePlayerScore - result.BlackPlayerScore;
                else
                    return result.BlackPlayerScore - result.WhitePlayerScore;
            }
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
                        double score = AlphaBetaMini(alpha, beta, depth - 1);
                        Board.UndoMoves(count);
                        Board.ActivePlayerMoves = newOffers;
                        if (score >= beta)
                        {
                            return beta;
                        }
                        if (score > alpha)
                        {
                            alpha = score;
                        }
                    }
                }
            }
            else
            {
                Board.ChangePlayer();
                AlphaBetaMini(alpha, beta, depth - 1);
                Board.ChangePlayer();
            }
            return alpha;
        }

        public double AlphaBetaMini(double alpha, double beta, int depth)
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
                        double score = AlphaBetaMax(alpha, beta, depth - 1);
                        Board.UndoMoves(count);
                        Board.ActivePlayerMoves = newOffers;
                        if (score <= alpha)
                            return alpha;
                        if (score < beta)
                            beta = score;
                    }
                }
            }
            else
            {
                Board.ChangePlayer();
                AlphaBetaMax(alpha, beta, depth - 1);
                Board.ChangePlayer();
            }
            return beta;
        }
    }
}
