using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Bishop : Piece
    {
        public Bishop(GameColors color, PieceType name, Player owner, Board board)
        {
            Color = color;
            Name = name;
            Owner = owner;
            Board = board;
        }

        public override bool TryMove(Move move)
        {
            if (Board.Pieces[move.EndRank][move.EndFile] != null && Board.Pieces[move.EndRank][move.EndFile].Color == Color)
            {
                return false;
            }

            if (move.EndRank > move.StartRank)
            {
                if (move.EndFile > move.StartFile)
                {
                    return TestPath(move, move.StartRank + 1, move.StartFile + 1, BishopPathOptions.IncXIncY);
                }
                else if (move.EndFile < move.StartFile)
                {
                    return TestPath(move, move.StartRank + 1, move.StartFile - 1, BishopPathOptions.IncXDecY);
                }
            }
            else if (move.EndRank < move.StartRank)
            {
                if (move.EndFile > move.StartFile)
                {
                    return TestPath(move, move.StartRank - 1, move.StartFile + 1, BishopPathOptions.DecXIncY);
                }
                else if (move.EndFile < move.StartFile)
                {
                    return TestPath(move, move.StartRank - 1, move.StartFile - 1, BishopPathOptions.DecXDecY);
                }
            }

            return false;
        }

        private bool TestPath(Move move, int rank, int file, BishopPathOptions trajectory)
        {
            if (rank == move.EndRank || file == move.EndFile)
            {
                return rank == move.EndRank && file == move.EndFile;
            }
            
            bool isEmpty = Board.Pieces[rank][file] == null;

            rank = trajectory == BishopPathOptions.IncXIncY || trajectory == BishopPathOptions.IncXDecY ? rank + 1 : rank - 1;
            file = trajectory == BishopPathOptions.IncXIncY || trajectory == BishopPathOptions.DecXIncY ? file + 1 : file - 1;

            return isEmpty && TestPath(move, rank, file, trajectory);
        }
    }

    enum BishopPathOptions
    {
        IncXIncY,
        IncXDecY,
        DecXIncY,
        DecXDecY
    }
}
