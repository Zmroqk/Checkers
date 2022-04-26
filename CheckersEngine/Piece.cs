using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckersEngine.Players;

namespace CheckersEngine
{
    public class Piece
    {
        public Piece(Field field, CheckersColor color, IPlayer player, int piedeid = -1)
        {
            Field = field;
            Color = color;
            IsQueen = false;
            Player = player;
            PieceId = piedeid;
        }

        public int PieceId;

        public Field Field { get; set; }
        public CheckersColor Color { get; }
        public bool IsQueen { get; set; }
        public bool MustAttack { get; set; }
        public IPlayer Player { get; }
        public event EventHandler<Piece> OnDestroy;

        public List<Stack<Move>> FindMoves()
        {
            List<Stack<Move>> moves = new List<Stack<Move>>();
            int x = Field.Row, y = Field.Column;
            if (IsQueen)
            {
                int xLocal = x, yLocal = y;
                while (xLocal > 0 && yLocal > 0)
                {
                    if (Field.Board[xLocal - 1, yLocal - 1].Piece == null)
                    {
                        Stack<Move> stack = new Stack<Move>();
                        stack.Push(new Move() { 
                            StartField = Field.Board[x, y], 
                            DestinationField = Field.Board[xLocal - 1, yLocal - 1] 
                        });
                        moves.Add(stack);
                        xLocal--;
                        yLocal--;
                    }
                    else
                    {
                        break;
                    }
                }
                xLocal = x; yLocal = y;
                while (xLocal > 0 && yLocal < Board.BoardSize - 1)
                {
                    if (Field.Board[xLocal - 1, yLocal + 1].Piece == null)
                    {
                        Stack<Move> stack = new Stack<Move>();
                        stack.Push(new Move()
                        {
                            StartField = Field.Board[x, y],
                            DestinationField = Field.Board[xLocal - 1, yLocal + 1]
                        });
                        moves.Add(stack);
                        xLocal--;
                        yLocal++;
                    }
                    else
                    {
                        break;
                    }
                }
                xLocal = x; yLocal = y;
                while (xLocal < Board.BoardSize - 1 && yLocal > 0)
                {
                    if (Field.Board[xLocal + 1, yLocal - 1].Piece == null)
                    {
                        Stack<Move> stack = new Stack<Move>();
                        stack.Push(new Move()
                        {
                            StartField = Field.Board[x, y],
                            DestinationField = Field.Board[xLocal + 1, yLocal - 1]
                        });
                        moves.Add(stack);
                        xLocal++;
                        yLocal--;
                    }
                    else
                    {
                        break;
                    }
                }
                xLocal = x; yLocal = y;
                while (xLocal < Board.BoardSize - 1 && yLocal < Board.BoardSize - 1)
                {
                    if (Field.Board[xLocal + 1, yLocal + 1].Piece == null)
                    {
                        Stack<Move> stack = new Stack<Move>();
                        stack.Push(new Move()
                        {
                            StartField = Field.Board[x, y],
                            DestinationField = Field.Board[xLocal + 1, yLocal + 1]
                        });
                        moves.Add(stack);
                        xLocal++;
                        yLocal++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                if (Color == CheckersColor.Black)
                {
                    if (x + 1 < Board.BoardSize && y + 1 < Board.BoardSize)
                    {
                        Field fieldChecked = Field.Board[x + 1, y + 1];
                        if (fieldChecked.Piece == null)
                        {
                            Stack<Move> stack = new Stack<Move>();
                            stack.Push(new Move()
                            {
                                StartField = Field.Board[x, y],
                                DestinationField = Field.Board[x + 1, y + 1]
                            });
                            moves.Add(stack);
                        }
                    }
                    if (y - 1 > 0 && x + 1 < Board.BoardSize)
                    {
                        Field fieldChecked = Field.Board[x + 1, y - 1];
                        if (fieldChecked.Piece == null)
                        {
                            Stack<Move> stack = new Stack<Move>();
                            stack.Push(new Move()
                            {
                                StartField = Field.Board[x, y],
                                DestinationField = Field.Board[x + 1, y - 1]
                            });
                            moves.Add(stack);
                        }
                    }
                }
                else
                {
                    if (x - 1 > 0 && y - 1 > 0)
                    {
                        Field fieldChecked = Field.Board[x - 1, y - 1];
                        if (fieldChecked.Piece == null)
                        {
                            Stack<Move> stack = new Stack<Move>();
                            stack.Push(new Move()
                            {
                                StartField = Field.Board[x, y],
                                DestinationField = Field.Board[x - 1, y - 1]
                            });
                            moves.Add(stack);
                        }
                    }

                    if (x - 1 > 0 && y + 1 < Board.BoardSize)
                    {
                        Field fieldChecked = Field.Board[x - 1, y + 1];
                        if (fieldChecked.Piece == null)
                        {
                            Stack<Move> stack = new Stack<Move>();
                            stack.Push(new Move()
                            {
                                StartField = Field.Board[x, y],
                                DestinationField = Field.Board[x - 1, y + 1]
                            });
                            moves.Add(stack);
                        }
                    }
                }
            }
            return moves;
        }

        public void Destroy()
        {
            OnDestroy?.Invoke(this, this);
            Field = null;
        }

        public List<Stack<Move>> FindAttacks()
        {
            return FindAttacks(Field);
        }
        public List<Stack<Move>> FindAttacks(Field field)
        {
            List<Move> foundAttacks = new List<Move>();
            int x = field.Row, y = field.Column;
            if (IsQueen)
            {
                int xLocal = x, yLocal = y;
                int xFoundLocal = x, yFoundLocal = y;
                bool found = false;
                while (xLocal > 1 && yLocal > 1)
                {
                    Field fieldChecked;
                    if (!found)
                        fieldChecked = Field.Board[xLocal - 1, yLocal - 1];
                    else
                        fieldChecked = Field.Board[xFoundLocal, yFoundLocal];
                    Field nextFieldChecked = Field.Board[xLocal - 2, yLocal - 2];
                    if (fieldChecked != null && fieldChecked.Piece != null && fieldChecked.Piece.Color != Color && nextFieldChecked.Piece == null)
                    {
                        if (!found)
                        {
                            found = true;
                            xFoundLocal = xLocal - 1;
                            yFoundLocal = yLocal - 1;
                        }
                        foundAttacks.Add(new Move()
                        {
                            StartField = field,
                            DestinationField = nextFieldChecked,
                            AttackedField = fieldChecked,
                            AttackedPiece = fieldChecked.Piece
                        });
                    }
                    else if (fieldChecked != null && fieldChecked.Piece != null && fieldChecked.Piece.Color != Color && nextFieldChecked.Piece != null)
                    {
                        break;
                    }
                    else if(found && nextFieldChecked.Piece != null)
                    {
                        break;
                    }
                    xLocal--;
                    yLocal--;
                }
                xLocal = x; yLocal = y;
                xFoundLocal = x; yFoundLocal = y;
                found = false;
                while (xLocal > 1 && yLocal < Board.BoardSize - 2)
                {
                    Field fieldChecked;
                    if (!found)
                        fieldChecked = Field.Board[xLocal - 1, yLocal + 1];
                    else
                        fieldChecked = Field.Board[xFoundLocal, yFoundLocal];
                    Field nextFieldChecked = Field.Board[xLocal - 2, yLocal + 2];
                    if (fieldChecked != null && fieldChecked.Piece != null && fieldChecked.Piece.Color != Color && nextFieldChecked.Piece == null)
                    {
                        if (!found)
                        {
                            found = true;
                            xFoundLocal = xLocal - 1;
                            yFoundLocal = yLocal + 1;
                        }
                        foundAttacks.Add(new Move()
                        {
                            StartField = field,
                            DestinationField = nextFieldChecked,
                            AttackedField = fieldChecked,
                            AttackedPiece = fieldChecked.Piece
                        });
                    }
                    else if (fieldChecked != null && fieldChecked.Piece != null && fieldChecked.Piece.Color != Color && nextFieldChecked.Piece != null)
                    {
                        break;
                    }
                    else if (found && nextFieldChecked.Piece != null)
                    {
                        break;
                    }
                    xLocal--;
                    yLocal++;
                }
                xLocal = x; yLocal = y;
                xFoundLocal = x; yFoundLocal = y;
                found = false;
                while (xLocal < Board.BoardSize - 2 && yLocal > 1)
                {
                    Field fieldChecked;
                    if (!found)
                        fieldChecked = Field.Board[xLocal + 1, yLocal - 1];
                    else
                        fieldChecked = Field.Board[xFoundLocal, yFoundLocal];
                    Field nextFieldChecked = Field.Board[xLocal + 2, yLocal - 2];
                    if (fieldChecked != null && fieldChecked.Piece != null && fieldChecked.Piece.Color != Color && nextFieldChecked.Piece == null)
                    {
                        if (!found)
                        {
                            found = true;
                            xFoundLocal = xLocal + 1;
                            yFoundLocal = yLocal - 1;
                        }
                        foundAttacks.Add(new Move()
                        {
                            StartField = field,
                            DestinationField = nextFieldChecked,
                            AttackedField = fieldChecked,
                            AttackedPiece = fieldChecked.Piece
                        });
                    }
                    else if(fieldChecked != null && fieldChecked.Piece != null && fieldChecked.Piece.Color != Color && nextFieldChecked.Piece != null)
                    {
                        break;
                    }
                    else if (found && nextFieldChecked.Piece != null)
                    {
                        break;
                    }
                    xLocal++;
                    yLocal--;
                }
                xLocal = x; yLocal = y;
                xFoundLocal = x; yFoundLocal = y;
                found = false;
                while (xLocal < Board.BoardSize - 2 && yLocal < Board.BoardSize - 2)
                {
                    Field fieldChecked;
                    if (!found)
                        fieldChecked = Field.Board[xLocal + 1, yLocal + 1];
                    else
                        fieldChecked = Field.Board[xFoundLocal, yFoundLocal];
                    Field nextFieldChecked = Field.Board[xLocal + 2, yLocal + 2];
                    if (fieldChecked != null && fieldChecked.Piece != null && fieldChecked.Piece.Color != Color && nextFieldChecked.Piece == null)
                    {
                        if (!found)
                        {
                            found = true;
                            xFoundLocal = xLocal + 1;
                            yFoundLocal = yLocal + 1;
                        }
                        foundAttacks.Add(new Move()
                        {
                            StartField = field,
                            DestinationField = nextFieldChecked,
                            AttackedField = fieldChecked,
                            AttackedPiece = fieldChecked.Piece
                        });
                    }
                    else if (fieldChecked != null && fieldChecked.Piece != null && fieldChecked.Piece.Color != Color && nextFieldChecked.Piece != null)
                    {
                        break;
                    }
                    else if (found && nextFieldChecked.Piece != null)
                    {
                        break;
                    }
                    xLocal++;
                    yLocal++;
                }
            }
            else
            {
                if (x + 1 < Board.BoardSize && y + 1 < Board.BoardSize && x + 2 < Board.BoardSize && y + 2 < Board.BoardSize)
                {
                    Field fieldChecked = Field.Board[x + 1, y + 1];
                    Field nextFieldChecked = Field.Board[x + 2, y + 2];
                    if (fieldChecked.Piece != null && fieldChecked.Piece.Color != Color && nextFieldChecked.Piece == null)
                    {
                        foundAttacks.Add(new Move()
                        {
                            StartField = field,
                            DestinationField = nextFieldChecked,
                            AttackedField = fieldChecked,
                            AttackedPiece = fieldChecked.Piece
                        });
                    }
                }
                if (x + 1 < Board.BoardSize && y - 1 > 0 && x + 2 < Board.BoardSize && y - 2 >= 0)
                {
                    Field fieldChecked = Field.Board[x + 1, y - 1];
                    Field nextFieldChecked = Field.Board[x + 2, y - 2];
                    if (fieldChecked.Piece != null && fieldChecked.Piece.Color != Color && nextFieldChecked.Piece == null)
                    {
                        foundAttacks.Add(new Move()
                        {
                            StartField = field,
                            DestinationField = nextFieldChecked,
                            AttackedField = fieldChecked,
                            AttackedPiece = fieldChecked.Piece
                        });
                    }
                }
                if (x - 1 > 0 && y - 1 > 0 && x - 2 >= 0 && y - 2 >= 0)
                {
                    Field fieldChecked = Field.Board[x - 1, y - 1];
                    Field nextFieldChecked = Field.Board[x - 2, y - 2];
                    if (fieldChecked.Piece != null && fieldChecked.Piece.Color != Color && nextFieldChecked.Piece == null)
                    {
                        foundAttacks.Add(new Move()
                        {
                            StartField = field,
                            DestinationField = nextFieldChecked,
                            AttackedField = fieldChecked,
                            AttackedPiece = fieldChecked.Piece
                        });
                    }
                }

                if (x - 1 > 0 && y + 1 < Board.BoardSize && x - 2 >= 0 && y + 2 < Board.BoardSize)
                {
                    Field fieldChecked = Field.Board[x - 1, y + 1];
                    Field nextFieldChecked = Field.Board[x - 2, y + 2];
                    if (fieldChecked.Piece != null && fieldChecked.Piece.Color != Color && nextFieldChecked.Piece == null)
                    {
                        foundAttacks.Add(new Move()
                        {
                            StartField = field,
                            DestinationField = nextFieldChecked,
                            AttackedField = fieldChecked,
                            AttackedPiece = fieldChecked.Piece
                        });
                    }
                }
            }
            if(foundAttacks.Count == 0)
            {             
                return null;
            }
            else
            {
                List<Stack<Move>> attacks = new List<Stack<Move>>();
                for (int i = 0; i < foundAttacks.Count; i++)
                {
                    Piece originalPiece = foundAttacks[i].StartField.Piece;
                    Piece attackedPiece = foundAttacks[i].AttackedField.Piece;
                    foundAttacks[i].StartField.InformPieceChanged = false;
                    foundAttacks[i].DestinationField.InformPieceChanged = false;
                    foundAttacks[i].AttackedField.InformPieceChanged = false;
                    foundAttacks[i].DestinationField.Piece = originalPiece;
                    foundAttacks[i].StartField.Piece = null;               
                    foundAttacks[i].AttackedField.Piece = null;
                    originalPiece.Field = foundAttacks[i].DestinationField;
                    attackedPiece.Field = null;
                    List<Stack<Move>> result = FindAttacks(foundAttacks[i].DestinationField);
                    originalPiece.Field = foundAttacks[i].StartField;
                    attackedPiece.Field = foundAttacks[i].AttackedField;
                    foundAttacks[i].StartField.Piece = originalPiece;
                    foundAttacks[i].DestinationField.Piece = null;
                    foundAttacks[i].AttackedField.Piece = attackedPiece;
                    foundAttacks[i].StartField.InformPieceChanged = true;
                    foundAttacks[i].DestinationField.InformPieceChanged = true;
                    foundAttacks[i].AttackedField.InformPieceChanged = true;
                    if (result == null)
                    {
                        Stack<Move> newStack = new Stack<Move>();
                        newStack.Push(foundAttacks[i]);
                        attacks.Add(newStack);
                    }
                    else
                    {
                        foreach(Stack<Move> stack in result)
                        {
                            stack.Push(foundAttacks[i]);
                        }
                        attacks.AddRange(result);
                    }
                }
                var deepestCount = attacks.Max((st) => st.Count);          
                return attacks.Where((st) => st.Count == deepestCount).ToList();
            }
        }
    }
}
