﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using C5;

namespace TowerDefense
{
    public class Grid
    {
        private Matrix m_world;

        private int m_capacity;
        private int m_width, m_height;

        private List<Cell> m_grid;
        public IntervalHeap<Cell> m_open;
        public List<Cell> m_closed;

        private BoundingBox[] m_boundingBoxes;
        private Dictionary<Enum, Texture2D> m_textures;
        private Dictionary<string, Model> m_models;
        private float m_scale = 0.396f;


        public List<Cell> Path { get; set; }
        

        public Grid(int width, int height)
        {
            m_world = Matrix.Identity;

            m_width = width;
            m_height = height;
            m_capacity = width * height;

            m_grid = new List<Cell>(m_capacity);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    m_grid.Add(new Cell(new Point(x, y)));

            m_open = new IntervalHeap<Cell>(m_capacity, new CellComparer());
            m_closed = new List<Cell>(m_capacity);
        }


        // TODO: Make this random :)
        public void Randomize()
        {
            for (int i = 2; i < 8; i++)
                this[i, 0].Status = Cell.Type.Closed;

            for (int i = 0; i < 7; i++)
                this[i, 3].Status = Cell.Type.Closed;

            for (int i = 4; i < 10; i++)
                this[i, 6].Status = Cell.Type.Closed;

            for (int i = 2; i < 9; i++)
                this[12, i].Status = Cell.Type.Closed;

            for (int i = 7; i < 11; i++)
                this[15, i].Status = Cell.Type.Closed;

            for (int i = 10; i < 18; i++)
                this[i, 11].Status = Cell.Type.Closed;
        }


        public void LoadContent(ContentManager Content)
        {
            // FIXME: Add a placeholder for keys that don't exist
            m_textures = new Dictionary<Enum, Texture2D>();
            m_textures[Cell.Type.Open] = Content.Load<Texture2D>("Sprites/Grass Block");
            m_textures[Cell.Type.Closed] = Content.Load<Texture2D>("Sprites/Stone Block Tall");
            m_textures[Cell.Type.OpenPath] = m_textures[Cell.Type.Open];

            m_models = new Dictionary<string, Model>();
            m_models["Cube"] = Content.Load<Model>("Models/Grass");
        }

        public void Update()
        {
        }


        private void InitBoundingBoxes()
        {
            List<BoundingBox> bbList = new List<BoundingBox>();

            foreach (Cell cell in m_grid)
            {
                Vector3[] cellPoints = new Vector3[2];
                cellPoints[0] = new Vector3(cell.Coord.X, 0, -cell.Coord.Y);
                cellPoints[1] = new Vector3(cell.Coord.X + 1, cell.Height, -cell.Coord.Y - 1);
                BoundingBox buildingBox = BoundingBox.CreateFromPoints(cellPoints);
                bbList.Add(buildingBox);

            }

            m_boundingBoxes = bbList.ToArray();
        }
        
        
        public void Draw3D(Camera camera)
        {
            Matrix position;
            foreach (Cell cell in m_grid)
            {
                position = Matrix.CreateTranslation((float)cell.Coord.X * 2, cell.Status.HasFlag(Cell.Type.Closed) ? 1 : 0, (float)cell.Coord.Y * 2) * m_world;

                cell.DrawModel(m_models["Cube"], position, camera.View, camera.Projection, new Vector3(cell.Status.HasFlag(Cell.Type.Path) ? 1 : 0.2f, 0.5f, 0.2f));
            }
        }


        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            foreach (Cell cell in m_grid)
            {
                cell.Draw(spriteBatch, m_textures, new Vector2(cell.Coord.X * 101 * m_scale + location.X, cell.Coord.Y * 80 * m_scale + location.Y), m_scale);
            }
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
            m_open = new IntervalHeap<Cell>(m_capacity, new CellComparer());
            m_closed = new List<Cell>(m_capacity);

            Cell temp;

            // Set parent to a starting point and set its g, h, f values
            Cell parent = this[start.X, start.Y];
            parent.G = 0;
            parent.H = EstimateCost(start, end);
            parent.F = parent.G + parent.H;

            // Add parent to the open list, should be the only cell at this point
            m_open.Add(parent);

            while ( ! m_open.IsEmpty)
            {
                // Find the cell with the lowest f value
                // Pop it off the open and assign the value to parent
                parent = m_open.DeleteMin();

                // If the best cell is the end, we're done
                if (parent.Coord == end)
                {
                    m_closed.Add(parent);
                    return ReconstructReversePath(m_closed);
                }

                // Walk through valid adjacent cells
                foreach (Point p in parent.Adjacent)
                {
                    if (p.X >= 0 && p.Y >= 0 && p.X < m_width && p.Y < m_height)
                    {

                        int g = parent.G + GetCellCost(this[p]);


                        if (g == parent.G)
                            continue;


                        // Check if m_open or m_closed contain a Cell with lower G value
                        if (m_open.Find(n => n.Equals(this[p]), out temp) && temp.G <= g)
                            continue;

                        if (m_closed.Contains(this[p]) && m_closed.Find(n => n.Equals(this[p])).G <= g)
                            continue;


                        this[p].Parent = parent;
                        this[p].G = g;
                        this[p].H = EstimateCost(this[p].Coord, end);
                        this[p].F = this[p].G + this[p].H;

                        m_open.Add(this[p]);

                    }
                }
                m_closed.Add(parent);
            }



            return null;
        }


        // TODO: handle null parents
        public List<Cell> ReconstructReversePath(List<Cell> closed)
        {
            Path = new List<Cell>();

            var current = closed.Last();

            // Hop through parents from the end to the start
            while (current != null)
            {
                Path.Insert(0, current);
                current = current.Parent;
            }

            return Path;
        }

        public Array FindPathArray(Point start, Point end)
        {
            try
            {
                return FindPath(start, end).Select(n => n.Coord).ToArray();
            }
            catch (System.ArgumentNullException)
            {
                return null;
            }
        }


        /// <summary>
        /// Cell indexer
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Cell this[int x, int y]
        {
            // FIXME: Range check for x and y
            get { return m_grid[m_width * y + x]; }
            set { m_grid[m_width * y + x] = value; }
        }

        public Cell this[Point position]
        {
            get { return this[position.X, position.Y]; }
            set { this[position.X, position.Y] = value; }
        }


        public int GetCellCost(Cell cell)
        {
            return cell.Status.HasFlag(Cell.Type.Open) ? 1 : 0;
        }

        public int Width { get { return m_width; } }
        public int Height { get { return m_height; } }
        public int Count { get { return m_grid.Count; } }
    }
}
