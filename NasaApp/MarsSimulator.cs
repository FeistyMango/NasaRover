using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasaApp
{
    public class MarsSimulator
    {
        public IEnvironment AreaToExplore { get; set; }
        public IParser Parser { get; set; }
        public Stack<string> DataFromMissionControl { get; set; }
        public IMovableFactory Factory { get; set; }
        public ILogger Logger { get; set; }


        public MarsSimulator(IEnvironment environment, IParser parser, IMovableFactory factory, ILogger logger)
        {
            Parser = parser;
            Factory = factory;
            AreaToExplore = environment;
            Logger = logger;
        }

        public MarsSimulator Init(string inputFromNASA)
        {
            var data = inputFromNASA.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Reverse();
            DataFromMissionControl = new Stack<string>(data);

            var gridBoundaries = DataFromMissionControl.Pop();

            if (!Parser.IsEnvironmentBoundary(gridBoundaries))
            {
                var error = "Error Invalid Environment Boundaries: " + gridBoundaries;
                Log.Fatal(error);
                throw new Exception(error);
            }

            AreaToExplore.Init(gridBoundaries);
            return this;
        }

        public string Simulate()
        {
            IMovable currMovable = null;
            while (DataFromMissionControl.Any())
            {
                var instruction = DataFromMissionControl.Pop();
                if (Parser.IsMovable(instruction))
                {
                    var tmp = Factory.Rover().Init(instruction);
                    currMovable = tmp;
                    Logger.Debug("Deploying Rover: " + tmp.ToString());
                    Logger.Debug(AreaToExplore.ToString());
                }
                else if (Parser.IsMovementCommand(instruction) && currMovable != null)
                {
                    var commands = instruction.ToCharArray();
                    foreach (var cmd in commands)
                    {
                        currMovable.Move(cmd);
                        Logger.Debug(AreaToExplore.ToString());
                    }
                }
                else
                {
                    Logger.Error("Unrecognized Instruction: " + instruction);
                }
            }
            return AreaToExplore.ToString();
        }
    }
}
