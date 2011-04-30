using System;
using System.Collections.Generic;
using System.Linq;

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

        private VertexPositionColor[] m_vertices;
        private int[] m_indices;
        private VertexBuffer m_vertexBuffer;
        private BasicEffect m_effect;


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


        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            SetupVertices();
            SetupIndices();

            // Set vertex data in VertexBuffer
            m_vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), m_vertices.Length, BufferUsage.None);
            m_vertexBuffer.SetData(m_vertices);

            // Initialize the BasicEffect
            m_effect = new BasicEffect(graphicsDevice);
        }

        public void Update()
        {
        }



        
        



        public void Draw(GraphicsDevice graphicsDevice, Camera camera)
        {
            graphicsDevice.SetVertexBuffer(m_vertexBuffer);

            m_effect.World = m_world;
            m_effect.View = camera.View;
            m_effect.Projection = camera.Projection;
            m_effect.VertexColorEnabled = true;

            // Begin effect and draw for each pass
            foreach (EffectPass pass in m_effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList, m_vertices, 0, m_vertices.Length,
                    m_indices, 0, m_indices.Length / 3, VertexPositionColor.VertexDeclaration);
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

        public int GetCellCost(Cell cell)
        {
            return cell.Status.HasFlag(Cell.Type.Open) ? 1 : 0;
        }



        /// <summary>
        /// Define points used to draw triangles.
        /// </summary>
        protected void SetupVertices()
        {
            m_vertices = new VertexPositionColor[m_width * m_height];

            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    m_vertices[x + y * m_width].Position = new Vector3(x, 0, -y);
                    m_vertices[x + y * m_width].Color = this[x, y].Status.HasFlag(Cell.Type.Path) ? Color.Red : Color.White;
                }
            }
        }


        /// <summary>
        /// Define indices that specify the order in which points are contected.
        /// </summary>
        protected void SetupIndices()
        {
            int index = 0;
            m_indices = new int[(m_width - 1) * (m_height - 1) * 6];
            
            for (int y = 0; y < m_height - 1; y++)
            {
                for (int x = 0; x < m_width - 1; x++)
                {
                    m_indices[index++] = x + (y + 1) * m_width; // top left
                    m_indices[index++] = (x + 1) + y * m_width; // bottom right
                    m_indices[index++] = x + y * m_width; // bottom left

                    m_indices[index++] = x + (y + 1) * m_width;
                    m_indices[index++] = (x + 1) + (y + 1) * m_width; // top right
                    m_indices[index++] = (x + 1) + y * m_width;
                }
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


        public List<Cell> Path { get; set; }
        public int Width { get { return m_width; } }
        public int Height { get { return m_height; } }
        public int Count { get { return m_grid.Count; } }
    }
}
