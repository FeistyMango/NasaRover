using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NasaApp
{
    public interface IMovable
    {
        int Id { get; set; }
        IEnvironment Environment { get; set; }
        Point Position { get; set; }
        char Direction { get; set; }
        void Move(char command);
    }

    public interface IEnvironment
    {
        IMovable[][] Grid { get; }
        bool IsPositionValid(Point coordinate);
        bool IsPositionOpen(Point coordinate);
        void SetPosition(IMovable movable, Point coordinate);
    }
}
