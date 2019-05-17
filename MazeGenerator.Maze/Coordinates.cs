using System;

namespace MazeGenerator.Maze
{
    public class Coordinates : IEquatable<Coordinates>
    {
        public int X { get; }
        public int Y { get; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coordinates Modify(Direction direction)
        {
            int x = -1;
            int y = -1;

            switch (direction)
            {
                case Direction.Up:
                    x = X; y = Y - 1;
                    break;

                case Direction.Down:
                    x = X; y = Y + 1;
                    break;

                case Direction.Left:
                    x = X - 1; y = Y;
                    break;

                case Direction.Right:
                    x = X + 1; y = Y;
                    break;
            }

            if (x < 0 || y < 0)
                return null;

            return new Coordinates(x, y);
        }

        public bool Equals(Coordinates other)
        {
            return other != null &&
                   X == other.X &&
                   Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Coordinates)
            {
                return Equals((Coordinates)obj);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}