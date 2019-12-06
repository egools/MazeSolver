using System.Collections.Generic;

namespace MazeSolver
{
    public class MazeNode
    {
        public (int X, int Y) Position { get; set; }
        public bool Visited { get; set; } = false;
        public float DistanceFromEnd { get; set; }

        public MazeNode(int x, int y)
        {
            Position = (x, y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MazeNode))
                return false;

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
    }
}