using System;
using System.Drawing;
using System.Linq;

namespace MazeGenerator.Maze.Renderers
{
    public class BitmapRenderer : IDisposable
    {
        private readonly MazeConfiguration _configuration;
        private readonly Cell[] _cells;
        private readonly int _cellWidth;
        private readonly int _cellHeight;
        private Bitmap _bitmap;
        private Graphics _graphics;

        private readonly Color _backgroundColor = Color.White;
        private readonly Color _highlightColor = Color.Lavender;
        private readonly Color _wallPenColor = Color.Black;
        private readonly int _wallPenWidth = 1;

        private int[] _cellsToHighlight = new int[0];

        public BitmapRenderer(MazeConfiguration configuration, Cell[] cells, int cellWidth, int cellHeight)
        {
            _configuration = configuration;
            _cells = cells;
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
        }

        public void HighlightCells(int[] ixs)
        {
            _cellsToHighlight = ixs;
        }

        public Bitmap Render()
        {
            InitializeBitmap();

            for (var x = 0; x < _configuration.ColumnsCnt; x++)
            {
                for (var y = 0; y < _configuration.RowsCnt; y++)
                {
                    // get current cell
                    var index = x + (y * _configuration.ColumnsCnt);
                    var cell = _cells[index];

                    // draw cells walls
                    var walls = cell.Walls.Where(w => w.Value == true)
                        .Select(w => w.Key);

                    // should we highlight current cell?
                    if (_cellsToHighlight.Contains(index))
                    {
                        HighlightCell(x, y);
                    }

                    foreach (var wall in walls)
                        DrawWall(x, y, wall);
                }
            }

            return _bitmap;
        }

        private void HighlightCell(int x, int y)
        {
            var brush = new SolidBrush(_highlightColor);
            _graphics.FillRectangle(brush, new Rectangle(x * _cellWidth, y * _cellHeight, _cellWidth, _cellHeight));
        }

        private void DrawWall(int x, int y, Direction wall)
        {
            var pen = new Pen(_wallPenColor, _wallPenWidth);

            x *= _cellWidth;
            y *= _cellHeight;

            var start = default(Point);
            var end = default(Point);

            if (wall == Direction.Up)
            {
                start = new Point(x, y);
                end = new Point(x + _cellWidth, y);
            }

            if (wall == Direction.Right)
            {
                start = new Point(x + _cellWidth, y);
                end = new Point(x + _cellWidth, y + _cellHeight);
            }
            if (wall == Direction.Down)
            {
                start = new Point(x, y + _cellHeight);
                end = new Point(x + _cellWidth, y + _cellHeight);
            }
            if (wall == Direction.Left)
            {
                start = new Point(x, y);
                end = new Point(x, y + _cellHeight);
            }

            _graphics.DrawLine(pen, start, end);
        }

        private void InitializeBitmap()
        {
            var bitmapWidth = _configuration.ColumnsCnt * _cellWidth;
            var bitmapHeight = _configuration.RowsCnt * _cellHeight;
            _bitmap = new Bitmap(bitmapWidth, bitmapHeight);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.Clear(_backgroundColor);
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _graphics.Dispose();
                    _bitmap.Dispose();
                }

                disposedValue = true;
            }
        }

        ~BitmapRenderer()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}