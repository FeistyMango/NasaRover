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
        public List<IMovable> Movables { get; set; }

        public MarsSimulator(IEnvironment environment, IParser parser, IMovableFactory factory, ILogger logger)
        {
            Parser = parser;
            Factory = factory;
            AreaToExplore = environment;
            Logger = logger;
        }

        public MarsSimulator Init(string inputFromNASA)
        {
            Movables = new List<IMovable>();

            var data = inputFromNASA.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Reverse();
            DataFromMissionControl = new Stack<string>(data);

            var gridBoundaries = DataFromMissionControl.Pop();

            if (!Parser.IsEnvironmentBoundary(gridBoundaries))
            {
                var error = "Error Invalid Environment Boundaries: " + gridBoundaries;
                Logger.Fatal(error);
                throw new Exception(error);
            }

            AreaToExplore.Init(gridBoundaries);
            return this;
        }

        public string Simulate()
        {
            IMovable currMovable = null;
            var movableIdIncrementer = 1;
            while (DataFromMissionControl.Any())
            {
                var instruction = DataFromMissionControl.Pop();
                if (Parser.IsMovable(instruction))
                {
                    try
                    {
                        var tmp = Factory.Rover().Init(movableIdIncrementer++, instruction);
                        Movables.Add(tmp);
                        currMovable = tmp;
                        Logger.Debug("Deploying Rover: " + tmp.ToString());
                        Logger.Debug(AreaToExplore.ToString());
                    }
                    catch
                    {
                        currMovable = null; //ensure that subsequent movement commands are aborted
                    }
                }
                else if (currMovable != null)
                {
                    currMovable.Move(instruction);
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
