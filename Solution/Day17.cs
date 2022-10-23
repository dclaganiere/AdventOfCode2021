using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day17
    {
        public class Range
        {
            public int min { get; set; }
            public int max { get; set; }

            public Range(string n)
            {
                var split = n[2..].Split("..");

                min = int.Parse(split[0]);
                max = int.Parse(split[1]);
            }
        }

        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day17.txt");

            string line = reader.ReadLine();

            var split = line[13..].Split(", ");
            var x = new Range(split[0]);
            var y = new Range(split[1]);

            Console.WriteLine($"{((y.min + 1) * (y.min)) / 2}");
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day17.txt");

            string line = reader.ReadLine();

            var split = line[13..].Split(", ");
            var x = new Range(split[0]);
            var y = new Range(split[1]);

            var horizonalVel = DetermineValidHorizonalVelocities(x);

            long count = DetermineValidVelocities(horizonalVel, x, y);

            Console.WriteLine(count);
        }

        private List<int> DetermineValidHorizonalVelocities(Range x)
        {
            List<int> horizonalVelocities = new List<int>();

            for (int i = 0; i <= x.max; i++)
            {
                int vel = i;
                int pos = 0;
                while (pos < x.max)
                {
                    pos += vel;
                    vel--;

                    if (pos >= x.min && pos <= x.max)
                    {
                        horizonalVelocities.Add(i);
                        pos = x.max + 1;
                    }
                }
            }

            return horizonalVelocities;
        }

        private long DetermineValidVelocities(List<int> horizontalVelocities, Range xRange, Range yRange)
        {
            long count = 0;

            foreach (int x0 in horizontalVelocities)
            {
                for (int y0 = yRange.min; y0 < Math.Abs(yRange.min); y0++)
                {
                    int x = 0;
                    int y = 0;
                    int xVel = x0;
                    int yVel = y0;

                    do
                    {
                        x += xVel;
                        xVel = Math.Max(xVel - 1, 0);

                        y += yVel;
                        yVel--;

                        if (x >= xRange.min && x <= xRange.max
                            && y >= yRange.min && y <= yRange.max)
                        {
                            count++;
                            x = xRange.max + 1;
                        }

                    } while (x <= xRange.max && y >= yRange.min);
                }
            }

            return count;
        }
    }
}
