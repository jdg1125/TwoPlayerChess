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
        public Game Game { get; set; }

        public Board(Game game)
        {
            Pieces = new Piece[TotalRanks][];
            for (int i = 0; i < TotalRanks; i++)
            {
                Pieces[i] = new Piece[TotalFiles];
            }

            Players = game.Players;
            Game = game;

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
                var pawn = new Pawn(player.Color, PieceType.Pn, player, this);
                Pieces[pawnRank][i] = pawn;
                player.Pieces.Add(pawn, new int[2] { pawnRank, i });
            }

            int kingRank = isWhite ? TotalRanks - 1 : 0;

            var rook1 = new Rook(player.Color, PieceType.Rk, player, this);
            var rook2 = new Rook(player.Color, PieceType.Rk, player, this);
            var knight1 = new Knight(player.Color, PieceType.Kt, player, this);
            var knight2 = new Knight(player.Color, PieceType.Kt, player, this);
            var bishop1 = new Bishop(player.Color, PieceType.Bp, player, this);
            var bishop2 = new Bishop(player.Color, PieceType.Bp, player, this);
            var queen = new Queen(player.Color, PieceType.Qn, player, this);
            var king = new King(player.Color, PieceType.Kg, player, this);

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

        public bool IsLegalMove(Move move)
        {
            var piece = Pieces[move.StartRank][move.StartFile];
            if(piece == null || piece.Color != Players[Game.WhoseTurn].Color)  //you can only move your own piece
            {
                return false;
            }
            return piece.TryMove(move);
        }

        public void ExecuteMove(Move move)
        {
            var moved = Pieces[move.StartRank][move.StartFile];
            var captured = Pieces[move.EndRank][move.EndFile];
            
            Pieces[move.EndRank][move.EndFile] = Pieces[move.StartRank][move.StartFile];
            Pieces[move.StartRank][move.StartFile] = null;

            if(captured != null)
            {
                moved.Capture(captured);
            }

            Players[(int)moved.Color].Pieces[moved] = new int[2] { move.EndRank, move.EndFile };
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
            sb.AppendFormat("\n\n\t White in check: {0}. Black in check: {1}", Players[0].King.IsInCheck, Players[1].King.IsInCheck);
            sb.Append("\n\n");
            Console.WriteLine(sb.ToString());

        }
    }
}
