﻿using NasaApp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Moq;
using Serilog;

namespace NasaAppTests
{
    [TestFixture]
    public class RoverTests
    {
        public static void Main(string[] args)
        {
        }

        public Mock<IEnvironment> SetupMockEnv()
        {
            var mockEnv = new Mock<IEnvironment>();
            mockEnv.Setup(p => p.IsPositionOpen(It.IsAny<Point>())).Returns(true);
            mockEnv.Setup(p => p.SetPosition(It.IsAny<IMovable>(), It.IsAny<Point>())).Callback<IMovable, Point>((m, p) => m.Position = p);
            return mockEnv;
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TestRoverDeploy(bool isPositionOpen)
        {
            var mockEnv = SetupMockEnv();
            mockEnv.Setup(p => p.IsPositionOpen(It.Is<Point>(q => q.X == 3 && q.Y == 1))).Returns(isPositionOpen);
            mockEnv.Setup(p => p.IsPositionOpen(It.Is<Point>(q => q.X == 1 && q.Y == 1))).Returns(true);

            var parserMock = new Mock<IParser>();
            parserMock.Setup(p => p.ParsePosition(It.IsAny<string>())).Returns(new Point(3, 1));

            var loggerMock = new Mock<ILogger>();
            
            var rover = new Rover(mockEnv.Object, parserMock.Object, loggerMock.Object).Init(1, "3 1 N");

            var expectedX = isPositionOpen ? 3 : 0;
            var expectedY = isPositionOpen ? 1 : 0;

            Assert.AreEqual(expectedX, rover.Position.X);
            Assert.AreEqual(expectedY, rover.Position.Y);
            Assert.AreEqual('N', rover.Direction);
        }

        [TestCase("L")]
        [TestCase("R")]
        [TestCase("M")]
        public void TestRoverSuccessfulMove(string command)
        {
            var mockEnv = SetupMockEnv();
            var parserMock = new Mock<IParser>();
            parserMock.Setup(p => p.ParsePosition(It.IsAny<string>())).Returns(new Point(3, 1));
            
            var loggerMock = new Mock<ILogger>();

            var rover = new Rover(mockEnv.Object, parserMock.Object, loggerMock.Object).Init(1, "3 1 N");
            rover.Move(command);

            Point expectedPosition = new Point(0, 0);
            char expectedDirection = ' ';
            switch(command)
            {
                case "L":
                    expectedPosition = new Point(3, 1);
                    expectedDirection = 'W';
                    break;
                case "R":
                    expectedPosition = new Point(3, 1);
                    expectedDirection = 'E';
                    break;
                case "M":
                    expectedPosition = new Point(3, 2);
                    expectedDirection = 'N';
                    break;
            }

            Assert.AreEqual(expectedPosition.X, rover.Position.X);
            Assert.AreEqual(expectedPosition.Y, rover.Position.Y);
            Assert.AreEqual(expectedDirection, rover.Direction);
        }
    }
}
