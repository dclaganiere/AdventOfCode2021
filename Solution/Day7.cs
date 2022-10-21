using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day7
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day7.txt");

            string input = reader.ReadToEnd();

            List<int> positions = input
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            int min = positions.Min();
            int max = positions.Max();

            int best = 0;
            int bestFuel = int.MaxValue;


            for(int i = min; i <= max; i++)
            {
                int fuel = CalculateFuelA(i, positions);

                if (fuel < bestFuel)
                {
                    bestFuel = fuel;
                    best = i;
                }
            }

            Console.WriteLine(bestFuel);
        }

        public int CalculateFuelA(int goal, List<int> positions)
        {
            return positions.Select(p => Math.Abs(goal - p)).Sum();
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day7.txt");

            string input = reader.ReadToEnd();

            List<int> positions = input
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            int min = positions.Min();
            int max = positions.Max();

            int best = 0;
            long bestFuel = long.MaxValue;


            for (int i = min; i <= max; i++)
            {
                long fuel = CalculateFuelB(i, positions);

                if (fuel < bestFuel)
                {
                    bestFuel = fuel;
                    best = i;
                }
            }

            Console.WriteLine(bestFuel);
        }

        public long CalculateFuelB(int goal, List<int> positions)
        {
            return positions.Select(p => GetFuelForDistanceB(goal - p)).Sum();
        }

        private long GetFuelForDistanceB(int distance)
        {
            distance = Math.Abs(distance);
            return (distance * (distance + 1)) / 2;
        }
    }
}
