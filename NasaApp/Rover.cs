using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NasaApp
{
    public class Rover : IMovable
    {
        private static int m_roverIdCounter = 0;
        public int Id { get; set; }
        public Point Position { get; set; }
        public char Direction { get; set; }
        public IEnvironment Environment { get; set; }

        public Rover(string startingInstruction, IEnvironment environment, IParser parser)
        {
            Id = ++m_roverIdCounter;
            Environment = environment;

            var position = startingInstruction.Split(new char[] { ' ' });

            Direction = position[2][0];

            var coordinate = parser.ParsePosition(startingInstruction);
            if (Environment.IsPositionOpen(coordinate))
            {
                Environment.SetPosition(this, coordinate);
            }
            else
            {
                Environment.SetPosition(this, new Point(0, 0));
            }
        }

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
