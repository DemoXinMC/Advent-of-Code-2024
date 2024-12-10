namespace AoC.Solvers
{
    public class Day01 : IPuzzle
    {
        public string Name { get => "Day 1"; }

        private List<int>? LeftList;
        private List<int>? RightList;

        public void Setup(List<string> data)
        {
            LeftList = [];
            RightList = [];
            LeftList.Capacity = data.Count;
            RightList.Capacity = data.Count;

            foreach(var item in data)
            {
                var numbers = item.Split("   ");
                if (numbers.Length != 2)
                    throw new Exception($"{IPuzzle.EXCEPT_INVALID_INPUT}: {item}");
                if (!int.TryParse(numbers[0], out var left))
                    throw new Exception($"{IPuzzle.EXCEPT_INVALID_INPUT}: {numbers[0]} on {item}");
                if (!int.TryParse(numbers[1], out var right))
                    throw new Exception($"{IPuzzle.EXCEPT_INVALID_INPUT}: {numbers[1]} on {item}");
                LeftList.Add(left);
                RightList.Add(right);
            }

            if (LeftList.Count != RightList.Count)
                throw new Exception($"{IPuzzle.EXCEPT_INVALID_INPUT}: Left and Right are different lengths.");

            LeftList.Sort();
            RightList.Sort();
        }

        public string SolvePart1()
        {
            if (LeftList == null || RightList == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            var dist = 0;
            for (int i = 0; i < LeftList.Count; i++)
                dist += Math.Abs(LeftList[i] - RightList[i]);

            return dist.ToString();
        }

        public string SolvePart2()
        {
            if (LeftList == null || RightList == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            var counts = new Dictionary<int, int>();
            foreach (int number in RightList)
            {
                if(counts.TryGetValue(number, out int current))
                    counts[number]++;
                else
                    counts.Add(number, 1);
            }

            var simScore = 0;
            foreach (int number in LeftList)
            {
                if(counts.TryGetValue(number, out int count))
                    simScore += number * count;
            }

            return simScore.ToString();
        }
    }
}
