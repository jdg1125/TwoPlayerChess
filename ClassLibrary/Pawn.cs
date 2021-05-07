using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Pawn : Piece
    {
        private int[][] captureMoves;
        public Pawn(Player owner, Board board) : base(owner, board)
        {
            Name = PieceType.Pn;       

            if (Color == GameColors.White)
            {
                Directions = new int[][] { new int[2] { -1, 0 } };
                captureMoves = new int[2][] { new int[2] { -1, -1 }, new int[2] { -1, 1 } };
            }
            else
            {
                Directions = new int[1][] { new int[2] { 1, 0 } };
                captureMoves = new int[2][] { new int[2] { 1, -1 }, new int[2] { 1, 1 } };
            }
        }

        public override bool IsMoveLegal(Move move) 
        {
            Piece target = Board.Pieces[move.EndRank][move.EndFile];

            if (target != null && (target.Color == Color || target is King))
            {
                return false;
            }

            bool tookTwoSteps;
            bool isLegal = IsRankValid(move, out tookTwoSteps) && IsFileValid(move, tookTwoSteps);
            return isLegal;
        }

        protected override List<Move> GetAllPossibleMoves(int[] myPosition)
        {
            List<Move> allMoves = new List<Move>() ;

            int endRank = myPosition[0] + Directions[0][0];   //test for a single step 
            int endFile = myPosition[1] + Directions[0][1];

            var piece = Board.Pieces[endRank][endFile];
            bool isEmpty = piece == null;

            if (isEmpty)
            {
                allMoves.Add(new Move(myPosition[0], myPosition[1], endRank, endFile));
            }

            if (isEmpty && TimesMoved == 0)  // Here, test for the double step 
            {
                endRank += Directions[0][0];    //take another step and check if it's empty
                endFile += Directions[0][1];   
                
                piece = Board.Pieces[endRank][endFile];
                if (isEmpty && piece == null)
                {
                    allMoves.Add(new Move(myPosition[0], myPosition[1], endRank, endFile));
                }
            }
            
            foreach (var move in captureMoves)  //Here, test if we can perform any diagonal captures
            {
                endRank = myPosition[0] + move[0];
                endFile = myPosition[1] + move[1];
                if (endRank < Board.TotalRanks && endRank >= 0 && endFile < Board.TotalFiles && endFile >= 0)
                {
                    piece = Board.Pieces[endRank][endFile];
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
                bool white = Color == GameColors.White && (move.StartRank - move.EndRank <= 2) && (move.StartRank - move.EndRank > 0);
                bool black = Color == GameColors.Black && (move.EndRank - move.StartRank <= 2) && (move.EndRank - move.StartRank > 0);

                tookTwoSteps = (white && move.StartRank - move.EndRank == 2) || (black && move.EndRank - move.StartRank == 2);
                return white || black;
            }
            else
            {
                bool white = Color == GameColors.White && (move.StartRank - move.EndRank <= 1) && (move.StartRank - move.EndRank > 0);
                bool black = Color == GameColors.Black && (move.EndRank - move.StartRank <= 1) && (move.EndRank - move.StartRank > 0);
                tookTwoSteps = false;
                return white || black;
            }
        }

        private bool IsFileValid(Move move, bool tookTwoSteps)
        {
            if (tookTwoSteps)
            {
                if (move.StartFile == move.EndFile)  
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
                    return move.StartFile == move.EndFile && Board.Pieces[move.EndRank][move.EndFile] == null;  //true if moved the pawn forward one to an empty square
                }
                else
                { 
                    var piece = Board.Pieces[move.EndRank][move.EndFile];
                    return piece != null && piece.Color != this.Color;
                }
            }
        }
    }
}
