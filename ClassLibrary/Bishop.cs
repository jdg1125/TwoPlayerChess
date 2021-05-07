using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Bishop : Piece
    {
        public Bishop(Player owner, Board board)
        {
            Color = owner.Color;
            Owner = owner;
            Board = board;
            Name = PieceType.Bp;
            Directions = new int[4][] { new int[2] { 1, 1 }, new int[2] { 1, -1 }, new int[2] { -1, 1 }, new int[2] { -1, -1 } };
        }

        public override bool IsMoveLegal(Move move)
        {
            Piece target = Board.Pieces[move.EndRank][move.EndFile];

            if (target != null && (target.Color == Color || target is King))
            {
                return false;
            }

            if (move.EndRank > move.StartRank)
            {
                if (move.EndFile > move.StartFile)
                {
                    return TestPath(move, move.StartRank + 1, move.StartFile + 1, new int[] { 1, 1 });
                }
                else if (move.EndFile < move.StartFile)
                {
                    return TestPath(move, move.StartRank + 1, move.StartFile - 1, new int[] { 1, -1 });
                }
            }
            else if (move.EndRank < move.StartRank)
            {
                if (move.EndFile > move.StartFile)
                {
                    return TestPath(move, move.StartRank - 1, move.StartFile + 1, new int[] { -1, 1 });
                }
                else if (move.EndFile < move.StartFile)
                {
                    return TestPath(move, move.StartRank - 1, move.StartFile - 1, new int[] { -1, -1 });
                }
            }

            return false;
        }

        protected override List<Move> GetAllPossibleMoves(int[] myPosition)
        {
            return GetAllRangeMoves(myPosition);
        }

        private bool TestPath(Move move, int rank, int file, int[] direction)
        {
            if (rank == move.EndRank || file == move.EndFile)
            {
                return rank == move.EndRank && file == move.EndFile;
            }
            
            bool isEmpty = Board.Pieces[rank][file] == null;

            rank += direction[0];
            file += direction[1];

            return isEmpty && TestPath(move, rank, file, direction);
        }
    }
}
