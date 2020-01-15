using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeComponents
{
    public static class MazeUtilities
    {
        /// <summary>
        /// Creates/saves new bitmap image from original with solution path drawn
        /// </summary>
        /// <param name="maze">Maze created from existing image</param>
        public static void SaveSolution(Maze maze)
        {
            if (maze.SolutionFound)
            {
                var step = 0;
                var totalSteps = maze.Solution.Count;
                foreach (var (x, y) in maze.Solution)
                {
                    var gb = (int)Math.Floor(step / (float)totalSteps * 255);
                    var red = (int)Math.Floor((totalSteps - step) / (float)totalSteps * 255);
                    maze.bmp.SetPixel(x, y, Color.FromArgb(red, gb, gb));
                    step++;
                }

                var mazePath = Path.GetDirectoryName(maze.ImagePath);
                var mazeFileName = Path.GetFileName(maze.ImagePath);
                Directory.CreateDirectory(Path.Combine(mazePath, "CompletedMazes"));
                var newPath = Path.Combine(mazePath, "CompletedMazes", mazeFileName);
                maze.bmp.Save(newPath);
            }
        }

        /// <summary>
        /// Creates/saves new bitmap image from maze created by generator. Creates entire image.
        /// </summary>
        /// <param name="maze">Maze that includes all path points.</param>
        public static void SaveMaze(Maze maze)
        {

        }
    }
}
