using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day10
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day10.txt");

            List<string> lines = new();

            while (!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine());
            }

            int total = lines.Sum(GetCorruptedValue);

            Console.WriteLine(total);
        }

        public int GetCorruptedValue(string line)
        {
            Stack<char> stack = new Stack<char>();

            foreach (char c in line)
            {
                switch (c)
                {
                    case '(':
                    case '[':
                    case '{':
                    case '<':
                        stack.Push(c);
                        break;

                    case ')':
                        if (stack.Count > 0 && stack.Peek() == '(')
                        {
                            stack.Pop();
                        }
                        else
                        {
                            return 3;
                        }
                        break;

                    case ']':
                        if (stack.Count > 0 && stack.Peek() == '[')
                        {
                            stack.Pop();
                        }
                        else
                        {
                            return 57;
                        }
                        break;

                    case '}':
                        if (stack.Count > 0 && stack.Peek() == '{')
                        {
                            stack.Pop();
                        }
                        else
                        {
                            return 1197;
                        }
                        break;

                    case '>':
                        if (stack.Count > 0 && stack.Peek() == '<')
                        {
                            stack.Pop();
                        }
                        else
                        {
                            return 25137;
                        }
                        break;

                    default:
                        break;
                }
            }

            return 0;
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day10.txt");

            List<string> lines = new();

            while (!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine());
            }

            var scores = lines
                .Where(l => GetCorruptedValue(l) == 0)
                .Select(GetIncompleteValue)
                .ToList();

            scores.Sort();

            Console.WriteLine(scores[(scores.Count - 1) / 2]);
        }


        public long GetIncompleteValue(string line)
        {
            Stack<char> stack = new Stack<char>();

            foreach (char c in line)
            {
                switch (c)
                {
                    case '(':
                    case '[':
                    case '{':
                    case '<':
                        stack.Push(c);
                        break;

                    case ')':
                        if (stack.Count > 0 && stack.Peek() == '(')
                        {
                            stack.Pop();
                        }
                        else
                        {
                            return 0;
                        }
                        break;

                    case ']':
                        if (stack.Count > 0 && stack.Peek() == '[')
                        {
                            stack.Pop();
                        }
                        else
                        {
                            return 0;
                        }
                        break;

                    case '}':
                        if (stack.Count > 0 && stack.Peek() == '{')
                        {
                            stack.Pop();
                        }
                        else
                        {
                            return 0;
                        }
                        break;

                    case '>':
                        if (stack.Count > 0 && stack.Peek() == '<')
                        {
                            stack.Pop();
                        }
                        else
                        {
                            return 0;
                        }
                        break;

                    default:
                        break;
                }
            }

            long score = 0;
            while (stack.Count > 0)
            {
                score *= 5;
                char c = stack.Pop();

                switch (c)
                {
                    case '(':
                        score += 1;
                        break;
                    case '[':
                        score += 2;
                        break;
                    case '{':
                        score += 3;
                        break;
                    case '<':
                        score += 4;
                        break;
                    default:
                        break;
                }
            }

            return score;
        }
    }
}
