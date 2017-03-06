using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NasaApp
{
    public interface IMovableFactory
    {
        IMovable Rover();
    }

    public interface IMovable
    {
        int Id { get; set; }
        IEnvironment Environment { get; set; }
        Point Position { get; set; }
        char Direction { get; set; }
        void Move(char command);
        IMovable Init(int id, string startingInstruction);
    }

    public interface IEnvironment
    {
        IMovable[][] Grid { get; }
        IEnvironment Init(string gridBoundaries);
        bool IsPositionValid(Point coordinate);
        bool IsPositionOpen(Point coordinate);
        void SetPosition(IMovable movable, Point coordinate);
    }

    public interface IParser
    {
        Point ParsePosition(string instruction);
        bool IsMovable(string input);
        bool IsMovementCommand(string input);
        bool IsEnvironmentBoundary(string input);
    }
}
