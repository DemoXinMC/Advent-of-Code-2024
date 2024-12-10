using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace AoC
{
    public interface IPuzzle
    {
        public const string EXCEPT_INVALID_INPUT = "Invalid Input";
        public const string EXCEPT_NO_SETUP = "Invalid Setup";
        public const string UNSOLVED = "Unsolved";
        public string Name { get; }
        public void Setup(List<string> data);
        public string SolvePart1() => UNSOLVED;
        public string SolvePart2() => UNSOLVED;
    }
}
