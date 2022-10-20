using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day6
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day6.txt");

            string line = reader.ReadLine();
            var fish = line.Split(',').Select(s => int.Parse(s)).ToList();

            int[] cycles = new int[9];

            foreach (var f in fish)
            {
                cycles[f]++;
            }

            for (int i = 1; i < 80; i++)
            {
                cycles[(i + 7) % 9] += cycles[i % 9];
            }

            Console.WriteLine(cycles.Sum());
        }
        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day6.txt");

            string line = reader.ReadLine();
            var fish = line.Split(',').Select(s => int.Parse(s)).ToList();

            long[] cycles = new long[9];

            foreach (var f in fish)
            {
                cycles[f]++;
            }

            for (int i = 1; i < 256; i++)
            {
                cycles[(i + 7) % 9] += cycles[i % 9];
            }

            Console.WriteLine(cycles.Sum());
        }
    }
}
