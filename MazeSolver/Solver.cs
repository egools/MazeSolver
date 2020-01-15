using MazeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public static class Solver
    {
        /// <summary>
        /// Solves maze with undiscovered nodes using A* search.
        /// </summary>
        /// <param name="maze">Maze created from existing image</param>
        public static void SolveAStar(Maze maze)
        {
            var nodeQueue = new PriorityQueue<MazeNode>();
            nodeQueue.Enqueue(maze.Start);

            void CheckNextNode((int x, int y) coords, MazeNode currentNode)
            {
                if (maze.IsPath(coords))
                {
                    var isNewNode = maze.GetCreateMazeNode(coords, currentNode, out MazeNode nextNode);
                    UpdatePreviousNode(currentNode, nextNode);
                    CheckForEnd(currentNode, nextNode, maze);
                    if (isNewNode && !nodeQueue.Contains(nextNode))
                        nodeQueue.Enqueue(nextNode);
                }
            }
            while (!nodeQueue.IsEmpty && !maze.SolutionFound)
            {
                var currentNode = nodeQueue.Dequeue();

                Console.Write($"{currentNode.Position.X}, {currentNode.Position.Y}");

                CheckNextNode(currentNode.Right, currentNode);
                CheckNextNode(currentNode.Left, currentNode);
                CheckNextNode(currentNode.Down, currentNode);
                CheckNextNode(currentNode.Up, currentNode);

                if (!maze.AllNodes.ContainsKey(currentNode.Position))
                    maze.AllNodes.Add(currentNode.Position, currentNode);
                
                int currentLineCursor = Console.CursorTop;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor);
            }
            if (maze.SolutionFound)
            {
                maze.Solution = new List<(int x, int y)>();
                var currentNode = maze.End;
                while (currentNode != maze.Start)
                {
                    maze.Solution.Add((currentNode.Position.X, currentNode.Position.Y));
                    currentNode = currentNode.PreviousNode;
                }
                maze.Solution.Add((currentNode.Position.X, currentNode.Position.Y)); //add start
            }
        }

        public static void UpdatePreviousNode(MazeNode currentNode, MazeNode nextNode)
        {
            if (nextNode.DistanceFromStart > currentNode.DistanceFromStart + 1)
            {
                nextNode.DistanceFromStart = currentNode.DistanceFromStart + 1;
                nextNode.PreviousNode = currentNode;
            }
        }

        public static void CheckForEnd(MazeNode currentNode, MazeNode nextNode, Maze maze)
        {
            if (nextNode == maze.End)
            {
                maze.End.PreviousNode = currentNode;
                maze.SolutionFound = true;
            }
        }
    }
}
