using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day18
    {
        public class Node
        {
            public int Value { get; set; }
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public Node? Parent { get; set; }
        }

        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day18.txt");

            List<Node> nodes = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                Node root = new Node();
                ParseLine(line, 1, root);
                nodes.Add(root);
            }

            Node result = nodes[0];

            foreach(Node n in nodes.Skip(1))
            {
                Node left = result;
                result = new Node()
                {
                    Left = left,
                    Right = n
                };

                left.Parent = result;
                n.Parent = result;

                bool isReduced = true;
                do
                {
                    isReduced = ReduceExplode(result, 0) && ReduceSplit(result);
                } while (!isReduced);
            }

            long total = CalculateNodeMagnitude(result);
            Console.WriteLine(total);
        }

        private int ParseLine(string line, int index, Node parent)
        {
            // Left
            if (line[index] == '[')
            {
                parent.Left = new Node()
                {
                    Parent = parent
                };

                index = ParseLine(line, index + 1, parent.Left);
            }
            else
            {
                parent.Left = new Node()
                {
                    Parent = parent,
                    Value = line[index++] - '0'
                };
            }

            // Skip comma
            index++;

            // Right
            if (line[index] == '[')
            {
                parent.Right = new Node()
                {
                    Parent = parent
                };

                index = ParseLine(line, index + 1, parent.Right);
            }
            else
            {
                parent.Right = new Node()
                {
                    Parent = parent,
                    Value = line[index++] - '0'
                };
            }

            index++;

            return index;
        }

        private bool ReduceExplode(Node n, int depth)
        {
            if (n == null)
            {
                return true;
            }

            if (depth == 4 && n.Left != null && n.Right != null)
            {
                // Update left value
                var parent = n;
                while (parent.Parent?.Left == parent)
                {
                    parent = parent.Parent;
                }
                parent = parent.Parent;

                if (parent != null)
                {
                    var left = parent.Left;

                    while (left.Right != null)
                    {
                        left = left.Right;
                    }

                    left.Value += n.Left.Value;
                }

                // Update right value
                parent = n;
                while (parent.Parent?.Right == parent)
                {
                    parent = parent.Parent;
                }
                parent = parent.Parent;

                if (parent != null)
                {
                    var right = parent.Right;

                    while (right.Left != null)
                    {
                        right = right.Left;
                    }

                    right.Value += n.Right.Value;
                }

                n.Left = null;
                n.Right = null;
                n.Value = 0;

                return false;
            }

            return ReduceExplode(n.Left, depth + 1) && ReduceExplode(n.Right, depth + 1);
        }

        private bool ReduceSplit(Node n)
        {
            if (n == null)
            {
                return true;
            }

            if (n.Left == null && n.Right == null)
            {
                if (n.Value > 9)
                {
                    n.Left = new Node()
                    {
                        Parent = n,
                        Value = n.Value / 2
                    };

                    n.Right = new Node()
                    {
                        Parent = n,
                        Value = n.Value - n.Left.Value
                    };

                    n.Value = 0;

                    return false;
                }
            }

            return ReduceSplit(n.Left) && ReduceSplit(n.Right);
        }

        private void PrintNode(Node n)
        {
            if (n.Left == null && n.Right == null)
            {
                Console.Write(n.Value);
            }
            else
            {
                Console.Write("[");
                PrintNode(n.Left);
                Console.Write(",");
                PrintNode(n.Right);
                Console.Write("]");
            }
        }

        private long CalculateNodeMagnitude(Node n)
        {
            if (n.Left == null && n.Right == null)
            {
                return n.Value;
            }
            else
            {
                return (3 * CalculateNodeMagnitude(n.Left)) + (2 * CalculateNodeMagnitude(n.Right));
            }
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day18.txt");

            List<Node> nodes = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                Node root = new Node();
                ParseLine(line, 1, root);
                nodes.Add(root);
            }

            long best = 0;

            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    Node node = new();
                    node.Left = CopyNode(nodes[i], node);
                    node.Right = CopyNode(nodes[j], node);

                    bool isReduced = true;
                    do
                    {
                        isReduced = ReduceExplode(node, 0) && ReduceSplit(node);
                    } while (!isReduced);

                    long test = CalculateNodeMagnitude(node);
                    if (test > best)
                    {
                        best = test;
                    }
                }
            }

            Console.WriteLine(best);
        }

        private Node? CopyNode(Node? n, Node? parent = null)
        {
            if (n == null)
            {
                return null;
            }

            Node res = new()
            {
                Parent = parent
            };

            res.Left = CopyNode(n.Left, res);
            res.Right = CopyNode(n.Right, res);
            res.Value = n.Value;

            return res;
        }
    }
}
