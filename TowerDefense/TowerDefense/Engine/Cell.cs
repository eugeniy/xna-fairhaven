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
        public int x, y;

        public Cell(Point position)
        {
            x = position.X;
            y = position.Y;
        }



    }
}
