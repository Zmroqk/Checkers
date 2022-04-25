using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine.Players
{
    public interface IPlayer
    {
        short PiecesLeft { get; set; }
        double Score { get; set; }
        CheckersColor Color { get; set; }

        event EventHandler NoPiecesLeft;
        Stack<Move> MakeMove(Paths offers);
    }
}
