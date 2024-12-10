namespace AoC.Solvers
{
    public class Day07 : IPuzzle
    {
        public string Name { get; private set; } = "Day 7";
        private List<PuzzleEntry>? PuzzleEntries { get; set; }

        private List<PuzzleEntry>? Part1Fails { get; set; }
        private long Part1Result = 0;

        public void Setup(List<string> data)
        {
            PuzzleEntries = [];
            PuzzleEntries.Capacity = data.Count;
            Part1Fails = [];

            foreach (var item in data)
            {
                var entry = new PuzzleEntry(item);
                PuzzleEntries.Add(entry);
            }
        }

        public string SolvePart1()
        {
            if (PuzzleEntries == null || Part1Fails == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            long sumOfPassed = 0;
            foreach (var entry in PuzzleEntries)
            {
                if (CheckFormulaPart1(entry, 1, entry.Numbers[0]))
                    sumOfPassed += entry.Result;
                else
                    Part1Fails.Add(entry);   
            }

            Part1Result = sumOfPassed;
            return sumOfPassed.ToString();
        }

        public string SolvePart2()
        {
            if (Part1Fails == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            long sumOfPassed = 0;
            foreach (var entry in Part1Fails)
            {
                if (CheckFormulaPart2(entry, 1, entry.Numbers[0]))
                    sumOfPassed += entry.Result;
            }

            long total = sumOfPassed + Part1Result;
            return total.ToString();
        }

        private static bool CheckFormulaPart1(PuzzleEntry entry, int step, long current)
        {
            long addition = current + entry.Numbers[step];
            long multiplication = current * entry.Numbers[step];
            var next = step + 1;
            if (next == entry.Numbers.Count)
                return (addition == entry.Result ||
                    multiplication == entry.Result);
            else
                return (CheckFormulaPart1(entry, next, addition) ||
                    CheckFormulaPart1(entry, next, multiplication));
        }

        private static bool CheckFormulaPart2(PuzzleEntry entry, int step, long current)
        {
            if(current > entry.Result)
                return false;
            var next = step + 1;

            long addition;
            long multiplication;
            long concat;

            if (next == entry.Numbers.Count)
            {
                addition = current + entry.Numbers[step];
                if (addition == entry.Result)
                    return true;

                multiplication = current * entry.Numbers[step];
                if (multiplication == entry.Result)
                    return true;

                concat = current * FasterMultiplier(entry.Numbers[step]) + entry.Numbers[step];
                if (concat == entry.Result)
                    return true;

                return false;
            }

            addition = current + entry.Numbers[step];
            if (CheckFormulaPart2(entry, next, addition))
                return true;

            multiplication = current * entry.Numbers[step];
            if (CheckFormulaPart2(entry, next, multiplication))
                return true;

            concat = current * FasterMultiplier(entry.Numbers[step]) + entry.Numbers[step];
            if (CheckFormulaPart2(entry, next, concat))
                return true;

            return false;
        }

        public static long Multiplier(long n) => n switch
        {
            < 10L => 10,
            < 100L => 100,
            < 1000L => 1000,
            < 10000L => 10000,
            < 100000L => 100000,
            < 1000000L => 1000000,
            < 10000000L => 10000000,
            < 100000000L => 100000000,
            < 1000000000L => 1000000000,
            < 10000000000L => 10000000000,
            < 100000000000L => 100000000000,
            < 1000000000000L => 1000000000000,
            < 10000000000000L => 10000000000000,
            < 100000000000000L => 100000000000000,
            < 1000000000000000L => 1000000000000000,
            < 10000000000000000L => 10000000000000000,
            < 100000000000000000L => 100000000000000000,
            < 1000000000000000000L => 1000000000000000000,
            _ => -1
        };

        private static long FasterMultiplier(long n)
        {
            // many of the digits are max. 3 long
            if (n < 10L)
                return 10;
            if (n < 100L)
                return 100;
            if (n < 1000L)
                return 1000;

            if (n < 1000000000000L)
            { // 5-12
                if (n < 100000000L)
                { // 5-8
                    if (n < 1000000L)
                    { // 5-6
                        if (n < 100000L)
                        {
                            return 100000;
                        }
                        else
                        {
                            return 1000000;
                        }
                    }
                    else
                    { // 7 - 8
                        if (n < 10000000L)
                        {
                            return 10000000;
                        }
                        else
                        {
                            return 100000000;
                        }
                    }
                }
                else
                { // 9 - 12
                    if (n < 10000000000L)
                    { // 9-10
                        if (n < 1000000000L)
                        {
                            return 1000000000;
                        }
                        else
                        {
                            return 10000000000;
                        }
                    }
                    else
                    { // 11-12
                        if (n < 100000000000L)
                        {
                            return 100000000000;
                        }
                        else
                        {
                            return 1000000000000;
                        }
                    }
                }
            }
            else
            { // 13-19
                if (n < 10000000000000000L)
                {// 13-16
                    if (n < 100000000000000L)
                    { // 13-14
                        if (n < 10000000000000L)
                        {
                            return 10000000000000;
                        }
                        else
                        {
                            return 100000000000000;
                        }
                    }
                    else
                    {//15-16
                        if (n < 1000000000000000)
                        {
                            return 1000000000000000;
                        }
                        else
                        {
                            return 10000000000000000;
                        }
                    }
                }
                else
                { // 17 - 19
                    if (n < 1000000000000000000)
                    { // 17-18
                        if (n < 100000000000000000)
                        {
                            return 100000000000000000;
                        }
                        else
                        {
                            return 1000000000000000000;
                        }
                    }
                    else
                    { // 19
                        return -1;
                    }
                }
            }
        }
    }

    public class PuzzleEntry
    {
        public long Result;
        public List<long> Numbers = [];

        public PuzzleEntry(string line)
        {
            var resultSplit = line.Split(':');
            if (resultSplit.Length != 2)
                throw new Exception($"Invalid Puzzle Entry: {line}");

            if (!long.TryParse(resultSplit[0], out Result))
                throw new Exception($"Invalid Puzzle Entry (Result): {line}");

            var numbers = resultSplit[1].Trim().Split(" ");
            foreach (var number in numbers)
            {
                if (!int.TryParse(number, out var parsed))
                    throw new Exception($"Invalid Puzzle Entry (Numbers): {line} [{number}]");
                Numbers.Add(parsed);
            }
        }

        public override string ToString() => string.Concat([Result.ToString(), ": ", string.Join(" ", Numbers)]);
    }
}
