using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
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
                filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"MazeImages\mazeTest2.bmp");
            }
            Console.WriteLine($"Using maze image file: {Path.GetFileName(filePath)}");

            var maze = new Maze(filePath);
            maze.Solve();
            if (maze.SolutionFound)
                Console.WriteLine($"Solution found in {maze.Solution.Count} steps.");
            else
                Console.WriteLine($"No solution found.");

            maze.SaveSolution();

            Console.WriteLine("Press Enter to close...");
            Console.ReadLine();
        }
    }
}
