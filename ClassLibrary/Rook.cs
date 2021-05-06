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
        }

        public override bool IsMoveLegal(Move move)
        {
            bool alongRank = move.StartRank == move.EndRank;
            bool alongFile = move.StartFile == move.EndFile;
            bool isValid = false;

            if (Board.Pieces[move.EndRank][move.EndFile] != null && Board.Pieces[move.EndRank][move.EndFile].Color == Color)
            {
                return false;
            }

            if (alongRank)
            {
                if(move.EndFile > move.StartFile)
                {
                    isValid = TestPath(move, move.StartRank, move.StartFile + 1, new int[] { 0, 1 });
                }
                else
                {
                    isValid = TestPath(move, move.StartRank, move.StartFile - 1, new int[] { 0, -1 });
                }
                
            }
            else if(alongFile)
            {
                if(move.EndRank > move.StartRank)
                {
                    isValid = TestPath(move, move.StartRank + 1, move.StartFile, new int[] { 1, 0 });
                }
                else
                {
                    isValid = TestPath(move, move.StartRank - 1, move.StartFile, new int[] { -1, 0 });
                }
            }

            if(isValid)
            {
                TimesMoved++;
            }
            return isValid;
        }

        private bool TestPath(Move move, int rank, int file, int[] direction)
        {
            bool isEmpty = false;

            if(direction[1] == 0)
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
                if(file == move.EndFile)
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
