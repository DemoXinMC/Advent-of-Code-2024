
namespace AoC.Solvers
{
    public class Day10 : IPuzzle
    {
        public string Name { get => "Day 10"; }

        private int[,] Map = new int[1,1];
        private int XSize;
        private int YSize;
        private List<KeyValuePair<int, int>> Trailheads = [];
        private List<KeyValuePair<int, int>> Directions = [];

        private int Part2Solution = 0;

        public void Setup(List<string> data)
        {
            Directions = [
                    new KeyValuePair<int, int>(0 + 1, 0 + 0),
                    new KeyValuePair<int, int>(0 - 1, 0 + 0),
                    new KeyValuePair<int, int>(0 + 0, 0 + 1),
                    new KeyValuePair<int, int>(0 + 0, 0 - 1)
                ];
            XSize = data.Count;
            YSize = data[0].Length;

            Map = new int[XSize, YSize];

            for(int x = 0; x < XSize; x++)
            {
                for(int y = 0; y < YSize; y++)
                {
                    Map[x, y] = data[x][y] - '0';
                    if(Map[x, y] < 0 || Map[x, y] > 9)
                        throw new Exception(IPuzzle.EXCEPT_INVALID_INPUT);
                    if (Map[x, y] == 0)
                        Trailheads.Add(new KeyValuePair<int, int>(x, y));
                }
            }
        }

        public string SolvePart1()
        {
            int score = 0;
            foreach (var trailhead in Trailheads)
            {
                var solved = new bool[XSize, YSize];
                Part2Solution += AdvanceTrail(trailhead.Key, trailhead.Value, solved);

                for(int x = 0; x < XSize; x++)
                    for (int y = 0; y < YSize; y++)
                        if (solved[x, y])
                            score++;

            }

            return score.ToString();
        }

        public string SolvePart2()
        {
            return Part2Solution.ToString();
        }

        private int AdvanceTrail(int x, int y, bool[,] solved)
        {
            var elevation = GetElevation(x, y);
            if (elevation < 0)
                return 0;

            if (elevation == 9)
            {
                solved[x, y] = true;
                return 1;
            }

            int score = 0;
            foreach (var dir in Directions)
            {
                if (GetElevation(x + dir.Key, y + dir.Value) == elevation + 1)
                    score += AdvanceTrail(x + dir.Key, y + dir.Value, solved);
            }

            return score;
        }

        private int GetElevation(int x, int y)
        {
            if (x < 0 || y < 0 || x >= XSize || y >= YSize)
                return -1;
            return Map[x, y];
        }
    }
}
