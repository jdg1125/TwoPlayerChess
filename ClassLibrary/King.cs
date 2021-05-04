using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class King : Piece
    {
        public int TimesMoved { get; set; }
        public bool IsInCheck { get; set; }
        public int TimesInCheck { get; set; }

        public King(GameColors color, PieceType name, Player owner, Board board)
        {
            Color = color;
            Name = name;
            Owner = owner;
            TimesMoved = 0;
            IsInCheck = false;
            TimesInCheck = 0;
            Board = board;
        }
    }
}
