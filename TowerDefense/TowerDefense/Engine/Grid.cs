using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using C5;

namespace TowerDefense
{
    public class Grid
    {
        protected List<Cell> grid;
        protected IPriorityQueue<Cell> open;
        protected List<Cell> closed;

        public Grid(int width, int height)
        {
        }

        /// <summary>
        /// Calculate the Manhattan Distance
        /// </summary>
        /// <param name="start">Starting point</param>
        /// <param name="end">End point</param>
        /// <returns>A heuristic cost to move from start to end point</returns>
        protected int EstimateCost(Point start, Point end)
        {
            return Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y);
        }

        public List<Cell> FindPath(Point start, Point end)
        {
            Cell parent = new Cell(start);
            parent.g = 0;
            parent.h = EstimateCost(start, end);
            parent.f = parent.g + parent.h;
            open.Add(parent);

            while (open.Count > 0)
            {
            }
            return closed;
        }
    }
}
