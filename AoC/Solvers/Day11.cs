using System.Collections.Generic;
using StoneType = long;

namespace AoC.Solvers
{
    public class Day11 : IPuzzle
    {
        public string Name { get => "Day 11"; }

        private Dictionary<StoneType, int> StoneCounts { get; set; } = [];
        private int DifferentStones = 0;

        public void Setup(List<string> data)
        {
            if(data.Count != 1)
                throw new Exception(IPuzzle.EXCEPT_INVALID_INPUT);

            var parts = data[0].Split(' ');
            foreach(var part in parts)
            {
                if (!StoneType.TryParse(part, out var parsed))
                    throw new Exception(IPuzzle.EXCEPT_INVALID_INPUT);

                if(!StoneCounts.ContainsKey(parsed))
                    StoneCounts[parsed] = 0;
                StoneCounts[parsed]++;
            }

            DifferentStones = StoneCounts.Count;
        }

        public string SolvePart1()
        {
            RunBlinks(25);
            return GetStoneCount().ToString();
        }

        public string SolvePart2()
        {
            RunBlinks(50);
            return GetStoneCount().ToString();
        }

        private long GetStoneCount()
        {
            long score = 0;
            foreach (var num in StoneCounts.Values)
                score += num;
            return score;
        }

        public void RunBlinks(int number)
        {
            for (int i = 0; i < number; i++)
                BlinkDict();
        }

        private void BlinkDict()
        {
            var ret = new Dictionary<StoneType, int>();
            ret.EnsureCapacity(DifferentStones * 2);
            foreach (var stone in StoneCounts)
            {
                if (stone.Key == 0)
                {
                    if (ret.TryGetValue(1, out var ones))
                        ret[1] = stone.Value + ones;
                    else
                        ret.Add(1, stone.Value);
                    continue;
                }
                    
                var numDigits = NumDigits(stone.Key);
                if(numDigits % 2 == 0)
                {
                    StoneType first = stone.Key / Divisors[numDigits];
                    StoneType second = stone.Key % Divisors[numDigits];

                    if (ret.TryGetValue(first, out var firstCount))
                        ret[first] = stone.Value + firstCount;
                    else
                        ret.Add(first, stone.Value);

                    if (ret.TryGetValue(second, out var secondCount))
                        ret[second] = stone.Value + secondCount;
                    else
                        ret.Add(second, stone.Value);

                    continue;
                }

                ret.Add(stone.Key * 2024, stone.Value);
            }

            DifferentStones = Math.Max(0, ret.Count);
            StoneCounts = ret;
        }

        public static int NumDigits(long n) => n switch
        {
            < 10L => 1,
            < 100L => 2,
            < 1000L => 3,
            < 10000L => 4,
            < 100000L => 5,
            < 1000000L => 6,
            < 10000000L => 7,
            < 100000000L => 8,
            < 1000000000L => 9,
            < 10000000000L => 10,
            < 100000000000L => 11,
            < 1000000000000L => 12,
            < 10000000000000L => 13,
            < 100000000000000L => 14,
            < 1000000000000000L => 15,
            < 10000000000000000L => 16,
            < 100000000000000000L => 17,
            < 1000000000000000000L => 18,
            _ => 19
        };

        private readonly long[] Divisors = [
            1,
            1,
            10,
            1,
            100,
            1,
            1000,
            1,
            10000,
            1,
            100000,
            1,
            1000000,
            1,
            10000000,
            1,
            100000000,
            1,
            1000000000,
            1,
            10000000000,
            1,
            100000000000,
            1,
            ];
    }
}
