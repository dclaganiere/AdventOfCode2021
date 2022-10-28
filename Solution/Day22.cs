using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AdventOfCode2021.Solution.Day22;

namespace AdventOfCode2021.Solution
{
    public class Day22
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

            public override string ToString()
            {
                return $"({x},{y},{z})";
            }
        }

        public class Step
        {
            public bool IsOn { get; set; }
            public Coordinate Start { get; set; }
            public Coordinate End { get; set; }

            public Step(string line)
            {
                var matches = Regex.Match(line, "(\\w+) x=(-?\\d+)..(-?\\d+),y=(-?\\d+)..(-?\\d+),z=(-?\\d+)..(-?\\d+)");

                IsOn = matches.Groups[1].Value == "on";

                int x0 = int.Parse(matches.Groups[2].Value);
                int x1 = int.Parse(matches.Groups[3].Value);
                int y0 = int.Parse(matches.Groups[4].Value);
                int y1 = int.Parse(matches.Groups[5].Value);
                int z0 = int.Parse(matches.Groups[6].Value);
                int z1 = int.Parse(matches.Groups[7].Value);

                Start = new Coordinate(x0, y0, z0);
                End = new Coordinate(x1, y1, z1);
            }

            public (int X0, int X1, int Y0, int Y1, int Z0, int Z1) Clamp()
            {
                int x0 = Math.Max(Start.x, -50);
                int x1 = Math.Min(End.x, 50);
                int y0 = Math.Max(Start.y, -50);
                int y1 = Math.Min(End.y, 50);
                int z0 = Math.Max(Start.z, -50);
                int z1 = Math.Min(End.z, 50);

                return (x0, x1, y0, y1, z0, z1);
            }

            public bool IsSuperset(Area a)
            {
                return Start.x <= a.Min.x
                    && End.x >= a.Max.x
                    && Start.y <= a.Min.y
                    && End.y >= a.Max.y
                    && Start.z <= a.Min.z
                    && End.z >= a.Max.z;
            }

            public override string ToString()
            {
                return $"{(IsOn ? "on" : "off")} x={Start.x}..{End.x},y={Start.y}..{End.y},z={Start.z}..{End.z}";
            }
        }

        public class Area
        {
            public Coordinate Min { get; set; }
            public Coordinate Max { get; set; }

            public Area (Step s)
            {
                Min = (s.Start.x, s.Start.y, s.Start.z);
                Max = (s.End.x, s.End.y, s.End.z);
            }

            public Area (Coordinate min, Coordinate max)
            {
                Min = min;
                Max = max;
            }

            public bool IsAffectedByStep(Step s)
            {
                return !((Min.x > s.Start.x && Min.x > s.End.x)
                    || (Max.x < s.Start.x && Max.x < s.End.x)
                    || (Min.y > s.Start.y && Min.y > s.End.y)
                    || (Max.y < s.Start.y && Max.y < s.End.y)
                    || (Min.z > s.Start.z && Min.z > s.End.z)
                    || (Max.z < s.Start.z && Max.z < s.End.z));
            }

            public bool IsSuperset(Area a)
            {
                return Min.x <= a.Min.x
                    && Max.x >= a.Max.x
                    && Min.y <= a.Min.y
                    && Max.y >= a.Max.y
                    && Min.z <= a.Min.z
                    && Max.z >= a.Max.z;
            }

            public ulong GetVolume()
            {
                if (Max.x < Min.x || Max.y < Min.y || Max.z < Min.z)
                {
                    return 0;
                }

                return (ulong)((Max.x - Min.x) + 1) * (ulong)((Max.y - Min.y) + 1) * (ulong)((Max.z - Min.z) + 1);
            }

            public List<Area> ApplyStep(Step s)
            {
                List<Area> result = new List<Area>();

                if (s.IsSuperset(this))
                {
                    return result;
                }

                List<int> xValues = new() { Min.x, Max.x, s.Start.x, s.End.x };
                xValues.Sort();

                List<int> yValues = new() { Min.y, Max.y, s.Start.y, s.End.y };
                yValues.Sort();

                List<int> zValues = new() { Min.z, Max.z, s.Start.z, s.End.z };
                zValues.Sort();

                List<(int x0, int x1)> xRanges = new() { (xValues[0], xValues[1] - 1), (xValues[1], xValues[2]), (xValues[2] + 1, xValues[3]) };
                List<(int y0, int y1)> yRanges = new() { (yValues[0], yValues[1] - 1), (yValues[1], yValues[2]), (yValues[2] + 1, yValues[3]) };
                List<(int z0, int z1)> zRanges = new() { (zValues[0], zValues[1] - 1), (zValues[1], zValues[2]), (zValues[2] + 1, zValues[3]) };


                foreach(var x in xRanges)
                {
                    foreach (var y in yRanges)
                    {
                        foreach (var z in zRanges)
                        {
                            var newArea = new Area((x.x0, y.y0, z.z0), (x.x1, y.y1, z.z1));

                            if (newArea.GetVolume() == 0
                                || (s.IsSuperset(newArea))
                                || (!this.IsSuperset(newArea)))
                            {
                                continue;
                            }

                            result.Add(newArea);
                        }
                    }
                }

                return result;
            }

            public override string ToString()
            {
                return $"x={Min.x}..{Max.x},y={Min.y}..{Max.y},z={Min.z}..{Max.z}";
            }
        }
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day22.txt");
            List<Step> steps = new();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                steps.Add(new Step(line));
            }

            bool[,,] cube = new bool[101, 101, 101];

            foreach (var step in steps)
            {
                var clamp = step.Clamp();

                for (int i = clamp.X0; i <= clamp.X1; i++)
                {
                    for (int j = clamp.Y0; j <= clamp.Y1; j++)
                    {
                        for (int k = clamp.Z0; k <= clamp.Z1; k++)
                        {
                            cube[i + 50, j + 50, k + 50] = step.IsOn;
                        }
                    }
                }
            }

            int total = 0;

            for (int i = 0; i < 101; i++)
            {
                for (int j = 0; j < 101; j++)
                {
                    for (int k = 0; k < 101; k++)
                    {
                        total += cube[i, j, k] ? 1 : 0;
                    }
                }
            }

            Console.WriteLine(total);

        }
        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day22.txt");
            List<Step> steps = new();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                steps.Add(new Step(line));
            }

            List<Area> areas = new();
            foreach(Step step in steps)
            {
                List<Area> newAreas = new();

                foreach(var area in areas)
                {
                    if (area.IsAffectedByStep(step))
                    {
                        var toAdd = area.ApplyStep(step);
                        newAreas.AddRange(toAdd);
                    }
                    else
                    {
                        newAreas.Add(area);
                    }
                }

                if (step.IsOn)
                {
                    newAreas.Add(new Area(step));
                }

                areas = newAreas;
            }

            ulong total = 0;

            foreach (Area area in areas)
            {
                total += area.GetVolume();
            }

            Console.WriteLine(total);
        }
    }
}
