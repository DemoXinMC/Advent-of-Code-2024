
using System.Reflection.Metadata.Ecma335;

namespace AoC.Solvers
{
    public class Day09 : IPuzzle
    {
        public string Name { get => "Day 9"; }

        private List<FileEntry>? Files { get; set; }
        private int DiskLength;
        private int PackedDiskLength;

        public void Setup(List<string> data)
        {
            if (data.Count != 1)
                throw new Exception(IPuzzle.EXCEPT_INVALID_INPUT);

            var input = data[0];

            Files = [];

            int[] parsed = new int[input.Length+1];

            for (int i = 0; i < input.Length; i++)
                parsed[i] = (int)(input[i] - '0');

            int id = 0;
            for (int i = 0; i < input.Length; i += 2)
            {
                var length = parsed[i];
                var after = parsed[i + 1];
                Files.Add(new FileEntry(id++, length, after));
                DiskLength += length + after;
                PackedDiskLength += length;
            }
        }

        public string SolvePart1()
        {
            if(Files == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            int[] disk = new int[DiskLength];
            for (int i = 0; i < DiskLength; i++)
                disk[i] = -1;

            int diskPointer = 0;

            Stack<Unpacked> unpacked = [];

            foreach(var file in Files)
            {
                unpacked.Push(new(file.ID, file.Length));
                for(int i = 0; i < file.Length; i++)
                {
                    disk[diskPointer + i] = file.ID;
                }
                diskPointer += file.Length + file.EmptyAfter;
            }

            for(int i = 0; i < PackedDiskLength; i++)
            {
                if (disk[i] != -1)
                    continue;

                if (!unpacked.TryPeek(out var toPack))
                    throw new Exception(IPuzzle.EXCEPT_INVALID_INPUT);

                disk[i] = toPack.Pack;

                if (toPack.Done)
                    unpacked.Pop();
            }

            long checksum = 0;
            for(int i = 0; i < PackedDiskLength; i++)
                checksum += i * disk[i];

            return checksum.ToString();
        }

        public string SolvePart2()
        {
            if(Files == null)
                throw new Exception(IPuzzle.EXCEPT_NO_SETUP);

            int[] disk = new int[DiskLength];
            for (int i = 0; i < DiskLength; i++)
                disk[i] = -1;

            Dictionary<FileEntry, int> original = [];
            LinkedList<KeyValuePair<int, int>> gaps = [];

            int diskPointer = 0;
            foreach (var file in Files)
            {
                original.Add(file, diskPointer);
                for (int i = 0; i < file.Length; i++)
                    disk[diskPointer + i] = file.ID;
                if (file.EmptyAfter > 0)
                    gaps.AddLast(new KeyValuePair<int, int>(diskPointer + file.Length, file.EmptyAfter));
                diskPointer += file.Length + file.EmptyAfter;
            }

            List<FileEntry> toPack = new(Files);
            toPack.Reverse();

            foreach (var file in toPack)
            {
                var originalIndex = original[file];
                LinkedListNode<KeyValuePair<int, int>>? target = null;
                var gap = gaps.First;
                while(gap != null)
                {
                    if (gap.Value.Key >= originalIndex)
                        break;
                    if (gap.Value.Value >= file.Length)
                    {
                        target = gap;
                        break;
                    }
                    gap = gap.Next;
                }

                if (target == null)
                    continue;

                if(target != null)
                {
                    for (int i = 0; i < file.Length; i++)
                    {
                        disk[target.Value.Key + i] = file.ID;
                        disk[original[file] + i] = -1;
                    }

                    var remaining = target.Value.Value - file.Length;

                    if (remaining > 0)
                        target.Value = new KeyValuePair<int, int>(target.Value.Key + file.Length, remaining);
                    else
                        gaps.Remove(target);
                }
            }

            long checksum = 0;
            for (int i = 0; i < diskPointer; i++)
            {
                if (disk[i] > 0)
                    checksum += i * disk[i];
            }

            return checksum.ToString();
        }
    }

    public class FileEntry(int id, int length, int emptyAfter)
    {
        public int ID { get; set; } = id;
        public int Length { get; set; } = length;
        public int EmptyAfter { get; set; } = emptyAfter;

        public override string ToString() => $"[{ID}]({Length})({EmptyAfter})";
    }

    public class Unpacked(int id, int remaining)
    {
        public int ID { get; set; } = id;
        public int Remaining { get; set; } = remaining;

        public int Pack
        {
            get
            {
                Remaining--;
                return ID;
            }
        }

        public bool Done { get => Remaining == 0; }
    }
}
