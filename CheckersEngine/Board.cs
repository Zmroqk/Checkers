using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine
{
    public class Board
    {
        public const int BoardSize = 8;

        public Field[][] Fields;
        public IPlayer FirstPlayer;
        public IPlayer SecondPlayer;
        public IPlayer ActivePlayer;

        public Board(IPlayer first, IPlayer second)
        {
            FirstPlayer = first;
            SecondPlayer = second;
            Fields = new Field[BoardSize][];
            for(int i = 0; i < BoardSize; i++)
            {
                Fields[i] = new Field[BoardSize];
                CheckersColor currentColor;
                if (i % 2 == 0)
                    currentColor = CheckersColor.White;
                else 
                    currentColor = CheckersColor.Black;
                for(int j = 0; j < BoardSize; j++)
                {
                    Fields[i][j] = new Field(i, j, this, currentColor);
                    if (currentColor == CheckersColor.White)
                        currentColor = CheckersColor.Black;
                    else
                    {
                        currentColor = CheckersColor.White;
                        if((i == 0 && j % 2 == 1) || (i == 1 && j % 2 == 0))
                        {
                            Fields[i][j].Piece = new Piece(Fields[i][j], CheckersColor.Black);
                        }
                        else if((i == BoardSize-1 && j % 2 == 0) || (i == BoardSize-2 && j % 2 == 1))
                        {
                            Fields[i][j].Piece = new Piece(Fields[i][j], CheckersColor.White);
                        }
                    }                                  
                }              
            }
        }

        public Paths FindPossibleMoves(IPlayer player)
        {
            return null;
        }

        public Field this[int index, int secondIndex] {
            get
            {
                return Fields[index][secondIndex];
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new String('-', 4*BoardSize + 1));
            sb.AppendLine();
            for(int i = 0; i < BoardSize; i++)
            {
                sb.Append("|");
                for (int j = 0; j < BoardSize; j++)
                {
                    if (Fields[i][j].Piece == null)
                        sb.Append(" . |");
                    else if (Fields[i][j].Piece.Color == CheckersColor.White)
                        sb.Append(" W |");
                    else
                        sb.Append(" B |");
                }
                sb.AppendLine();
            }
            sb.Append(new String('-', 4 * BoardSize + 1));
            sb.AppendLine();
            return sb.ToString();
        }

    }
}
