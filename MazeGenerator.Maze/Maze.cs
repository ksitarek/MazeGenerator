using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator.Maze
{
    public class Maze
    {
        public Maze(MazeConfiguration configuration, Cell[] cells, List<int> path)
        {
            Configuration = configuration;
            Cells = cells;
            Path = path;
            HasSolution = Path.Any();
        }

        public readonly MazeConfiguration Configuration;
        public readonly Cell[] Cells;
        public readonly List<int> Path;
        public readonly bool HasSolution;
    }
}