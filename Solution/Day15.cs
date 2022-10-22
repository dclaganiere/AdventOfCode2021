using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day15
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day15.txt");

            List<List<int>> maze = new List<List<int>>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                maze.Add(line.ToCharArray().Select(c => c - '0').ToList());
            }

            long best = TraverseMaze(maze) - maze[0][0];

            Console.WriteLine(best);
        }

        private long TraverseMaze(List<List<int>> maze)
        {
            List<List<int>> minRisk = new();

            for (int i = 0; i < maze.Count; i++)
            {
                minRisk.Add(Enumerable.Repeat(int.MaxValue, maze[0].Count).ToList());
            }

            PriorityQueue<(int risk, int x, int y), int> queue = new();

            queue.Enqueue((0, maze.Count - 1, maze[0].Count - 1), 0);

            while (queue.Count > 0)
            {
                (int risk, int x, int y) = queue.Dequeue();

                if (x >= maze.Count || y >= maze[0].Count || x < 0 || y < 0)
                {
                    continue;
                }

                if (minRisk[x][y] != int.MaxValue)
                {
                    continue;
                }

                int newRisk = risk + maze[x][y];
                minRisk[x][y] = newRisk;

                queue.Enqueue((newRisk, x + 1, y), newRisk);
                queue.Enqueue((newRisk, x, y + 1), newRisk);
                queue.Enqueue((newRisk, x - 1, y), newRisk);
                queue.Enqueue((newRisk, x, y - 1), newRisk);
            }

            return minRisk[0][0];
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day15.txt");

            List<List<int>> origMaze = new List<List<int>>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                origMaze.Add(line.ToCharArray().Select(c => c - '0').ToList());
            }

            List<List<int>> maze = new List<List<int>>();

            for (int i = 0; i < origMaze.Count * 5; i++)
            {
                maze.Add(new List<int>());
                for (int j = 0; j < origMaze[0].Count * 5; j++)
                {
                    int value = origMaze[i % origMaze.Count][j % origMaze[0].Count];
                    maze[i].Add(((value - 1 + (i / origMaze.Count) + (j / origMaze[0].Count)) % 9) + 1);
                }
            }

            long best = TraverseMaze(maze) - maze[0][0];

            Console.WriteLine(best);
        }
    }
}
