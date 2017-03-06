using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Serilog;

namespace NasaApp
{
    public class Rover : IMovable
    {
        private static int m_roverIdCounter = 0;
        public int Id { get; set; }
        public Point Position { get; set; }
        public char Direction { get; set; }
        public IEnvironment Environment { get; set; }
        public IParser Parser { get; set; }
        public ILogger Logger { get; set; }

        public Rover(IEnvironment environment, IParser parser, ILogger logger)
        {
            Environment = environment;
            Parser = parser;
            Logger = logger;
        }

        public IMovable Init(string startingInstruction)
        {
            Id = ++m_roverIdCounter;
            Point coordinate = new Point(0, 0);
            var error = false;

            try
            {
                var position = startingInstruction.Split(new char[] { ' ' });
                Direction = position[2][0];
                coordinate = Parser.ParsePosition(startingInstruction);
            }
            catch
            {
                error = true;
                Environment.SetPosition(this, new Point(0, 0));
                Logger.Error("Deploying Rover ID: Error Parsing Coordinates {coordinate}, Using Contigency Location at " + this.ToString(), coordinate);
            }

            if (!error)
            {
                if (Environment.IsPositionOpen(coordinate))
                {
                    Environment.SetPosition(this, coordinate);
                    Logger.Information("Deploying Rover: " + this.ToString());
                }
                else
                {
                    Environment.SetPosition(this, new Point(0, 0));
                    Logger.Warning("Deploying Rover ID: Coorindates Occupied/Invalid {coordinate}, Using Contigency Location at " + this.ToString(), coordinate);
                }
            }

            return this;
        }

        /// <summary>
        /// Move Rover.
        /// Movement Command: Can only move if position is available
        /// Turn Command: Will always successfully turn
        /// </summary>
        /// <param name="command"></param>
        public void Move(char command)
        {
            Point nextPosition;
            switch (command)
            {
                case 'L':
                case 'R':
                    ChangeDirection(command);
                    break;
                case 'M':
                    var newPosition = CalculateNewPosition();
                    if (Environment.IsPositionOpen(newPosition))
                    {
                        Environment.SetPosition(this, newPosition);
                    }
                    else
                    {
                        Logger.Information("Rover: Movement obstructued at {newPosition}, " + this.ToString(), newPosition);
                    }
                    break;
                default:
                    Logger.Error("Rover " + Id + " - Unrecognized Command: " + command);
                    break;
            }
        }

        private Point CalculateNewPosition()
        {
            switch (Direction)
            {
                case 'N':
                    return new Point(Position.X, Position.Y + 1);
                case 'E':
                    return new Point(Position.X + 1, Position.Y);
                case 'W':
                    return new Point(Position.X - 1, Position.Y);
                case 'S':
                    return new Point(Position.X, Position.Y - 1);
            }
            return Position;
        }

        private void ChangeDirection(char command)
        {
            switch (Direction)
            {
                case 'N':
                    Direction = command == 'L' ? 'W' : 'E';
                    break;
                case 'E':
                    Direction = command == 'L' ? 'N' : 'S';
                    break;
                case 'W':
                    Direction = command == 'L' ? 'S' : 'N';
                    break;
                case 'S':
                    Direction = command == 'L' ? 'E' : 'W';
                    break;
            }
        }

        public override string ToString()
        {
            return string.Format("ID: {0}, Position: ({1}, {2}), Direction: {3}", Id, Position.X, Position.Y, Direction);
        }
    }
}
