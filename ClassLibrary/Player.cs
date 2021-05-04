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

        private bool IsInCheck()
        {
            return false;
        }

        private bool AbleToMove()
        {
            return true;
        }

        private bool AttemptMove(int[] startPos, int[] endPos)
        {
            return true;
        }

        private bool TryParseInput(string input, out int[][] move)
        {
            move = new int[2][];
            return true;
        }

        public void RemovePiece(Piece victim)
        {
            if(Pieces.ContainsKey(victim)) {
                Pieces.Remove(victim);
            }
        }
        public PlayerStatus Play()
        {
            return PlayerStatus.Playing;
        }


        public Player(GameColors color)
        {
            Color = color;
            Pieces = new Dictionary<Piece, int[]>(16);
        }
    }

    public enum PlayerStatus {
        Playing, 
        Checkmate,
        Stalemate,
        Draw
    }

    public enum GameColors
    {
        White,
        Green
    }
}
