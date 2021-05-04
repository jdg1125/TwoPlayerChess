using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Pawn : Piece 
    {
        public int TimesMoved { get; private set; }
        public Pawn(GameColors color, PieceType name, Player owner)
        {
            Color = color;
            Name = name;
            Owner = owner;
            TimesMoved = 0;
        }
    }
}
