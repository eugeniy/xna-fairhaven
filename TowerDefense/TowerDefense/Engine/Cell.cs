using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TowerDefense
{
    public class Cell
    {
        protected int g, h, f;
        protected int x, y;

        public Cell(Point position)
        {
            x = position.X;
            y = position.Y;
        }
    }
}
