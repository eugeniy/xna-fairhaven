using System;
using System.Drawing;
using NUnit.Framework;

using TowerDefense;

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
    }
}
