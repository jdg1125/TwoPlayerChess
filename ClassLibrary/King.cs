using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class King : Piece
    {
        public bool Check { get; set; }
        public int TimesInCheck { get; set; }   

        private int[][] knightLocations;  //used for testing whether king is being attacked by a knight
        private int[][] rookDirections;
        private int[][] bishopDirections;
        public King(Player owner, Board board) : base(owner, board)
        {
            Name = PieceType.Kg;
            Check = false;
            TimesInCheck = 0;
      
            Directions = new int[8][] { new int[2] { 1, 1 }, new int[2] { 1, -1 }, new int[2] { -1, 1 }, new int[2] { -1, -1 },
                        new int[2] { 1, 0 }, new int[2] { -1, 0 }, new int[2] { 0, 1 }, new int[2] { 0, -1 } };

            knightLocations = new int[8][] { new int[2] { -2, -1}, new int[2] { -2, 1 }, new int[2] { -1, -2 }, new int[2] { -1, 2 },
                new int[2] { 1, -2 }, new int[2] { 1, 2}, new int[2] { 2, -1}, new int[2] { 2, 1} };

            rookDirections = new int[4][] { new int[2] { 0, 1 }, new int[2] { 1, 0 }, new int[2] { 0, -1 }, new int[2] { -1, 0 } };

            bishopDirections = new int[4][] { new int[2] { 1, 1 }, new int[2] { 1, -1 }, new int[2] { -1, 1 }, new int[2] { -1, -1 } };
        }
        public bool TryToMove(Move move)
        {
            if(!IsMoveLegal(move))
            {
                return false;
            }
            
            int fileDiff = move.EndFile - move.StartFile;
            if(fileDiff == 2 || fileDiff == -2)
            {
                ExecuteCastle(move);
                return true;
            }
            else
            {
                Piece captured = StageMove(move);
                bool canTakeMove = IsInCheck(new int[2] { move.EndRank, move.EndFile }) == false;

                if (canTakeMove)
                {
                    this.ExecuteMove(move, captured);
                }
                else
                {
                    this.RevertMove(move, captured);
                }

                return canTakeMove;
            }
        }

        private void ExecuteCastle(Move move)
        {
            this.StageMove(move);   //move the king. 
            this.ExecuteMove(move, null);   
            
            int fileDiff = move.EndFile - move.StartFile;
            var rook = fileDiff < 0 ? Board.Pieces[move.StartRank][0] : Board.Pieces[move.StartRank][7];  //find rook to castle with
            int rookEndFile = fileDiff < 0 ? move.EndFile + 1 : move.EndFile - 1;
            var rookMove = new Move(Owner.Pieces[rook][0], Owner.Pieces[rook][1], Owner.Pieces[rook][0], rookEndFile);
            
            rook.StageMove(rookMove);  //move the rook. 
            rook.ExecuteMove(rookMove, null);
        }

      
        public override bool IsMoveLegal(Move move)
        {
            int rankDiff = move.EndRank - move.StartRank;
            int fileDiff = move.EndFile - move.StartFile;

            if ((rankDiff >= -1 && rankDiff <= 1) && (fileDiff >= -1 && fileDiff <= 1)) //check for an ordinary move, 1 step in any direction
            {
                var piece = Board.Pieces[move.EndRank][move.EndFile];
                if (piece != null && piece.Color == Color || piece is King)  //we cannot take our own piece or the opponent's king
                {
                    return false;  
                }
                return true;//IsInCheck(new int[2] { move.EndRank, move.EndFile }) == false;
            }
            else if (rankDiff == 0 && (fileDiff == 2 || fileDiff == -2))  //check if castling is allowed
            {
                return CanCastle(move, fileDiff);
            }
            return false;
        }

        private bool CanCastle(Move move, int fileDiff)
        {
            if (TimesMoved == 0 && TimesInCheck == 0)
            {
                Piece rookToMove = fileDiff < 0 ? Board.Pieces[move.StartRank][0] : Board.Pieces[move.StartRank][7];

                bool validRook = rookToMove != null && rookToMove is Rook && rookToMove.TimesMoved == 0;
                if (!validRook)
                {
                    return false;
                }

                bool isEmpty = IsCastleSideEmpty(move.StartRank, move.StartFile + fileDiff / 2, fileDiff / 2); //  fileDiff/2 gives the direction to check 
                if (isEmpty)
                {
                    int[] testSquare = new int[2] { move.StartRank, move.StartFile + fileDiff / 2 };  //test that the square adjacent to king is not under attack
                    bool tryOneStep = IsInCheck(testSquare);
                    testSquare[1] += fileDiff / 2;      //test that the square two steps away is not under attack
                    return (tryOneStep || IsInCheck(testSquare)) == false;
                }
            }
            return false;
        }

        private bool IsCastleSideEmpty(int rank, int file, int direction)
        {
            if(file == 0 || file == Board.TotalFiles - 1)
            {
                return true;
            }
            Piece test = Board.Pieces[rank][file];

            return test == null && IsCastleSideEmpty(rank, file + direction, direction);
        }  //checks that no pieces are between the king and the rook it's trying to castle with
        public bool IsInCheck()
        {
            int[] myPosition = Owner.Pieces[this];
            bool testForCheck = TestForKnights(myPosition) || TestForPawns(myPosition) || TestForBishops(myPosition) || TestForRooks(myPosition);
            return testForCheck;
        }

        public bool IsInCheck(int[] testPosition)
        {
            bool testForCheck = TestForKnights(testPosition) || TestForPawns(testPosition) || TestForBishops(testPosition) || TestForRooks(testPosition);
            return testForCheck;
        }

        public void UpdateCheckStatus(bool testResult)
        {
            if (!Check && testResult)
            {
                Check = true;
                TimesInCheck++;
            }
            else if (Check && !testResult)
            {
                Check = false;
            }
        }

        private bool TestForKnights(int[] myPosition)
        {
            for(int i=0; i<knightLocations.Length; i++)
            {
                int testRank = myPosition[0] + knightLocations[i][0];
                int testFile = myPosition[1] + knightLocations[i][1];
                bool rankInBounds = testRank >= 0 && testRank < Board.TotalRanks;
                bool fileInBounds = testFile >= 0 && testFile < Board.TotalFiles;

                if (rankInBounds && fileInBounds)
                {
                    var piece = Board.Pieces[testRank][testFile];
                    if (piece is Knight && piece.Color != Color)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TestForPawns(int[] myPosition)
        {
            int[] leftPosition, rightPosition;

            if (Color == GameColors.White)
            {
                leftPosition = new int[2] { myPosition[0] - 1, myPosition[1] - 1 };
                rightPosition = new int[2] { myPosition[0] - 1, myPosition[1] + 1 };
            }
            else
            {
                leftPosition = new int[2] { myPosition[0] + 1, myPosition[1] + 1 };
                rightPosition = new int[2] { myPosition[0] + 1, myPosition[1] - 1 };
            }

            bool leftInBounds = (leftPosition[0] >= 0 && leftPosition[0] < Board.TotalRanks) && (leftPosition[1] >= 0 && leftPosition[1] < Board.TotalFiles);
            bool rightInBounds = (rightPosition[0] >= 0 && rightPosition[0] < Board.TotalRanks) && (rightPosition[1] >= 0 && rightPosition[1] < Board.TotalFiles);

            return (leftInBounds && IsPawnAttacking(leftPosition)) || (rightInBounds && IsPawnAttacking(rightPosition));
        }

        private bool IsPawnAttacking(int[] pawnPosition)
        {
            var piece = Board.Pieces[pawnPosition[0]][pawnPosition[1]];
            return piece is Pawn && Owner.Color != piece.Color;
        }


        private bool TestForBishops(int[] myPosition)
        {
            for (int i = 0; i < bishopDirections.Length; i++)
            {
                bool foundBishop = TestPath<Bishop>(myPosition[0] + bishopDirections[i][0], myPosition[1] + bishopDirections[i][1], bishopDirections[i]);
                if (foundBishop)
                {
                    return true;
                }
            }

            return false;
        }

        private bool TestPath<T>(int rank, int file, int[] direction)  //checks whether positions along the path described by direction contain long range pieces
        {
            if (rank < 0 || file < 0 || rank == Board.TotalRanks || file == Board.TotalFiles)
            {
                return false;
            }

            var piece = Board.Pieces[rank][file];

            if (piece != null && (piece is T || piece is Queen) && piece.Color != Color)
            {
                return true;
            }
            else if (piece != null)
            {
                return false;
            }

            rank += direction[0];
            file += direction[1];

            return TestPath<T>(rank, file, direction);
        }

        private bool TestForRooks(int[] myPosition)
        {
            for(int i=0; i<rookDirections.Length; i++)
            {
                bool foundRook = TestPath<Rook>(myPosition[0] + rookDirections[i][0], myPosition[1] + rookDirections[i][1], rookDirections[i]);
                if(foundRook)
                {
                    return true; 
                }
            }

            return false;
        }

        protected override List<Move> GetAllPossibleMoves(int[] myPosition)
        {
            return GetAllShortMoves(myPosition);
        }
    }
}
