using AoC.Solvers;
using System.Diagnostics;

namespace AoC
{
    internal class Program
    {
        private static readonly List<IPuzzle> solvers = [];
        private static readonly Dictionary<IPuzzle, string> files = [];
        private static readonly Dictionary<IPuzzle, Solution> solutions = [];

        static void Main(string[] args)
        {
            // AddOldSolvers();
            RunSetup();
            RunPart1();
            RunPart2();

            Benchmark.Run(typeof(Day10), "input/day10.txt", 20000, 1000);

            RunOutput();
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }

        private static void AddOldSolvers()
        {
            AddSolver(new Day01(), "input/day01.txt");
            AddSolver(new Day02(), "input/day02.txt");
            AddSolver(new Day03(), "input/day03.txt"); // TODO
            AddSolver(new Day04(), "input/day04.txt");
            AddSolver(new Day05(), "input/day05.txt");
            AddSolver(new Day06(), "input/day06.txt");
            AddSolver(new Day07(), "input/day07.txt");
            AddSolver(new Day08(), "input/day08.txt");
            AddSolver(new Day09(), "input/day09.txt");
            AddSolver(new Day10(), "input/day10.txt");
        }

        private static void AddSolver(IPuzzle solver, string inputFile)
        {
            solvers.Add(solver);
            files.Add(solver, inputFile);
            solutions.Add(solver, new Solution());
        }

        private static void RunSetup()
        {
            var stopwatch = new Stopwatch();
            foreach (var solver in solvers)
            {
                stopwatch.Restart();
                var data = new List<string>(File.ReadAllLines(files[solver]));
                stopwatch.Stop();
                solutions[solver].FileRead = stopwatch.Elapsed;
                stopwatch.Restart();
                solver.Setup(data);
                stopwatch.Stop();
                solutions[solver].Setup = stopwatch.Elapsed;
                solutions[solver].Name = solver.Name;
            }
        }

        private static void RunPart1()
        {
            var stopwatch = new Stopwatch();
            foreach (var solver in solvers)
            {
                stopwatch.Restart();
                solutions[solver].Part1 = solver.SolvePart1();
                stopwatch.Stop();
                solutions[solver].Part1Time = stopwatch.Elapsed;
            }
        }

        private static void RunPart2()
        {
            var stopwatch = new Stopwatch();
            foreach (var solver in solvers)
            {
                stopwatch.Restart();
                solutions[solver].Part2 = solver.SolvePart2();
                stopwatch.Stop();
                solutions[solver].Part2Time = stopwatch.Elapsed;
            }
        }

        private static void RunOutput()
        {
            foreach (var entry in solutions)
            {
                var solution = entry.Value;
                Console.WriteLine($"{solution.Name}");
                Console.WriteLine($"  Solve Time: {solution.TotalTime.TotalMilliseconds}ms");
                Console.WriteLine($"  Solve + File: {solution.TotalTimeWithFile.TotalMilliseconds}ms");
                Console.WriteLine($"\tSetup ({solution.Setup.TotalMilliseconds}ms)");
                Console.WriteLine($"\tPart 1 ({solution.Part1Time.TotalMilliseconds}ms): {solution.Part1} ");
                Console.WriteLine($"\tPart 2 ({solution.Part2Time.TotalMilliseconds}ms): {solution.Part2} ");
                Console.WriteLine();
            }
        }
    }
}
