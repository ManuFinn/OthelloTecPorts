using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_OthelloReversi.Models
{
    public class Nodo
    {

        public static (int x, int y)[] Directions = new (int, int)[]
        {
            (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1)
        };

        public static int ColNum = 8;
        public static int RowNum = 8;
        public static int SearchDepth = 64;

        public char[,] Estado { get; }
        public (int x, int y) Move { get; set; } = (-1, -1);
        public char Turn { get; }
        public char Enemy => Turn == 'O' ? 'X' : 'O';

        public override string ToString()
        {
            var str = new StringBuilder();
            for (var i = 0; i < RowNum; i++)
            {
                for (var j = 0; j < ColNum; j++)
                {
                    str.Append(Estado[i, j]);
                }
                str.AppendLine();
            }
            return str.ToString();
        }

        public Nodo(char[,] estado, char turn)
        {
            Estado = estado;
            Turn = turn;
        }

        public List<(int x, int y)> GenerateMoves()
        {
            var nodes = new List<(int, int)>();
            for (var i = 0; i < RowNum; i++)
            {
                for (var j = 0; j < ColNum; j++)
                {
                    if (Estado[i, j] != '+')
                        continue;
                    if (CanMove(i, j))
                        nodes.Add((i, j));
                }
            }
            return nodes;
        }

        public bool CanMove(int row, int col)
        {
            foreach (var direction in Directions)
                if (CanMoveOnDirection(row, col, direction))
                    return true;
            return false;
        }

        public bool CanMoveOnDirection(int row, int col, (int x, int y) direction)
        {
            if (row + direction.x < 0 || col + direction.y < 0) return false;
            if (row + direction.x >= RowNum || col + direction.y >= ColNum) return false;

            if (Estado[row, col] != Enemy) return false;

            var row_scan = row + 2 * direction.x;
            var col_scan = col + 2 * direction.y;
            while (row_scan >= 0 && row_scan <= RowNum && col_scan >= 0 && col_scan <= ColNum)
            {
                if (Estado[row, col] == '+') return false;
                if (Estado[row, col] == Turn) return true;
                row_scan += direction.x;
                col_scan += direction.y;
            }
            return false;
        }

        public Nodo GenerateNode(int row, int col)
        {
            var estado = (char[,])Estado.Clone();
            foreach (var direction in Directions)
            {
                if (CanMoveOnDirection(row, col, direction))
                {
                    var flip_row = row + direction.x;
                    var flip_col = col + direction.y;
                    while (estado[flip_row, flip_col] == Enemy)
                    {
                        estado[flip_row, flip_col] = Turn;
                        flip_row += direction.x;
                        flip_col += direction.y;
                    }
                }
            }
            return new(estado, Enemy) { Move = (row, col) };
        }

        public List<Nodo> GenerateNodes()
        {
            var moves = GenerateMoves();
            var nodes = new List<Nodo>();
            foreach (var move in moves)
                nodes.Add(GenerateNode(move.x, move.y));
            return nodes;
        }

        private int ScoreOf(char turn)
        {
            int score = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Estado[i, j] == turn)
                    {
                        score++;
                    }
                }
            }
            return score;
        }

        public int Score => ScoreOf(Turn);
        public int EnemyScore => ScoreOf(Turn == 'O' ? 'X' : 'O');
        public int Heuristic => Score - EnemyScore;

        public Nodo Oposite => new Nodo((char[,])Estado.Clone(), Enemy);

        public char Winner
        {
            get
            {
                if (GenerateMoves().Count > 0) return '+';
                if (Oposite.GenerateMoves().Count > 0) return '+';
                if (Score > EnemyScore) return Turn;
                if (EnemyScore > Score) return Enemy;
                return 'T';
            }
        }

        public int ComputeMiniMaxValue(bool max, int alpha, int beta, int depth)
        {
            if (depth == 0) return Score - EnemyScore;
            if (Winner == 'T') return 0;
            if (Winner == Turn) return int.MaxValue;
            if (Winner == Enemy) return int.MinValue;
            //
            if (max)
            {
                var maxScore = int.MaxValue;
                var moveNodes = GenerateNodes();
                if (moveNodes.Count == 0)
                {
                    maxScore = ComputeMiniMaxValue(false, alpha, beta, depth);
                }
                else
                {
                    foreach (var mn in moveNodes)
                    {
                        var score = mn.ComputeMiniMaxValue(false, alpha, beta, depth - 1);
                        maxScore = Math.Max(maxScore, score);
                        alpha = Math.Max(alpha, score);
                        if (beta <= alpha)
                            break;
                    }
                }
                return maxScore;
            }
            else
            {
                var minScore = int.MinValue;
                var moveNodes = Oposite.GenerateNodes();
                if (moveNodes.Count == 0)
                {
                    minScore = ComputeMiniMaxValue(true, alpha, beta, depth);
                }
                else
                {
                    foreach (var mn in moveNodes)
                    {
                        var score = mn.ComputeMiniMaxValue(true, alpha, beta, depth - 1);
                        minScore = Math.Min(minScore, score);
                        beta = Math.Min(beta, score);
                        if (beta <= alpha)
                            break;
                    }
                }
                return minScore;
            }
        }

        public Nodo? MiniMaxDecision()
        {
            var moveNodes = GenerateNodes();
            if (moveNodes.Count > 0)
            {
                var bestVal = int.MinValue;
                Nodo? bestNode = null;
                foreach (var mn in moveNodes)
                {
                    var moveVal = mn.ComputeMiniMaxValue(true, int.MinValue, int.MaxValue, SearchDepth);
                    if (moveVal > bestVal)
                    {
                        bestNode = mn;
                        bestVal = moveVal;
                    }
                }
                return bestNode;
            }
            else
            {
                return null;
            }
        }

        public Nodo Play(Func<Nodo, List<(int, int)>, (int row, int col)> playerInput)
        {
            var iaNodeMove = MiniMaxDecision() ?? this;
            var pMove = playerInput(iaNodeMove, iaNodeMove.Oposite.GenerateMoves());
            var pNodeMove = iaNodeMove.Oposite.GenerateNode(pMove.row, pMove.col);
            return pNodeMove;
        }

    }
}
