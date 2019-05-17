using MazeGenerator.Maze;
using MazeGenerator.Maze.Renderers;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MazeGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Maze.Maze _maze;
        private MazeConfiguration _configuration;

        public MainWindow()
        {
            InitializeComponent();

            ShowMazeConfiguration();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            _configuration = new MazeConfiguration(TbNumberOfColumns.NumValue, TbNumberOfRows.NumValue);

            GenerateMaze();
            RenderMaze();
            ShowGeneratedMaze();
        }

        private void RenderMaze()
        {
            using (var renderer = new BitmapRenderer(_configuration, _maze.Cells, TbCellWidth.NumValue, TbCellHeight.NumValue))
            {
                if (CbHighlightSolution.IsChecked.HasValue && CbHighlightSolution.IsChecked.Value)
                {
                    renderer.HighlightCells(_maze.Path.ToArray());
                }

                var bitmap = renderer.Render();
                MazeVisualization.Source = ConvertBitmapToImageSource(bitmap);
            }
        }

        private void GenerateMaze()
        {
            var builder = new MazeBuilder(_configuration);
            _maze = builder.Generate();
        }

        private ImageSource ConvertBitmapToImageSource(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                bitmap.Dispose();
                ms.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private void ShowGeneratedMaze()
        {
            // when done, show visualization pane
            MazeConfigurationGrid.Visibility = Visibility.Hidden;
            GeneratedMazeGrid.Visibility = Visibility.Visible;
        }

        private void ShowMazeConfiguration()
        {
            // when done, show visualization pane
            MazeConfigurationGrid.Visibility = Visibility.Visible;
            GeneratedMazeGrid.Visibility = Visibility.Hidden;
        }

        private void CbHighlightSolution_Checked(object sender, RoutedEventArgs e)
        {
            if (_maze != null)
                RenderMaze();
        }

        private void CbHighlightSolution_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_maze != null)
                RenderMaze();
        }

        private void GenerateNewMazeBtn_Click(object sender, RoutedEventArgs e)
        {
            _configuration = null;
            _maze = null;

            MazeVisualization.Source = null;

            ShowMazeConfiguration();
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            using (var fs = new FileStream("maze.png", FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(MazeVisualization.Source as BitmapSource));
                encoder.Save(fs);
            }
        }
    }
}