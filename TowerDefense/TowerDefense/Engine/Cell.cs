using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TowerDefense
{
    public class Cell
    {
        private bool m_passable = true;

        public Cell(Point position)
        {
            Position = position;
        }

        public int G { get; set; }
        public int H { get; set; }
        public int F { get; set; }
        public bool Passable {
            get { return m_passable; }
            set { m_passable = value; }
        }
        public Point Position { get; set; }
        public Point ParentPosition { get; set; }
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
            return x.F.CompareTo(y.F);
        }
    }
}
