using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Knight : Piece
    {
        public Knight(GameColors color, PieceType name, Player owner, Board board)
        {
            Color = color;
            Name = name;
            Owner = owner;
            Board = board;
        }
}
}
