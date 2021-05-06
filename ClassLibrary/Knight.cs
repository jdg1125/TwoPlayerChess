﻿using System;
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
        }

        public override bool IsMoveLegal(Move move)
        {
            Piece target = Board.Pieces[move.EndRank][move.EndFile];
            
            if (target != null && target.Color == Color)
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
    }
}
