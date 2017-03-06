using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NasaApp;
using System.Drawing;
using Serilog;

namespace NasaTests.IntegrationTests
{
    [TestFixture]
    public class MarsSimulatorTests
    {
        //public struct TestData 
        //{
        //    public string Input;
        //    public Point[] ExpectedCoordinates;
        //    public bool HasError;

        //    public TestCase (string input, Point[] expectedCoordinates, bool hasError)
        //    {
        //        Input = input;
        //        ExpectedCoordinates = expectedCoordinates;
        //        HasError = hasError;
        //    }
        //}

        public static IEnumerable<TestCaseData> SetupData()
        {
            yield return new TestCaseData(@"
            5 5
            1 2 N
            LMLMLMLMM
            3 3 E
            MMRMMRMRRM", new Point[] { new Point(2, 3), new Point(5, 1) }, new char[] { 'N', 'E' }, false);

            yield return new TestCaseData(@"
            10 10
            2 4 W
            MMRMRMM
            1 1 S
            MLMMMLMMMLMMMLMMMR", new Point[] { new Point(3, 5), new Point(1, 1) }, new char[] { 'E', 'W' }, false);

            yield return new TestCaseData(@"
            3 6
            2 4 E
            MMLMMMRRRMM
            1 1 N
            MMLMMMRRMMMLMLMLM", new Point[] { new Point(1, 6), new Point(2, 3) }, new char[] { 'W', 'S' }, false);
        }

        public void SetUpLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Level}]:\t{Message}{NewLine}{Exception}{NewLine}")
                .MinimumLevel.Information()
                .CreateLogger();
        }

        [TestCaseSource("SetupData")]
        public void TestSimulateWithNoErrors(string input, Point[] expectedCoordinates, char[] expectedDirections, bool hasError)
        {
            SetUpLogging();
            var parser = new Parser(Log.Logger);
            var plateau = new Plateau(parser);
            var factory = new MovableFactory(plateau, parser, Log.Logger);
            var mars = new MarsSimulator(plateau, parser, factory, Log.Logger).Init(input);
            
            mars.Simulate();
            for(var i = 0; i < expectedCoordinates.Length; i++){
                var movable = mars.Movables.FirstOrDefault(p => p.Id == i + 1);
                Assert.IsFalse(mars.AreaToExplore.IsPositionOpen(expectedCoordinates[i]), "Rover " + (i +1) + " in wrong position");
                Assert.AreEqual(expectedDirections[i], movable.Direction, "Rover " + (i + 1) + " facing wrong direction");
            }
        }

        [TestCase(@"
            5 5
            1 1 N
            LLLLLL
            1 1 E
            MMRMMRMRRM
            2 1 E
            RRRRR")]
        public void TestSimulateContingencyDeploymentErrorOnRover2(string input)
        {
            var loggerMock = new Mock<ILogger>();
            var msgLogged = "";
            loggerMock.Setup(p => p.Error(It.IsAny<string>()))
                            .Callback<string>(msg => {
                                msgLogged = msg;
                            });

            var parser = new Parser(loggerMock.Object);
            var plateau = new Plateau(parser);
            var factory = new MovableFactory(plateau, parser, loggerMock.Object);
            var mars = new MarsSimulator(plateau, parser, factory, loggerMock.Object).Init(input);
            mars.Simulate();

            Assert.AreNotEqual(2, plateau.Grid[4][0].Id);
            loggerMock.Verify(p => p.Error(It.IsAny<string>()), Times.Exactly(2)); //bad deploy, aborted movement cmd
        }

        [TestCase(@"
            5 5
            1 1 N
            LLMLLxR")]
        public void TestSimulateInvalidMovementCommandErrorHandled(string input)
        {
            var loggerMock = new Mock<ILogger>();
            var msgLogged = "";
            loggerMock.Setup(p => p.Error(It.IsAny<string>()));

            var parser = new Parser(loggerMock.Object);
            var plateau = new Plateau(parser);
            var factory = new MovableFactory(plateau, parser, loggerMock.Object);
            var mars = new MarsSimulator(plateau, parser, factory, loggerMock.Object).Init(input);
            mars.Simulate();

            var movable = mars.AreaToExplore.Grid[4][0];
            Assert.IsNotNull(movable);
            Assert.AreEqual('N', movable.Direction);
            loggerMock.Verify(p => p.Error(It.IsAny<string>()), Times.Exactly(1));
        }
    }
}
