using MazeGenerator.Maze;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MazeGenerator.Tests
{
    [TestClass]
    public class MazeBuilderTests
    {
        [TestMethod]
        public void Correctly_Converts_Index_To_Coordinates()
        {
            var config = new MazeConfiguration(4, 3);
            var generator = new MazeBuilder(config);

            Assert.AreEqual(new Coordinates(0, 0), generator.ConvertIndexToCoordinates(0));
            Assert.AreEqual(new Coordinates(2, 1), generator.ConvertIndexToCoordinates(6));
            Assert.AreEqual(new Coordinates(1, 2), generator.ConvertIndexToCoordinates(9));
            Assert.AreEqual(new Coordinates(3, 2), generator.ConvertIndexToCoordinates(11));
        }

        [TestMethod]
        public void Correctly_Converts_Coordinates_To_Index()
        {
            var config = new MazeConfiguration(4, 3);
            var generator = new MazeBuilder(config);

            Assert.AreEqual(0, generator.ConvertCoordinatesToIndex(new Coordinates(0, 0)));
            Assert.AreEqual(6, generator.ConvertCoordinatesToIndex(new Coordinates(2, 1)));
            Assert.AreEqual(9, generator.ConvertCoordinatesToIndex(new Coordinates(1, 2)));
            Assert.AreEqual(11, generator.ConvertCoordinatesToIndex(new Coordinates(3, 2)));
        }

        [TestMethod]
        public void Correctly_Determines_Direction()
        {
            var config = new MazeConfiguration(4, 3);
            var generator = new MazeBuilder(config);

            Assert.AreEqual(Direction.Up, generator.DetermineDirection(new Coordinates(1, 1), new Coordinates(1, 0)));
            Assert.AreEqual(Direction.Right, generator.DetermineDirection(new Coordinates(1, 1), new Coordinates(2, 1)));
            Assert.AreEqual(Direction.Down, generator.DetermineDirection(new Coordinates(1, 1), new Coordinates(1, 2)));
            Assert.AreEqual(Direction.Left, generator.DetermineDirection(new Coordinates(1, 1), new Coordinates(0, 1)));
        }
    }
}