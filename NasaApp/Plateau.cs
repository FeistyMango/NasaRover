using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NasaApp
{
    public class Plateau : IEnvironment
    {
        private IParser Parser { get; set; }
        protected IMovable[][] m_grid;
        public IMovable[][] Grid { get { return m_grid; } }

        public Plateau(IParser parser)
        {
            Parser = parser;
        }

        public IEnvironment Init(string gridBoundaries)
        {
            var points = gridBoundaries.Split(new char[] { ' ' });
            var boundary = Parser.ParsePosition(gridBoundaries);
            m_grid = new IMovable[boundary.Y][];

            for (var y = 0; y < Grid.Length; y++)
            {
                m_grid[y] = new IMovable[boundary.X];
            }
            return this;
        }

        public bool IsPositionValid(Point coordinate)
        {
            var y = coordinate.X;
            var x = coordinate.Y;
            return y >= 0 && y < Grid.Length
                && x >= 0 && x < Grid.GetLength(0);
        }

        public bool IsPositionOpen(Point coordinate)
        {
            var y = coordinate.X;
            var x = coordinate.Y;
            return IsPositionValid(coordinate) && Grid[y][x] == null;
        }

        public void SetPosition(IMovable movable, Point coordinate)
        {
            var isAvailable = IsPositionOpen(coordinate);
            if (isAvailable)
            {
                var oldY = movable.Position.X;
                var oldX = movable.Position.Y;
                m_grid[oldY][oldX] = null;

                var newY = coordinate.X;
                var newX = coordinate.Y;
                m_grid[newY][newX] = movable;
                movable.Position = coordinate;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < Grid.Length; y++)
            {
                var stuff = Grid[y].Select(p => p != null ? (p.Id + "(" + p.Direction + ")") : " ").Select(p => p.PadLeft(4));
                var row = String.Join("|", stuff);
                row = "|" + row + "|";
                sb.AppendLine(row);
                sb.AppendLine("---------------------------");
            }
            return sb.ToString();
        }
    }
}
