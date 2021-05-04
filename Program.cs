using System;
using TwoPlayerChess.ClassLibrary;

namespace TwoPlayerChess
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            game.DrawScreen();
            Console.ReadLine();
        }
    }
}
