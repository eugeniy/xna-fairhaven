using System;
using System.Drawing;
using NUnit.Framework;

using TowerDefense;
using C5;

namespace UnitTests
{
    [TestFixture]
    class GridTest
    {
        [Test]
        public void CalculatingDistance()
        {
            Grid grid = new Grid(5, 5);
            Point start = new Point(2, 2);
            Point end = new Point(1, 0);

            Assert.AreEqual(3, grid.EstimateCost(start, end));
        }

        [Test]
        public void PriorityQueueCreation()
        {
            Grid grid = new Grid(4, 4);
            Assert.AreEqual(0, grid.m_open.Count);
        }

        [Test]
        public void CompareCellsInQueue()
        {
            Grid grid = new Grid(4, 4);
            Point p = new Point(0, 0);

            Assert.AreEqual(0, grid.m_open.Count);

            Cell x = new Cell(p);
            x.f = 10;
            grid.m_open.Add(x);

            Cell y = new Cell(p);
            y.f = 5;
            grid.m_open.Add(y);

            Assert.AreEqual(2, grid.m_open.Count);
            Assert.AreEqual(y, grid.m_open.FindMin());
            Assert.AreEqual(x, grid.m_open.FindMax());
        }

        [Test]
        public void CheckAdjacent()
        {
            Point zero = new Point(0, 0);
            var zeroAdjacent = new Point[] {
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
                new Point(1, -1),
                new Point(0, -1),
                new Point(-1, -1),
                new Point(-1, 0),
                new Point(-1, 1)
            };

            Cell cell = new Cell(zero);
            Assert.AreEqual(8, cell.Adjacent.Length);
            Assert.AreEqual(zeroAdjacent, cell.Adjacent);

            Point point = new Point(5, 5);
            var pointAdjacent = new Point[] {
                new Point(5, 6),
                new Point(6, 6),
                new Point(6, 5),
                new Point(6, 4),
                new Point(5, 4),
                new Point(4, 4),
                new Point(4, 5),
                new Point(4, 6)
            };

            cell = new Cell(point);
            Assert.AreEqual(pointAdjacent, cell.Adjacent);

        }
    }
}
