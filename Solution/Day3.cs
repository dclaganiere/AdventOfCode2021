using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day3
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day3.txt");

            int[] bitCounts = new int[12];
            int readings = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (line?.Length != 12)
                {
                    break;
                }

                var arr = line.ToCharArray();
                readings++;
                for (int i = 0; i < 12; i++)
                {
                    if (arr[i] == '1')
                    {
                        bitCounts[i]++;
                    }
                }
            }

            long gamma = 0;
            long epsilon = 0;

            for (int i = 0; i < 12; i++)
            {
                gamma <<= 1;
                epsilon <<= 1;

                if ((bitCounts[i] * 2) < readings)
                {
                    epsilon++;
                }
                else
                {
                    gamma++;
                }
            }

            Console.WriteLine(gamma * epsilon);
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day3.txt");

            int bitCount = 0;
            List<string> readings = new List<string>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (line?.Length != 12)
                {
                    break;
                }

                readings.Add(line);
                var arr = line.ToCharArray();

                if (arr[0] == '1')
                {
                    bitCount++;
                }
            }

            string mostCommon = (bitCount * 2) >= readings.Count ? "1" : "0";

            var oxygen = readings.Where(x => x.StartsWith(mostCommon)).ToList();
            var carbon = readings.Where(x => !x.StartsWith(mostCommon)).ToList();

            for (int i = 1; i < 12; i++)
            {
                if (oxygen.Count == 1)
                {
                    break;
                }

                bitCount = 0;
                foreach (var line in oxygen)
                {
                    var arr = line.ToCharArray();

                    if (arr[i] == '1')
                    {
                        bitCount++;
                    }
                }

                mostCommon += (bitCount * 2) >= oxygen.Count ? "1" : "0";
                oxygen = oxygen.Where(x => x.StartsWith(mostCommon)).ToList();
            }

            mostCommon = (mostCommon[..1] == "1") ? "0" : "1";

            for (int i = 1; i < 12; i++)
            {
                if (carbon.Count == 1)
                {
                    break;
                }

                bitCount = 0;
                foreach (var line in carbon)
                {
                    var arr = line.ToCharArray();

                    if (arr[i] == '1')
                    {
                        bitCount++;
                    }
                }

                mostCommon += (bitCount * 2) >= carbon.Count ? "0" : "1";
                carbon = carbon.Where(x => x.StartsWith(mostCommon)).ToList();

            }

            long o = Convert.ToInt64(oxygen[0], 2);
            long c = Convert.ToInt64(carbon[0], 2);

            Console.WriteLine(o * c);
        }
    }
}
