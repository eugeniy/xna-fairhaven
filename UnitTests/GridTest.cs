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
            Assert.AreEqual(0, grid.open.Count);
        }

        [Test]
        public void CompareCellsInQueue()
        {
            Grid grid = new Grid(4, 4);
            Point p = new Point(0, 0);

            Assert.AreEqual(0, grid.open.Count);

            Cell x = new Cell(p);
            x.f = 10;
            grid.open.Add(x);

            Cell y = new Cell(p);
            y.f = 5;
            grid.open.Add(y);

            Assert.AreEqual(2, grid.open.Count);
            Assert.AreEqual(y, grid.open.FindMin());
            Assert.AreEqual(x, grid.open.FindMax());
        }
    }
}
