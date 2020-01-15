using MazeComponents;
using MazeSolver;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MazeApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "";
            if (args.Length > 0)
                filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File Not Found");
                filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"MazeImages\braid200.bmp");
            }
            Console.WriteLine($"Using maze image file: {Path.GetFileName(filePath)}");

            Stopwatch s = new Stopwatch();
            s.Start();
            var maze = new Maze(filePath);
            Solver.SolveAStar(maze);
            if (maze.SolutionFound)
                Console.WriteLine($"Solution found in {maze.Solution.Count} steps.");
            else
                Console.WriteLine($"No solution found.");

            MazeUtilities.SaveSolution(maze);
            s.Stop();
            Console.WriteLine("Time Elapsed: " + s.Elapsed.ToString(@"mm\:ss\.fff"));

            Console.WriteLine("Press Enter to close...");
            Console.ReadLine();
        }
    }
}
