using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode2021.Solution.Day5;

namespace AdventOfCode2021.Solution
{
    public class Day13
    {
        public class Point
        {
            public int x { get; set; }
            public int y { get; set; }

            public Point(string xy)
            {
                var split = xy.Split(',');

                x = int.Parse(split[0]);
                y = int.Parse(split[1]);
            }
        }

        public class Fold
        {
            public string axis { get; set; }
            public int pos { get; set; }

            public Fold(string fold)
            {
                fold = fold[11..];
                var split = fold.Split('=');

                axis = split[0];
                pos = int.Parse(split[1]);
            }
        }

        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day13.txt");
            List<Point> points = new List<Point>();
            List<Fold> folds = new List<Fold>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                points.Add(new Point(line));
            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                folds.Add(new Fold(line));
            }

            int maxRow = points.Max(p => p.y) + 1;
            int maxCol = points.Max(p => p.x) + 1;

            bool[,] prevPaper = new bool[maxRow, maxCol];
            bool[,] currPaper = new bool[maxRow, maxCol];

            foreach (Point p in points)
            {
                currPaper[p.y, p.x] = true;
            }

            foreach (Fold fold in folds.Take(1))
            {
                switch (fold.axis)
                {
                    case "x":
                        prevPaper = currPaper;

                        int left = fold.pos;
                        int right = maxCol - fold.pos - 1;

                        int cols = Math.Max(left, right);

                        currPaper = new bool[maxRow, cols];

                        // Copy right
                        for (int i = 0; i < maxRow; i++)
                        {
                            for (int j = 0; j < right; j++)
                            {
                                if (prevPaper[i, j + fold.pos + 1])
                                {
                                    currPaper[i, j] = true;
                                }
                            }
                        }

                        // Copy left
                        for (int i = 0; i < maxRow; i++)
                        {
                            for (int j = 0; j < left; j++)
                            {
                                if (prevPaper[i, fold.pos - j - 1])
                                {
                                    currPaper[i, j] = true;
                                }
                            }
                        }

                        maxCol = cols;
                        break;

                    case "y":
                        prevPaper = currPaper;

                        int top = fold.pos;
                        int bottom = maxRow - fold.pos - 1;

                        int rows = Math.Max(top, bottom);

                        currPaper = new bool[rows, maxCol];

                        // Copy top
                        for (int i = 0; i < top; i++)
                        {
                            for (int j = 0; j < maxCol; j++)
                            {
                                if (prevPaper[i, j])
                                {
                                    currPaper[i, j] = true;
                                }
                            }
                        }

                        // Copy bottom
                        for (int i = 0; i < bottom; i++)
                        {
                            for (int j = 0; j < maxCol; j++)
                            {
                                if (prevPaper[fold.pos + 1 + i, j])
                                {
                                    currPaper[fold.pos - 1 - i, j] = true;
                                }
                            }
                        }

                        maxRow = rows;
                        break;

                    default:
                        break;
                }
            }

            int total = 0;
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxCol; j++)
                {
                    if (currPaper[i, j])
                    {
                        total++;
                    }
                }
            }

            Console.WriteLine(total);
        }

        private static void PrintPaper(int maxRow, int maxCol, bool[,] currPaper)
        {
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = maxCol - 1; j >= 0; j--)
                {
                    Console.Write(currPaper[i, j] ? "#" : ".");
                }

                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day13.txt");
            List<Point> points = new List<Point>();
            List<Fold> folds = new List<Fold>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                points.Add(new Point(line));
            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                folds.Add(new Fold(line));
            }

            int maxRow = points.Max(p => p.y) + 1;
            int maxCol = points.Max(p => p.x) + 1;

            bool[,] prevPaper = new bool[maxRow, maxCol];
            bool[,] currPaper = new bool[maxRow, maxCol];

            foreach (Point p in points)
            {
                currPaper[p.y, p.x] = true;
            }

            foreach (Fold fold in folds)
            {
                switch (fold.axis)
                {
                    case "x":
                        prevPaper = currPaper;

                        int left = fold.pos;
                        int right = maxCol - fold.pos - 1;

                        int cols = Math.Max(left, right);

                        currPaper = new bool[maxRow, cols];

                        // Copy right
                        for (int i = 0; i < maxRow; i++)
                        {
                            for (int j = 0; j < right; j++)
                            {
                                if (prevPaper[i, j + fold.pos + 1])
                                {
                                    currPaper[i, j] = true;
                                }
                            }
                        }

                        // Copy left
                        for (int i = 0; i < maxRow; i++)
                        {
                            for (int j = 0; j < left; j++)
                            {
                                if (prevPaper[i, fold.pos - j - 1])
                                {
                                    currPaper[i, j] = true;
                                }
                            }
                        }

                        maxCol = cols;
                        break;

                    case "y":
                        prevPaper = currPaper;

                        int top = fold.pos;
                        int bottom = maxRow - fold.pos - 1;

                        int rows = Math.Max(top, bottom);

                        currPaper = new bool[rows, maxCol];

                        // Copy top
                        for (int i = 0; i < top; i++)
                        {
                            for (int j = 0; j < maxCol; j++)
                            {
                                if (prevPaper[i, j])
                                {
                                    currPaper[i, j] = true;
                                }
                            }
                        }

                        // Copy bottom
                        for (int i = 0; i < bottom; i++)
                        {
                            for (int j = 0; j < maxCol; j++)
                            {
                                if (prevPaper[fold.pos + 1 + i, j])
                                {
                                    currPaper[fold.pos - 1 - i, j] = true;
                                }
                            }
                        }

                        maxRow = rows;
                        break;

                    default:
                        break;
                }
            }

            PrintPaper(maxRow, maxCol, currPaper);
        }
    }
}
