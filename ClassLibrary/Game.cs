using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Game
    {
        public Player[] Players { get; private set; }
        public Board Board { get; private set; }

        private const string instructions = "This is a two-player game of Chess. White player's pieces are placed at the bottom of the screen.\n\nTo move, type <startRank><startFile> <endRank><endFile>, where a space separates the start and end positions.  \nExample: White enters 2c 3c and white's 3rd pawn from the left is moved up one rank.  \nPiece names are as follows: P = pawn, Rk = rook, Kt = knight, Bp = bishop, Qn = queen, and Kg = king.\n\nIf a player chooses to forfeit, or draw, enter DRAW in all caps on the player's turn.\nEnter EXIT in all caps to quit the game.\n";

        public int WhoseTurn { get; private set; }

        public Game()
        {
            Players = new Player[2];
            WhoseTurn = (int)GameColors.White;
            Players[WhoseTurn] = new Player(GameColors.White);
            Players[WhoseTurn ^ 1] = new Player((GameColors)(WhoseTurn^1));
            Board = new Board(Players);
        }

        public Player SwitchTurns()
        {
            WhoseTurn ^= 1;
            return Players[WhoseTurn];
        }

        public void DrawScreen()
        {
            Console.Clear();
            Console.WriteLine(instructions);
            Board.Draw();
        }

        
    }
}
