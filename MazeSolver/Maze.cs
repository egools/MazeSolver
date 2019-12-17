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
        public bool IsPath(int x, int y)
        {
            if (x < 0 || y < 0)
                return false;
            else
                return bmp.GetPixel(x, y).Name == White;
        }
        public Maze(string path)
        {
            _imagePath = path;
            bmp = new Bitmap(_imagePath);
            Width = bmp.Width;
            Height = bmp.Height;
            AllNodes = new List<MazeNode>();

            for (int x = 1; x < bmp.Width - 1; x++)
            {
                if (Start == null && IsPath(x, 0)) Start = new MazeNode(x, 0);
                if (End == null && IsPath(x, Height - 1)) End = new MazeNode(x, Height - 1);
            }
            Start.SetDistanceFromEnd(End);
            End.SetDistanceFromEnd(End);
            Start.SetDistanceFromStart(Start);
            End.SetDistanceFromStart(Start);
        }


        public void Solve()
        {
            var nodeQueue = new PriorityQueue<MazeNode>();
            nodeQueue.Enqueue(Start);
            while (!nodeQueue.IsEmpty && !SolutionFound)
            {
                var currentNode = nodeQueue.Dequeue();
                currentNode.Visited = true;
                if (IsPath(currentNode.Position.X + 1, currentNode.Position.Y)) //right
                {
                    var tmp = new MazeNode(currentNode.Position.X + 1, currentNode.Position.Y);
                    tmp.SetDistanceFromStart(Start);
                    tmp.SetDistanceFromEnd(End);
                    if (!AllNodes.Contains(tmp))
                    {
                        AllNodes.Add(tmp);
                        nodeQueue.Enqueue(tmp);
                    }
                    SolutionFound = SolutionFound || tmp.DistanceFromEnd == 0;
                }
                if (IsPath(currentNode.Position.X - 1, currentNode.Position.Y)) //left
                {
                    var tmp = new MazeNode(currentNode.Position.X - 1, currentNode.Position.Y);
                    tmp.SetDistanceFromStart(Start);
                    tmp.SetDistanceFromEnd(End);
                    if(!AllNodes.Contains(tmp))
                    {
                        AllNodes.Add(tmp);
                        nodeQueue.Enqueue(tmp);
                    }
                    SolutionFound = SolutionFound || tmp.DistanceFromEnd == 0;
                }
                if (IsPath(currentNode.Position.X, currentNode.Position.Y + 1)) //down
                {
                    var tmp = new MazeNode(currentNode.Position.X, currentNode.Position.Y + 1);
                    tmp.SetDistanceFromStart(Start);
                    tmp.SetDistanceFromEnd(End);
                    if (!AllNodes.Contains(tmp))
                    {
                        AllNodes.Add(tmp);
                        nodeQueue.Enqueue(tmp);
                    }
                    SolutionFound = SolutionFound || tmp.DistanceFromEnd == 0;
                }
                if (IsPath(currentNode.Position.X, currentNode.Position.Y - 1)) //up
                {
                    var tmp = new MazeNode(currentNode.Position.X, currentNode.Position.Y - 1);
                    tmp.SetDistanceFromStart(Start);
                    tmp.SetDistanceFromEnd(End);
                    if (!AllNodes.Contains(tmp))
                    {
                        AllNodes.Add(tmp);
                        nodeQueue.Enqueue(tmp);
                    }
                    SolutionFound = SolutionFound || tmp.DistanceFromEnd == 0;
                }
            }
            if (SolutionFound) Solution = AllNodes.Where(n => n.Visited).Select(n => n.Position).ToList();
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