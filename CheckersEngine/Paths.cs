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
            FoundPaths = new Dictionary<Piece, List<List<Field>>>();
        }
        public Dictionary<Piece, List<List<Field>>> FoundPaths { get; }
    }
}
