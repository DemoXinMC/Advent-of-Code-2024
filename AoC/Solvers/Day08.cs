using System.Text;

namespace AoC.Solvers
{
    public class Day08 : IPuzzle
    {
        public string Name { get => "Day 8"; }

        private Dictionary<char, List<Coordinate>>? Antennas;
        private Dictionary<char, HashSet<Coordinate>>? RangeAntinodes;
        private Dictionary<char, HashSet<Coordinate>>? LineAntinodes;

        private int XSize;
        private int YSize;

        public void Setup(List<string> data)
        {
            Antennas = [];
            RangeAntinodes = [];
            LineAntinodes = [];

            XSize = data[0].Length;
            YSize = data.Count;

            for (int y = 0; y < YSize; y++)
            {
                if (data[y].Length != XSize)
                    throw new Exception($"{IPuzzle.EXCEPT_INVALID_INPUT} on Line {y + 1}");
            }

            for (var x = 0; x < XSize; x++)
            {
                for (var y = 0; y < YSize; y++)
                {
                    char parsing = data[y][x];
                    if (parsing == '.')
                        continue;

                    if (!Antennas.ContainsKey(parsing))
                    {
                        Antennas.Add(parsing, []);
                        RangeAntinodes.Add(parsing, []);
                        LineAntinodes.Add(parsing, []);
                    }

                    Antennas[parsing].Add(new (x,y));
                }
            }
        }

        public string SolvePart1()
        {
            if (Antennas == null || RangeAntinodes == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            foreach (var freq in Antennas)
                CalculateRangeAntinodes(freq.Key);

            HashSet<Coordinate> allAntinodes = [];

            foreach (var freq in RangeAntinodes)
                foreach (var antinode in freq.Value)
                    allAntinodes.Add(antinode);

            return allAntinodes.Count.ToString();
        }

        public string SolvePart2()
        {
            if (Antennas == null || RangeAntinodes == null || LineAntinodes == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            foreach (var freq in Antennas)
                CalculateLineAntinodes(freq.Key);

            HashSet<Coordinate> allAntinodes = [];

            foreach (var freq in LineAntinodes)
                foreach (var antinode in freq.Value)
                    allAntinodes.Add(antinode);

            foreach (var freq in RangeAntinodes)
                foreach (var antinode in freq.Value)
                    allAntinodes.Add(antinode);

            foreach(var freq in Antennas)
                foreach(var antenna in freq.Value)
                    allAntinodes.Add(antenna);

            return allAntinodes.Count.ToString();
        }

        public string BuildVisualization()
        {
            if (Antennas == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            List<StringBuilder> visualization = [];
            for (var y = 0; y < YSize; y++)
                visualization.Add(new StringBuilder(new string('.', XSize)));

            foreach(var freq in Antennas)
            {
                foreach(var antenna in freq.Value)
                    visualization[antenna.Y][antenna.X] = freq.Key;
            }

            return string.Join(Environment.NewLine, visualization);
        }

        public string BuildRangeVisualization(char freq)
        {
            if(Antennas == null || RangeAntinodes == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            if (!Antennas.TryGetValue(freq, out var antennas))
                throw new Exception($"Visualization for {freq} failed.");
            if (!RangeAntinodes.TryGetValue(freq, out var antinodes))
                throw new Exception($"Visualization for {freq} failed.");

            List<StringBuilder> visualization = [];
            for (var y = 0; y < YSize; y++)
                visualization.Add(new StringBuilder(new string('.', XSize)));

            foreach(var antenna in antennas)
                visualization[antenna.Y][antenna.X] = 'X';

            foreach (var antinode in antinodes)
                visualization[antinode.Y][antinode.X] = '0';

            return string.Join(Environment.NewLine, visualization);
        }

        public string BuildLineVisualization(char freq)
        {
            if (Antennas == null || LineAntinodes == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            if (!Antennas.TryGetValue(freq, out var antennas))
                throw new Exception($"Visualization for {freq} failed.");
            if (!LineAntinodes.TryGetValue(freq, out var antinodes))
                throw new Exception($"Visualization for {freq} failed.");

            List<StringBuilder> visualization = [];
            for (var y = 0; y < YSize; y++)
                visualization.Add(new StringBuilder(new string('.', XSize)));

            foreach (var antenna in antennas)
                visualization[antenna.Y][antenna.X] = 'O';

            foreach (var antinode in antinodes)
            {
                if (visualization[antinode.Y][antinode.X] == 'O')
                    visualization[antinode.Y][antinode.X] = '#';
                else
                    visualization[antinode.Y][antinode.X] = 'X';
            }

            visualization.Reverse();
            return string.Join(Environment.NewLine, visualization);
        }

        private void CalculateRangeAntinodes(char freq)
        {
            if (Antennas == null || RangeAntinodes == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            if (!Antennas.TryGetValue(freq, out var antennasList))
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            if (!RangeAntinodes.TryGetValue(freq, out var antinodeList))
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            if (antennasList == null || antinodeList == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            foreach (var antenna in antennasList)
            {
                foreach (var otherAntenna in antennasList)
                {
                    if (antenna == otherAntenna)
                        continue;

                    var antinode = otherAntenna.Rotate180(antenna);
                    if (antinode.X < 0 || antinode.X >= XSize)
                        continue;
                    if (antinode.Y < 0 || antinode.Y >= YSize)
                        continue;
                    antinodeList.Add(antinode);
                }
            }
        }

        private void CalculateLineAntinodes(char freq)
        {
            if (Antennas == null || LineAntinodes == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            if (!Antennas.TryGetValue(freq, out var antennasList))
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            if (!LineAntinodes.TryGetValue(freq, out var antinodeList))
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            if (antennasList == null || antinodeList == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            foreach (var antenna in antennasList)
            {
                foreach (var otherAntenna in antennasList)
                {
                    if (antenna == otherAntenna)
                        continue;

                    Coordinate offset = new Coordinate(otherAntenna.X - antenna.X, otherAntenna.Y - antenna.Y);

                    var current = antenna;
                    while(current.X >= 0 && current.X < XSize &&
                        current.Y >= 0 && current.Y < YSize)
                    {
                        antinodeList.Add(current);
                        current = current + offset;
                    }
                }
            }
        }

        public struct Coordinate(int x, int y)
        {
            public int X = x, Y = y;

            public Coordinate Rotate180(Coordinate around)
            {
                var xDiff = around.X - X;
                var yDiff = around.Y - Y;
                return new(around.X + xDiff, around.Y + yDiff);
            }

            public override int GetHashCode() => X & Y;

            public override readonly bool Equals(object? obj)
            {
                if (obj is not Coordinate other)
                    return false;
                return other.X == X && other.Y == Y;
            }

            public static bool operator ==(Coordinate a, Coordinate b) => a.X == b.X && a.Y == b.Y;
            public static bool operator !=(Coordinate a, Coordinate b) => a.X != b.X || a.Y != b.Y;
            public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.X + b.X, a.Y + b.Y);
            public static Coordinate operator -(Coordinate a, Coordinate b) => new(a.X - b.X, a.Y - b.Y);

            public override readonly string ToString() => $"[{X},{Y}]";
        }
    }
}
