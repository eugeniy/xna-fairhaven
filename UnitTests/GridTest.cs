﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using NUnit.Framework;

using TowerDefense;
using C5;

namespace UnitTests
{
    [TestFixture]
    public class GridTest
    {
        Stopwatch timer;

        [SetUp]
        public void SetUp()
        {
            timer = new Stopwatch();
        }

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
            x.F = 10;
            grid.m_open.Add(x);

            Cell y = new Cell(p);
            y.F = 5;
            grid.m_open.Add(y);

            Assert.AreEqual(2, grid.m_open.Count);
            Assert.AreEqual(y, grid.m_open.FindMin());
            Assert.AreEqual(x, grid.m_open.FindMax());
        }


        [Test]
        public void SetPoints()
        {
            Point point = new Point(5, 5);
            Cell cell = new Cell(point);
            cell.Position = point;
            cell.ParentPosition = cell.Position;
            Point newPoint = new Point(1, 1);
            cell.Position = newPoint;

            Assert.AreNotEqual(cell.ParentPosition, cell.Position);
        }


        [Test]
        public void CheckIndexers()
        {
            Point point = new Point(1, 1);
            Grid grid = new Grid(2, 2);
            Assert.IsInstanceOf<Cell>(grid[0, 0]);
            Assert.IsInstanceOf<Cell>(grid[point]);

            Assert.AreEqual(Cell.Type.Open, grid[point].Status);
            grid[point].Status = Cell.Type.Closed;
            Assert.AreEqual(Cell.Type.Closed, grid[point].Status);
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


        [Test]
        public void FindInList()
        {
            Point firstPoint = new Point(1, 2);
            Grid grid = new Grid(4, 4);
            Cell cell = grid.FindCellInList(grid.m_open, firstPoint);

            Assert.IsNull(cell);

            Point secondPoint = new Point(3, 4);
            Cell c1 = new Cell(secondPoint);
            grid.m_open.Add(c1);

            cell = grid.FindCellInList(grid.m_open, firstPoint);
            Assert.IsNull(cell);

            cell = grid.FindCellInList(grid.m_open, secondPoint);
            Assert.IsNotNull(cell);
            Assert.AreEqual(c1, cell);
        }

        [Test]
        public void CheckInitialBoard()
        {
            Grid grid = new Grid(2, 2);
            Assert.IsTrue(grid[0, 0].Passable);
            Assert.IsTrue(grid[1, 1].Passable);
            Assert.AreEqual(4, grid.Count);
            try
            {
                Cell cell = grid[2, 2];
                Assert.Fail();
            }
            catch (System.ArgumentOutOfRangeException) {}

            Assert.IsTrue(grid.m_open.IsEmpty);
            Assert.AreEqual(0, grid.m_closed.Count);
        }

        [Test]
        public void FindShortestPath()
        {
            Point start = new Point(0, 0);
            Point zeroEnd = new Point(0, 0);
            Point end = new Point(1, 1);
            Grid grid = new Grid(2, 2);

            Assert.IsNotNull(grid.FindPath(start, zeroEnd));
            Assert.AreEqual(start, grid.FindPath(start, zeroEnd)[0].Position);
            Assert.AreEqual(zeroEnd, grid.FindPath(start, zeroEnd)[grid.m_closed.Count - 1].Position);

            grid = new Grid(2, 2);
            Assert.IsNotNull(grid.FindPath(start, end));
            //Assert.AreEqual(null, grid.FindPath(start, end));
            Assert.AreEqual(start, grid.FindPath(start, end)[0].Position);
            Assert.AreEqual(end, grid.FindPath(start, end)[grid.m_closed.Count - 1].Position);
        }

        [Test]
        public void EverythingResetBetweenCalls()
        {
            Point start = new Point(0, 0);
            Point end = new Point(5, 5);
            Grid grid = new Grid(10, 10);

            Assert.AreEqual(6, grid.FindPath(start, end).Count);
            Assert.AreEqual(6, grid.FindPath(start, end).Count);
        }


        [Test]
        public void CheckFlags()
        {
            Grid grid = new Grid(2, 2);

            Assert.AreEqual(Cell.Type.Open, grid[0,0].Status);
            Assert.AreEqual(Cell.Type.Open, grid[0, 0].Status);
            Assert.IsTrue(grid[0, 0].Passable);

            // Toggle on a path flag
            grid[0, 0].Status |= Cell.Type.Path;
            Assert.AreEqual(Cell.Type.Open | Cell.Type.Path, grid[0, 0].Status);
            Assert.IsTrue(grid[0, 0].Status.HasFlag(Cell.Type.Open));
            Assert.IsTrue(grid[0, 0].Status.HasFlag(Cell.Type.Path));
            Assert.IsTrue(grid[0, 0].Passable);

            // Toggle off an open flag
            grid[0, 0].Status ^= Cell.Type.Open;
            Assert.AreEqual(Cell.Type.Path, grid[0, 0].Status);
            Assert.IsFalse(grid[0, 0].Status.HasFlag(Cell.Type.Open));
            Assert.IsTrue(grid[0, 0].Status.HasFlag(Cell.Type.Path));
            Assert.IsFalse(grid[0, 0].Passable);
        }


        [Test]
        [Category("Long")]
        public void EvaluatePerformance()
        {
            Cell cell;
            Point start = new Point(0, 0);
            Point end = new Point(50, 50);
            Grid grid = new Grid(60, 60);

            // Create a relatively complex map
            for (int i = 0; i < 40; i++)
            {
                cell = new Cell(new Point(10, i));
                cell.Status = Cell.Type.Closed;
                grid[10, i] = cell;
            }

            for (int i = 59; i > 1; i--)
            {
                cell = new Cell(new Point(30, i));
                cell.Status = Cell.Type.Closed;
                grid[30, i] = cell;
            }

            timer.Start();
            grid.FindPath(start, end);
            timer.Stop();

            Assert.That(timer.ElapsedMilliseconds, Is.LessThan(750));

        }



        [Test]
        public void FindShortestPathArray()
        {
            Point start = new Point(0, 0);
            Point end = new Point(1, 1);
            Grid grid = new Grid(2, 2);

            var list = grid.FindPath(start, end);

            var array = grid.FindPathArray(start, end);
            var expected = new Point[] { start, end };

            Assert.AreEqual(list.Count, array.Length);
            Assert.AreEqual(expected, array);
        }

        [Test]
        public void NoPathWithALargerGrid()
        {
            Cell cell;
            Point start = new Point(0, 0);
            Point end = new Point(5, 5);
            Grid grid = new Grid(10, 10);

            for (int i = 0; i < 10; i++)
            {
                cell = new Cell(new Point(2, i));
                cell.Status = Cell.Type.Closed;
                grid[2, i] = cell;
            }

            Assert.AreEqual(null, grid.FindPathArray(start, end));
        }


        [Test]
        public void FoundPathCellsReferenceCellsInGrid()
        {
            Point start = new Point(0, 0);
            Point end = new Point(1, 1);
            Grid grid = new Grid(2, 2);

            Cell cellOne = new Cell(start);
            Cell cellTwo = cellOne;
            Assert.AreEqual(cellOne, cellTwo);

            Assert.AreEqual(grid[0, 0].Position, grid.FindPath(start, end)[0].Position);
            Assert.IsTrue(grid[0, 0].Equals(grid.FindPath(start, end)[0]));
            Assert.AreEqual(grid[0, 0], grid.FindPath(start, end)[0]);
            Assert.AreEqual(grid[1, 1], grid.FindPath(start, end)[1]);
        }

        [Test]
        public void FoundPathCorrectness()
        {
            Point start = new Point(0, 0);
            Point end = new Point(0, 4);
            Grid grid = new Grid(5, 5);
            var expected = new List<Cell> { grid[0, 0], grid[0, 1], grid[0, 2], grid[0, 3], grid[0, 4] };

            Assert.AreEqual(expected, grid.FindPath(start, end));

            grid.Path = grid.FindPath(new Point(0, 0), new Point(0, 4));

            Assert.AreEqual(expected, grid.Path);
        }






    }
}
