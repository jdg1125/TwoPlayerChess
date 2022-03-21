This is a two-player game of Chess. It runs in the Windows CLI or macOS shell in an environment set up with .NET Core version 3.1.0 or greater.
To play the game, simply enter "dotnet run" in the project directory. 

----------------------------------------------------------------------------------------------------------------------------------------------------------

The game board appears similarly to the rendering below, but with colors added. White's pieces are placed at the bottom of the screen. 


	----------------------------------------------------------------
8	|   Rk	|   Kt	|   Bp	|   Qn	|   Kg	|   Bp	|   Kt	|   Rk	|
	----------------------------------------------------------------
7	|   Pn	|   Pn	|   Pn	|   Pn	|   Pn	|   Pn	|   Pn	|   Pn	|
	----------------------------------------------------------------
6	|	      |	      |	      |	      |	      |	      |      	|	      |
	----------------------------------------------------------------
5	|	      |	      |	      |	      |	      |      	|      	|	      |
	----------------------------------------------------------------
4	|	      |	      |	      |	      |	      |	      |	      |	      |
	----------------------------------------------------------------
3	|	      |	      |	      |	      |	      |	      |      	|	      |
	----------------------------------------------------------------
2	|   Pn	|   Pn	|   Pn	|   Pn	|   Pn	|   Pn	|   Pn	|   Pn	|
	----------------------------------------------------------------
1	|   Rk	|   Kt	|   Bp	|   Qn	|   Kg	|   Bp	|   Kt	|   Rk	|
	----------------------------------------------------------------
	    a       b	      c	      d	       e	     f	     g	    h	


To make a move, type <startRank><startFile> <endRank><endFile>, where a space separates the start and end positions.  
Example: Black enters 7c 6c and black's 3rd pawn from the left is moved down one rank.  
To forfeit, enter DRAW in all caps on the player's turn.
Enter EXIT in all caps to quit the game.
  
Pawn promotion and capture en passant are not yet implemented.

Piece names are as follows: P = pawn, Rk = rook, Kt = knight, Bp = bishop, Qn = queen, and Kg = king.

Eventually, I intend to implement a more robust UI. In the meantime, enjoy playing and please feel free to report any bugs you may find!


