using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine
{
    public class Move
    {
        public Field StartField { get; set; }
        public Field DestinationField { get; set; }
        public Field? AttackedField { get; set; }
        public Piece? AttackedPiece { get; set; }
        public bool ChangedToQueen { get; set; }
        public short DrawCounter { get; set; }
    }
}
