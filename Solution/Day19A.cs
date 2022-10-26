using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AdventOfCode2021.Solution.Day19;

namespace AdventOfCode2021.Solution
{
    public class Day19A
    {
        public class Coordinate : IComparable<Coordinate>, IEquatable<Coordinate>
        {

            public int x { get; set; }
            public int y { get; set; }
            public int z { get; set; }

            public Coordinate(string xyz)
            {
                var split = xyz.Split(',');

                x = int.Parse(split[0]);
                y = int.Parse(split[1]);
                z = int.Parse(split[2]);
            }

            public Coordinate(int x, int y, int z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public void Deconstruct(out int x, out int y, out int z)
            {
                x = this.x;
                y = this.y;
                z = this.z;
            }

            public static implicit operator (int, int, int)(Coordinate c) => (c.x, c.y, c.z);
            public static implicit operator Coordinate((int x, int y, int z) c) => new Coordinate(c.x, c.y, c.z);

            public static Coordinate operator +(Coordinate c1, Coordinate c2)
            {
                return new Coordinate(c1.x + c2.x, c1.y + c2.y, c1.z + c2.z);
            }

            public static Coordinate operator -(Coordinate c1, Coordinate c2)
            {
                return new Coordinate(c1.x - c2.x, c1.y - c2.y, c1.z - c2.z);
            }

            public override bool Equals(object? other)
            {
                if (other is Coordinate b)
                {
                    return this.x == b.x && this.y == b.y && this.z == b.z;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(x, y, z);
            }

            public Coordinate GetRotation(int rotation)
            {
                switch (rotation)
                {
                    case 0:
                        return this;
                    case 1:
                        return (-y, x, z);
                    case 2:
                        return (-x, -y, z);
                    case 3:
                        return (y, -x, z);
                    case 4:
                        return (y, x, -z);
                    case 5:
                        return (-x, y, -z);
                    case 6:
                        return (-y, -x, -z);
                    case 7:
                        return (x, -y, -z);
                    case 8:
                        return (y, z, x);
                    case 9:
                        return (-z, y, x);
                    case 10:
                        return (-y, -z, x);
                    case 11:
                        return (z, -y, x);
                    case 12:
                        return (z, y, -x);
                    case 13:
                        return (-y, z, -x);
                    case 14:
                        return (-z, -y, -x);
                    case 15:
                        return (y, -z, -x);
                    case 16:
                        return (z, x, y);
                    case 17:
                        return (-x, z, y);
                    case 18:
                        return (-z, -x, y);
                    case 19:
                        return (x, -z, y);
                    case 20:
                        return (x, z, -y);
                    case 21:
                        return (-z, x, -y);
                    case 22:
                        return (-x, -z, -y);
                    case 23:
                        return (z, -x, -y);
                    default:
                        return this;
                }
            }

            public int CompareTo(Coordinate? other)
            {
                if (other == null)
                {
                    return -1;
                }

                if (this.z != other.z)
                {
                    return this.z < other.z ? -1 : 1;
                }

                if (this.y != other.y)
                {
                    return this.y < other.y ? -1 : 1;
                }

                if (this.x != other.x)
                {
                    return this.x < other.x ? -1 : 1;
                }

                return 0;
            }

            public bool Equals(Coordinate? other)
            {
                return this.x == other?.x && this.y == other?.y && this.z == other?.z;
            }
        }

        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day19.txt");
            List<List<Coordinate>> scanners = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                var matches = Regex.Match(line, "--- scanner (\\d+) ---");
                int scannerNumber = int.Parse(matches.Groups[1].Value);
                List<Coordinate> beacons = new();

                string xyz = reader.ReadLine();
                while (!string.IsNullOrWhiteSpace(xyz))
                {
                    beacons.Add(new Coordinate(xyz));
                    xyz = reader.ReadLine();
                }

                scanners.Add(beacons);
            }

            bool[] visited = new bool[scanners.Count];
            while (visited.Count(v => !v) > 1)
            {
                (Coordinate c, int rot) combined = ((0, 0, 0), -1);

                for (int i = 0; i < scanners.Count; i++)
                {
                    if (visited[i])
                    {
                        continue;
                    }

                    for (int j = i + 1; j < scanners.Count; j++)
                    {
                        if (visited[j])
                        {
                            continue;
                        }

                        combined = CombineScanners(scanners[i], scanners[j]);

                        if (combined.rot != -1)
                        {
                            visited[j] = true;
                            //Console.WriteLine($"Scanners {i} and {j} align, remaining {visited.Count(v => !v)}");
                        }
                    }
                }
            }

            Console.WriteLine(scanners[0].Count);
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day19.txt");
            List<List<Coordinate>> scanners = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                var matches = Regex.Match(line, "--- scanner (\\d+) ---");
                int scannerNumber = int.Parse(matches.Groups[1].Value);
                List<Coordinate> beacons = new();

                string xyz = reader.ReadLine();
                while (!string.IsNullOrWhiteSpace(xyz))
                {
                    beacons.Add(new Coordinate(xyz));
                    xyz = reader.ReadLine();
                }

                scanners.Add(beacons);
            }

            bool[] visited = new bool[scanners.Count];
            Dictionary<int, Dictionary<int, (int r, Coordinate c)>> distances = new();
            for (int i = 0; i < scanners.Count; i++)
            {
                distances[i] = new();
            }

            while (visited.Count(v => !v) > 1)
            {
                (Coordinate c, int rot) combined = ((0, 0, 0), -1);

                for (int i = 0; i < scanners.Count; i++)
                {
                    if (visited[i])
                    {
                        continue;
                    }

                    for (int j = i + 1; j < scanners.Count; j++)
                    {
                        if (visited[j])
                        {
                            continue;
                        }

                        combined = CombineScanners(scanners[i], scanners[j]);

                        if (combined.rot != -1)
                        {
                            visited[j] = true;
                            distances[i][j] = (combined.rot, combined.c);
                            //Console.WriteLine($"Scanners {i} and {j} align, remaining {visited.Count(v => !v)}");
                        }
                    }
                }
            }

            Dictionary<int, Coordinate> positions = new();
            Stack<(int scanner, int parent, int rot)> toVisit = new();

            positions[0] = (0, 0, 0);
            foreach (var d in distances[0].Keys)
            {
                toVisit.Push((d, 0, 0));
            }

            while (toVisit.Count > 0)
            {
                var next = toVisit.Pop();

                var parentPos = positions[next.parent];
                var distance = distances[next.parent][next.scanner];

                positions[next.scanner] = parentPos + distance.c.GetRotation(next.rot);
                foreach (var d in distances[next.scanner].Keys)
                {
                    toVisit.Push((d, next.scanner, distance.r));
                }
            }

            int best = int.MinValue;
            for (int i = 0; i < positions.Count; i++)
            {
                var pi = positions[i];
                for (int j = i + 1; j < positions.Count; j++)
                {
                    var pj = positions[j];
                    int dist = Math.Abs(pi.x - pj.x) + Math.Abs(pi.y - pj.y) + Math.Abs(pi.z - pj.z);
                    best = Math.Max(best, dist);
                }
            }

            Console.WriteLine(best);
        }

        private (Coordinate offset, int rot) CombineScanners(List<Coordinate> s1, List<Coordinate> s2)
        {
            for (int rot = 0; rot < 24; rot++)
            {
                var rotatedS2 = s2.Select(b => b.GetRotation(rot)).ToList();

                Dictionary<Coordinate, int> offsets = new();

                for (int i = 0; i < s1.Count; i++)
                {
                    for (int j = 0; j < rotatedS2.Count; j++)
                    {
                        Coordinate offset = s1[i] - rotatedS2[j];

                        if (!offsets.ContainsKey(offset))
                        {
                            offsets[offset] = 0;
                        }

                        offsets[offset]++;
                    }
                }

                if (offsets.Any(o => o.Value >= 12))
                {
                    var c = offsets.First(o => o.Value >= 12).Key;

                    HashSet<Coordinate> hs1 = new HashSet<Coordinate>(s1);
                    var offsetS2 = rotatedS2.Select(b => b + c).ToList();
                    HashSet<Coordinate> hs2 = new HashSet<Coordinate>(offsetS2);

                    s1.AddRange(hs2.Except(hs1));

                    s2.Clear();

                    return (c, rot);
                }
            }

            return ((0, 0, 0), -1);
        }
    }
}
