using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine.Players
{
    public interface IAIAlgorithm
    {
        Stack<Move> FindBestMove(Paths offers);
    }
}
