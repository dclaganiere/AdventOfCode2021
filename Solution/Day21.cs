using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solution
{
    public class Day21
    {
        private class Player
        {
            public int Id { get; set; }
            public int Position { get; set; }
            public int Score { get; set; }

            public Player(int id, int position, int score = 0)
            {
                Id = id;
                Position = position;
                Score = score;
            }
        }

        private const string RegexString = "Player \\d starting position: (\\d+)";
        public void SolveA()
        {
            using StreamReader reader = new StreamReader("Input\\Day21.txt");

            List<Player> players = new(); 

            string line = reader.ReadLine();
            var matches = Regex.Match(line, RegexString);

            players.Add(new Player(1, int.Parse(matches.Groups[1].Value) - 1));

            line = reader.ReadLine();
            matches = Regex.Match(line, RegexString);

            players.Add(new Player(2, int.Parse(matches.Groups[1].Value) - 1));

            int dice = 0;
            int turn = 0;

            while (players.All(p => p.Score < 1000))
            {
                var player = players[turn % 2];
                int move = (dice % 100) + ((dice + 1) % 100) + ((dice + 2) % 100) + 3;

                player.Position = ((player.Position + move) % 10);
                player.Score += player.Position + 1;

                //Console.WriteLine($"Player {player.Id} rolls {(dice % 100) + 1} + {((dice + 1) % 100) + 1} + {((dice + 2) % 100) + 1} and moves to space {player.Position + 1} for a total score of {player.Score}.");

                dice = (dice + 3) % 100;
                turn++;
            }

            Player loser = players.First(p => p.Score < 1000);

            Console.WriteLine(loser.Score * 3 * turn);
        }

        public void SolveB()
        {
            using StreamReader reader = new StreamReader("Input\\Day21.txt");

            List<Player> players = new();

            string line = reader.ReadLine();
            var matches = Regex.Match(line, RegexString);

            players.Add(new Player(1, int.Parse(matches.Groups[1].Value) - 1));

            line = reader.ReadLine();
            matches = Regex.Match(line, RegexString);

            players.Add(new Player(2, int.Parse(matches.Groups[1].Value) - 1));

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        int roll = (i + j + k) + 3;

                        if(!rollOdds.ContainsKey(roll))
                        {
                            rollOdds[roll] = 0;
                        }

                        rollOdds[roll]++;
                    }
                }
            }

            var res = PlayTurn(players[0], players[1]);

            Console.WriteLine(res);
        }

        private Dictionary<(int p1p, int p1s, int p2p, int p2s), (long p1w, long p2w)> dp = new();
        private Dictionary<int, int> rollOdds = new();

        private (long p1w, long p2w) PlayTurn(Player p1, Player p2)
        {
            if (dp.ContainsKey((p1.Position, p1.Score, p2.Position, p2.Score)))
            {
                return dp[(p1.Position, p1.Score, p2.Position, p2.Score)];
            }

            if (p2.Score >= 21)
            {
                return (0, 1);
            }

            long p1w = 0;
            long p2w = 0;

            foreach((int roll, int odds) in rollOdds)
            {
                int newPos = (p1.Position + roll) % 10;
                int newScore = p1.Score + newPos + 1;
                var newP1 = new Player(p1.Id, newPos, newScore);
                var newP2 = new Player(p2.Id, p2.Position, p2.Score);

                // Switch turns by swapping players
                var res = PlayTurn(newP2, newP1);
                p1w += (res.p2w) * odds;
                p2w += (res.p1w) * odds;
            }

            dp[(p1.Position, p1.Score, p2.Position, p2.Score)] = (p1w, p2w);
            return (p1w, p2w);
        }
    }
}
