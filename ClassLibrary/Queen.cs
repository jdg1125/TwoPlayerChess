using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Queen : Piece
    {
        public Queen(Player owner, Board board)
        {
            Color = owner.Color;
            Owner = owner;
            Board = board;
            Name = PieceType.Qn;
        }

        public override bool IsMoveLegal(Move move)
        {
            if(move.StartRank == move.EndRank || move.StartFile == move.EndFile)  //queen behaving like a rook
            {
                Rook tmpRook = new Rook(Owner, Board);
                return tmpRook.IsMoveLegal(move);
            }
            else
            {
                Bishop tmpBishop = new Bishop(Owner, Board);  //queen behaving like a rook
                return tmpBishop.IsMoveLegal(move);
            }
        }
    }
}
