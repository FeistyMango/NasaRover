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
    public class RoverTests
    {
        public static void Main(string[] args)
        {
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TestRoverDeployToOpenLocation(bool isPositionOpen)
        {
            var position = isPositionOpen ? new Point(3, 1) : new Point(0, 0);
            var mockEnv = new Mock<IEnvironment>();
            mockEnv.Setup(p => p.IsPositionOpen(It.IsAny<Point>())).Returns(isPositionOpen);
            mockEnv.Setup(p => p.SetPosition(It.IsAny<IMovable>(), It.IsAny<Point>())).Callback<IMovable, Point>((m, p) => m.Position = position);

            var rover = new Rover("3 1 N", mockEnv.Object);

            var expectedX = isPositionOpen ? 3 : 0;
            var expectedY = isPositionOpen ? 1 : 0;

            Assert.AreEqual(expectedX, rover.Position.X);
            Assert.AreEqual(expectedY, rover.Position.Y);
        }
    }
}
