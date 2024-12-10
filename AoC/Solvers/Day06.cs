using System.Numerics;

namespace AoC.Solvers
{
    public class Day06 : IPuzzle
    {
        public string Name { get => "Day 6"; }

        private bool[,]? Map;
        private Vector2? GuardStartPosition;
        private Facing GuardStartFacing;

        private List<PathPoint>? MainPath;

        public void Setup(List<string> data)
        {
            var xSize = data.Count;
            var ySize = 0;

            foreach (var line in data)
                ySize = Math.Max(ySize, line.Length);

            Map = new bool[xSize, ySize];

            GuardStartPosition = new Vector2(-1, -1);
            GuardStartFacing = Facing.END;

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    switch (data[x][y])
                    {
                        case '#': Map[x, y] = true; break;
                        case '.': break;
                        case '^':
                            GuardStartPosition = new(x, y);
                            GuardStartFacing = Facing.UP;
                            break;
                        case 'v':
                            GuardStartPosition = new(x, y);
                            GuardStartFacing = Facing.DOWN;
                            break;
                        case '>':
                            GuardStartPosition = new(x, y);
                            GuardStartFacing = Facing.RIGHT;
                            break;
                        case '<':
                            GuardStartPosition = new(x, y);
                            GuardStartFacing = Facing.LEFT;
                            break;
                    }
                }
            }

            if (GuardStartPosition == null || GuardStartFacing == Facing.END)
                throw new Exception(IPuzzle.EXCEPT_INVALID_INPUT);

            MainPath = GetPath(Map, new PathPoint(GuardStartPosition.Value, GuardStartFacing));
        }

        public string SolvePart1()
        {
            if (MainPath == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            var uniquePoints = new HashSet<Vector2>();
            foreach (var point in MainPath)
                uniquePoints.Add(point.StartPosition);

            return uniquePoints.Count.ToString();
        }

        public string SolvePart2()
        {
            if (Map == null || MainPath == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            var xSize = Map.GetLength(0);
            var ySize = Map.GetLength(1);
            var testMap = new bool[xSize, ySize];
            var loopMap = new bool[xSize, ySize];
            var checkMap = new bool[xSize, ySize];

            foreach (var point in MainPath)
            {
                if (point.ExitDirection == Facing.END)
                    break;

                Array.Copy(Map, testMap, Map.Length);

                var next = point.Next;
                if (next == null ||
                    next?.X < 0 || next?.Y < 0 ||
                    next?.X >= xSize || next?.Y >= ySize)
                    break;

                if (checkMap[(int)next.Value.X, (int)next.Value.Y])
                    continue;
                checkMap[(int)next.Value.X, (int)next.Value.Y] = true;

                testMap[(int)next.Value.X, (int)next.Value.Y] = true;

                var simPath = GetPath(testMap, MainPath.First());
                if (simPath.Last().ExitDirection == Facing.LOOP)
                    loopMap[(int)next.Value.X, (int)next.Value.Y] = true;
            }

            var totalLoops = 0;
            foreach (var loop in loopMap)
                if (loop)
                    totalLoops++;

            return totalLoops.ToString();
        }

        public static List<PathPoint> GetPath(bool[,] map, PathPoint start)
        {
            var result = new List<PathPoint>();

            var currX = (int)start.StartPosition.X;
            var currY = (int)start.StartPosition.Y;

            Facing currentFacing = start.ExitDirection;

            var xSize = map.GetLength(0);
            var ySize = map.GetLength(1);

            var exitDirections = new bool[xSize, ySize, 4];
            Array.Clear(exitDirections);

            while (true)
            {
                int destX = currX;
                int destY = currY;

                Vector2 current = new(currX, currY);

                switch (currentFacing)
                {
                    case Facing.UP: destX -= 1; break;
                    case Facing.DOWN: destX += 1; break;
                    case Facing.RIGHT: destY += 1; break;
                    case Facing.LEFT: destY -= 1; break;
                }

                if (destX < 0 || destY < 0 || destX >= xSize || destY >= ySize)
                {
                    result.Add(new PathPoint(current, Facing.END));
                    break;
                }

                if (map[destX, destY])
                {
                    currentFacing += 1;
                    if (currentFacing == Facing.END)
                        currentFacing = Facing.UP;
                    continue;
                }

                if (exitDirections[currX, currY, (int)currentFacing])
                {
                    result.Add(new PathPoint(current, Facing.LOOP));
                    break;
                }

                exitDirections[currX, currY, (int)currentFacing] = true;
                result.Add(new(current, currentFacing));

                currX = destX;
                currY = destY;
            }

            return result;
        }

        public class PathPoint(Vector2 pos, Facing dir)
        {
            public Vector2 StartPosition = pos;
            public Facing ExitDirection = dir;

            public Vector2? Next
            {
                get
                {
                    return ExitDirection switch
                    {
                        Facing.UP => new(StartPosition.X - 1, StartPosition.Y),
                        Facing.DOWN => new(StartPosition.X + 1, StartPosition.Y),
                        Facing.LEFT => new(StartPosition.X, StartPosition.Y - 1),
                        Facing.RIGHT => new(StartPosition.X, StartPosition.Y + 1),
                        _ => null
                    };
                }
            }
        }

        public enum Facing
        {
            UP,
            RIGHT,
            DOWN,
            LEFT,
            END,
            LOOP
        }
    }
}
