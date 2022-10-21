using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode2021.Solution.Day5;

namespace AdventOfCode2021.Solution
{
    public class Day5
    {
        public class Line
        {
            public Point a { get; set; }
            public Point b { get; set; }

            public Line(string ab)
            {
                string[] points = ab.Split(" -> ");

                a = new Point(points[0]);
                b = new Point(points[1]);
            }

            public bool IsStraight()
            {
                return a.x == b.x || a.y == b.y;
            }
        }

        public class Point
        {
            public int x { get; set; }
            public int y { get; set; }

            public Point (string xy)
            {
                var split = xy.Split(',');

                x = int.Parse(split[0]);
                y = int.Parse(split[1]);
            }
        }

        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day5.txt");

            List<Line> lines = new();

            while (!reader.EndOfStream)
            {
                string ab = reader.ReadLine();
                lines.Add(new Line(ab));
            }

            lines = lines.Where(l => l.IsStraight()).ToList();

            int[][] board = new int[1000][];
            for (int i = 0; i < 1000; i++)
            {
                board[i] = new int[1000];
            }

            foreach (var line in lines)
            {
                if (line.a.x == line.b.x)
                {
                    if (line.a.y < line.b.y)
                    {
                        for (int i = line.a.y; i <= line.b.y; i++)
                        {
                            board[line.a.x][i]++;
                        }
                    }
                    else
                    {
                        for (int i = line.b.y; i <= line.a.y; i++)
                        {
                            board[line.a.x][i]++;
                        }
                    }
                }
                else if (line.a.y == line.b.y)
                {
                    if (line.a.x < line.b.x)
                    {
                        for (int i = line.a.x; i <= line.b.x; i++)
                        {
                            board[i][line.a.y]++;
                        }
                    }
                    else
                    {
                        for (int i = line.b.x; i <= line.a.x; i++)
                        {
                            board[i][line.a.y]++;
                        }
                    }
                }
            }

            int overlap = 0;
            foreach(var row in board)
            {
                int count = row.Count(x => x > 1);
                overlap += count;
            }

            Console.WriteLine(overlap);
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day5.txt");

            List<Line> lines = new();

            while (!reader.EndOfStream)
            {
                string ab = reader.ReadLine();
                lines.Add(new Line(ab));
            }

            int[][] board = new int[1000][];
            for (int i = 0; i < 1000; i++)
            {
                board[i] = new int[1000];
            }

            foreach (var line in lines)
            {
                if (line.a.x == line.b.x)
                {
                    if (line.a.y < line.b.y)
                    {
                        for (int i = line.a.y; i <= line.b.y; i++)
                        {
                            board[line.a.x][i]++;
                        }
                    }
                    else
                    {
                        for (int i = line.b.y; i <= line.a.y; i++)
                        {
                            board[line.a.x][i]++;
                        }
                    }
                }
                else if (line.a.y == line.b.y)
                {
                    if (line.a.x < line.b.x)
                    {
                        for (int i = line.a.x; i <= line.b.x; i++)
                        {
                            board[i][line.a.y]++;
                        }
                    }
                    else
                    {
                        for (int i = line.b.x; i <= line.a.x; i++)
                        {
                            board[i][line.a.y]++;
                        }
                    }
                }
                else if (line.a.x < line.b.x)
                {
                    if (line.a.y < line.b.y)
                    {
                        for (int i = line.a.x, j = line.a.y; i <= line.b.x; i++, j++)
                        {
                            board[i][j]++;
                        }
                    }
                    else
                    {
                        for (int i = line.a.x, j = line.a.y; i <= line.b.x; i++, j--)
                        {
                            board[i][j]++;
                        }
                    }
                }
                else
                {
                    if (line.a.y < line.b.y)
                    {
                        for (int i = line.b.x, j = line.b.y; i <= line.a.x; i++, j--)
                        {
                            board[i][j]++;
                        }
                    }
                    else
                    {
                        for (int i = line.b.x, j = line.b.y; i <= line.a.x; i++, j++)
                        {
                            board[i][j]++;
                        }
                    }
                }
            }

            int overlap = 0;
            foreach (var row in board)
            {
                int count = row.Count(x => x > 1);
                overlap += count;
            }

            Console.WriteLine(overlap);
        }
    }
}
