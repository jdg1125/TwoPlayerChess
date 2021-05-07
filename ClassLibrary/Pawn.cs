using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Pawn : Piece
    {

        public Pawn(Player owner, Board board)
        {
            Color = owner.Color;
            Name = PieceType.Pn;
            Owner = owner;
            TimesMoved = 0;
            Board = board;

            if (Color == GameColors.White)
            {
                Directions = new int[][] { new int[2] { -1, 0 } };
            }
            else
            {
                Directions = new int[1][] { new int[2] { 1, 0 } };
            }
        }

        public override bool IsMoveLegal(Move move)  //by the time the move gets here, we're sure it's within board bounds
        {
            Piece target = Board.Pieces[move.EndRank][move.EndFile];

            if (target != null && (target.Color == Color || target is King))
            {
                return false;
            }

            bool tookTwoSteps;
            bool isValid = IsRankValid(move, out tookTwoSteps) && IsFileValid(move, tookTwoSteps);
            return isValid;
        }

        protected override List<Move> GetAllPossibleMoves(int[] myPosition)
        {
            List<Move> allMoves = GetAllShortMoves(myPosition);

            if (TimesMoved == 0)
            {
                int[] singleStep = Color == GameColors.White ? new int[2] { -1, 0 } : new int[2] { 1, 0 };
                int endRank = myPosition[0] + singleStep[0];
                int endFile = myPosition[1] + singleStep[1];
                var piece = Board.Pieces[endRank][endFile];
                bool isEmpty = piece == null;
                endRank += singleStep[0];
                endFile += singleStep[1];
                piece = Board.Pieces[endRank][endFile];
                if (isEmpty && piece == null)
                {
                    allMoves.Add(new Move(myPosition[0], myPosition[1], endRank, endFile));
                }
            }
            int[][] captureMoves;
            if (Color == GameColors.White)
            {
                captureMoves = new int[2][] { new int[2] { -1, -1 }, new int[2] { -1, 1 } };
            }
            else
            {
                captureMoves = new int[2][] { new int[2] { 1, -1 }, new int[2] { 1, 1 } };
            }
            foreach (var move in captureMoves)
            {
                int endRank = myPosition[0] + move[0];
                int endFile = myPosition[1] + move[1];
                if (endRank < Board.TotalRanks && endRank >= 0 && endFile < Board.TotalFiles && endFile >= 0)
                {
                    var piece = Board.Pieces[endRank][endFile];
                    if (piece != null && piece.Color != Color && !(piece is King))  //we can capture this piece
                    {
                        allMoves.Add(new Move(myPosition[0], myPosition[1], endRank, endFile));
                    }
                }
            }


            return allMoves;
        }

        private bool IsRankValid(Move move, out bool tookTwoSteps)
        {
            if (move.StartRank == move.EndRank)
            {
                tookTwoSteps = false;
                return false;
            }

            if (TimesMoved == 0)
            {
                bool white = Owner.Color == GameColors.White && (move.StartRank - move.EndRank <= 2) && (move.StartRank - move.EndRank > 0);
                bool black = Owner.Color == GameColors.Black && (move.EndRank - move.StartRank <= 2) && (move.EndRank - move.StartRank > 0);

                if (white)
                {
                    tookTwoSteps = move.StartRank - move.EndRank == 2;
                }
                else if (black)
                {
                    tookTwoSteps = move.EndRank - move.StartRank == 2;
                }
                else
                {
                    tookTwoSteps = false;
                }
                return white || black;
            }
            else
            {
                bool white = Owner.Color == GameColors.White && (move.StartRank - move.EndRank <= 1) && (move.StartRank - move.EndRank > 0);
                bool black = Owner.Color == GameColors.Black && (move.EndRank - move.StartRank <= 1) && (move.EndRank - move.StartRank > 0);
                tookTwoSteps = false;
                return white || black;
            }
        }

        private bool IsFileValid(Move move, bool tookTwoSteps)
        {
            if (tookTwoSteps)
            {
                if (move.StartFile == move.EndFile)  //can only take leaps forward
                {
                    if (this.Color == GameColors.White) //spaces moving to and over must be empty
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
                if (!triedCapture)
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
