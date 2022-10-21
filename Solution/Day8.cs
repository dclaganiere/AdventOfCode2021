using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day8
    {
        public class Display
        {
            public char[] segments = new char[7];
            public HashSet<char>[] digits = new HashSet<char>[10];

            public void SolveInput(string input)
            {
                List<HashSet<char>> displayValues = input
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => new HashSet<char>(i.ToCharArray()))
                    .ToList();

                //Knowns
                digits[1] = displayValues.Single(v => v.Count == 2);
                digits[4] = displayValues.Single(v => v.Count == 4);
                digits[7] = displayValues.Single(v => v.Count == 3);
                digits[8] = displayValues.Single(v => v.Count == 7);

                List<HashSet<char>> fives = displayValues.Where(v => v.Count == 5).ToList();
                List<HashSet<char>> sixes = displayValues.Where(v => v.Count == 6).ToList();

                segments[0] = digits[7].Except(digits[1]).Single();

                digits[3] = fives.Single(f => digits[1].IsProperSubsetOf(f));
                digits[9] = sixes.Single(s => digits[4].IsProperSubsetOf(s));

                segments[4] = digits[8].Except(digits[9]).Single();

                digits[2] = fives.Single(f => f.Contains(segments[4]));

                segments[2] = digits[1].Intersect(digits[2]).Single();

                digits[6] = sixes.Single(s => digits[8].Except(s).Contains(segments[2]));

                digits[0] = sixes.Single(f => f != digits[6] && f != digits[9]);
                digits[5] = fives.Single(f => f != digits[2] && f != digits[3]);

                // Ignores rest of segments, oh well
            }

            public int ReadOutput(string output)
            {
                List<HashSet<char>> displayValues = output
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => new HashSet<char>(i.ToCharArray()))
                    .ToList();

                int result = 0;
                foreach(var value in displayValues)
                {
                    result *= 10;

                    for (int i = 0; i < digits.Length; i++)
                    {
                        var digit = digits[i];
                        if (digit.IsSubsetOf(value) && value.IsSubsetOf(digit))
                        {
                            result += i;
                            break;
                        }
                    }
                }

                return result;
            }
        }

        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day8.txt");

            var total = 0;
            HashSet<int> uniqueDisplays = new HashSet<int> { 2, 3, 4, 7 };

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var io = line.Split('|', StringSplitOptions.RemoveEmptyEntries);

                string output = io[1].Trim();
                var digits = output.Split(' ', StringSplitOptions.RemoveEmptyEntries);


                total += digits.Count(d => uniqueDisplays.Contains(d.Length));
            }

            Console.WriteLine(total);

        }
        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day8.txt");

            var total = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var io = line.Split('|', StringSplitOptions.RemoveEmptyEntries);

                string input = io[0].Trim();
                string output = io[1].Trim();

                var display = new Display();
                display.SolveInput(input);
                total += display.ReadOutput(output);
            }

            Console.WriteLine(total);
        }
    }
}
