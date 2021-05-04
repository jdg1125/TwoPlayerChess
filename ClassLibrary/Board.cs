using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Board
    {
        public const int TotalRanks = 8;
        public const int TotalFiles = 8;

        private Piece[][] board;

        public Board(Player[] players)
        {
            board = new Piece[TotalRanks][];
            for (int i = 0; i < TotalRanks; i++)
            {
                board[i] = new Piece[TotalFiles];
            }

            Setup(players);
        }

        private void Setup(Player[] players)
        {
            Player white = players[(int)GameColors.White];
            Player black = players[(int)(GameColors.White) ^ 1];

            AssignPieces(white, true);
            AssignPieces(black, false);
            white.Board = this;
            black.Board = this;
        }

        private void AssignPieces(Player player, bool isWhite)
        {
            int pawnRank = isWhite ? TotalRanks - 2 : 1;

            for (int i = 0; i < TotalFiles; i++)
            {
                var pawn = new Pawn(player.Color, PieceType.Pn, player);
                board[pawnRank][i] = pawn;
                player.Pieces.Add(pawn, new int[2] { pawnRank, i });
            }

            int kingRank = isWhite ? TotalRanks - 1 : 0;

            var rook1 = new Rook(player.Color, PieceType.Rk, player);
            var rook2 = new Rook(player.Color, PieceType.Rk, player);
            var knight1 = new Knight(player.Color, PieceType.Kt, player);
            var knight2 = new Knight(player.Color, PieceType.Kt, player);
            var bishop1 = new Bishop(player.Color, PieceType.Bp, player);
            var bishop2 = new Bishop(player.Color, PieceType.Bp, player);
            var queen = new Queen(player.Color, PieceType.Qn, player);
            var king = new King(player.Color, PieceType.Kg, player);

            board[kingRank][0] = rook1;
            player.Pieces.Add(rook1, new int[2] { kingRank, 0 });

            board[kingRank][7] = rook2;
            player.Pieces.Add(rook2, new int[2] { kingRank, 7 });

            board[kingRank][1] = knight1;
            player.Pieces.Add(knight1, new int[2] { kingRank, 1 });

            board[kingRank][6] = knight2;
            player.Pieces.Add(knight2, new int[2] { kingRank, 6 });

            board[kingRank][2] = bishop1;
            player.Pieces.Add(bishop1, new int[2] { kingRank, 2 });

            board[kingRank][5] = bishop2;
            player.Pieces.Add(bishop2, new int[2] { kingRank, 5 });

            board[kingRank][3] = queen;
            player.Pieces.Add(queen, new int[2] { kingRank, 3 });

            board[kingRank][4] = king;
            player.Pieces.Add(king, new int[2] { kingRank, 4 });
        }

        public void Draw()
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
                    if (board[i-1][j] != null)
                    {
                        sb.Append("|   ");
                        Console.Write(sb.ToString());
                        sb.Clear();
                        Console.ForegroundColor = board[i-1][j].Color == GameColors.White ? ConsoleColor.White : ConsoleColor.Green;
                        Console.Write(board[i-1][j]);
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
            sb.Append("\n\n");
            Console.WriteLine(sb.ToString());

        }
    }
}
