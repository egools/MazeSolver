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

            for (int x = 1; x < bmp.Width - 1; x++)
            {
                if (Start == null && IsPath(x, 0)) Start = new MazeNode(x, 0);
                if (End == null && IsPath(x, Height - 1)) End = new MazeNode(x, Height - 1);
            }
            AllNodes = new List<MazeNode>();
            FindNeighbors(Start);

            Solution = new List<(int, int)>();
            SolutionFound = Solve(Start);
        }

        public void FindNeighbors(MazeNode mn)
        {
            AllNodes.Add(mn);
            mn.Neighbors.Add(CheckNeighbor(mn.Position.X, mn.Position.Y - 1));
            mn.Neighbors.Add(CheckNeighbor(mn.Position.X - 1, mn.Position.Y));
            mn.Neighbors.Add(CheckNeighbor(mn.Position.X, mn.Position.Y + 1));
            mn.Neighbors.Add(CheckNeighbor(mn.Position.X + 1, mn.Position.Y));
            mn.Neighbors.RemoveAll(n => n == null);

            foreach (var n in mn.Neighbors)
            {
                if (!AllNodes.Contains(n))
                    FindNeighbors(n);
            }
        }

        public MazeNode CheckNeighbor(int x, int y)
        {
            var tmp = new MazeNode(x, y);
            if (!AllNodes.Contains(tmp))
            {
                if (x >= 0 && y >= 0 && x < Width && y < Height && IsPath(x, y))
                    return tmp;
                else
                    return null;
            }
            else
            {
                return AllNodes.First(n => n == tmp);
            }
        }

        public bool Solve(MazeNode mn)
        {
            mn.Visited = true;
            if (mn == End)
                mn.InSolution = true;
            foreach (var neighbor in mn.Neighbors)
            {
                if (!neighbor.Visited)
                    mn.InSolution = Solve(neighbor) || mn.InSolution;
            }
            if (mn.InSolution)
                Solution.Add(mn.Position);
            return mn.InSolution;
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