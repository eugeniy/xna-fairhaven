using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TowerDefense
{
    public class Cell
    {
        public int g, h, f;
        protected bool valid = true;

        public Cell(Point position)
        {
            Position = position;
        }

        public bool Valid { get { return valid; } }
        public Point Position { get; set; }
        public Point[] Adjacent { get { return new Point[] {
            new Point(Position.X, Position.Y + 1),
            new Point(Position.X + 1, Position.Y + 1),
            new Point(Position.X + 1, Position.Y),
            new Point(Position.X + 1, Position.Y - 1),
            new Point(Position.X, Position.Y - 1),
            new Point(Position.X - 1, Position.Y - 1),
            new Point(Position.X - 1, Position.Y),
            new Point(Position.X - 1, Position.Y + 1)
        }; } }

    }

    public class CellComparer : Comparer<Cell>
    {
        public override int Compare(Cell x, Cell y)
        {
            return x.f.CompareTo(y.f);
        }
    }
}
