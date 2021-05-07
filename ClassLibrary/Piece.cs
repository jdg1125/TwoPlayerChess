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

        public Piece(Player owner, Board board)
        {
            Color = owner.Color;
            Owner = owner;
            Board = board;
            TimesMoved = 0;
        }
        public bool HasLegalMoves()                 //determines whether there exists a legal move that will not result in check or checkmate
        {
            var moveList = GetAllPossibleMoves(Owner.Pieces[this]);
            foreach(var move in moveList)
            {
                Piece captured = StageMove(move);
                bool isLegal;
                if(this is King)   //stage move doesn't update the coords of the piece in the player's Piece map
                {
                    isLegal = (this as King).IsInCheck(new int[] { move.EndRank, move.EndFile }) == false;  //pass in the staged postion
                }
                else
                {
                    isLegal = Owner.King.IsInCheck() == false;  //King.IsInCheck() without parameters uses the king's location given in the owner's Piece map
                }
                
                RevertMove(move, captured);
                if(isLegal)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool IsMoveLegal(Move move)          //determines whether the move is one this kind of piece can perform 
        {
            return false;  //implemented by derived classes
        }

        protected virtual List<Move> GetAllPossibleMoves(int[] myPosition)    //returns a list of all positions a piece can access
        {
            return null;   //implemented by derived classes
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
        }    //gets all possible moves for a bishop, rook, queen

        protected List<Move> GetAllShortMoves(int[] myPosition)        //get all possible moves for a king, knight. 
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

        public void ExecuteMove(Move move, Piece victim)
        {
            if (victim != null)
            {
                Capture(victim);
            }

            TimesMoved++;
            Owner.Pieces[this] = new int[2] { move.EndRank, move.EndFile };   
        }
        private void Capture(Piece victim)
        {
            Player opponent = Board.Players[(int)victim.Color];
            opponent.Pieces.Remove(victim);
        }

        public Piece StageMove(Move move)
        {
            var victim = Board.Pieces[move.EndRank][move.EndFile];
            Board.Pieces[move.EndRank][move.EndFile] = this; 
            Board.Pieces[move.StartRank][move.StartFile] = null;

            return victim;
        }

        public void RevertMove(Move move, Piece victim)
        {
            Board.Pieces[move.StartRank][move.StartFile] = this; 
            Board.Pieces[move.EndRank][move.EndFile] = victim;
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
