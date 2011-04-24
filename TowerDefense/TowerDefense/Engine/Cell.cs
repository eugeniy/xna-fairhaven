using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public class Cell : GameObject
    {
        //public Model Model;
        //public Matrix[] Transforms;

        // Cell position in the world space
        //public Vector3 Position = Vector3.Zero;


        public int Width = 1;
        public int Height = 1;

        private Type m_status = Type.Open;

        [Flags]
        public enum Type {
            Closed = 1, Open = 2, Path = 4,
            OpenPath = Open | Path
        }

        public Cell(Point position)
        {
            Coord = position;
        }


        public void Draw(SpriteBatch spriteBatch, Dictionary<Enum, Texture2D> textures, Vector2 location, float scale)
        {
            spriteBatch.Draw(textures[Status], location, null, m_status.HasFlag(Type.Path) ? Color.Red : Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public int G { get; set; }
        public int H { get; set; }
        public int F { get; set; }
        public bool Passable {
            get { return m_status.HasFlag(Type.Open); }
        }
        public Type Status {
            get { return m_status; }
            set { m_status = value; }
        }
        
        /// <summary>
        /// Coordinates of the cell on the grid.
        /// </summary>
        public Point Coord { get; set; }
        public Cell Parent { get; set; }

        public Point[] Adjacent { get { return new Point[] {
            new Point(Coord.X, Coord.Y + 1),
            new Point(Coord.X + 1, Coord.Y + 1),
            new Point(Coord.X + 1, Coord.Y),
            new Point(Coord.X + 1, Coord.Y - 1),
            new Point(Coord.X, Coord.Y - 1),
            new Point(Coord.X - 1, Coord.Y - 1),
            new Point(Coord.X - 1, Coord.Y),
            new Point(Coord.X - 1, Coord.Y + 1)
        }; } }

        public override string ToString()
        {
            return Coord.ToString();
        }
    }

    public class CellComparer : Comparer<Cell>
    {
        public override int Compare(Cell x, Cell y)
        {
            return x.F.CompareTo(y.F);
        }
    }
}
