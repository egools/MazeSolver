using System;
using System.Collections.Generic;

namespace MazeSolver
{
    public class MazeNode : IComparable
    {
        public (int X, int Y) Position { get; set; }
        public bool Visited { get; set; } = false;
        public double DistanceFromStart { get; private set; }
        public double DistanceFromEnd { get; private set; }
        public MazeNode PreviousNode { get; set; }

        public MazeNode(int x, int y)
        {
            Position = (x, y);
            DistanceFromStart = double.MaxValue;
            DistanceFromEnd = double.MaxValue;
        }

        public MazeNode(int x, int y, MazeNode start, MazeNode end)
        {
            SetDistanceFromStart(start);
            SetDistanceFromEnd(end);
            Position = (x, y);
        }

        public double GetDistanceFromNode(MazeNode node)
        {
            return Math.Sqrt(Math.Pow(Position.X - node.Position.X, 2) + Math.Pow(Position.Y - node.Position.Y, 2));
        }

        public void SetDistanceFromStart(MazeNode start)
        {
            DistanceFromStart = Math.Abs(GetDistanceFromNode(start));
        }

        public void SetDistanceFromEnd(MazeNode end)
        {
            DistanceFromEnd = Math.Abs(GetDistanceFromNode(end));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MazeNode))
                return false;
            else
                return Position == (obj as MazeNode).Position;
        }

        public static bool operator ==(MazeNode l, MazeNode r)
        {
            if (l is null && r is null)
                return true;
            else if (l is null ^ r is null)
                return false;
            else
                return l.Equals(r);
        }

        public static bool operator !=(MazeNode l, MazeNode r)
        {
            if (l is null && r is null)
                return false;
            else if (l is null ^ r is null)
                return true;
            else
                return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            var hashCode = -365790041;
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + Visited.GetHashCode();
            return hashCode;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            MazeNode otherMazeNode = obj as MazeNode;

            if (otherMazeNode != null)
                return (DistanceFromStart + DistanceFromEnd).CompareTo((otherMazeNode.DistanceFromStart + otherMazeNode.DistanceFromEnd));
            else
                throw new ArgumentException("Object is not a MazeNode");
        }
    }
}