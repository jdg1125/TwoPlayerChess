using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Pawn : Piece
    {
        public int TimesMoved { get; private set; }
        public Pawn(GameColors color, PieceType name, Player owner, Board board)
        {
            Color = color;
            Name = name;
            Owner = owner;
            TimesMoved = 0;
            Board = board;
        }

        public override bool TryMove(Move move)  //by the time the move gets here, we're sure it's within board bounds
        {
            bool tookTwoSteps;
            bool isValid = IsRankValid(move, out tookTwoSteps) && IsFileValid(move, tookTwoSteps);
            if (isValid)
            {
                TimesMoved++;
            }
            return isValid;
        }

        private bool IsRankValid(Move move, out bool tookTwoSteps)
        {
            if (TimesMoved == 0)
            {
                bool white = Owner.Color == GameColors.White && (move.StartRank - move.EndRank <= 2);
                bool black = Owner.Color == GameColors.Black && (move.EndRank - move.StartRank <= 2);
                tookTwoSteps = true;
                return white || black;
            }
            else
            {
                bool white = Owner.Color == GameColors.White && (move.StartRank - move.EndRank <= 1);
                bool black = Owner.Color == GameColors.Black && (move.EndRank - move.StartRank <= 1);
                tookTwoSteps = false;
                return white || black;
            }
        }

        private bool IsFileValid(Move move, bool tookTwoSteps)
        {
            if(tookTwoSteps)
            {
                if(move.StartFile == move.EndFile)  //can only take forward leaps
                {
                    if(this.Color == GameColors.White) //spaces moving to and over must be empty
                    {
                        return Board.Pieces[move.StartRank - 1][move.StartFile] == null && Board.Pieces[move.StartRank - 2][move.StartFile] == null;
                    }
                    else   //spaces moving to and over must be empty
                    {
                        return Board.Pieces[move.StartRank + 1][move.StartFile] == null && Board.Pieces[move.StartRank + 2][move.StartFile] == null;
                    }
                }
                return false;
            }
            else
            {
                bool triedCapture = (move.EndFile == move.StartFile - 1) || (move.EndFile == move.StartFile + 1);
                if(!triedCapture)
                {
                    return move.StartFile == move.EndFile && Board.Pieces[move.EndRank][move.EndFile] == null;  //moved forward one square - nobody there
                }
                else
                {
                    //TO DO: handle en passant 
                    var piece = Board.Pieces[move.EndRank][move.EndFile];
                    return piece != null && piece.Color != this.Color;
                }
            }
        }
    }
}
