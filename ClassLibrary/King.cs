using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class King : Piece
    {
        public int TimesMoved { get; private set; }

        public King(GameColors color, PieceType name, Player owner)
        {
            Color = color;
            Name = name;
            Owner = owner;
            TimesMoved = 0;
        }
    }
}
