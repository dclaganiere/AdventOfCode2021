using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day20
    {
        private const int extra = 100;

        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day20.txt");

            string line = reader.ReadLine();

            List<bool> enhancement = line.Select(l => l == '#').ToList();

            List<List<bool>> image = new List<List<bool>>();

            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();

                List<bool> row = Enumerable.Repeat(false, extra).ToList();
                row.AddRange(line.Select(l => l == '#'));
                row.AddRange(Enumerable.Repeat(false, extra));

                if (image.Count == 0)
                {
                    for (int i = 0; i < extra; i++)
                    {
                        image.Add(Enumerable.Repeat(false, row.Count).ToList());
                    }
                }

                image.Add(row);
            }

            for (int i = 0; i < extra; i++)
            {
                image.Add(Enumerable.Repeat(false, image[0].Count).ToList());
            }

            for (int i = 0; i < 2; i++)
            {
                image = EnhanceImage(image, enhancement, i % 2 == 0);
            }

            int total = 0;
            foreach(var row in image)
            {
                total += row.Where(r => r).Count();
            }

            Console.WriteLine(total);
        }

        private List<List<bool>> EnhanceImage(List<List<bool>> image, List<bool> enhancement, bool defaultVal)
        {
            List<(int r, int c)> window = new()
            {
                (-1, -1), (-1, 0), (-1, 1),
                ( 0, -1), ( 0, 0), ( 0, 1),
                ( 1, -1), ( 1, 0), ( 1, 1),
            };

            List<List<bool>> newImage = new();
            newImage.Add(Enumerable.Repeat(defaultVal, image[0].Count).ToList());

            for (int i = 1; i < image.Count - 1; i++)
            {
                List<bool> row = new();
                row.Add(defaultVal);

                for (int j = 1; j < image[0].Count - 1; j++)
                {
                    var pixel = string.Concat(window.Select(w => image[w.r + i][w.c + j] ? '1' : '0'));
                    var index = Convert.ToInt32(pixel, 2);
                    row.Add(enhancement[index]);
                }

                row.Add(defaultVal);
                newImage.Add(row);
            }

            newImage.Add(Enumerable.Repeat(defaultVal, image[0].Count).ToList());

            return newImage;
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day20.txt");

            string line = reader.ReadLine();

            List<bool> enhancement = line.Select(l => l == '#').ToList();

            List<List<bool>> image = new List<List<bool>>();

            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();

                List<bool> row = Enumerable.Repeat(false, extra).ToList();
                row.AddRange(line.Select(l => l == '#'));
                row.AddRange(Enumerable.Repeat(false, extra));

                if (image.Count == 0)
                {
                    for (int i = 0; i < extra; i++)
                    {
                        image.Add(Enumerable.Repeat(false, row.Count).ToList());
                    }
                }

                image.Add(row);
            }

            for (int i = 0; i < extra; i++)
            {
                image.Add(Enumerable.Repeat(false, image[0].Count).ToList());
            }

            for (int i = 0; i < 50; i++)
            {
                image = EnhanceImage(image, enhancement, i % 2 == 0);
            }

            int total = 0;
            foreach (var row in image)
            {
                total += row.Where(r => r).Count();
            }

            Console.WriteLine(total);
        }
    }
}
