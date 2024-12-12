using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace AoC
{
    public static class Benchmark
    {
        public static void UpdateConsole(int completedIterations)
        {
            Console.WriteLine($"Completed {completedIterations} iterations...");
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        public static void Run(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type solverType,
        string filename, int iterations, int iterUpdates)
        {
            Console.WriteLine($"Running benchmark of {solverType.Name} with {iterations} iterations...");

            Solution MinTimes = new(int.MaxValue);
            Solution MaxTimes = new();
            Solution TotalTimes = new();

            TimeSpan MinComplete = new(int.MaxValue);
            TimeSpan MaxComplete = TimeSpan.Zero;
            TimeSpan TotalComplete = TimeSpan.Zero;

            TimeSpan MinCompleteNF = new(int.MaxValue);
            TimeSpan MaxCompleteNF = TimeSpan.Zero;
            TimeSpan TotalCompleteNF = TimeSpan.Zero;

            var totalTime = new Stopwatch();
            totalTime.Start();

            var stopwatch = new Stopwatch();

            for (int i = 0; i < iterations; i++)
            {
                if (i % iterUpdates == 0)
                {
                    totalTime.Stop();
                    UpdateConsole(i);
                    totalTime.Start();
                }

                stopwatch.Restart();
                var currentTest = new Solution();
                if (Activator.CreateInstance(solverType) is not IPuzzle solver)
                    throw new Exception($"Couldn't Create Instance of {solverType.Name}");
                stopwatch.Stop();
                MinTimes.Construction = new(Math.Min(MinTimes.Construction.Ticks, stopwatch.ElapsedTicks));
                MaxTimes.Construction = new(Math.Max(MaxTimes.Construction.Ticks, stopwatch.ElapsedTicks));
                TotalTimes.Construction = new(TotalTimes.Construction.Ticks + stopwatch.ElapsedTicks);
                currentTest.Construction = stopwatch.Elapsed;

                stopwatch.Restart();
                var data = new List<string>(File.ReadAllLines(filename));
                stopwatch.Stop();
                MinTimes.FileRead = new(Math.Min(MinTimes.FileRead.Ticks, stopwatch.ElapsedTicks));
                MaxTimes.FileRead = new(Math.Max(MaxTimes.FileRead.Ticks, stopwatch.ElapsedTicks));
                TotalTimes.FileRead = new(TotalTimes.FileRead.Ticks + stopwatch.ElapsedTicks);
                currentTest.FileRead = stopwatch.Elapsed;

                stopwatch.Restart();
                solver.Setup(data);
                stopwatch.Stop();
                MinTimes.Setup = new(Math.Min(MinTimes.Setup.Ticks, stopwatch.ElapsedTicks));
                MaxTimes.Setup = new(Math.Max(MaxTimes.Setup.Ticks, stopwatch.ElapsedTicks));
                TotalTimes.Setup = new(TotalTimes.Setup.Ticks + stopwatch.ElapsedTicks);
                currentTest.Setup = stopwatch.Elapsed;

                stopwatch.Restart();
                solver.SolvePart1();
                stopwatch.Stop();
                MinTimes.Part1Time = new(Math.Min(MinTimes.Part1Time.Ticks, stopwatch.ElapsedTicks));
                MaxTimes.Part1Time = new(Math.Max(MaxTimes.Part1Time.Ticks, stopwatch.ElapsedTicks));
                TotalTimes.Part1Time = new(TotalTimes.Part1Time.Ticks + stopwatch.ElapsedTicks);
                currentTest.Setup = stopwatch.Elapsed;

                stopwatch.Restart();
                solver.SolvePart2();
                stopwatch.Stop();
                MinTimes.Part2Time = new(Math.Min(MinTimes.Part2Time.Ticks, stopwatch.ElapsedTicks));
                MaxTimes.Part2Time = new(Math.Max(MaxTimes.Part2Time.Ticks, stopwatch.ElapsedTicks));
                TotalTimes.Part2Time = new(TotalTimes.Part2Time.Ticks + stopwatch.ElapsedTicks);
                currentTest.Part2Time = stopwatch.Elapsed;

                MinComplete = new(Math.Min(MinComplete.Ticks, currentTest.TotalTimeWithFile.Ticks));
                MaxComplete = new(Math.Max(MaxComplete.Ticks, currentTest.TotalTimeWithFile.Ticks));
                TotalComplete = new(TotalComplete.Ticks + currentTest.TotalTimeWithFile.Ticks);

                MinCompleteNF = new(Math.Min(MinComplete.Ticks, currentTest.TotalTime.Ticks));
                MaxCompleteNF = new(Math.Max(MaxComplete.Ticks, currentTest.TotalTime.Ticks));
                TotalCompleteNF = new(TotalComplete.Ticks + currentTest.TotalTime.Ticks);
            }
            totalTime.Stop();

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine($"Benchmarking of {solverType.Name} [Iterations: {iterations}] ({Math.Round((double)totalTime.ElapsedMilliseconds / 1000, 3)}s)");

            Console.WriteLine($"\tConstruction ({TotalTimes.Construction.TotalMilliseconds}ms)");
            Console.WriteLine($"\t\tMin:  {MinTimes.Construction.TotalMilliseconds}ms");
            Console.WriteLine($"\t\tMax:  {MaxTimes.Construction.TotalMilliseconds}ms");
            Console.WriteLine($"\t\tMean: {Math.Round((double)TotalTimes.Construction.TotalMilliseconds / iterations, 4)}ms");

            Console.WriteLine($"\tFile Read ({TotalTimes.FileRead.TotalMilliseconds}ms)");
            Console.WriteLine($"\t\tMin:  {MinTimes.FileRead.TotalMilliseconds}ms");
            Console.WriteLine($"\t\tMax:  {MaxTimes.FileRead.TotalMilliseconds}ms");
            Console.WriteLine($"\t\tMean: {Math.Round((double)TotalTimes.FileRead.TotalMilliseconds / iterations, 4)}ms");

            Console.WriteLine($"\tSetup ({TotalTimes.Setup.TotalMilliseconds}ms)");
            Console.WriteLine($"\t\tMin:  {MinTimes.Setup.TotalMilliseconds}ms");
            Console.WriteLine($"\t\tMax:  {MaxTimes.Setup.TotalMilliseconds}ms");
            Console.WriteLine($"\t\tMean: {Math.Round((double)TotalTimes.Setup.TotalMilliseconds / iterations, 4)}ms");

            Console.WriteLine($"\tPart 1 ({TotalTimes.Part1Time.TotalMilliseconds}ms)");
            Console.WriteLine($"\t\tMin:  {MinTimes.Part1Time.TotalMilliseconds}ms");
            Console.WriteLine($"\t\tMax:  {MaxTimes.Part1Time.TotalMilliseconds}ms");
            Console.WriteLine($"\t\tMean: {Math.Round((double)TotalTimes.Part1Time.TotalMilliseconds / iterations, 4)}ms");

            Console.WriteLine($"\tPart 2 ({TotalTimes.Part2Time.TotalMilliseconds}ms)");
            Console.WriteLine($"\t\tMin:  {MinTimes.Part2Time.TotalMilliseconds}ms");
            Console.WriteLine($"\t\tMax:  {MaxTimes.Part2Time.TotalMilliseconds}ms");
            Console.WriteLine($"\t\tMean: {Math.Round((double)TotalTimes.Part2Time.TotalMilliseconds / iterations, 4)}ms");

            Console.WriteLine($"\tComplete Solve ({TotalComplete.TotalMilliseconds}ms)");
            Console.WriteLine($"\t\tMin:  {MinCompleteNF.TotalMilliseconds}ms ({MinComplete.TotalMilliseconds}ms)");
            Console.WriteLine($"\t\tMax:  {MaxCompleteNF.TotalMilliseconds}ms ({MaxComplete.TotalMilliseconds}ms)");
            Console.WriteLine($"\t\tMean: {Math.Round((double)TotalCompleteNF.TotalMilliseconds / iterations, 4)} ({Math.Round((double)TotalComplete.TotalMilliseconds / iterations, 4)}ms)");
        }
    }
}
