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


        public MarsSimulator(IEnvironment environment, IParser parser, IMovableFactory factory)
        {
            Parser = parser;
            Factory = factory;
            AreaToExplore = environment;
        }

        public MarsSimulator Init(string inputFromNASA)
        {
            var data = inputFromNASA.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Reverse();
            DataFromMissionControl = new Stack<string>(data);

            var gridBoundaries = DataFromMissionControl.Pop();

            if (!Parser.IsEnvironmentBoundary(gridBoundaries))
            {
                throw new Exception("Error Invalid Environment Boundaries: " + gridBoundaries);
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
                }
                else if (Parser.IsMovementCommand(instruction) && currMovable != null)
                {
                    var commands = instruction.ToCharArray();
                    foreach (var cmd in commands)
                    {
                        currMovable.Move(cmd);
                    }
                }
            }
            return AreaToExplore.ToString();
        }
    }
}
