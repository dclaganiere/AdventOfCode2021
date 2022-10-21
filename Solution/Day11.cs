using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day11
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day11.txt");

            List<List<int>> octopuses = new List<List<int>>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                octopuses.Add(line.Select(c => (c - '0')).ToList());
            }

            long total = 0;
            for (int iteration = 0; iteration < 100; iteration++)
            {
                bool[,] flashed = new bool[octopuses.Count, octopuses[0].Count];

                for (int i = 0; i < octopuses.Count; i++)
                {
                    for (int j = 0; j < octopuses[0].Count; j++)
                    {
                        octopuses[i][j]++;
                        if (octopuses[i][j] > 9)
                        {
                            total += Flash(i, j, octopuses, flashed);
                        }
                    }
                }

                for (int i = 0; i < octopuses.Count; i++)
                {
                    for (int j = 0; j < octopuses[0].Count; j++)
                    {
                        if (flashed[i, j])
                        {
                            octopuses[i][j] = 0;
                        }
                    }
                }
            }

            Console.WriteLine(total);
        }

        private long Flash(int i, int j, List<List<int>> octopuses, bool[,] flashed)
        {
            if (i < 0 || j < 0 || i >= octopuses.Count || j >= octopuses[0].Count)
            {
                return 0;
            }

            if (flashed[i, j] || octopuses[i][j] <= 9)
            {
                return 0;
            }

            flashed[i, j] = true;

            if (i + 1 < octopuses.Count)
            {
                if (j + 1 < octopuses[0].Count)
                {
                    octopuses[i + 1][j + 1]++;
                }

                if (j - 1 >= 0)
                {
                    octopuses[i + 1][j - 1]++;
                }

                octopuses[i + 1][j]++;
            }

            if (j + 1 < octopuses[0].Count)
            {
                octopuses[i][j + 1]++;
            }

            if (j - 1 >= 0)
            {
                octopuses[i][j - 1]++;
            }

            if (i - 1 >= 0)
            {
                if (j + 1 < octopuses[0].Count)
                {
                    octopuses[i - 1][j + 1]++;
                }

                if (j - 1 >= 0)
                {
                    octopuses[i - 1][j - 1]++;
                }

                octopuses[i - 1][j]++;
            }

            return 1
                + Flash(i + 1, j + 1, octopuses, flashed)
                + Flash(i + 1, j, octopuses, flashed)
                + Flash(i + 1, j - 1, octopuses, flashed)
                + Flash(i, j + 1, octopuses, flashed)
                + Flash(i, j - 1, octopuses, flashed)
                + Flash(i - 1, j + 1, octopuses, flashed)
                + Flash(i - 1, j, octopuses, flashed)
                + Flash(i - 1, j - 1, octopuses, flashed);
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day11.txt");

            List<List<int>> octopuses = new List<List<int>>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                octopuses.Add(line.Select(c => (c - '0')).ToList());
            }

            int iteration = 0;
            bool allFlashed = false;

            while (!allFlashed)
            {
                iteration++;
                bool[,] flashed = new bool[octopuses.Count, octopuses[0].Count];

                for (int i = 0; i < octopuses.Count; i++)
                {
                    for (int j = 0; j < octopuses[0].Count; j++)
                    {
                        octopuses[i][j]++;
                        if (octopuses[i][j] > 9)
                        {
                            Flash(i, j, octopuses, flashed);
                        }
                    }
                }

                allFlashed = true;
                for (int i = 0; i < octopuses.Count; i++)
                {
                    for (int j = 0; j < octopuses[0].Count; j++)
                    {
                        if (flashed[i, j])
                        {
                            octopuses[i][j] = 0;
                        }
                        else
                        {
                            allFlashed = false;
                        }
                    }
                }
            }

            Console.WriteLine(iteration);
        }
    }
}
