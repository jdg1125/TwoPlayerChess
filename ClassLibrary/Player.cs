using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Player
    {
        public King King { get; set; }
        public Dictionary<Piece, int[]> Pieces { get; set; }
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
            return true;
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

            bool wasMoveMade = false;
            while (!wasMoveMade)
            {
                Move move = GetUserInput();
                if (move == null)           //user entered EXIT or DRAW
                {
                    return;
                }

                wasMoveMade = Board.TryToMove(move);

                //Console.WriteLine("in play: {0}, {1}", move[0][0] + ", " + move[0][1], move[1][0] + ", " + move[1][1]);

            }
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
                    else
                    {
                        Console.WriteLine("Invalid move. Try again: ");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again: ");
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
