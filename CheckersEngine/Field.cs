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
            InformPieceChanged = true;
        }

        public int Row { get; }
        public int Column { get; }
        public Board Board { get; }
        public Piece? Piece { get { return _piece; } set { _piece = value; if(InformPieceChanged) PieceChanged?.Invoke(this, _piece); } }
        public bool InformPieceChanged;

        Piece? _piece;
        public CheckersColor Color { get; }

        public event EventHandler<Piece?> PieceChanged;
    }
}