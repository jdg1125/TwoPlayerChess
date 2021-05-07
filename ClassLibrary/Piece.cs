using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Piece
    {
        public Player Owner { get; protected set; }

        public int TimesMoved { get; set; }
        public Board Board { get; protected set; }

        public int[][] Directions { get; set; }

        public GameColors Color { get; protected set; }
        public PieceType Name { get; protected set; }

        public bool HasLegalMoves()                 //determines whether there exists a legal move that will not result in check or checkmate
        {
            var moveList = GetAllPossibleMoves(Owner.Pieces[this]);
            foreach(var move in moveList)
            {
                Piece captured = Board.StageMove(move);
                bool isLegal;
                if(this is King)   //stage move doesn't update the coords of the piece in the player's Piece map
                {
                    isLegal = Board.Players[(int)Color].King.IsInCheck(new int[] { move.EndRank, move.EndFile }) == false;  //pass in the staged postion
                }
                else
                {
                    isLegal = Board.Players[(int)Color].King.IsInCheck() == false;  //King.IsInCheck() without parameters uses the king's location given in the player's Piece map
                }
                
                Board.RevertMove(move, captured);
                if(isLegal)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool IsMoveLegal(Move move)          //determines whether the move is one this kind of piece can perform 
        {
            return false;
        }

        protected virtual List<Move> GetAllPossibleMoves(int[] myPosition)    //returns a list of all positions a piece can access
        {
            return null;
        }
        protected List<Move> GetAllRangeMoves(int[] myPosition)
        {
            var allMoves = new List<Move>();

            for (int i = 0; i < Directions.Length; i++)
            {
                int multiplier = 1;
                Piece piece = null;

                while (piece == null)
                {
                    int endRank = myPosition[0] + Directions[i][0] * multiplier;
                    int endFile = myPosition[1] + Directions[i][1] * multiplier;

                    if (endRank == Board.TotalRanks || endRank == -1 || endFile == Board.TotalFiles || endFile == -1)
                    {
                        break;
                    }
                    piece = Board.Pieces[endRank][endFile];

                    if (piece == null || (piece.Color != Color && !(piece is King)))  //the square is empty or it's a piece we can capture
                    {
                        allMoves.Add(new Move(myPosition[0], myPosition[1], endRank, endFile));
                    }
                    multiplier++;
                }
            }
            return allMoves;
        }

        protected List<Move> GetAllShortMoves(int[] myPosition)
        {
            var allMoves = new List<Move>();

            for (int i = 0; i < Directions.Length; i++)
            {
                int endRank = myPosition[0] + Directions[i][0];
                int endFile = myPosition[1] + Directions[i][1];

                if (endRank < Board.TotalRanks && endRank >= 0 && endFile < Board.TotalFiles && endFile >= 0)
                {
                    var piece = Board.Pieces[endRank][endFile];
                    if (piece == null || (piece.Color != Color && !(piece is King)))  //the square is empty or it's a piece we can capture
                    {
                        allMoves.Add(new Move(myPosition[0], myPosition[1], endRank, endFile));
                    }
                }
            }
            return allMoves;
        }

        public void Capture(Piece victim)
        {
            Player opponent = Board.Players[(int)victim.Color];
            opponent.Pieces.Remove(victim);
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }

    public enum PieceType {
        Pn,
        Rk, 
        Kt,
        Bp,
        Qn, 
        Kg
    }
}
