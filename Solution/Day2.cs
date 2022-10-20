using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day2
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day2.txt");

            int depth = 0;
            int distance = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var split = line.Split(" ");

                if (split.Length != 2)
                {
                    break;
                }

                int amt = int.Parse(split[1]);

                switch (split[0])
                {
                    case "forward":
                        distance += amt;
                        break;
                    case "up":
                        depth -= amt;
                        break;
                    case "down":
                        depth += amt;
                        break;
                    default:
                        break;

                }
            }

            Console.WriteLine(depth * distance);
        }
        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day2.txt");

            int depth = 0;
            int distance = 0;
            int aim = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var split = line.Split(" ");

                if (split.Length != 2)
                {
                    break;
                }

                int amt = int.Parse(split[1]);

                switch (split[0])
                {
                    case "forward":
                        distance += amt;
                        depth += (amt * aim);
                        break;
                    case "up":
                        aim -= amt;
                        break;
                    case "down":
                        aim += amt;
                        break;
                    default:
                        break;

                }

                //Console.WriteLine($"Input: {line} | Dist: {distance} | Depth: {depth} | Aim: {aim}");
                //Console.ReadLine();
            }

            Console.WriteLine(depth * distance);
        }
    }
}
