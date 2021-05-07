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
            Directions = new int[8][] { new int[2] { 1, 1 }, new int[2] { 1, -1 }, new int[2] { -1, 1 }, new int[2] { -1, -1 },
                        new int[2] { 1, 0 }, new int[2] { -1, 0 }, new int[2] { 0, 1 }, new int[2] { 0, -1 } };
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
            }
            else
            {
                ExecuteMove(move);
            }

            return true;
        }

        private void ExecuteCastle(Move move)
        {
            Board.StageMove(move);   //move the king. Don't call Board.ExecuteMove() because no capturing is necessary
            TimesMoved++;
            Owner.Pieces[this] = new int[2] { move.EndRank, move.EndFile };
            
            int fileDiff = move.EndFile - move.StartFile;
            var rook = fileDiff < 0 ? Board.Pieces[move.StartRank][0] : Board.Pieces[move.StartRank][7];  //find rook to castle with
            int rookEndFile = fileDiff < 0 ? move.EndFile + 1 : move.EndFile - 1;
            var rookMove = new Move(Owner.Pieces[rook][0], Owner.Pieces[rook][1], Owner.Pieces[rook][0], rookEndFile);
            
            Board.StageMove(rookMove);  //move the rook. 
            rook.TimesMoved++;
            Owner.Pieces[rook] = new int[2] { rookMove.EndRank, rookMove.EndFile };
        }

        private void ExecuteMove(Move move)
        {
            Piece captured = Board.StageMove(move);
            Board.ExecuteMove(move, captured);
        }
        public override bool IsMoveLegal(Move move)
        {
            //check for ordinary move - 1 step in any direction

            int rankDiff = move.EndRank - move.StartRank;
            int fileDiff = move.EndFile - move.StartFile;

            if ((rankDiff >= -1 && rankDiff <= 1) && (fileDiff >= -1 && fileDiff <= 1))
            {
                Piece target = Board.Pieces[move.EndRank][move.EndFile];
                if (target != null && target.Color == Color || target is King)
                {
                    return false;
                }
                return IsInCheck(new int[2] { move.EndRank, move.EndFile }) == false;
            }

            else if (rankDiff == 0 && (fileDiff == 2 || fileDiff == -2))
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
                    int[] testSquare = new int[2] { move.StartRank, move.StartFile + fileDiff / 2 };  //test square adjacent to king is not under attack
                    bool tryOneStep = IsInCheck(testSquare);
                    testSquare[1] += fileDiff / 2;      //test square two steps away is not under attack
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
        }
        public bool IsInCheck()
        {
            int[] location = Owner.Pieces[this];

            bool testForCheck = TestForKnights(location) || TestForPawns(location) || TestForBishops(location) || TestForRooks(location);

            return testForCheck;
        }

        public bool IsInCheck(int[] location)
        {
            bool testForCheck = TestForKnights(location) || TestForPawns(location) || TestForBishops(location) || TestForRooks(location);

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

        private bool TestForKnights(int[] location)
        {
            int[] offsets = new int[4] { -2, -1, 2, 1 };
            for (int i = 0; i < offsets.Length; i++)
            {
                for (int j = 0; j < offsets.Length; j++)
                {
                    if ((offsets[i] & 1) == (offsets[j] & 1)) //skip the combination if abs val of offsets[i] == offsets[j]
                    {
                        continue;
                    }
                    bool rankInBounds = location[0] + offsets[i] >= 0 && location[0] + offsets[i] < Board.TotalRanks;
                    bool fileInBounds = location[1] + offsets[j] >= 0 && location[1] + offsets[j] < Board.TotalFiles;

                    if (rankInBounds && fileInBounds)
                    {
                        var piece = Board.Pieces[location[0] + offsets[i]][location[1] + offsets[j]];
                        if (piece is Knight && piece.Color != Color)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool TestForPawns(int[] location)
        {
            int[] leftAttack, rightAttack;

            if (Color == GameColors.White)
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

            if (leftInBounds)
            {
                var piece = Board.Pieces[leftAttack[0]][leftAttack[1]];
                if (piece is Pawn && Owner.Color != piece.Color)
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

        private bool TestForBishops(int[] location)
        {
            bool path1 = TestPath<Bishop>(location[0] + 1, location[1] + 1, new int[2] { 1, 1 });
            bool path2 = TestPath<Bishop>(location[0] + 1, location[1] - 1, new int[2] { 1, -1 });
            bool path3 = TestPath<Bishop>(location[0] - 1, location[1] + 1, new int[2] { -1, 1 });
            bool path4 = TestPath<Bishop>(location[0] - 1, location[1] - 1, new int[2] { -1, -1 });

            return path1 || path2 || path3 || path4;
        }

        private bool TestPath<T>(int rank, int file, int[] direction)
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

        private bool TestForRooks(int[] location)
        {
            bool path1 = TestPath<Rook>(location[0] + 1, location[1], new int[2] { 1, 0 });
            bool path2 = TestPath<Rook>(location[0], location[1] + 1, new int[2] { 0, 1 });
            bool path3 = TestPath<Rook>(location[0] - 1, location[1], new int[2] { -1, 0 });
            bool path4 = TestPath<Rook>(location[0], location[1] - 1, new int[2] { 0, -1 });

            return path1 || path2 || path3 || path4;
        }

        protected override List<Move> GetAllPossibleMoves(int[] position)
        {
            return GetAllShortMoves(position);
        }
    }
}
