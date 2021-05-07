using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Game
    {
        public Player[] Players { get; private set; }
        public Board Board { get; private set; }

        private const string instructions = "This is a two-player game of Chess. White's pieces are placed at the bottom of the screen.\n\nTo move, type <startRank><startFile> <endRank><endFile>, where a space separates the start and end positions.  \nExample: Black enters 7c 6c and black's 3rd pawn from the left is moved down one rank.  \nPiece names are as follows: P = pawn, Rk = rook, Kt = knight, Bp = bishop, Qn = queen, and Kg = king.\n\nIf a player chooses to forfeit, or draw, enter DRAW in all caps on the player's turn.\nEnter EXIT in all caps to quit the game.\n";

        public int WhoseTurn { get; private set; }

        public Game()
        {
            Players = new Player[2];
            WhoseTurn = 0;
            Players[0] = new Player(GameColors.White);
            Players[1] = new Player(GameColors.Black);
            Board = new Board(Players);
        }

        public Player SwitchPlayers()
        {
            WhoseTurn ^= 1;  //1^1 == 0, 0^1 == 1
            return Players[WhoseTurn];
        }

        public void DrawScreen()
        {
            Console.Clear();
            Console.WriteLine(instructions);
            Board.DisplayBoard();
            Console.WriteLine();

            foreach (var player in Players)
            {
                Console.WriteLine("Color {0}", player.Color);
                foreach (var piece in player.Pieces.Keys)
                {
                    Console.Write("{0}, ({1});   ", piece, player.Pieces[piece][0] + ", " + player.Pieces[piece][1]);
                }
                Console.WriteLine("\n");
            }
        }

        public bool IsGameOver()
        {
            return Players[0].Status != PlayerStatus.Playing || Players[1].Status != PlayerStatus.Playing;
        }

        public Player GetGameWinner()
        {
            if(Players[0].Status == PlayerStatus.None || Players[1].Status == PlayerStatus.None)   //someone exited the game
            {
                return null;
            }
 
            if (Players[0].Status == PlayerStatus.Playing && Players[1].Status != PlayerStatus.Stalemate)  
            {
                return Players[0];
            }
            else if (Players[1].Status == PlayerStatus.Playing && Players[0].Status != PlayerStatus.Stalemate)
            {
                return Players[1];
            }

            return null;  //in the case of stalemate, nobody wins
        }
    }
}
