This is a two-player game of Chess. It runs in the Windows CLI or macOS shell in an environment set up with .NET Core version 3.1.0 or greater.
To play the game, simply enter "dotnet run" in the project directory. 

Black's pieces are placed at the top of the screen; white's pieces are at the bottom. 
Your desired move is indicated by the form: startPosition endPosition, i.e. 7c 6c, where numbers indicate ranks (rows) and letters files (columns). 
Example move: Black enters 7c 6c and black's 3rd pawn from the left is moved down one rank.  
To forfeit, enter DRAW in all caps on the player's turn.
Enter EXIT in all caps to quit the game.
  
Pawn promotion and capture en passant are not yet implemented.

Piece names are as follows: P = pawn, Rk = rook, Kt = knight, Bp = bishop, Qn = queen, and Kg = king.

Eventually, I intend to implement a more robust UI. In the meantime, enjoy playing and please feel free to report any bugs you may find!


