namespace AoC.Solvers
{
    public class Day04 : IPuzzle
    {
        public string Name { get => "Day 4"; }

        private char[,]? Puzzle;
        private List<KeyValuePair<int, int>>? Directions;

        public void Setup(List<string> data)
        {
            SetupDirections();

            int xSize = 0;
            int ySize = data.Count;

            foreach (var line in data)
                xSize = Math.Max(xSize, line.Length);

            Puzzle = new char[xSize, ySize];

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (x >= data[y].Length)
                        break;
                    Puzzle[x, y] = data[y][x];
                }
            }
        }

        public string SolvePart1()
        {
            if (Puzzle == null || Directions == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            var xSize = Puzzle.GetLength(0);
            var ySize = Puzzle.GetLength(1);

            var foundXmases = 0;
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (Puzzle[x, y] == 'X')
                    {
                        foreach (var dir in Directions)
                        {
                            if (CheckForXMAS(Puzzle, x, y, dir.Key, dir.Value))
                                foundXmases++;
                        }
                    }
                }
            }

            return foundXmases.ToString();
        }

        public string SolvePart2()
        {
            if (Puzzle == null || Directions == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            var xSize = Puzzle.GetLength(0);
            var ySize = Puzzle.GetLength(1);

            var foundXmases = 0;
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (Puzzle[x, y] == 'A')
                    {
                        if (CheckForCrossMAS(Puzzle, x, y))
                            foundXmases++;
                    }
                }
            }
            
            return foundXmases.ToString();
        }

        public static bool CheckForXMAS(char[,] puzzle, int xStart, int yStart, int xOffset, int yOffset)
        {
            if (xStart < 0 || yStart < 0)
                return false;

            if (xStart > puzzle.GetLength(0) - 1 || yStart > puzzle.GetLength(1) - 1)
                return false;

            if (xStart + (xOffset * 3) < 0 || xStart + (xOffset * 3) > puzzle.GetLength(0) - 1)
                return false;

            if (yStart + (yOffset * 3) < 0 || yStart + (yOffset * 3) > puzzle.GetLength(1) - 1)
                return false;

            char[] read = new char[4];
            for (int i = 0; i < 4; i++)
            {
                read[i] = puzzle[xStart + (xOffset * i), yStart + (yOffset * i)];
            }

            if (read[0] == 'X' &&
                read[1] == 'M' &&
                read[2] == 'A' &&
                read[3] == 'S')
                return true;

            return false;

        }

        public static bool CheckForCrossMAS(char[,] puzzle, int xStart, int yStart)
        {
            if (puzzle[xStart, yStart] != 'A')
                return false;
            if (xStart - 1 < 0 || yStart - 1 < 0)
                return false;
            if (xStart + 1 > puzzle.GetLength(0) - 1 || yStart + 1 > puzzle.GetLength(1) - 1)
                return false;

            char[,] read = new char[2, 2];
            read[0, 0] = puzzle[xStart - 1, yStart - 1];
            read[0, 1] = puzzle[xStart + 1, yStart - 1];
            read[1, 0] = puzzle[xStart - 1, yStart + 1];
            read[1, 1] = puzzle[xStart + 1, yStart + 1];

            if ((read[0, 0] == 'M' && read[1, 1] == 'S') || (read[0, 0] == 'S' && read[1, 1] == 'M'))
            {
                if ((read[0, 1] == 'M' && read[1, 0] == 'S') || (read[0, 1] == 'S' && read[1, 0] == 'M'))
                    return true;
            }

            return false;
        }

        private void SetupDirections()
        {
            Directions = [];
            Directions.Add(new KeyValuePair<int, int>(-1, -1));
            Directions.Add(new KeyValuePair<int, int>(-1, 0));
            Directions.Add(new KeyValuePair<int, int>(-1, 1));

            Directions.Add(new KeyValuePair<int, int>(0, -1));
            Directions.Add(new KeyValuePair<int, int>(0, 1));

            Directions.Add(new KeyValuePair<int, int>(1, -1));
            Directions.Add(new KeyValuePair<int, int>(1, 0));
            Directions.Add(new KeyValuePair<int, int>(1, 1));
        }
    }
}
