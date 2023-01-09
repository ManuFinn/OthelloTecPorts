using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_OthelloReversi.Models
{
    public class Othello
    {
        public string scan = "";
        public Player player;
        public string move="";
        public char playerString;
        public int depthLimit;
        public int timeLimit1;
        public int timeLimit2;
        public Othello(char playerString,int depthLimit,int timeLimit1,int timeLimit2)
        {
            player = new Player(playerString, depthLimit, timeLimit1, timeLimit2);
            if (player.color == 'B')
            { /* Black moves first */
                move = player.makeMove();
                //System.out.println(move);
            }

        }
        public char[,] CapturarSiguienteMovidaJugadorHumano(bool IsMe, string movimiento)
        {   
            scan = movimiento;
         
                move = scan;

                if (!(move == "pass"))
                {
                    String[] xy = move.Split(" ");
                    player.update(IsMe, int.Parse(xy[0]), int.Parse(xy[1]));
                }

                move = player.makeMove();

            return player.printBoard();
            
        }
       
    }
}
