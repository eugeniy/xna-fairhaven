using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public class Cell
    {
        private Type m_status = Type.Open;

        [Flags]
        public enum Type {
            Closed = 1, Open = 2, Path = 4,
            OpenPath = Open | Path
        }

        public Cell(Point position)
        {
            Position = position;
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
        
        public Point Position { get; set; }
        public Cell Parent { get; set; }
        public Point ParentPosition { get; set; }
        public Point[] Adjacent { get { return new Point[] {
            new Point(Position.X, Position.Y + 1),
            new Point(Position.X + 1, Position.Y + 1),
            new Point(Position.X + 1, Position.Y),
            new Point(Position.X + 1, Position.Y - 1),
            new Point(Position.X, Position.Y - 1),
            new Point(Position.X - 1, Position.Y - 1),
            new Point(Position.X - 1, Position.Y),
            new Point(Position.X - 1, Position.Y + 1)
        }; } }

    }

    public class CellComparer : Comparer<Cell>
    {
        public override int Compare(Cell x, Cell y)
        {
            return x.F.CompareTo(y.F);
        }
    }
}
