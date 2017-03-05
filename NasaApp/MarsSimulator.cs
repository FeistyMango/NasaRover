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

        public Stack<string> DataFromMissionControl { get; set; }

        public MarsSimulator(string inputFromNASA)
        {
            var data = inputFromNASA.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Reverse();
            DataFromMissionControl = new Stack<string>(data);
            Movables = new List<IMovable>();

            var gridBoundaries = DataFromMissionControl.Pop();

            if (!ParsingHelper.IsEnvironmentBoundary.IsMatch(gridBoundaries))
            {
                throw new Exception("Error Invalid Environment Boundaries: " + gridBoundaries);
            }

            AreaToExplore = new Plateau(gridBoundaries);
        }

        public string Simulate()
        {
            IMovable currMovable = null;
            while (DataFromMissionControl.Any())
            {
                var instruction = DataFromMissionControl.Pop();
                if (ParsingHelper.IsMovable.IsMatch(instruction))
                {
                    var tmp = new Rover(instruction, AreaToExplore);
                    //output.AppendLine(currMovable != null ? currMovable.ToString() : tmp.ToString());
                    currMovable = tmp;
                    Movables.Add(currMovable);

                    Console.WriteLine("Deploying Rover:" + tmp);
                }
                else if (ParsingHelper.IsMovementCommand.IsMatch(instruction) && currMovable != null)
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
