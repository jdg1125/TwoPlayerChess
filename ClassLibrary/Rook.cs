using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Rook : Piece
    {
        public Rook(Player owner, Board board)
        {
            Color = owner.Color;
            Name = PieceType.Rk;
            Owner = owner;
            TimesMoved = 0;
            Board = board;
            Directions = new int[4][] { new int[2] { 1, 0 }, new int[2] { -1, 0 }, new int[2] { 0, 1 }, new int[2] { 0, -1 } };
        }

        public override bool IsMoveLegal(Move move)
        {
            Piece target = Board.Pieces[move.EndRank][move.EndFile];

            if (target != null && (target.Color == Color || target is King))
            {
                return false;
            }
            
            bool alongRank = move.StartRank == move.EndRank;
            bool alongFile = move.StartFile == move.EndFile;
            bool isValid = false;

            if (alongRank)
            {
                if (move.EndFile > move.StartFile)
                {
                    isValid = TestPath(move, move.StartRank, move.StartFile + 1, new int[] { 0, 1 });
                }
                else
                {
                    isValid = TestPath(move, move.StartRank, move.StartFile - 1, new int[] { 0, -1 });
                }

            }
            else if (alongFile)
            {
                if (move.EndRank > move.StartRank)
                {
                    isValid = TestPath(move, move.StartRank + 1, move.StartFile, new int[] { 1, 0 });
                }
                else
                {
                    isValid = TestPath(move, move.StartRank - 1, move.StartFile, new int[] { -1, 0 });
                }
            }

            return isValid;
        }

        protected override List<Move> GetAllPossibleMoves(int[] myPosition)
        {
            return GetAllRangeMoves(myPosition);
        }
        private bool TestPath(Move move, int rank, int file, int[] direction)
        {
            bool isEmpty = false;

            if (direction[1] == 0)
            {
                if (rank == move.EndRank)
                {
                    return true;
                }
                else
                {
                    isEmpty = Board.Pieces[rank][move.StartFile] == null;
                }
            }
            else
            {
                if (file == move.EndFile)
                {
                    return true;
                }
                else
                {
                    isEmpty = Board.Pieces[move.StartRank][file] == null;
                }
            }

            rank += direction[0];
            file += direction[1];
            return isEmpty && TestPath(move, rank, file, direction);
        }
    }
}
