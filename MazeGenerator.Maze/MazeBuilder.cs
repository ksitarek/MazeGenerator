using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator.Maze
{
    public class MazeBuilder
    {
        private readonly MazeConfiguration _configuration;
        private Cell[] _cells;
        private readonly Random _random;
        private Stack<int> _cellIxStack = new Stack<int>();

        private readonly Dictionary<Direction, Direction> OppositeDirections = new Dictionary<Direction, Direction>()
        {
            { Direction.Up, Direction.Down },
            { Direction.Right, Direction.Left },
            { Direction.Down, Direction.Up },
            { Direction.Left, Direction.Right }
        };

        public MazeBuilder(MazeConfiguration configuration)
        {
            _configuration = configuration;
            _random = new Random();
        }

        public Maze Generate()
        {
            InitializeCells();

            List<int> path = new List<int>();

            var currentCellIx = 0;
            var currentCell = _cells[currentCellIx];
            do
            {
                // mark cell as visited
                currentCell.Visited = true;

                // retrieve neighbors we might visit
                var nonVisitedNeighbors = GetNonVisitedNeighbors(currentCell);

                if (nonVisitedNeighbors.Any())
                {
                    // choose random neighbor
                    var randomNeighborIx = _random.Next(nonVisitedNeighbors.Count());
                    var randomNextCell = nonVisitedNeighbors.ElementAt(randomNeighborIx);

                    // determine direction in which we are going
                    var currentCoordinates = currentCell.Coordinates;
                    var nextCoordinates = randomNextCell.Coordinates;
                    Direction direction = DetermineDirection(currentCoordinates, nextCoordinates);

                    // break walls
                    currentCell.Walls[direction] = false;
                    randomNextCell.Walls[OppositeDirections[direction]] = false;

                    // add current to stack
                    _cellIxStack.Push(currentCellIx);

                    // if we have bottom right cell, it means that we have complete path in stack
                    if (currentCell.Coordinates.X == _configuration.ColumnsCnt - 1
                        && currentCell.Coordinates.Y == _configuration.RowsCnt - 1)
                    {
                        // save the path
                        path = _cellIxStack.ToList();
                    }

                    // set next cell as current
                    currentCellIx = ConvertCoordinatesToIndex(randomNextCell.Coordinates);
                    currentCell = randomNextCell;
                }
                else
                {
                    // go back
                    currentCellIx = _cellIxStack.Pop();
                    currentCell = _cells[currentCellIx];
                }
            }
            while (_cellIxStack.Count > 0);

            return new Maze(_configuration, _cells, path);
        }

        public Direction DetermineDirection(Coordinates currentCoordinates, Coordinates nextCoordinates)
        {
            Direction direction = default(Direction);

            if (nextCoordinates.Y < currentCoordinates.Y) direction = Direction.Up;
            if (nextCoordinates.X > currentCoordinates.X) direction = Direction.Right;
            if (nextCoordinates.Y > currentCoordinates.Y) direction = Direction.Down;
            if (nextCoordinates.X < currentCoordinates.X) direction = Direction.Left;

            return direction;
        }

        private IEnumerable<Cell> GetNonVisitedNeighbors(Cell currentCell)
        {
            // get neighbor coordinates
            var neighborsCoordinates = currentCell.Neighbors
                .Select(kv => kv.Value)
                // make sure we will no go out of boundaries
                .Where(coordinates => coordinates != null && coordinates.X < _configuration.ColumnsCnt && coordinates.Y < _configuration.RowsCnt)
                .ToArray();

            // get cells matching neighbor coordinates and leave only unvisited yet
            var neigborCells = GetCellsByCoordinates(neighborsCoordinates);
            var nonVisitedNeighbors = neigborCells.Where(c => c != null && c.Visited == false);

            return nonVisitedNeighbors;
        }

        public void InitializeCells()
        {
            var totalCells = _configuration.ColumnsCnt * _configuration.RowsCnt;
            _cells = new Cell[totalCells];

            for (var i = 0; i < totalCells; i++)
            {
                // instantiate cell
                var cellCoordinates = ConvertIndexToCoordinates(i);
                var cell = new Cell(cellCoordinates);

                _cells[i] = cell;
            }
        }

        public Cell[] GetCellsByCoordinates(Coordinates[] coordinates)
        {
            var cells = new Cell[coordinates.Length];
            for (var i = 0; i < coordinates.Length; i++)
            {
                if (coordinates[i] == null)
                {
                    cells[i] = null;
                    continue;
                }

                var index = ConvertCoordinatesToIndex(coordinates[i]);
                cells[i] = _cells[index];
            }

            return cells;
        }

        public Direction ChooseRandomDirection()
        {
            var directions = Enum.GetValues(typeof(Direction));
            return (Direction)directions.GetValue(_random.Next(directions.Length));
        }

        public int ConvertCoordinatesToIndex(Coordinates coordinates)
        {
            return coordinates.X + (coordinates.Y * _configuration.ColumnsCnt);
        }

        public Coordinates ConvertIndexToCoordinates(int index)
        {
            int x = index % _configuration.ColumnsCnt;
            int y = index / _configuration.ColumnsCnt;

            return new Coordinates(x, y);
        }
    }
}