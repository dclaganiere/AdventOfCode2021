using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day1
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day1.txt");

            int count = 0;
            int curr = int.Parse(reader.ReadLine());
            int prev = 0;

            while (!reader.EndOfStream)
            {
                prev = curr;
                curr = int.Parse(reader.ReadLine());

                count += curr > prev ? 1 : 0;
            }

            Console.WriteLine(count);
        }
        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day1.txt");

            int[] depths = new int[4];
            depths[0] = int.Parse(reader.ReadLine());
            depths[1] = int.Parse(reader.ReadLine());
            depths[2] = int.Parse(reader.ReadLine());

            int count = 0;
            int reading = 2;

            int curr = depths[0] + depths[1] + depths[2];
            int prev = 0;

            while (!reader.EndOfStream)
            {
                reading++;

                prev = curr;
                depths[reading % 4] = int.Parse(reader.ReadLine());

                curr += depths[reading % 4];
                curr -= depths[(reading + 1) % 4];

                count += curr > prev ? 1 : 0;
            }

            Console.WriteLine(count);
        }
    }
}
