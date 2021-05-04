using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Rook : Piece
    {
        public int TimesMoved { get; private set; }
        public Rook(GameColors color, PieceType name, Player owner)
        {
            Color = color;
            Name = name;
            Owner = owner;
            TimesMoved = 0;
        }
    }
}
