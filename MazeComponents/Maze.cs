using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeComponents
{
    public class Maze
    {
        public string ImagePath;
        public Bitmap bmp { get; }
        public MazeNode Start { get; }
        public MazeNode End { get; }
        public int Width { get; }
        public int Height { get; }
        public Dictionary<(int, int), MazeNode> AllNodes { get; }
        public bool SolutionFound { get; set; } = false;
        public List<(int x, int y)> Solution { get; set; }
        public const string White = "ffffffff";
        public bool IsPath((int x, int y) coords)
        {
            if (coords.x < 0 || coords.y < 0 || coords.x >= Width || coords.y >= Height)
                return false;
            else
                return bmp.GetPixel(coords.x, coords.y).Name == White;
        }

        /// <summary>
        /// Creates maze from existing image
        /// </summary>
        /// <param name="path">Path to existing bitmap</param>
        public Maze(string path)
        {
            ImagePath = path;
            bmp = new Bitmap(ImagePath);
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

        /// <summary>
        /// Checks node list for existing nodes at given coordinates and a
        /// </summary>
        /// <param name="coords">coordinates of new node</param>
        /// <param name="currentNode"></param>
        /// <param name="outNode">new node or found node at pos</param>
        /// <returns>A boolean value of whether an existing node was found</returns>
        public bool GetCreateMazeNode((int x, int y) coords, MazeNode currentNode, out MazeNode outNode)
        {

            var isNewNode = !AllNodes.TryGetValue(coords, out outNode);
            if (isNewNode)
                outNode = new MazeNode(coords, End);

            return isNewNode;
        }
    }
}