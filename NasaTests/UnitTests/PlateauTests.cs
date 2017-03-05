using NasaApp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Moq;

namespace NasaAppTests
{
    [TestFixture]
    public class PlateauTests
    {
        [TestCase(5, 5)]
        [TestCase(10, 10)]
        [TestCase(1, 1)]
        [TestCase(3, 6)]
        [TestCase(7, 10)]
        public void TestPlateauGridSetup(int lengthX, int lengthY)
        {
            var parserMock = new Mock<IParser>();
            parserMock.Setup(p => p.ParsePosition(It.IsAny<string>())).Returns(new Point(lengthX, lengthY));

            var plateau = new Plateau(parserMock.Object).Init(lengthX + " " + lengthY);

            Assert.AreEqual(lengthY, plateau.Grid.Length);
            Assert.AreEqual(lengthX, plateau.Grid[0].Length);
        }

        [TestCase(3, 2, true)]
        [TestCase(4, 2, false)]
        public void TestIsPositionOpen(int x, int y, bool isOccupiedOrInvalid)
        {
            var parserMock = new Mock<IParser>();
            parserMock.Setup(p => p.ParsePosition(It.IsAny<string>())).Returns(new Point(5, 5));
            
            var movableMock = new Mock<IMovable>();

            var plateau = new Plateau(parserMock.Object).Init("5 5");
            plateau.Grid[x][y] = isOccupiedOrInvalid ? movableMock.Object : null;
            
            var expectedPosition = new Point(x, y);
            if (isOccupiedOrInvalid)
            {
                Assert.IsFalse(plateau.IsPositionOpen(expectedPosition));
            }
            else
            {
                Assert.IsTrue(plateau.IsPositionOpen(expectedPosition));
            }
        }

        [TestCase(3, 2, false)]
        [TestCase(4, 2, false)]
        [TestCase(5, 7, true)]
        [TestCase(9, 4, true)]
        [TestCase(9, 9, true)]
        public void TestIsPositionValid(int x, int y, bool isInvalid)
        {
            var parserMock = new Mock<IParser>();
            parserMock.Setup(p => p.ParsePosition(It.IsAny<string>())).Returns(new Point(5, 5));

            var plateau = new Plateau(parserMock.Object).Init("5 5");

            var expectedPosition = new Point(x, y);
            if (isInvalid)
            {
                Assert.IsFalse(plateau.IsPositionValid(expectedPosition));
            }
            else
            {
                Assert.IsTrue(plateau.IsPositionValid(expectedPosition));
            }
        }

        [TestCase(3, 2, false)]
        [TestCase(4, 2, false)]
        [TestCase(5, 7, true)]
        [TestCase(9, 4, true)]
        [TestCase(9, 9, true)]
        public void TestSetPosition(int x, int y, bool isInvalid)
        {
             var parserMock = new Mock<IParser>();
            parserMock.Setup(p => p.ParsePosition(It.IsAny<string>())).Returns(new Point(5, 5));
            
            var movableMock = new Mock<IMovable>();
            var movableMocksStartingPosition = movableMock.Object.Position;

            var plateau = new Plateau(parserMock.Object).Init("5 5");

            var expectedPosition = new Point(x, y);
            plateau.SetPosition(movableMock.Object, expectedPosition);

            if (isInvalid)
            {
                foreach (var array in plateau.Grid)
                {
                    foreach (var movable in array)
                    {
                        Assert.IsNull(movable);
                    }
                }
            }
            else
            {
                var movable = plateau.Grid[expectedPosition.X][expectedPosition.Y];
                Assert.IsNotNull(movable);
            }
        }
    }
}
