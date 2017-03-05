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
        public List<IMovable> Movables { get; set; }
        public IParser Parser { get; set; }
        public Stack<string> DataFromMissionControl { get; set; }

        public MarsSimulator(string inputFromNASA, IParser parser)
        {
            var data = inputFromNASA.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Reverse();
            DataFromMissionControl = new Stack<string>(data);
            Movables = new List<IMovable>();
            Parser = parser;

            var gridBoundaries = DataFromMissionControl.Pop();

            if (!Parser.IsEnvironmentBoundary(gridBoundaries))
            {
                throw new Exception("Error Invalid Environment Boundaries: " + gridBoundaries);
            }

            AreaToExplore = new Plateau(gridBoundaries, Parser);
        }

        public string Simulate()
        {
            IMovable currMovable = null;
            while (DataFromMissionControl.Any())
            {
                var instruction = DataFromMissionControl.Pop();
                if (Parser.IsMovable(instruction))
                {
                    var tmp = new Rover(instruction, AreaToExplore, Parser);
                    //output.AppendLine(currMovable != null ? currMovable.ToString() : tmp.ToString());
                    currMovable = tmp;
                    Movables.Add(currMovable);

                    Console.WriteLine("Deploying Rover:" + tmp);
                }
                else if (Parser.IsMovementCommand(instruction) && currMovable != null)
                {
                    var commands = instruction.ToCharArray();
                    foreach (var cmd in commands)
                    {
                        currMovable.Move(cmd);
                    }
                }
                Console.WriteLine(AreaToExplore);
            }
            return AreaToExplore.ToString();
        }
    }
}
