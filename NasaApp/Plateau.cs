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
        private int ConvertY(int y)
        {
            if (y > Grid.Length)
            {
                return -1;
            }
            var newY = Grid.Length - y;
            newY = Grid.Length == newY ? newY - 1 : newY;
            newY = newY < 0 ? 0 : newY;
            return newY;
        }

        private int ConvertX(int x)
        {
            if (x > Grid.GetLength(0))
            {
                return -1;
            }
            var newX = x - 1;
            newX = Grid.Length == newX ? newX - 1 : newX;
            newX = newX < 0 ? 0 : newX;
            return newX;
        }

        public bool IsPositionValid(Point coordinate)
        {
            var y = ConvertY(coordinate.Y);
            var x = ConvertX(coordinate.X);
            return y >= 0 && y < Grid.Length
                && x >= 0 && x < Grid[y].Length;
        }

        public bool IsPositionOpen(Point coordinate)
        {
            var y = ConvertY(coordinate.Y);
            var x = ConvertX(coordinate.X);
            return IsPositionValid(coordinate) && Grid[y][x] == null;
        }

        public void SetPosition(IMovable movable, Point coordinate)
        {
            var isAvailable = IsPositionOpen(coordinate);
            if (isAvailable)
            {
                var oldY = ConvertY(movable.Position.Y);
                var oldX = ConvertX(movable.Position.X);
                m_grid[oldY][oldX] = null;

                var newY = ConvertY(coordinate.Y);
                var newX = ConvertX(coordinate.X);
                m_grid[newY][newX] = movable;
                movable.Position = coordinate;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(new String('-', 5 * Grid.GetLength(0) + 1));
            for (var y = 0; y < Grid.Length; y++)
            {
                var stuff = Grid[y].Select(p => p != null ? (p.Id + "(" + p.Direction + ")") : " ").Select(p => p.PadLeft(4));
                var row = String.Join("|", stuff);
                row = "|" + row + "|";
                sb.AppendLine(row);

                sb.AppendLine(new String('-', 5 * Grid.GetLength(0) + 1));
            }
            return "\n" + sb.ToString();
        }
    }
}
