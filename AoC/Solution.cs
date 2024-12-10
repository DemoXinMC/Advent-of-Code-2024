namespace AoC
{
    public class Solution
    {
        public Solution(int allDefault = 0)
        {
            if (allDefault == 0)
                return;

            FileRead = new(allDefault);
            Construction = new(allDefault);
            Setup = new(allDefault);
            Part1Time = new(allDefault);
            Part2Time = new(allDefault);
        }

        public string Name = "";
        public TimeSpan FileRead;
        public TimeSpan Construction;
        public TimeSpan Setup;
        public string Part1 = "";
        public string Part2 = "";
        public TimeSpan Part1Time;
        public TimeSpan Part2Time;
        public TimeSpan TotalTimeWithFile { get => FileRead + TotalTime; }
        public TimeSpan TotalTime { get => Setup + Part1Time + Part2Time; }
    }
}
