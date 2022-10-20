using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day4
    {
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day4.txt");

            string line = reader.ReadLine();
            var order = line.Split(',');

            List<string[]> boards = new List<string[]>();
            List<bool[]> marks = new List<bool[]>();

            while (!reader.EndOfStream)
            {
                var board = new string[25];

                reader.ReadLine();
                for (int i = 0; i < 5; i++)
                {
                    line = reader.ReadLine();
                    var nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < 5; j++)
                    {
                        board[5*i + j] = nums[j];
                    }
                }

                boards.Add(board);
                marks.Add(new bool[25]);
            }


            foreach (var ball in order)
            {
                for (int i = 0; i < boards.Count; i++)
                {
                    for (int j = 0; j < 25; j++)
                    {
                        if (boards[i][j] == ball)
                        {
                            marks[i][j] = true;

                            if (HasRow(marks[i]))
                            {
                                Console.WriteLine(GetSum(ball, boards[i], marks[i]));
                                return;
                            }
                        }
                    }
                }
            }
        }

        public bool HasRow(bool[] markedGoals)
        {
            int[] row = new int[5] { 0, 1, 2, 3, 4 };
            int[] col = new int[5] { 0, 5, 10, 15, 20 };

            for (int i = 0; i < 5; i++)
            {
                bool isMarked = true;
                foreach (var idx in row)
                {
                    if (!markedGoals[idx + (5*i)])
                    {
                        isMarked = false;
                        break;
                    }
                }

                if (isMarked)
                {
                    return true;
                }

                isMarked = true;
                foreach (var idx in col)
                {
                    if (!markedGoals[idx + i])
                    {
                        isMarked = false;
                        break;
                    }
                }

                if (isMarked)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetSum(string ball, string[] board, bool[] markedGoals)
        {
            int lastNum = int.Parse(ball);

            int sum = 0;

            for(int i = 0; i < 25; i++)
            {
                if (!markedGoals[i])
                {
                    sum += int.Parse(board[i]);
                }
            }

            return lastNum * sum;
        }


        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day4.txt");

            string line = reader.ReadLine();
            var order = line.Split(',');

            List<string[]> boards = new List<string[]>();
            List<bool[]> marks = new List<bool[]>();


            while (!reader.EndOfStream)
            {
                var board = new string[25];

                reader.ReadLine();
                for (int i = 0; i < 5; i++)
                {
                    line = reader.ReadLine();
                    var nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < 5; j++)
                    {
                        board[5 * i + j] = nums[j];
                    }
                }

                boards.Add(board);
                marks.Add(new bool[25]);
            }

            bool[] completed = new bool[boards.Count];
            int completedBoards = 0;
            int lastCompleted = 0;

            foreach (var ball in order)
            {
                for (int i = 0; i < boards.Count; i++)
                {
                    if (completed[i])
                    {
                        continue;
                    }

                    for (int j = 0; j < 25; j++)
                    {
                        if (boards[i][j] == ball)
                        {
                            marks[i][j] = true;

                            if (HasRow(marks[i]))
                            {
                                completed[i] = true;
                                completedBoards++;
                                lastCompleted = GetSum(ball, boards[i], marks[i]);
                            }
                        }
                    }
                }
            }

            Console.WriteLine(lastCompleted);
        }
    }
}
