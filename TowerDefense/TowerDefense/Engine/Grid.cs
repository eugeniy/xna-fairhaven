using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using C5;

namespace TowerDefense
{
    public unsafe class Grid
    {
        private bool found = false;

        private int m_capacity;
        private int m_width, m_height;

        //protected List<Cell> grid;
        public byte[,] m_grid = null;
        public IntervalHeap<Cell> m_open;
        public List<Cell> closed;
        

        public Grid(int width, int height)
        {
            m_width = width;
            m_height = height;
            m_capacity = width * height;
            //grid = new List<Cell>(m_capacity);


            m_grid = new byte[1024, 1024];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    m_grid[x, y] = 1;


            m_open = new IntervalHeap<Cell>(m_capacity, new CellComparer());
            closed = new List<Cell>(m_capacity);
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

        // FIXME: RESET EVERYTHING BETWEEN CALLS!!
        public List<Cell> FindPath(Point start, Point end)
        {
            // Set parent to a starting point and set its g, h, f values
            Cell parent = new Cell(start);
            parent.g = 0;
            parent.h = EstimateCost(start, end);
            parent.f = parent.g + parent.h;
            parent.ParentPosition = start;

            // Add parent to the open list, should be the only cell at this point
            m_open.Add(parent);

            while ( ! m_open.IsEmpty)
            {
                // Find the cell with the lowest f value
                // Pop it off the open and assign the value to parent
                parent = m_open.DeleteMin();

                // If the best cell is the end, we're done
                if (parent.Position == end)
                {
                    closed.Add(parent);
                    found = true;
                    break;
                }

                // Open list is empty means we weren't able to find the path
                //if (m_open.IsEmpty)
                //{
                //    return null;
                //}

                // Walk through valid adjacent cells
                foreach (Point p in parent.Adjacent)
                {
                    if (p.X >= 0 && p.Y >= 0 && p.X <= m_width && p.Y <= m_height)
                    {

                        int g = parent.g + m_grid[p.X, p.Y];





                        if (g == parent.g)
                        {
                            //Unbrekeable
                            continue;
                        }

                        Cell cellInOpen = FindCellInList(m_open, p);

                        if (cellInOpen != null && cellInOpen.g <= g)
                            continue;

                        Cell cellInClosed = FindCellInList(closed, p);
                        if (cellInClosed != null && cellInClosed.g <= g)
                            continue;


                        Cell child = new Cell(p);
                        child.ParentPosition = new Point(parent.Position.X, parent.Position.Y);
                        child.g = g;
                        child.h = EstimateCost(child.Position, end);
                        child.f = child.g + child.h;


                        m_open.Add(child);

                    }
                }
                closed.Add(parent);
            }

            if (found)
            {
                Cell fNode = closed[closed.Count - 1];
                for (int i = closed.Count - 1; i >= 0; i--)
                {
                    if (fNode.ParentPosition.X == closed[i].Position.X && fNode.ParentPosition.Y == closed[i].Position.Y || i == closed.Count - 1)
                    {
                        fNode = closed[i];
                    }
                    else
                        closed.RemoveAt(i);
                }

                return closed;
            }

            return null;
        }



        public Array FindPathArray(Point start, Point end)
        {
            try
            {
                return FindPath(start, end).Select(n => n.Position).ToArray();
            }
            catch (System.ArgumentNullException e)
            {
                return null;
            }
        }

        public Cell FindCellInList(IEnumerable<Cell> list, Point target)
        {
            foreach (Cell c in list)
            {
                if (c.Position == target)
                    return c;
            }
            return null;
        }

        public int Width { get { return m_width; } }
        public int Height { get { return m_height; } }
    }
}
