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
        public Dictionary<(int, int), MazeNode> AllNodes { get; }
        public PriorityQueue<MazeNode> NodeQueue { get; set; }
        public bool SolutionFound { get; set; } = false;
        public List<(int x, int y)> Solution { get; set; }
        public const string White = "ffffffff";
        public bool IsPath((int x, int y) pos)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x >= Width || pos.y >= Height)
                return false;
            else
                return bmp.GetPixel(pos.x, pos.y).Name == White;
        }
        public Maze(string path)
        {
            _imagePath = path;
            bmp = new Bitmap(_imagePath);
            Width = bmp.Width;
            Height = bmp.Height;
            AllNodes = new Dictionary<(int, int), MazeNode>();

            for (int x = 1; x < bmp.Width - 1; x++)
            {
                if (Start == null && IsPath((x, 0))) Start = new MazeNode((x, 0));
                if (End == null && IsPath((x, Height - 1))) End = new MazeNode((x, Height - 1));
            }
            Start.DistanceFromStart = 0;
            Start.DistanceFromEnd = Start.GetDistanceFromNode(End);
            End.DistanceFromEnd = 0;
        }


        public void Solve()
        {
            NodeQueue = new PriorityQueue<MazeNode>();
            NodeQueue.Enqueue(Start);
            while (!NodeQueue.IsEmpty && !SolutionFound)
            {
                var currentNode = NodeQueue.Dequeue();
                if (IsPath(currentNode.Right))
                {
                    var isNewNode = GetCreateMazeNode(currentNode.Right, currentNode, out MazeNode tmp);
                    if (isNewNode && !NodeQueue.Contains(tmp))
                        NodeQueue.Enqueue(tmp);
                }
                if (IsPath(currentNode.Left))
                {
                    var isNewNode = GetCreateMazeNode(currentNode.Left, currentNode, out MazeNode tmp);
                    if (isNewNode && !NodeQueue.Contains(tmp))
                        NodeQueue.Enqueue(tmp);
                }
                if (IsPath(currentNode.Down))
                {
                    var isNewNode = GetCreateMazeNode(currentNode.Down, currentNode, out MazeNode tmp);
                    if (isNewNode && !NodeQueue.Contains(tmp))
                        NodeQueue.Enqueue(tmp);
                }
                if (IsPath(currentNode.Up)) //up
                {
                    var isNewNode = GetCreateMazeNode(currentNode.Up, currentNode, out MazeNode tmp);
                    if (isNewNode && !NodeQueue.Contains(tmp))
                        NodeQueue.Enqueue(tmp);
                }
                if (!AllNodes.ContainsKey(currentNode.Position))
                    AllNodes.Add(currentNode.Position, currentNode);
            }
            if (SolutionFound)
            {
                Solution = new List<(int x, int y)>();
                var currentNode = End;
                while (currentNode != Start)
                {
                    Solution.Add((currentNode.Position.X, currentNode.Position.Y));
                    currentNode = currentNode.PreviousNode;
                }
                Solution.Add((currentNode.Position.X, currentNode.Position.Y)); //add start
            }
        }

        public bool GetCreateMazeNode((int x, int y) pos, MazeNode currentNode, out MazeNode outNode)
        {

            var isNewNode = !AllNodes.TryGetValue(pos, out outNode);
            if (isNewNode)
                outNode = new MazeNode(pos, End);
            if (outNode.DistanceFromStart > currentNode.DistanceFromStart + 1)
            {
                outNode.DistanceFromStart = currentNode.DistanceFromStart + 1;
                outNode.PreviousNode = currentNode;
            }
            if (outNode == End)
            {
                End.PreviousNode = currentNode;
                SolutionFound = true;
            }

            return isNewNode;
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