using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day12
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day12.txt");

            Dictionary<string, List<string>> graph = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var caves = line.Split('-');

                if (!graph.ContainsKey(caves[0]))
                {
                    graph[caves[0]] = new List<string>();
                }

                if (!graph.ContainsKey(caves[1]))
                {
                    graph[caves[1]] = new List<string>();
                }

                graph[caves[0]].Add(caves[1]);
                graph[caves[1]].Add(caves[0]);
            }

            HashSet<string> visited = new HashSet<string>();
            int total = TraverseCaves("start", graph, visited);

            Console.WriteLine(total);
        }

        private int TraverseCaves(string curr, Dictionary<string, List<string>> graph, HashSet<string> visited, string visitedSmallTwice = "end")
        {
            if (visited.Contains(curr) && (curr == "start" || !string.IsNullOrEmpty(visitedSmallTwice)))
            {
                return 0;
            }

            if (curr == "end")
            {
                return 1;
            }

            if (curr == curr.ToLower())
            {
                if(visited.Contains(curr))
                {
                    visitedSmallTwice = curr;
                }
                else
                {
                    visited.Add(curr);
                }
            }

            int total = 0;

            foreach (var cave in graph[curr])
            {
                total += TraverseCaves(cave, graph, visited, visitedSmallTwice);
            }

            if (curr == curr.ToLower())
            {
                if (visitedSmallTwice == curr)
                {
                    visitedSmallTwice = string.Empty;
                }
                else
                {
                    visited.Remove(curr);
                }
            }

            return total;
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day12.txt");

            Dictionary<string, List<string>> graph = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var caves = line.Split('-');

                if (!graph.ContainsKey(caves[0]))
                {
                    graph[caves[0]] = new List<string>();
                }

                if (!graph.ContainsKey(caves[1]))
                {
                    graph[caves[1]] = new List<string>();
                }

                graph[caves[0]].Add(caves[1]);
                graph[caves[1]].Add(caves[0]);
            }

            HashSet<string> visited = new HashSet<string>();
            int total = TraverseCaves("start", graph, visited, string.Empty);

            Console.WriteLine(total);
        }
    }
}
