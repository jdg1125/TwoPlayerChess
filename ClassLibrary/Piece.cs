using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Piece
    {
        public Player Owner { get; protected set; }

        public Board Board { get; protected set; }

        public GameColors Color { get; protected set; }
        public PieceType Name { get; protected set; }

        public virtual bool HasLegalMoves()
        {
            return false;
        }

        public virtual bool TryMove(Move move)
        {
            return false;
        }

        public virtual bool CanBreakCheck()
        {
            return false;
        }

        public void Capture(Piece victim)
        {

        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }

    public enum PieceType {
        Pn,
        Rk, 
        Kt,
        Bp,
        Qn, 
        Kg
    }
}
