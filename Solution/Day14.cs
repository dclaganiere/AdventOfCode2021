using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day14
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day14.txt");

            string state = reader.ReadLine() ?? string.Empty;
            Dictionary<string, string> pairInsertions = new();

            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                var split = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);

                pairInsertions[split[0]] = split[1];
            }

            for (int step = 1; step <= 10; step++)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 1; i < state.Length; i++)
                {
                    string pair = state.Substring(i - 1, 2);

                    sb.Append(state[i - 1]);
                    if (pairInsertions.ContainsKey(pair))
                    {
                        sb.Append(pairInsertions[pair]);
                    }
                    else
                    {
                        Console.WriteLine(pair);
                    }
                }

                sb.Append(state[^1]);

                state = sb.ToString();
            }

            var sorted = state.ToCharArray().ToList();
            sorted.Sort();

            var lookup = sorted
                .GroupBy(c => c)
                .Select(g => (g.Key, Count: g.Count()))
                .OrderBy(c => c.Count)
                .ToList();

            int leastCommon = lookup.First().Count;
            int mostCommon = lookup.Last().Count;

            Console.WriteLine(mostCommon - leastCommon);
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day14.txt");

            string state = reader.ReadLine() ?? string.Empty;
            Dictionary<string, string> pairInsertions = new();
            HashSet<string> nodes = new();

            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                var split = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);

                pairInsertions[split[0]] = split[1];

                nodes.Add(split[0][..1]);
                nodes.Add(split[0][1..]);
                nodes.Add(split[1]);
            }

            Dictionary<string, long> nodeCount = new Dictionary<string, long>();

            foreach(var node in nodes)
            {
                nodeCount[node] = 0;
            }
            
            for (int i = 0; i < state.Length; i++)
            {
                nodeCount[state.Substring(i, 1)]++;
            }

            for (int i = 0; i < 40; i++)
            {
                dp.Add(new Dictionary<string, Dictionary<string, long>>());
            }

            for (int i = 1; i < state.Length; i++)
            {
                var nc = BuildPolymer(1, state.Substring(i - 1, 2), pairInsertions, 40);

                foreach ((string k, long v) in nc)
                {
                    nodeCount[k] += v;
                }
            }

            var lookup = nodeCount
                .Select(g => (Key: g.Key, Count: g.Value))
                .OrderBy(c => c.Count)
                .ToList();

            long leastCommon = lookup.First().Count;
            long mostCommon = lookup.Last().Count;

            Console.WriteLine(mostCommon - leastCommon);
        }

        private List<Dictionary<string, Dictionary<string, long>>> dp = new();

        public Dictionary<string, long> BuildPolymer(int depth, string pair, Dictionary<string, string> pairInsertions, int maxDepth = 10)
        {
            if (depth == maxDepth)
            {
                return new Dictionary<string, long>() { { pairInsertions[pair], 1 } };
            }

            if (dp[depth].ContainsKey(pair))
            {
                return dp[depth][pair];
            }

            string left = pair[0] + pairInsertions[pair];
            string right =pairInsertions[pair] + pair[1];

            var l = BuildPolymer(depth + 1, left, pairInsertions, maxDepth);
            var r =BuildPolymer(depth + 1, right, pairInsertions, maxDepth);

            var res = new Dictionary<string, long>();

            foreach ((string k, long v) in l)
            {
                if (!res.ContainsKey(k))
                {
                    res[k] = 0;
                }

                res[k] += v;
            }

            foreach ((string k, long v) in r)
            {
                if (!res.ContainsKey(k))
                {
                    res[k] = 0;
                }

                res[k] += v;
            }

            if (!res.ContainsKey(pairInsertions[pair]))
            {
                res[pairInsertions[pair]] = 0;
            }
            res[pairInsertions[pair]]++;

            dp[depth][pair] = res;

            return res;
        }
    }
}
