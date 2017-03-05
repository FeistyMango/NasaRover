using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;

namespace NasaApp
{
    public static class ParsingHelper
    {
        public static Regex IsMovable = new Regex(@"^\d \d [NEWS]$");
        public static Regex IsMovementCommand = new Regex("^[LRM]+$");
        public static Regex IsEnvironmentBoundary = new Regex(@"^\d \d$");
        public static Point ParsePosition(string instruction)
        {
            var parts = instruction.Split(new char[] { ' ' });
            int x, y;
            if (int.TryParse(parts[0], out x) && int.TryParse(parts[1], out y))
            {
                return new Point(x, y);
            }
            throw new Exception("Error Building Coordinate: " + instruction);
        }
    }
}
