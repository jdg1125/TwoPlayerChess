using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Bishop : Piece
    {
        public Bishop(GameColors color, PieceType name, Player owner)
        {
            Color = color;
            Name = name;
            Owner = owner;
        }
    }
}
