using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day9
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day9.txt");

            List<string> lines = new List<string>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (line.Length > 0)
                {
                    lines.Add(line);
                }
            }

            int rows = lines.Count;
            int cols = lines[0].Length;

            int[,] map = new int[rows + 2, cols + 2];

            for (int i = 1; i <= rows; i++)
            {
                map[i, 0] = 9;
                map[i, cols + 1] = 9;
            }

            for (int j = 1; j <= cols; j++)
            {
                map[0, j] = 9;
                map[rows + 1, j] = 9;
            }

            for (int i = 0; i < rows; i++)
            {
                List<int> row = lines[i].ToCharArray().Select(c => (int)(c - '0')).ToList();

                for (int j = 0; j < row.Count; j++)
                {
                    map[i + 1, j + 1] = row[j];
                }
            }

            int total = 0;
            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= cols; j++)
                {
                    if (map[i, j] < map[i + 1, j]
                        && map[i, j] < map[i - 1, j]
                        && map[i, j] < map[i, j + 1]
                        && map[i, j] < map[i, j - 1])
                    {
                        total += map[i, j] + 1;
                    }
                }
            }

            Console.WriteLine(total);
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day9.txt");

            List<string> lines = new List<string>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (line.Length > 0)
                {
                    lines.Add(line);
                }
            }

            int rows = lines.Count;
            int cols = lines[0].Length;

            int[,] map = new int[rows + 2, cols + 2];

            for (int i = 1; i <= rows; i++)
            {
                map[i, 0] = 9;
                map[i, cols + 1] = 9;
            }

            for (int j = 1; j <= cols; j++)
            {
                map[0, j] = 9;
                map[rows + 1, j] = 9;
            }

            for (int i = 0; i < rows; i++)
            {
                List<int> row = lines[i].ToCharArray().Select(c => (int)(c - '0')).ToList();

                for (int j = 0; j < row.Count; j++)
                {
                    map[i + 1, j + 1] = row[j];
                }
            }

            bool[,] marked = new bool[rows + 2, cols + 2];
            List<int> basins = new();

            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= cols; j++)
                {
                    if (map[i, j] < map[i + 1, j]
                        && map[i, j] < map[i - 1, j]
                        && map[i, j] < map[i, j + 1]
                        && map[i, j] < map[i, j - 1])
                    {
                        basins.Add(CalculateBasin(-1, i, j, map, marked));
                    }
                }
            }

            basins.Sort();
            basins.Reverse();

            int total = basins.Take(3).Aggregate(1, (a, b) => a * b);

            Console.WriteLine(total);
        }

        public int CalculateBasin(int prev, int i, int j, int[,] map, bool[,] marked)
        {
            if (marked[i, j] || map[i, j] == 9 || map[i, j] < prev)
            {
                return 0;
            }

            marked[i, j] = true;

            return 1
                + CalculateBasin(map[i, j], i + 1, j, map, marked)
                + CalculateBasin(map[i, j], i - 1, j, map, marked)
                + CalculateBasin(map[i, j], i, j + 1, map, marked)
                + CalculateBasin(map[i, j], i, j - 1, map, marked);
        }
    }
}
