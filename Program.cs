
using AdventOfCode2021.Solution;
using System.Diagnostics;

Day23 day = new();

Stopwatch sw = Stopwatch.StartNew();

day.SolveA();
day.SolveB();

sw.Stop();

Console.WriteLine(sw.Elapsed.ToString());