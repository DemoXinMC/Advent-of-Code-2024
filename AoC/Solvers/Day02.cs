namespace AoC.Solvers
{
    public class Day02 : IPuzzle
    {
        public string Name { get => "Day 2"; }

        private List<List<int>>? Reports { get; set; }
        private List<List<int>>? Part1Fails { get; set; }
        private int Part1Result = 0;

        public void Setup(List<string> data)
        {
            Reports = [];
            Reports.Capacity = data.Count;
            Part1Fails = [];

            foreach (var item in data)
            {
                var report = new List<int>();
                var numbers = item.Split(' ');
                foreach (var number in numbers)
                {
                    if (!int.TryParse(number, out var parsed))
                        throw new Exception($"{IPuzzle.EXCEPT_INVALID_INPUT}: {number} in {item}");
                    report.Add(parsed);
                }
                Reports.Add(report);
            }
        }

        public string SolvePart1()
        {
            if (Reports == null || Part1Fails == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            var validReports = 0;
            foreach (var report in Reports)
            {
                if(ValidateReport(report))
                    validReports++;
                else
                    Part1Fails.Add(report);
            }

            Part1Result = validReports;
            return validReports.ToString();
        }

        public string SolvePart2()
        {
            if (Part1Fails == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            var validReports = 0;
            foreach(var report in Part1Fails)
            {
                for (int i = 0; i < report.Count; i++)
                {
                    var testList = new List<int>(report);
                    testList.RemoveAt(i);
                    if(ValidateReport(testList))
                    {
                        validReports++;
                        break;
                    }
                }
            }

            var total = validReports + Part1Result;
            return total.ToString();
        }

        private static bool ValidateReport(List<int> report)
        {
            bool increasing = true;

            if (report.First() > report.Last())
                increasing = false;

            for (int i = 0; i < report.Count - 1; i++)
            {
                var diff = Math.Abs(report[i] - report[i+1]);
                if (diff < 1 || diff > 3)
                    return false;

                if (increasing && report[i] > report[i+1])
                    return false;
                if (!increasing && report[i] < report[i+1])
                    return false;
            }

            return true;
        }
    }
}
