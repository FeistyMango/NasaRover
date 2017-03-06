using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;
using Serilog;

namespace NasaApp
{
    public class Parser: IParser
    {
        private static Regex m_isMovableRegex;
        private static Regex m_isMovementCommandRegex;
        private static Regex m_isEnvironmentBoundaryRegex;
        public ILogger Logger { get; set; }

        public Parser(ILogger logger)
        {
            Logger = logger;
            m_isMovableRegex = new Regex(@"^\d+ \d+ [NEWS]$");
            m_isMovementCommandRegex = new Regex("^[LRM]+$"); //^(?=([LRM]+)).*$
            m_isEnvironmentBoundaryRegex = new Regex(@"^\d+ \d+$");
        }

        public Point ParsePosition(string instruction)
        {
            var parts = instruction.Split(new char[] { ' ' });
            int x, y;
            if (int.TryParse(parts[0], out x) && int.TryParse(parts[1], out y))
            {
                return new Point(x, y);
            }
            var error = "Error Building Coordinate: " + instruction;

            throw new Exception(error);
        }

        public bool IsMovable(string input)
        {
            return m_isMovableRegex.IsMatch(input);
        }

        public bool IsMovementCommand(string input)
        {
            return m_isMovementCommandRegex.IsMatch(input);
        }

        public bool IsEnvironmentBoundary(string input)
        {
            return m_isEnvironmentBoundaryRegex.IsMatch(input);
        }
    }
}
