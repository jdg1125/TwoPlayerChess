using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Queen : Piece
    {
        public Queen(Player owner, Board board) : base(owner, board)
        {
            Name = PieceType.Qn;
            Directions = new int[8][] { new int[2] { 1, 1 }, new int[2] { 1, -1 }, new int[2] { -1, 1 }, new int[2] { -1, -1 }, 
                        new int[2] { 1, 0 }, new int[2] { -1, 0 }, new int[2] { 0, 1 }, new int[2] { 0, -1 } };
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
                Bishop tmpBishop = new Bishop(Owner, Board);  //queen behaving like a bishop
                return tmpBishop.IsMoveLegal(move);
            }
        }

        protected override List<Move> GetAllPossibleMoves(int[] myPosition)
        {
            return GetAllRangeMoves(myPosition);
        }
    }
}
