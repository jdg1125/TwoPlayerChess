using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Knight : Piece
    {
        public Knight(Player owner, Board board)
        {
            Color = owner.Color;
            Name = PieceType.Kt;
            Owner = owner;
            Board = board;
            Directions = new int[8][] { new int[2] { -2, -1}, new int[2] { -2, 1 }, new int[2] { -1, -2 }, new int[2] { -1, 2 },
                new int[2] { 1, -2 }, new int[2] { 1, 2}, new int[2] { 2, -1}, new int[2] { 2, 1} };
        }

        public override bool IsMoveLegal(Move move)
        {
            Piece target = Board.Pieces[move.EndRank][move.EndFile];
            
            if (target != null && (target.Color == Color || target is King))
            {
                return false;
            }

            int rankDiff = move.StartRank - move.EndRank;
            int fileDiff = move.StartFile - move.EndFile;

            if(rankDiff == 1 || rankDiff == -1)
            {
                return fileDiff == 2 || fileDiff == -2;
            }
            else if(rankDiff == 2 || rankDiff == -2)
            {
                return fileDiff == 1 || fileDiff == -1; ;
            }

            return false;
        }

        protected override List<Move> GetAllPossibleMoves(int[] position)
        {
            return GetAllShortMoves(position);
        }
    }
}
