using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Rook : Piece
    {
        public int TimesMoved { get; private set; }
        public Rook(GameColors color, PieceType name, Player owner, Board board)
        {
            Color = color;
            Name = name;
            Owner = owner;
            TimesMoved = 0;
            Board = board;
        }

        public override bool TryMove(Move move)
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
                    isValid = TestPath(move, move.StartFile + 1, RookPathOptions.IncY);
                }
                else
                {
                    isValid = TestPath(move, move.StartFile - 1, RookPathOptions.DecY);
                }
                
            }
            else if(alongFile)
            {
                if(move.EndRank > move.StartRank)
                {
                    isValid = TestPath(move, move.StartRank + 1, RookPathOptions.IncX);
                }
                else
                {
                    isValid = TestPath(move, move.StartRank - 1, RookPathOptions.DecX);
                }
            }

            if(isValid)
            {
                TimesMoved++;
            }
            return isValid;
        }

        private bool TestPath(Move move, int index, RookPathOptions trajectory)
        {
            if(trajectory == RookPathOptions.IncX || trajectory == RookPathOptions.DecX)
            {
                if (index == move.EndRank)
                {
                    return true;
                }
                else
                {
                    bool isEmpty = Board.Pieces[index][move.StartFile] == null;
                    index = trajectory == RookPathOptions.IncX ? index + 1 : index - 1;
                    return isEmpty && TestPath(move, index, trajectory);
                }
            }
            else
            {
                if(index == move.EndFile)
                {
                    return true;
                }
                else
                {
                    bool isEmpty = Board.Pieces[move.StartRank][index] == null;
                    index = trajectory == RookPathOptions.IncY ? index + 1 : index - 1;
                    return isEmpty && TestPath(move, index, trajectory);
                }
            }
        }
    }

    enum RookPathOptions
    {
        IncX, 
        DecX, 
        IncY, 
        DecY
    }
}
