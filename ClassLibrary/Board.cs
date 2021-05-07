using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Board
    {
        public const int TotalRanks = 8;
        public const int TotalFiles = 8;
        public Player[] Players;
        public Piece[][] Pieces { get; set; }

        public Board(Player[] players)
        {
            Pieces = new Piece[TotalRanks][];
            for (int i = 0; i < TotalRanks; i++)
            {
                Pieces[i] = new Piece[TotalFiles];
            }

            Players = players;

            Setup();
        }

        private void Setup()
        {
            AssignPieces(Players[0], true);
            AssignPieces(Players[1], false);
            Players[0].Board = this;
            Players[1].Board = this;
        }

        private void AssignPieces(Player player, bool isWhite)
        {
            int pawnRank = isWhite ? TotalRanks - 2 : 1;

            for (int i = 0; i < TotalFiles; i++)
            {
                var pawn = new Pawn(player, this);
                Pieces[pawnRank][i] = pawn;
                player.Pieces.Add(pawn, new int[2] { pawnRank, i });
            }

            int kingRank = isWhite ? TotalRanks - 1 : 0;

            var rook1 = new Rook(player, this);
            var rook2 = new Rook(player, this);
            var knight1 = new Knight(player, this);
            var knight2 = new Knight(player, this);
            var bishop1 = new Bishop(player, this);
            var bishop2 = new Bishop(player, this);
            var queen = new Queen(player, this);
            var king = new King(player, this);

            Pieces[kingRank][0] = rook1;
            player.Pieces.Add(rook1, new int[2] { kingRank, 0 });

            Pieces[kingRank][7] = rook2;
            player.Pieces.Add(rook2, new int[2] { kingRank, 7 });

            Pieces[kingRank][1] = knight1;
            player.Pieces.Add(knight1, new int[2] { kingRank, 1 });

            Pieces[kingRank][6] = knight2;
            player.Pieces.Add(knight2, new int[2] { kingRank, 6 });

            Pieces[kingRank][2] = bishop1;
            player.Pieces.Add(bishop1, new int[2] { kingRank, 2 });

            Pieces[kingRank][5] = bishop2;
            player.Pieces.Add(bishop2, new int[2] { kingRank, 5 });

            Pieces[kingRank][3] = queen;
            player.Pieces.Add(queen, new int[2] { kingRank, 3 });

            Pieces[kingRank][4] = king;
            player.Pieces.Add(king, new int[2] { kingRank, 4 });
            player.King = king;
        }

       
        public void DisplayBoard()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\t");
            for (int j = 0; j < TotalFiles; j++)
            {
                sb.Append("--------");
            }

            Console.WriteLine(sb.ToString());
            sb.Clear();

            for (int i = TotalRanks; i > 0; i--)
            {
                sb.AppendFormat("{0}\t", i);
                for (int j = 0; j < TotalFiles; j++)
                {
                    if (Pieces[TotalRanks - i][j] != null)
                    {
                        sb.Append("|   ");
                        Console.Write(sb.ToString());
                        sb.Clear();
                        Console.ForegroundColor = Pieces[TotalRanks - i][j].Color == GameColors.White ? ConsoleColor.White : ConsoleColor.Green;
                        Console.Write(Pieces[TotalRanks-i][j]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        sb.Append("\t");
                    }
                    else
                    {
                        sb.Append("|\t");
                    }
                }
                sb.Append("|\n\t");
                for (int j = 0; j < TotalFiles; j++)
                {
                    sb.Append("--------");
                }
                Console.WriteLine(sb.ToString());
                sb.Clear();
            }
            sb.Append("\t");
            for (int i = 0; i < TotalFiles; i++)
            {
                sb.AppendFormat("    {0}\t", (char)('a' + i));
            }

            Console.WriteLine(sb.ToString());
        }
    }
}
