using System.ComponentModel;

namespace CheckersEngine
{
    public class Field
    {
        public Field(int row, int column, Board board, CheckersColor color)
        {
            Row = row;
            Column = column;
            Color = color;
            Board = board;
        }

        public int Row { get; }
        public int Column { get; }
        public Board Board { get; }
        public Piece? Piece { get { return _piece; } set { _piece = value; PieceChanged?.Invoke(this, _piece); } }

        Piece? _piece;
        public CheckersColor Color { get; }

        public event EventHandler<Piece?> PieceChanged;
    }
}