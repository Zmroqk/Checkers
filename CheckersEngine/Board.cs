using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine
{
    public class Board : IDisposable
    {
        public const int BoardSize = 8;

        public Field[][] Fields;
        public List<Piece> BlackPieces;
        public List<Piece> WhitePieces;
        public IPlayer WhitePlayer;
        public IPlayer BlackPlayer;
        public IPlayer? ActivePlayer;

        public IPlayer? Winner;
        public EventHandler<IPlayer> OnWinnerAnnouncement;

        private Paths ActivePlayerMoves;

        public Board(IPlayer first, IPlayer second)
        {
            BlackPieces = new List<Piece>();
            WhitePieces = new List<Piece>();
            WhitePlayer = first;
            BlackPlayer = second;
            ActivePlayer = first;

            WhitePlayer.NoPiecesLeft += OnNoPiecesLeft;
            BlackPlayer.NoPiecesLeft += OnNoPiecesLeft;
        }

        private void OnNoPiecesLeft(object? sender, EventArgs e)
        {
            if (sender == WhitePlayer)
            {
                ActivePlayer = null;
                OnWinnerAnnouncement?.Invoke(this, BlackPlayer);
                Dispose();
            }
            else if (sender == BlackPlayer)
            {
                ActivePlayer = null;
                OnWinnerAnnouncement?.Invoke(this, WhitePlayer);
                Dispose();
            }
        }

        public void InitBoard()
        {
            Fields = new Field[BoardSize][];
            for (int i = 0; i < BoardSize; i++)
            {
                Fields[i] = new Field[BoardSize];
                CheckersColor currentColor;
                if (i % 2 == 0)
                    currentColor = CheckersColor.White;
                else
                    currentColor = CheckersColor.Black;
                for (int j = 0; j < BoardSize; j++)
                {
                    Fields[i][j] = new Field(i, j, this, currentColor);
                    if (currentColor == CheckersColor.White)
                        currentColor = CheckersColor.Black;
                    else
                    {
                        currentColor = CheckersColor.White;
                        if ((i == 0 && j % 2 == 1) || (i == 1 && j % 2 == 0))
                        {
                            Fields[i][j].Piece = new Piece(Fields[i][j], CheckersColor.Black, BlackPlayer);
                            Fields[i][j].Piece.OnDestroy += Piece_OnDestroy;
                            BlackPieces.Add(Fields[i][j].Piece);
                        }
                        else if ((i == BoardSize - 1 && j % 2 == 0) || (i == BoardSize - 2 && j % 2 == 1))
                        {
                            Fields[i][j].Piece = new Piece(Fields[i][j], CheckersColor.White, WhitePlayer);
                            Fields[i][j].Piece.OnDestroy += Piece_OnDestroy;
                            WhitePieces.Add(Fields[i][j].Piece);
                        }
                    }
                }
            }
        }

        private void Piece_OnDestroy(object? sender, Piece e)
        {
            if(e.Color == CheckersColor.White)
            {
                WhitePieces.Remove(e);
                WhitePlayer.PiecesLeft--;
            }         
            else if(e.Color == CheckersColor.Black)
            {
                BlackPieces.Remove(e);
                BlackPlayer.PiecesLeft--;
            }     
        }

        public void MovePiece(Piece piece, Stack<Move> moves)
        {
            List<Stack<Move>> legalMoves = ActivePlayerMoves.FoundPaths[piece]; //TODO EXTEND MOVE SECURITY
            if (legalMoves.Contains(moves))
            {
                while (moves.Count > 0)
                {
                    Move poppedMove = moves.Pop();
                    poppedMove.StartField.Piece = null;
                    poppedMove.DestinationField.Piece = piece;
                    if (poppedMove.AttackedField != null)
                    {
                        poppedMove.AttackedField.Piece = null;
                    }
                    piece.Field = poppedMove.DestinationField;
                }
            }      
        }

        public Paths FindPossibleMoves(IPlayer player)
        {
            List<Piece> checkedList;
            if(player.Color == CheckersColor.White)
            {
                checkedList = WhitePieces;
            }
            else
            {
                checkedList = BlackPieces;
            }
            Paths paths = new Paths();
            bool foundAttack = false;
            foreach(Piece piece in checkedList)
            {
                List<Stack<Move>> possibleAttacks = piece.FindAttacks();
                if(possibleAttacks != null)
                {
                    piece.MustAttack = true;
                    paths.FoundPaths.Add(piece, possibleAttacks);
                    foundAttack = true;
                }
                else if(!foundAttack)
                {
                    piece.MustAttack = false;
                    List<Stack<Move>> possibleMoves = piece.FindMoves();
                    paths.FoundPaths.Add(piece, possibleMoves);
                }
            }
            if (foundAttack)
            {
                Piece[] pieces = paths.FoundPaths.Keys.ToArray();
                for(int i = 0; i < pieces.Length; i++)
                {
                    if (!pieces[i].MustAttack)
                    {
                        paths.FoundPaths.Remove(pieces[i]);
                    }
                }
            }
            ActivePlayerMoves = paths;
            return paths;
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
            sb.Append(new String('-', 4 * BoardSize + 1));
            sb.AppendLine();
            for (int i = 0; i < BoardSize; i++)
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

        public void Dispose()
        {
            Fields = null;
            ActivePlayerMoves = null;
        }
    }
}
