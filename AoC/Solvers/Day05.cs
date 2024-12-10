namespace AoC.Solvers
{
    public class Day05 : IPuzzle
    {
        public string Name { get => "Day 5"; }

        private Dictionary<int, List<int>>? Rules;
        private List<List<int>>? Records;

        private List<List<int>>? Part1Fails;

        public void Setup(List<string> data)
        {
            Rules = [];
            Records = [];
            Part1Fails = [];

            bool rules = true;
            foreach (var line in data)
            {
                if (rules && string.IsNullOrEmpty(line))
                {
                    rules = false;
                    continue;
                }

                if (rules)
                    ParseRule(line);
                else
                    ParseRecord(line);
            }
        }

        public string SolvePart1()
        {
            if (Records == null || Part1Fails == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            var sumOfMids = 0;
            foreach(var record in Records)
            {
                if(ValidateRecord(record))
                {
                    int middleIndex = (record.Count / 2);
                    sumOfMids += record[middleIndex];
                    continue;
                }
                
                Part1Fails.Add(record);
            }

            return sumOfMids.ToString();
        }

        public string SolvePart2()
        {
            if (Part1Fails == null)
                throw new Exception (IPuzzle.EXCEPT_NO_SETUP);

            var sumOfMids = 0;
            foreach (var record in Part1Fails)
            {
                var correctedRecord = CorrectRecord(record);
                int middleIndex = (correctedRecord.Count / 2);
                sumOfMids += correctedRecord[middleIndex];
            }

            return sumOfMids.ToString();
        }

        private void ParseRule(string line)
        {
            if (Rules == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            var rule = line.Split('|');
            if (rule.Length != 2)
                throw new Exception($"{IPuzzle.EXCEPT_INVALID_INPUT}: Malformed {line}");

            if (!int.TryParse(rule[0], out int page) || !int.TryParse(rule[1], out int beforePage))
                throw new Exception($"{IPuzzle.EXCEPT_INVALID_INPUT}: Couldn't parse {line}");

            if (!Rules.ContainsKey(page))
                Rules.Add(page, []);
            Rules[page].Add(beforePage);
        }

        private void ParseRecord(string line)
        {
            if (Records == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            var record = line.Split(',');
            var parsedRecord = new List<int>();
            foreach (var item in record)
            {
                if (!int.TryParse(item, out int parsed))
                    throw new Exception($"{IPuzzle.EXCEPT_INVALID_INPUT}: Couldn't parse {item} on {line}");

                parsedRecord.Add(parsed);
            }

            Records.Add(parsedRecord);
        }

        private bool ValidateRecord(List<int> record)
        {
            if (Rules == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            for (int i = 0; i < record.Count; i++)
            {
                if (!Rules.ContainsKey(record[i]))
                    continue;
                var rules = Rules[record[i]];
                for (int j = 0; j < i; j++)
                {
                    if (rules.Contains(record[j]))
                        return false;
                }
            }

            return true;
        }

        private List<int> CorrectRecord(List<int> record)
        {
            if (Rules == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            var correctedRecord = new List<int>();
            var recordItems = new Queue<int>(record);
            while (recordItems.Count > 0)
            {
                var item = recordItems.Dequeue();
                if (!Rules.ContainsKey(item))
                {
                    correctedRecord.Add(item);
                    continue;
                }

                var canAdd = true;
                foreach (var rule in Rules[item])
                {
                    if (recordItems.Contains(rule))
                    {
                        canAdd = false;
                        break;
                    }
                }

                if (canAdd)
                    correctedRecord.Add(item);
                else
                    recordItems.Enqueue(item);
            }

            return correctedRecord;
        }
    }
}
