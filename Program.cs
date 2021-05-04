using System;
using TwoPlayerChess.ClassLibrary;

namespace TwoPlayerChess
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            var player = game.Players[game.WhoseTurn];

            while(!game.IsGameOver())
            {
                game.DrawScreen();
                player.Play();
                player = game.SwitchPlayers();
            }

            Player winner = game.GetGameWinner();
            if(winner == null)
            {
                Console.WriteLine("Game over. The game was a draw");
            }
            else
            {
                Console.WriteLine("Game over. {0} won.", winner.Color);
            }
        }
    }
}
