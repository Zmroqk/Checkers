using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine.Heuristics
{
    public class MixedHeuristic : IHeuristic
    {
        (IHeuristic heuristic, double weight)[] Heuristics { get; set; }

        public MixedHeuristic((IHeuristic heuristic, double weight)[] heuristics)
        {
            Heuristics = heuristics;
        }

        public HeuristicEvaluationResult Evaluate()
        {
            HeuristicEvaluationResult result = new HeuristicEvaluationResult();
            foreach(var heuristic in Heuristics)
            {
                HeuristicEvaluationResult tempRes = heuristic.heuristic.Evaluate();
                result.WhitePlayerScore += tempRes.WhitePlayerScore * heuristic.weight;
                result.BlackPlayerScore += tempRes.BlackPlayerScore * heuristic.weight;
            }
            return result;
        }
    }
}
