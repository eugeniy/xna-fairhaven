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
        private int capacity = 16;
        protected List<Cell> grid;
        public IntervalHeap<Cell> open;
        protected List<Cell> closed;
        

        public Grid(int width, int height)
        {
            capacity = width * height;
            grid = new List<Cell>(capacity);
            open = new IntervalHeap<Cell>(capacity, new CellComparer());
            closed = new List<Cell>(capacity);
        }

        /// <summary>
        /// Calculate the Manhattan Distance
        /// </summary>
        /// <param name="start">Starting point</param>
        /// <param name="end">End point</param>
        /// <returns>A heuristic cost to move from start to end point</returns>
        public int EstimateCost(Point start, Point end)
        {
            return Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y);
        }

        public List<Cell> FindPath(Point start, Point end)
        {
            // Set parent to a starting point and set its g, h, f values
            Cell parent = new Cell(start);
            parent.g = 0;
            parent.h = EstimateCost(start, end);
            parent.f = parent.g + parent.h;

            // Add parent to the open list, should be the only cell at this point
            open.Add(parent);

            while (open.Count > 0)
            {
                // Find the cell with the lowest f value
                var best = open.FindMin();

                // If the best cell is the end, we're done
                if (best.Position == end)
                {
                    return closed;
                }

                // Open list is empty means we weren't able to find the path
                if (open.IsEmpty)
                {
                    return null;
                }

                // Walk through valid adjacent cells
                foreach (Point p in best.Adjacent)
                {

                }
            }
            return closed;
        }
    }
}
