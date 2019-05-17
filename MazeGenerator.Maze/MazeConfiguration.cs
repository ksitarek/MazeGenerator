namespace MazeGenerator.Maze
{
    public class MazeConfiguration
    {
        public MazeConfiguration(int columnsCnt, int rowsCnt)
        {
            ColumnsCnt = columnsCnt;
            RowsCnt = rowsCnt;
        }

        public int ColumnsCnt { get; }
        public int RowsCnt { get; }
    }
}