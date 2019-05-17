using System.Collections.Generic;

namespace MazeGenerator.Maze
{
    public class Cell
    {
        public bool Visited { get; set; }

        public Coordinates Coordinates { get; }

        public readonly Dictionary<Direction, bool> Walls;

        public readonly Dictionary<Direction, Coordinates> Neighbors;

        public Cell(Coordinates coordinates)
        {
            Coordinates = coordinates;

            Walls = new Dictionary<Direction, bool> {
                { Direction.Up, true },
                { Direction.Right, true },
                { Direction.Down, true },
                { Direction.Left, true }
            };

            Neighbors = new Dictionary<Direction, Coordinates> {
                { Direction.Up, Coordinates.Modify(Direction.Up) },
                { Direction.Right, Coordinates.Modify(Direction.Right) },
                { Direction.Down, Coordinates.Modify(Direction.Down) },
                { Direction.Left, Coordinates.Modify(Direction.Left) },
            };
        }
    }
}