using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TowerDefense
{
    public class Cell
    {
        public Texture2D Texture { get; set; }

        private bool m_passable = true;

        public Cell(System.Drawing.Point position)
        {
            Position = position;
        }

        public Cell(int x, int y, Texture2D texture)
        {
            Position = new System.Drawing.Point(x, y);
            Texture = texture;
        }



        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, location, Microsoft.Xna.Framework.Color.White);
            spriteBatch.End();
        }

        public int G { get; set; }
        public int H { get; set; }
        public int F { get; set; }
        public bool Passable {
            get { return m_passable; }
            set { m_passable = value; }
        }
        public System.Drawing.Point Position { get; set; }
        public System.Drawing.Point ParentPosition { get; set; }
        public System.Drawing.Point[] Adjacent { get { return new System.Drawing.Point[] {
            new System.Drawing.Point(Position.X, Position.Y + 1),
            new System.Drawing.Point(Position.X + 1, Position.Y + 1),
            new System.Drawing.Point(Position.X + 1, Position.Y),
            new System.Drawing.Point(Position.X + 1, Position.Y - 1),
            new System.Drawing.Point(Position.X, Position.Y - 1),
            new System.Drawing.Point(Position.X - 1, Position.Y - 1),
            new System.Drawing.Point(Position.X - 1, Position.Y),
            new System.Drawing.Point(Position.X - 1, Position.Y + 1)
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
