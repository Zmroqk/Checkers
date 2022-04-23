using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine
{
    public class Paths
    {
        public Paths()
        {
            FoundPaths = new Dictionary<Piece, List<Stack<Move>>>();
        }
        public Dictionary<Piece, List<Stack<Move>>> FoundPaths { get; }
    }
}
