using System;
using System.Collections.Generic;
using System.Text;

namespace TwoPlayerChess.ClassLibrary
{
    public class Move
    {
        public int StartRank { get; set; }
        public int StartFile { get; set; }
        public int EndRank { get; set; }
        public int EndFile { get; set; }

        public Move(int startRank, int startFile, int endRank, int endFile)
        {
            StartRank = startRank;
            StartFile = startFile;
            EndRank = endRank;
            EndFile = endFile;
        }
    }
}
