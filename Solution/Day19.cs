using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day19
    {
        public class Beacon : IComparable<Beacon>, IEquatable<Beacon>
        {
            public Beacon GetRotation(int rotation)
            {
                switch (rotation)
                {
                    case 0:
                        return this;
                    case 1:
                        return new Beacon(-y, x, z);
                    case 2:
                        return new Beacon(-x, -y, z);
                    case 3:
                        return new Beacon(y, -x, z);
                    case 4:
                        return new Beacon(y, x, -z);
                    case 5:
                        return new Beacon(-x, y, -z);
                    case 6:
                        return new Beacon(-y, -x, -z);
                    case 7:
                        return new Beacon(x, -y, -z);
                    case 8:
                        return new Beacon(y, z, x);
                    case 9:
                        return new Beacon(-z, y, x);
                    case 10:
                        return new Beacon(-y, -z, x);
                    case 11:
                        return new Beacon(z, -y, x);
                    case 12:
                        return new Beacon(z, y, -x);
                    case 13:
                        return new Beacon(-y, z, -x);
                    case 14:
                        return new Beacon(-z, -y, -x);
                    case 15:
                        return new Beacon(y, -z, -x);
                    case 16:
                        return new Beacon(z, x, y);
                    case 17:
                        return new Beacon(-x, z, y);
                    case 18:
                        return new Beacon(-z, -x, y);
                    case 19:
                        return new Beacon(x, -z, y);
                    case 20:
                        return new Beacon(x, z, -y);
                    case 21:
                        return new Beacon(-z, x, -y);
                    case 22:
                        return new Beacon(-x, -z, -y);
                    case 23:
                        return new Beacon(z, -x, -y);
                    default:
                        return this;
                }
            }

            public int CompareTo(Beacon? other)
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

            public bool Equals(Beacon? other)
            {
                return this.x == other?.x && this.y == other?.y && this.z == other?.z;
            }

            public int x { get; set; }
            public int y { get; set; }
            public int z { get; set; }

            public Beacon(string xyz)
            {
                var split = xyz.Split(',');

                x = int.Parse(split[0]);
                y = int.Parse(split[1]);
                z = int.Parse(split[2]);
            }

            public Beacon(int x, int y, int z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public override bool Equals(object? other)
            {
                if (other is Beacon b)
                {
                    return this.x == b.x && this.y == b.y && this.z == b.z;
                }

                return false;
            }

            public override int GetHashCode()
            {
                int hash = 17;

                hash = hash * 23 + this.x;
                hash = hash * 23 + this.y;
                hash = hash * 23 + this.z;

                return hash;
            }
        }

        private Random rng = new Random();

        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day19.txt");
            List<List<Beacon>> scanners = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                var matches = Regex.Match(line, "--- scanner (\\d+) ---");
                int scannerNumber = int.Parse(matches.Groups[1].Value);
                List<Beacon> beacons = new();

                string xyz = reader.ReadLine();
                while (!string.IsNullOrWhiteSpace(xyz))
                {
                    beacons.Add(new Beacon(xyz));
                    xyz = reader.ReadLine();
                }

                scanners.Add(beacons);
            }

            bool[] visited = new bool[scanners.Count];
            while (visited.Count(v => !v) > 1)
            {
                (int x, int y, int z, int rot) combined = (0, 0, 0, -1);

                for (int j = 1; j < scanners.Count; j++)
                {
                    if (visited[j])
                    {
                        continue;
                    }

                    combined = CombineScanners(scanners[0], scanners[j]);

                    if (combined.rot != -1)
                    {
                        visited[j] = true;
                        Console.WriteLine($"Scanners {0} and {j} align, remaining {visited.Count(v => !v)}");
                    }
                }
            }

            Console.WriteLine(scanners[0].Count);
        }

        private (int x, int y, int z) GetOffset(Beacon beacon1, Beacon beacon2)
        {
            return (beacon1.x - beacon2.x, beacon1.y - beacon2.y, beacon1.z - beacon2.z);
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day19.txt");
            List<List<Beacon>> scanners = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                var matches = Regex.Match(line, "--- scanner (\\d+) ---");
                int scannerNumber = int.Parse(matches.Groups[1].Value);
                List<Beacon> beacons = new();

                string xyz = reader.ReadLine();
                while (!string.IsNullOrWhiteSpace(xyz))
                {
                    beacons.Add(new Beacon(xyz));
                    xyz = reader.ReadLine();
                }

                scanners.Add(beacons);
            }

            bool[] visited = new bool[scanners.Count];
            Dictionary<int, Dictionary<int, (int r, int x, int y, int z)>> distances = new();
            for (int i = 0; i < scanners.Count; i++)
            {
                distances[i] = new Dictionary<int, (int r, int x, int y, int z)>();
            }

            while (visited.Count(v => !v) > 1)
            {
                (int x, int y, int z, int rot) combined = (0, 0, 0, -1);

                for (int j = 1; j < scanners.Count; j++)
                {
                    if (visited[j])
                    {
                        continue;
                    }

                    combined = CombineScanners(scanners[0], scanners[j]);

                    if (combined.rot != -1)
                    {
                        visited[j] = true;
                        distances[0][j] = (combined.rot, combined.x, combined.y, combined.z);
                        Console.WriteLine($"Scanners {0} and {j} align, remaining {visited.Count(v => !v)}");
                    }
                }
            }

            Dictionary<int, (int x, int y, int z)> positions = new();
            Stack<(int scanner, int parent)> toVisit = new();

            positions[0] = (0, 0, 0);
            foreach (var d in distances[0].Keys)
            {
                toVisit.Push((d, 0));
            }

            while (toVisit.Count > 0)
            {
                var next = toVisit.Pop();

                var parentPos = positions[next.parent];
                var distance = distances[next.parent][next.scanner];

                positions[next.scanner] = (parentPos.x + distance.x, parentPos.y + distance.y, parentPos.z + distance.z);
                foreach (var d in distances[next.scanner].Keys)
                {
                    toVisit.Push((d, next.scanner));
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

        private (int x, int y, int z, int rot) CombineScanners(List<Beacon> s1, List<Beacon> s2)
        {
            HashSet<Beacon> hs1 = new HashSet<Beacon>(s1);

            int idx = rng.Next(s2.Count);

            for (int i = 0; i < 24; i++)
            {
                var rotatedS2 = s2.Select(b => b.GetRotation(i)).ToList();

                for (int k = 0; k < s1.Count; k++)
                {
                    var (x, y, z) = GetOffset(s1[k], rotatedS2[idx]);

                    var offsetS2 = rotatedS2.Select(b => new Beacon(b.x + x, b.y + y, b.z + z)).ToList();
                    HashSet<Beacon> hs2 = new HashSet<Beacon>(offsetS2);

                    if (hs1.Intersect(hs2).Count() >= 12)
                    {
                        s1.AddRange(hs2.Except(hs1));

                        s2.Clear();

                        return (x, y, z, i);
                    }
                }
            }

            return (0, 0, 0, -1);
        }
    }
}
