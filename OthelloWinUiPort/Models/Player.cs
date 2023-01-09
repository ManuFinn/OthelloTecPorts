using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_OthelloReversi.Models
{
    public class Player
    {
        public char color;
        public char oppColor;
        public int depthLimit;
        public int timeLimit1;
       public int timeLimit2;
        public char[,] board;

        public Player(char color, int dLim, int tLim1, int tLim2)
        {
            this.color = color;
            this.depthLimit = (dLim == 0) ? 8 : dLim; 
            this.timeLimit1 = tLim1;
            this.timeLimit2 = tLim2;

            if (this.color == 'B')
                oppColor = 'W';
            else
                oppColor = 'B';


            this.board = new char[8, 8]{
                    {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'B'},
                    {'W', 'W', 'W', 'W', 'W', 'W', 'B', 'B'},
                    {'W', 'W', 'W', 'W', 'W', 'B', 'W', 'B'},
                    {'W', 'W', 'W', 'W', 'B', 'W', 'W', 'B'},
                    {'W', 'W', 'W', 'W', 'B', 'W', 'B', 'B'},
                    {'W', 'W', 'W', 'B', 'W', 'B', 'B', 'B'},
                    {'W', 'W', 'W', 'W', 'B', 'W', ' ', 'B'},
                    {'B', 'B', 'B', 'B', 'B', 'B', 'B', 'B'}
                };
            this.resetBoard();
        }

        public void resetBoard()
        {
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    this.board[r,c] = ' ';

            this.board[3,3] = 'W';
            this.board[3,4] = 'B';
            this.board[4,3] = 'B';
            this.board[4,4] = 'W';
        }

        public String makeMove()
        {
            Move root = new Move(this.board, -1, -1, color, true);
            Move nextMove = alphaBeta(root, 0, int.MinValue, int.MaxValue, true);

            if (nextMove.x >= 0 && nextMove.y >= 0)
            {
                update(true, nextMove.x, nextMove.y);
            }
            return nextMove.toString();
        }

        public Move alphaBeta(Move move, int depth, int alp, int bet, bool isMax)
        {
            List<Move> children = getLegal(move.parentBoard, move.color);

           
            if (depth == depthLimit || children.Count == 0)
            {
                return move;
            }

            if (isMax)
            {
                int v = int.MinValue;
                Move maxMove = null;
                foreach (Move child in children)
                {
                    Move nextMove = alphaBeta(child, depth + 1, alp, bet, false);

                    if (nextMove.scoreVariable > v)
                    {
                        maxMove = child;
                        v = nextMove.scoreVariable;
                    }

                    alp = Math.Max(alp, v);
                    if (bet <= alp) 
                        break;
                }
                return maxMove;
            }
            else
            {
                int v = int.MaxValue;
                Move minMove = null;
                foreach (Move child in children)
                {
                    Move nextMove = alphaBeta(child, depth + 1, alp, bet, true);

                    if (nextMove.scoreVariable < v)
                    {
                        minMove = child;
                        v = nextMove.scoreVariable;
                    }

                    bet = Math.Min(bet, v);
                    if (bet <= alp) 
                        break;
                }
                return minMove;
            }
        }

        public List<Move> getLegal(char[,] board, char setColor)
        {
            List<Move> moves = new List<Move>();
            bool isMe;

            if (setColor == color) isMe = true;
            else isMe = false;

            char oppColor = (setColor == 'B') ? 'W' : 'B';

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (board[r,c] != ' ') 
                        continue;

                    int row;
                    int col;

                   
                    for (row = r + 1; row < 8 && board[row,c] == oppColor; row++) { }
                    if (row < 8 && board[row,c] == setColor && row != r + 1)
                    {
                        moves.Add(new Move(board, c, r, color, isMe));
                        continue;
                    }

                   
                    for (row = r - 1; row >= 0 && board[row,c] == oppColor; row--) { }
                    if (row >= 0 && board[row,c] == setColor && row != r - 1)
                    {
                        moves.Add(new Move(board, c, r, color, isMe));
                        continue;
                    }

                
                    for (col = c + 1; col < 8 && board[r,col] == oppColor; col++) { }
                    if (col < 8 && board[r,col] == setColor && col != c + 1)
                    {
                        moves.Add(new Move(board, c, r, color, isMe));
                        continue;
                    }

               
                    for (col = c - 1; col >= 0 && board[r,col] == oppColor; col--) { }
                    if (col >= 0 && board[r,col] == setColor && col != c - 1)
                    {
                        moves.Add(new Move(board, c, r, color, isMe));
                        continue;
                    }

                  
                    for (row = r + 1, col = c + 1;
                         row < 8 && col < 8 && board[row,col] == oppColor;
                         row++, col++) { }
                    if (row < 8 && col < 8 && board[row,col] == setColor
                            && !(row == r + 1 && col == c + 1))
                    {
                        moves.Add(new Move(board, c, r, color, isMe));
                        continue;
                    }

                   
                    for (row = r + 1, col = c - 1;
                         row < 8 && col >= 0 && board[row,col] == oppColor;
                         row++, col--) { }
                    if (row < 8 && col >= 0 && board[row,col] == setColor
                            && !(row == r + 1 && col == c - 1))
                    {
                        moves.Add(new Move(board, c, r, color, isMe));
                        continue;
                    }

                 
                    for (row = r - 1, col = c + 1;
                         row >= 0 && col < 8 && board[row,col] == oppColor;
                         row--, col++) { }
                    if (row >= 0 && col < 8 && board[row,col] == setColor
                            && !(row == r - 1 && col == c + 1))
                    {
                        moves.Add(new Move(board, c, r, color, isMe));
                        continue;
                    }

                  
                    for (row = r - 1, col = c - 1;
                         row >= 0 && col >= 0 && board[row,col] == oppColor;
                         row--, col--) { }
                    if (row >= 0 && col >= 0 && board[row,col] == setColor
                            && !(row == r - 1 && col == c - 1))
                    {
                        moves.Add(new Move(board, c, r, color, isMe));
                        continue;
                    }
                }
            }

            return moves;
        }

        public void update(bool isMe, int x, int y)
        {
            int r = y;
            int c = x;

            char setColor = this.color;
            char oppColor = this.oppColor;

            if (!isMe)
            {
                if (this.color == 'W')
                {
                    setColor = 'B';
                    oppColor = 'W';
                }
                else
                {
                    setColor = 'W';
                    oppColor = 'B';
                }
            }

            this.board[r,c] = setColor;

            int row;
            int col;

        
            for (row = r + 1; row < 8 && this.board[row,c] == oppColor; row++) { }
            if (row < 8 && this.board[row,c] == setColor)
            {
                flip("S", setColor, r, c, row, c);
            }

         
            for (row = r - 1; row >= 0 && this.board[row,c] == oppColor; row--) { }
            if (row >= 0 && this.board[row,c] == setColor)
            {
                flip("N", setColor, r, c, row, c);
            }

          
            for (col = c + 1; col < 8 && this.board[r,col] == oppColor; col++) { }
            if (col < 8 && this.board[r,col] == setColor)
            {
                flip("E", setColor, r, c, r, col);
            }

           
            for (col = c - 1; col >= 0 && this.board[r,col] == oppColor; col--) { }
            if (col >= 0 && this.board[r,col] == setColor)
            {
                flip("W", setColor, r, c, r, col);
            }

           
            for (row = r + 1, col = c + 1;
                 row < 8 && col < 8 && this.board[row,col] == oppColor;
                 row++, col++) { }
            if (row < 8 && col < 8 && this.board[row,col] == setColor)
            {
                flip("SE", setColor, r, c, row, col);
            }

           
            for (row = r + 1, col = c - 1;
                 row < 8 && col >= 0 && this.board[row,col] == oppColor;
                 row++, col--) { }
            if (row < 8 && col >= 0 && this.board[row,col] == setColor)
            {
                flip("SW", setColor, r, c, row, col);
            }

          
            for (row = r - 1, col = c + 1;
                 row >= 0 && col < 8 && this.board[row,col] == oppColor;
                 row--, col++) { }
            if (row >= 0 && col < 8 && this.board[row,col] == setColor)
            {
                flip("NE", setColor, r, c, row, col);
            }

          
            for (row = r - 1, col = c - 1;
                 row >= 0 && col >= 0 && this.board[row,col] == oppColor;
                 row--, col--) { }
            if (row >= 0 && col >= 0 && this.board[row,col] == setColor)
            {
                flip("NW", setColor, r, c, row, col);
            }
        }

        public int flip(String dir, char setColor, int r1, int c1, int r2, int c2)
        {
            int flipped = 0;

            switch (dir)
            {
                case "S":
                    for (int row = r1 + 1; row < r2; row++)
                    {
                        this.board[row,c1] = setColor;
                        flipped++;
                    }
                    break;
                case "N":
                    for (int row = r2 + 1; row < r1; row++)
                    {
                        this.board[row,c1] = setColor;
                        flipped++;
                    }
                    break;
                case "E":
                    for (int col = c1 + 1; col < c2; col++)
                    {
                        this.board[r1,col] = setColor;
                        flipped++;
                    }
                    break;
                case "W":
                    for (int col = c2 + 1; col < c1; col++)
                    {
                        this.board[r1,col] = setColor;
                        flipped++;
                    }
                    break;
                case "SE":
                    for (int row = r1 + 1, col = c1 + 1; row < r2 && col < c2; row++, col++)
                    {
                        this.board[row,col] = setColor;
                        flipped++;
                    }
                    break;
                case "SW":
                    for (int row = r1 + 1, col = c1 - 1; row < r2 && col > c2; row++, col--)
                    {
                        this.board[row,col] = setColor;
                        flipped++;
                    }
                    break;
                case "NE":
                    for (int row = r1 - 1, col = c1 + 1; row > r2 && col < c2; row--, col++)
                    {
                        this.board[row,col] = setColor;
                        flipped++;
                    }
                    break;
                case "NW":
                    for (int row = r2 + 1, col = c2 + 1; row < r1 && col < c1; row++, col++)
                    {
                        this.board[row,col] = setColor;
                        flipped++;
                    }
                    break;
            }

            return flipped;
        }

        public char[,] printBoard()
        {
            return this.board;
        }
    }
}
