using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public class Maze
    {
        private string _imagePath;
        public Bitmap bmp { get; }
        public MazeNode Start { get; }
        public MazeNode End { get; }
        public int Width { get; }
        public int Height { get; }
        public List<MazeNode> AllNodes { get; }
        public bool SolutionFound { get; set; } = false;
        public List<(int x, int y)> Solution { get; set; }
        public const string White = "ffffffff";
        public bool IsPath(int x, int y) => bmp.GetPixel(x, y).Name == White;
        public Maze(string path)
        {
            _imagePath = path;
            bmp = new Bitmap(_imagePath);
            Width = bmp.Width;
            Height = bmp.Height;
            var a = new MinHeap<MazeNode>();

            for (int x = 1; x < bmp.Width - 1; x++)
            {
                if (Start == null && IsPath(x, 0)) Start = new MazeNode(x, 0);
                if (End == null && IsPath(x, Height - 1)) End = new MazeNode(x, Height - 1);
            }
            AllNodes = new List<MazeNode>();

            Solution = new List<(int, int)>();
            SolutionFound = Solve(Start);
        }


        public bool Solve(MazeNode mn)
        {
            return false;
        }

        public void SaveSolution()
        {
            if (SolutionFound)
            {

                var step = 0;
                var totalSteps = Solution.Count;
                foreach (var (x, y) in Solution)
                {
                    var gb = (int)Math.Floor(step / (float)totalSteps * 255);
                    var red = (int)Math.Floor((totalSteps - step) / (float)totalSteps * 255);
                    bmp.SetPixel(x, y, Color.FromArgb(red, gb, gb));
                    step++;
                }

                var mazePath = Path.GetDirectoryName(_imagePath);
                var mazeFileName = Path.GetFileName(_imagePath);
                Directory.CreateDirectory(Path.Combine(mazePath, "CompletedMazes"));
                var newPath = Path.Combine(mazePath, "CompletedMazes", mazeFileName);
                bmp.Save(newPath);
            }
        }
    }
}