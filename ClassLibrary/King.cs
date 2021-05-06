using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class King : Piece
    {
        public bool Check { get; set; }
        public int TimesInCheck { get; set; }

        public King(Player owner, Board board)
        {
            Color = owner.Color;
            Name = PieceType.Kg;
            Owner = owner;
            TimesMoved = 0;
            Check = false;
            TimesInCheck = 0;
            Board = board;
        }

        public bool IsInCheck(bool changeCheckStatus)
        {
            int[] location = Owner.Pieces[this];
            
            bool testForCheck = CheckForKnights(location) || CheckForPawns(location) || CheckForBishops(location) || CheckForRooks(location);
            
            if (changeCheckStatus) //false if this method is called when we are trying out a move before committing to it
            {
                if (!Check && testForCheck)
                {
                    Check = true;
                    TimesInCheck++;
                }
                else if (Check && !testForCheck)
                {
                    Check = false;
                }
            }
            return testForCheck;
        }

        private bool CheckForKnights(int[] location)
        {
            int[] offsets = new int[4] { -2, -1, 2, 1 };
            for(int i=0; i<offsets.Length; i++)
            {
                for(int j=0; j<offsets.Length; j++)
                {
                    if((offsets[i] & 1) == (offsets[j] & 1)) //skip the combination if abs val of offsets[i] == offsets[j]
                    {
                        continue;
                    }
                    bool rankInBounds = location[0] + offsets[i] >= 0 && location[0] + offsets[i] < Board.TotalRanks;
                    bool fileInBounds = location[1] + offsets[j] >= 0 && location[1] + offsets[j] < Board.TotalFiles;

                    if(rankInBounds && fileInBounds)
                    {
                        var piece = Board.Pieces[location[0] + offsets[i]][location[1] + offsets[j]];
                        if(piece is Knight && piece.Color != Color)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        
        private bool CheckForPawns(int[] location)
        {
            int[] leftAttack, rightAttack;
            
            if(Color == GameColors.White)
            {
                leftAttack = new int[2] { location[0] - 1, location[1] - 1 };  
                rightAttack = new int[2] { location[0] - 1, location[1] + 1 };
            }
            else
            {
                leftAttack = new int[2] { location[0] + 1, location[1] + 1 };
                rightAttack = new int[2] { location[0] + 1, location[1] - 1 };
            }

            bool leftInBounds = (leftAttack[0] >= 0 && leftAttack[0] < Board.TotalRanks) && (leftAttack[1] >= 0 && leftAttack[1] < Board.TotalFiles);
            bool rightInBounds = (rightAttack[0] >= 0 && rightAttack[0] < Board.TotalRanks) && (rightAttack[1] >= 0 && rightAttack[1] < Board.TotalFiles);

            if(leftInBounds)
            {
                var piece = Board.Pieces[leftAttack[0]][leftAttack[1]];
                if(piece is Pawn && Owner.Color != piece.Color)
                {
                    return true;
                }
            }
            if (rightInBounds)
            {
                var piece = Board.Pieces[rightAttack[0]][rightAttack[1]];
                if (piece is Pawn && Owner.Color != piece.Color)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckForBishops(int[] location)
        {
            bool path1 = BishopCheckHelper(location[0], location[1], new int[2] { 1, 1 });
            bool path2 = BishopCheckHelper(location[0], location[1], new int[2] { 1, -1 });
            bool path3 = BishopCheckHelper(location[0], location[1], new int[2] { -1, 1 });
            bool path4 = BishopCheckHelper(location[0], location[1], new int[2] { -1, -1 });

            return path1 || path2 || path3 || path4;
        }

        private bool BishopCheckHelper(int rank, int file, int[] direction)
        {
            if(rank == Board.TotalRanks || file == Board.TotalFiles)
            {
                return false;
            }

            var piece = Board.Pieces[rank][file];

            if (piece != null && (piece is Bishop || piece is Queen) && piece.Color != Color) 
            {
                return true;
            }
            else if(piece != null)
            {
                return false;
            }

            rank += direction[0];
            file += direction[1];

            return BishopCheckHelper(rank, file, direction);
        }

        private bool CheckForRooks(int[] location)
        {
            bool path1 = RookCheckHelper(location[0] + 1, location[1], new int[2] { 1, 0 });
            bool path2 = RookCheckHelper(location[0], location[1] + 1, new int[2] { 0, 1 });
            bool path3 = RookCheckHelper(location[0] - 1, location[1], new int[2] { -1, 0 });
            bool path4 = RookCheckHelper(location[0], location[1] - 1, new int[2] { 0, -1 });

            return path1 || path2 || path3 || path4;
        }

        private bool RookCheckHelper(int rank, int file, int[] direction)
        {
            if (rank == Board.TotalRanks || file == Board.TotalFiles)
            {
                return false;
            }

            var piece = Board.Pieces[rank][file];

            if (piece != null && (piece is Rook || piece is Queen) && piece.Color != Color)
            {
                return true;
            }
            else if (piece != null)
            {
                return false;
            }

            rank += direction[0];
            file += direction[1];

            return RookCheckHelper(rank, file, direction);
        }
    }
}
