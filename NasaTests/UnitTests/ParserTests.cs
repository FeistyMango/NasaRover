using Moq;
using NasaApp;
using NUnit.Framework;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasaTests.UnitTests
{
    [TestFixture]
    public class ParserTests
    {
        [TestCase("55 55 S", 55, 55, false)]
        [TestCase("5 5 N", 5, 5, false)]
        [TestCase("3 1 E", 3, 1, false)]
        [TestCase("0 0 W", 0, 0, false)]
        [TestCase("A A W", 0, 0, true)]
        [TestCase("0 A W", 5, 5, true)]
        [TestCase("A 0 W", 5, 5, true)]
        public void TestParsePosition(string instruction, int expectedX, int expectedY, bool hasError)
        {
            var mockLogger = new Mock<ILogger>();

            var parser = new Parser(mockLogger.Object);
            if (hasError)
            {
                Assert.Throws<Exception>(delegate { parser.ParsePosition(instruction); });
            }
            else 
            {
                var coordinate = parser.ParsePosition(instruction);
                Assert.AreEqual(expectedX, coordinate.X);
                Assert.AreEqual(expectedY, coordinate.Y);                
            }
        }

        [TestCase("55 55 S", 55, 55, false)]
        [TestCase("5 5 N", 5, 5, false)]
        [TestCase("3 1 E", 3, 1, false)]
        [TestCase("0 0 W", 0, 0, false)]
        [TestCase("A A W", 0, 0, true)]
        [TestCase("0 A W", 5, 5, true)]
        [TestCase("A 0 W", 5, 5, true)]
        [TestCase("8 7", 5, 5, true)]
        [TestCase("A 0", 5, 5, true)]
        public void TestIsMovable(string instruction, int expectedX, int expectedY, bool hasError)
        {
            var mockLogger = new Mock<ILogger>();

            var parser = new Parser(mockLogger.Object);
            
            if (hasError)
            {
                Assert.IsFalse(parser.IsMovable(instruction));
            }
            else
            {
                Assert.IsTrue(parser.IsMovable(instruction));
            }
        }

        [TestCase("LRLRMMLR", false)]
        [TestCase("RLRLMM", false)]
        [TestCase("MMMMMRRLL", false)]
        [TestCase("M1LRLRMR", true)]
        [TestCase("$Malkasldk", true)]
        [TestCase("RR L M ", true)]
        public void TestIsMovementCommand(string instruction, bool hasError)
        {
            var mockLogger = new Mock<ILogger>();

            var parser = new Parser(mockLogger.Object);

            if (hasError)
            {
                Assert.IsFalse(parser.IsMovementCommand(instruction));
            }
            else
            {
                Assert.IsTrue(parser.IsMovementCommand(instruction));
            }
        }

        [TestCase("10 10", 10, 10, false)]
        [TestCase("100 100", 100, 100, false)]
        [TestCase("2 2", 2, 2, false)]
        [TestCase("", 2, 2, true)]
        [TestCase("1   2", 2, 2, true)]
        [TestCase("2 - 2", 2, 2, true)]
        [TestCase("2 - A", 2, 2, true)]
        [TestCase("L - A", 2, 2, true)]
        [TestCase("L - 3", 2, 2, true)]
        [TestCase("L 3", 2, 2, true)]
        public void TestIsEnvironmentBoundary(string instruction, int expectedX, int expectedY, bool hasError)
        {
            var mockLogger = new Mock<ILogger>();

            var parser = new Parser(mockLogger.Object);

            if (hasError)
            {
                Assert.IsFalse(parser.IsEnvironmentBoundary(instruction));
            }
            else
            {
                Assert.IsTrue(parser.IsEnvironmentBoundary(instruction));
            }
        }
    }
}
