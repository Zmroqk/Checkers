using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine.Heuristics
{
    public interface IHeuristic
    {
        HeuristicEvaluationResult Evaluate();
    }
}
