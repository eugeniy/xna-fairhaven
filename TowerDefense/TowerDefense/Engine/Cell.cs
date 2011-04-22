﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public class Cell
    {
        public Texture2D Texture { get; set; }

        private bool m_passable = true;
        public enum Type { Open, Closed }

        public Cell(Point position)
        {
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch, Dictionary<Enum, Texture2D> textures, Vector2 location)
        {
            spriteBatch.Draw(textures[m_passable ? Type.Open : Type.Closed], location, Color.White);
        }

        public int G { get; set; }
        public int H { get; set; }
        public int F { get; set; }
        public bool Passable {
            get { return m_passable; }
            set { m_passable = value; }
        }
        public Point Position { get; set; }
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
