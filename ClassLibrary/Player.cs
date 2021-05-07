using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Player
    {
        public King King { get; set; }
        public Dictionary<Piece, int[]> Pieces { get; set; }     //maps pieces to their coordinates on the board
        public Board Board { get; set; }
        public GameColors Color { get; private set; }

        public PlayerStatus Status { get; private set; }

        public Player(GameColors color)
        {
            Color = color;
            Pieces = new Dictionary<Piece, int[]>(16);
            Status = PlayerStatus.Playing;
        }

        private bool AbleToMove()
        {
            foreach(var piece in Pieces.Keys)
            {
                bool test = piece.HasLegalMoves();
                int[] coords = Pieces[piece];
                if(test)
                {
                    return true;
                }
            }
            return false;
        }

        public void RemovePiece(Piece victim)
        {
            if (Pieces.ContainsKey(victim))
            {
                Pieces.Remove(victim);
            }
        }
        public void Play()
        {
            bool isInCheck = King.IsInCheck();
            King.UpdateCheckStatus(isInCheck);

            if (isInCheck)
            {
                if (!AbleToMove())
                {
                    Status = PlayerStatus.Checkmate;
                    return;
                }
            }
            else
            {
                if (!AbleToMove())
                {
                    Status = PlayerStatus.Stalemate;
                    return;
                }
            }

            Console.WriteLine("\n\t {0} in check: {1}\n", Color, King.Check);

            while (true)
            {
                Move move = GetUserInput();
                if (move == null)           //user entered EXIT or DRAW
                {
                    return;
                }
                
                if (TryToMove(move))
                {
                    return;
                }
            }
        }

        private bool TryToMove(Move move)
        {
            var piece = Board.Pieces[move.StartRank][move.StartFile];
            if (piece == null || piece.Color != Color)  //you can only move your own piece
            {
                return false;
            }

            if (piece is King)
            {
                return (piece as King).TryToMove(move); //king tests and executes his own moves
            }

            bool isLegal = piece.IsMoveLegal(move);

            if (isLegal)
            {
                Piece captured = piece.StageMove(move);
                isLegal = King.IsInCheck() == false;

                if (isLegal)
                {
                    piece.ExecuteMove(move, captured);
                }
                else
                {
                    piece.RevertMove(move, captured);
                }
            }
            return isLegal;
        }

        private Move GetUserInput()
        {
            string[] positions;
            Move move;

            while (true)
            {
                do
                {
                    Console.Write("{0}'s turn. Specify your move, then press ENTER: ", Color);
                    string input = Console.ReadLine();
                    if (input.Contains("EXIT") || input.Contains("DRAW"))
                    {
                        if (input.Contains("DRAW"))
                        {
                            this.Status = PlayerStatus.Draw;
                        }
                        else
                        {
                            this.Status = PlayerStatus.None;
                        }
                        return null;
                    }
                    positions = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                } while (positions == null || positions.Length != 2);

                if (positions[0].Length == 2 && positions[1].Length == 2)
                {
                    if (TryParseInput(positions, out move))
                    {
                        break;
                    }
                }
            }

            return move;
        }

        private bool TryParseInput(string[] positions, out Move move)
        {
            string startPos = positions[0];
            string endPos = positions[1];
            int startRank, endRank;
            int startFile, endFile;

            bool foundStartRank = Int32.TryParse(startPos[0].ToString(), out startRank);
            bool foundEndRank = Int32.TryParse(endPos[0].ToString(), out endRank);
            foundStartRank = foundEndRank && startRank > 0 && startRank <= Board.TotalRanks;
            foundEndRank = foundEndRank && endRank > 0 && endRank <= Board.TotalRanks;

            startFile = startPos[1] - 'a';
            endFile = endPos[1] - 'a';

            bool foundStartFile = startFile >= 0 && startFile < Board.TotalFiles;
            bool foundEndFile = endFile >= 0 && endFile < Board.TotalFiles;
            bool arePositionsUnique = startRank == endRank ? startFile != endFile : true;

            if (foundStartRank && foundEndRank && foundStartFile && foundEndFile && arePositionsUnique)
            {
                startRank = Board.TotalRanks - startRank;
                endRank = Board.TotalRanks - endRank;
                move = new Move(startRank, startFile, endRank, endFile);
                return true;
            }

            move = null;
            return false;
        }
    }

    public enum PlayerStatus
    {
        None,
        Playing,
        Checkmate,
        Stalemate,
        Draw
    }

    public enum GameColors
    {
        White,
        Black
    }
}
