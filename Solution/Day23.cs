using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2021.Solution
{
    public class State
    {
        public int a1 { get; set; }
        public int a2 { get; set; }
        public int b1 { get; set; }
        public int b2 { get; set; }
        public int c1 { get; set; }
        public int c2 { get; set; }
        public int d1 { get; set; }
        public int d2 { get; set; }

        public State(string line1, string line2)
        {
            List<char> initialPositions = new();
            initialPositions.Add(line1[3]);
            initialPositions.Add(line1[5]);
            initialPositions.Add(line1[7]);
            initialPositions.Add(line1[9]);

            initialPositions.Add(line2[3]);
            initialPositions.Add(line2[5]);
            initialPositions.Add(line2[7]);
            initialPositions.Add(line2[9]);

            int pos = 12;
            foreach (char c in initialPositions)
            {
                switch (c)
                {
                    case 'A':
                        if (a1 == 0)
                        {
                            a1 = pos;
                        }
                        else
                        {
                            a2 = pos;
                        }
                        break;

                    case 'B':
                        if (b1 == 0)
                        {
                            b1 = pos;
                        }
                        else
                        {
                            b2 = pos;
                        }
                        break;

                    case 'C':
                        if (c1 == 0)
                        {
                            c1 = pos;
                        }
                        else
                        {
                            c2 = pos;
                        }
                        break;

                    case 'D':
                        if (d1 == 0)
                        {
                            d1 = pos;
                        }
                        else
                        {
                            d2 = pos;
                        }
                        break;
                    default:
                        break;
                }

                pos++;
            }
        }

        public State(int a1, int a2, int b1, int b2, int c1, int c2, int d1, int d2)
        {
            this.a1 = a1;
            this.a2 = a2;
            this.b1 = b1;
            this.b2 = b2;
            this.c1 = c1;
            this.c2 = c2;
            this.d1 = d1;
            this.d2 = d2;
        }

        public static implicit operator (int, int, int, int, int, int, int, int)(State s) => (s.a1, s.a2, s.b1, s.b2, s.c1, s.c2, s.d1, s.d2);
        public static implicit operator State((int a1, int a2, int b1, int b2, int c1, int c2, int d1, int d2) s) => new State(s.a1, s.a2, s.b1, s.b2, s.c1, s.c2, s.d1, s.d2);

        public void Deconstruct(out int a1, out int a2, out int b1, out int b2, out int c1, out int c2, out int d1, out int d2)
        {
            a1 = this.a1;
            a2 = this.a2;
            b1 = this.b1;
            b2 = this.b2;
            c1 = this.c1;
            c2 = this.c2;
            d1 = this.d1;
            d2 = this.d2;
        }

        public override bool Equals(object? other)
        {
            if (other is State b)
            {
                return this.a1 == b.a1
                    && this.a2 == b.a2
                    && this.b1 == b.b1
                    && this.b2 == b.b2
                    && this.c1 == b.c1
                    && this.c2 == b.c2
                    && this.d1 == b.d1
                    && this.d2 == b.d2;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(a1, a2, b1, b2, c1, c2, d1, d2);
        }

        public static readonly HashSet<State> EndStates = new();

        public bool IsEndState()
        {
            if (EndStates.Count == 0)
            {
                int[] a = new int[2] { 12, 16 };
                int[] b = new int[2] { 13, 17 };
                int[] c = new int[2] { 14, 18 };
                int[] d = new int[2] { 15, 19 };

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            for (int l = 0; l < 2; l++)
                            {
                                EndStates.Add(new State(a[i], a[(i+1) % 2], b[j], b[(j + 1) % 2], c[k], c[(k + 1) % 2], d[l], d[(l + 1) % 2]));
                            }
                        }
                    }
                }
            }

            return EndStates.Contains(this);
        }

        public List<(State s, int score)> GetValidNextStates(State start, int currScore)
        {
            List<(State s, int score)> states = new();

            HashSet<int> occupied = new() { a1, a2, b1, b2, c1, c2, d1, d2 };

            // a1
            if (start.a1 == a1 || a1 < 12)
            {
                List<int> pos = GetValidPositions(a1, 12, occupied);

                foreach (int i in pos)
                {
                    states.Add(((i, a2, b1, b2, c1, c2, d1, d2), GetScore(a1, i, 1)));
                }
            }

            // a2
            if (start.a2 == a2 || a2 < 12)
            {
                List<int> pos = GetValidPositions(a2, 12, occupied);

                foreach (int i in pos)
                {
                    states.Add(((a1, i, b1, b2, c1, c2, d1, d2), GetScore(a2, i, 1)));
                }
            }

            // b1
            if (start.b1 == b1 || b1 < 12)
            {
                List<int> pos = GetValidPositions(b1, 13, occupied);

                foreach (int i in pos)
                {
                    states.Add(((a1, a2, i, b2, c1, c2, d1, d2), GetScore(b1, i, 10)));
                }
            }

            // b2
            if (start.b2 == b2 || b2 < 12)
            {
                List<int> pos = GetValidPositions(b2, 13, occupied);

                foreach (int i in pos)
                {
                    states.Add(((a1, a2, b1, i, c1, c2, d1, d2), GetScore(b2, i, 10)));
                }
            }

            // c1
            if (start.c1 == c1 || c1 < 12)
            {
                List<int> pos = GetValidPositions(c1, 14, occupied);

                foreach (int i in pos)
                {
                    states.Add(((a1, a2, b1, b2, i, c2, d1, d2), GetScore(c1, i, 100)));
                }
            }

            // c2
            if (start.c2 == c2 || c2 < 12)
            {
                List<int> pos = GetValidPositions(c2, 14, occupied);

                foreach (int i in pos)
                {
                    states.Add(((a1, a2, b1, b2, c1, i, d1, d2), GetScore(c2, i, 100)));
                }
            }

            // d1
            if (start.d1 == d1 || d1 < 12)
            {
                List<int> pos = GetValidPositions(d1, 15, occupied);


                foreach (int i in pos)
                {
                    states.Add(((a1, a2, b1, b2, c1, c2, i, d2), GetScore(d1, i, 1000)));
                }
            }

            // d2
            if (start.d2 == d2 || d2 < 12)
            {
                List<int> pos = GetValidPositions(d2, 15, occupied);

                foreach (int i in pos)
                {
                    states.Add(((a1, a2, b1, b2, c1, c2, d1, i), GetScore(d2, i, 1000)));
                }
            }

            return states;
        }

        private int GetScore(int start, int end, int mul)
        {
            int corrNum = 0;
            if (start >= 16)
            {
                corrNum = (start - 16) * 2 + 3;

                return mul * (Math.Abs(end - corrNum) + 2);
            }

            if (start >= 12)
            {
                corrNum = (start - 12) * 2 + 3;

                return mul * (Math.Abs(end - corrNum) + 1);
            }

            corrNum = ((end > 15) ? end - 16 : end - 12) * 2 + 3;

            return mul * (Math.Abs(start - corrNum) + (end > 15 ? 2 : 1));
        }

        private static readonly List<int> topRow = new() { 1, 2, 4, 6, 8, 10, 11 };
        private static readonly Dictionary<int, List<int>> graph = new();
        private List<int> GetValidPositionsN(int curr, int corridor, HashSet<int> occupied)
        {
            if (graph.Count == 0)
            {
                graph[1] = new() { 2 };
                graph[2] = new() { 1, 3 };
                graph[3] = new() { 2, 4, 12 };
                graph[4] = new() { 3, 5 };
                graph[5] = new() { 4, 6, 13 };
                graph[6] = new() { 5, 7 };
                graph[7] = new() { 6, 8, 14 };
                graph[8] = new() { 7, 9 };
                graph[9] = new() { 8, 10, 15 };
                graph[10] = new() { 9, 11 };
                graph[11] = new() { 10 };
                graph[12] = new() { 3, 16 };
                graph[13] = new() { 4, 17 };
                graph[14] = new() { 5, 18 };
                graph[15] = new() { 6, 19 };
                graph[16] = new() { 12 };
                graph[17] = new() { 13 };
                graph[18] = new() { 14 };
                graph[19] = new() { 15 };
            }

            HashSet<int> reachable = new();
            Stack<int> stack = new();

            foreach (var n in graph[curr])
            {
                stack.Push(n);
            }

            while (stack.Count > 0)
            {
                int next = stack.Pop();

                if (occupied.Contains(next) || reachable.Contains(next))
                {
                    continue;
                }

                reachable.Add(next);

                foreach (var n in graph[next])
                {
                    stack.Push(n);
                }
            }

            reachable.Remove(curr);

            for (int i = 0; i < 4; i++)
            {
                if (reachable.Contains(16 + i))
                {
                    reachable.Remove(12 + i);
                }
            }

            HashSet<int> valid = new();

            if (curr >= 12)
            {
                foreach (var i in topRow)
                {
                    valid.Add(i);
                }
            }

            valid.Add(corridor);
            valid.Add(corridor + 4);

            return valid.Intersect(reachable).ToList();
        }

        private List<int> GetValidPositions(int curr, int corridor, HashSet<int> occupied)
        {
            List<int> positions = new();

            if (curr > 11)
            {
                // In corridor
                int corrNum = ((curr > 15) ? curr - 16 : curr - 12) * 2 + 3;

                if (curr >= 16 && occupied.Contains(curr - 4))
                {
                    return positions;
                }

                // Pos 1 2 4
                if (corrNum == 3
                    || (corrNum == 5 && !occupied.Contains(4))
                    || (corrNum == 7 && !occupied.Contains(4) && !occupied.Contains(6))
                    || (corrNum == 9 && !occupied.Contains(4) && !occupied.Contains(6) && !occupied.Contains(8)))
                {
                    if (!occupied.Contains(2))
                    {
                        positions.Add(2);

                        if(!occupied.Contains(1))
                        {
                            positions.Add(1);
                        }
                    }

                    if (!occupied.Contains(4))
                    {
                        positions.Add(4);
                    }
                }

                // Pos 8 10 11
                if (corrNum == 9
                    || (corrNum == 7 && !occupied.Contains(8))
                    || (corrNum == 5 && !occupied.Contains(8) && !occupied.Contains(6))
                    || (corrNum == 3 && !occupied.Contains(8) && !occupied.Contains(6) && !occupied.Contains(4)))
                {
                    if (!occupied.Contains(10))
                    {
                        positions.Add(10);

                        if (!occupied.Contains(11))
                        {
                            positions.Add(11);
                        }
                    }

                    if (!occupied.Contains(8))
                    {
                        positions.Add(8);
                    }
                }
                if ((corrNum == 3 && !occupied.Contains(4) && !occupied.Contains(6))
                    || (corrNum == 5 && !occupied.Contains(6))
                    || (corrNum == 7 && !occupied.Contains(6))
                    || (corrNum == 9 && !occupied.Contains(8) && !occupied.Contains(6)))
                {
                    positions.Add(6);
                }

                HashSet<int> corridors = new();
                for (int i = corrNum - 1; i > 2; i--)
                {
                    if (occupied.Contains(i))
                    {
                        break;
                    }

                    if (i == 3)
                    {
                        corridors.Add(12);
                    }

                    if (i == 5)
                    {
                        corridors.Add(13);
                    }

                    if (i == 7)
                    {
                        corridors.Add(14);
                    }

                    if (i == 9)
                    {
                        corridors.Add(15);
                    }
                }

                for (int i = corrNum + 1; i < 10; i++)
                {
                    if (occupied.Contains(i))
                    {
                        break;
                    }

                    if (i == 3)
                    {
                        corridors.Add(12);
                    }

                    if (i == 5)
                    {
                        corridors.Add(13);
                    }

                    if (i == 7)
                    {
                        corridors.Add(14);
                    }

                    if (i == 9)
                    {
                        corridors.Add(15);
                    }
                }

                if (corridors.Contains(12) && corridor == 12)
                {
                    if (!occupied.Contains(12))
                    {
                        if (!occupied.Contains(16))
                        {
                            positions.Add(16);
                        }
                        else if (a1 == 16 || a2 == 16)
                        {
                            positions.Add(12);
                        }
                    }
                }

                if (corridors.Contains(13) && corridor == 13)
                {
                    if (!occupied.Contains(13))
                    {
                        if (!occupied.Contains(17))
                        {
                            positions.Add(17);
                        }
                        else if (b1 == 17 || b2 == 17)
                        {
                            positions.Add(13);
                        }
                    }
                }

                if (corridors.Contains(14) && corridor == 14)
                {
                    if (!occupied.Contains(14))
                    {
                        if (!occupied.Contains(18))
                        {
                            positions.Add(18);
                        }
                        else if (c1 == 18 || c2 == 18)
                        {
                            positions.Add(14);
                        }
                    }
                }

                if (corridors.Contains(15) && corridor == 15)
                {
                    if (!occupied.Contains(15))
                    {
                        if (!occupied.Contains(19))
                        {
                            positions.Add(19);
                        }
                        else if (d1 == 19 || d2 == 19)
                        {
                            positions.Add(15);
                        }
                    }
                }
            }
            else
            {
                HashSet<int> corridors = new();
                for (int i = curr - 1; i > 2; i--)
                {
                    if (occupied.Contains(i))
                    {
                        break;
                    }
                    
                    if (i == 3)
                    {
                        corridors.Add(12);
                    }

                    if (i == 5)
                    {
                        corridors.Add(13);
                    }

                    if (i == 7)
                    {
                        corridors.Add(14);
                    }

                    if (i == 9)
                    {
                        corridors.Add(15);
                    }
                }

                for (int i = curr + 1; i < 10; i++)
                {
                    if (occupied.Contains(i))
                    {
                        break;
                    }

                    if (i == 3)
                    {
                        corridors.Add(12);
                    }

                    if (i == 5)
                    {
                        corridors.Add(13);
                    }

                    if (i == 7)
                    {
                        corridors.Add(14);
                    }

                    if (i == 9)
                    {
                        corridors.Add(15);
                    }
                }

                if (corridors.Contains(12) && corridor == 12)
                {
                    if (!occupied.Contains(12))
                    {
                        if (!occupied.Contains(16))
                        {
                            positions.Add(16);
                        }
                        else if (a1 == 16 || a2 == 16)
                        {
                            positions.Add(12);
                        }
                    }
                }

                if (corridors.Contains(13) && corridor == 13)
                {
                    if (!occupied.Contains(13))
                    {
                        if (!occupied.Contains(17))
                        {
                            positions.Add(17);
                        }
                        else if (b1 == 17 || b2 == 17)
                        {
                            positions.Add(13);
                        }
                    }
                }

                if (corridors.Contains(14) && corridor == 14)
                {
                    if (!occupied.Contains(14))
                    {
                        if (!occupied.Contains(18))
                        {
                            positions.Add(18);
                        }
                        else if (c1 == 18 || c2 == 18)
                        {
                            positions.Add(14);
                        }
                    }
                }

                if (corridors.Contains(15) && corridor == 15)
                {
                    if (!occupied.Contains(15))
                    {
                        if (!occupied.Contains(19))
                        {
                            positions.Add(19);
                        }
                        else if (d1 == 19 || d2 == 19)
                        {
                            positions.Add(15);
                        }
                    }
                }
            }

            return positions;
        }

        public override string ToString()
        {
            char[] letters = Enumerable.Repeat('.', 20).ToArray();
            letters[a1] = 'A';
            letters[a2] = 'A';
            letters[b1] = 'B';
            letters[b2] = 'B';
            letters[c1] = 'C';
            letters[c2] = 'C';
            letters[d1] = 'D';
            letters[d2] = 'D';

            return $"#############\r\n#{letters[1]}{letters[2]}{letters[3]}{letters[4]}{letters[5]}{letters[6]}{letters[7]}{letters[8]}{letters[9]}{letters[10]}{letters[11]}#\r\n###{letters[12]}#{letters[13]}#{letters[14]}#{letters[15]}###\r\n  #{letters[16]}#{letters[17]}#{letters[18]}#{letters[19]}#\r\n  #########";
        }
    }


    public class Day23
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day23.txt");

            reader.ReadLine();
            reader.ReadLine();

            var line1 = reader.ReadLine();
            var line2 = reader.ReadLine();

            var start = new State(line1, line2);

            int score = FindOptimalScore(start);

            Console.WriteLine(score);
        }
        
        private int FindOptimalScore(State start)
        {
            Dictionary<State, (int score, State prev)> dp = new();
            PriorityQueue<(State s, int score, int depth, State prev), int> queue = new();

            queue.Enqueue((start, 0, 0, start), 0);

            while (queue.Count > 0)
            {
                (State s, int score, int depth, State prev) = queue.Dequeue();
                if (dp.ContainsKey(s))
                {
                    continue;
                }

                Console.WriteLine($"{score} - {depth}");
                Console.WriteLine(s.ToString());
                if( score == 13580 && depth > 9)
                {
                    //Console.ReadLine();
                }
                //Console.ReadLine();

                dp.Add(s, (score, prev));

                if (s.IsEndState())
                {
                    List<State> moves = new();
                    moves.Add(s);
                    while (prev != start)
                    {
                        moves.Add(prev);
                        prev = dp[prev].prev;
                    }
                    moves.Add(start);

                    moves.Reverse();

                    Console.WriteLine("----------------------------");
                    foreach(var move in moves)
                    {
                        Console.WriteLine(move);
                    }
                    Console.ReadLine();

                    return score;
                }

                var nextStates = s.GetValidNextStates(start, score);

                foreach(var next in nextStates)
                {
                    queue.Enqueue((next.s, score + next.score, depth + 1, s), (score + next.score));
                }
            }

            return int.MinValue;
        }

        public void SolveB()
        {
        }
    }
}
