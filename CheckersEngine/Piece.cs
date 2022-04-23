using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine
{
    public class Piece
    {
        public Piece(Field field, CheckersColor color)
        {
            Field = field;
            Color = color;
            IsQueen = false;
        }

        public Field Field { get; set; }
        public CheckersColor Color { get; }
        public bool IsQueen { get; set; }
    }
}
