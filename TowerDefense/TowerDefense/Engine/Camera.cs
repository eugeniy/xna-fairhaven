using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace TowerDefense
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }


        private float m_windowWidth;
        private float m_windowHeight;
        private float m_aspectRatio;


        public Vector3 Position;
        public Vector3 Translation;

        private float m_rotation = 45;

        private float m_distance = 20;
        private const float m_minDistance = 15;
        private const float m_maxDistance = 40;

        private float m_tilt = -30;
        private const float m_minTilt = -75;
        private const float m_maxTilt = 0;

        private const float m_scrollSpeed = 0.25f;
        private const int m_edgeSize = 20;

        private MouseState m_prevMouse;


        public Camera(Game game)
            : base(game)
        {
            // TODO: Construct any child components here


            m_windowWidth = (float)Game.Window.ClientBounds.Width;
            m_windowHeight = (float)Game.Window.ClientBounds.Height;
            m_aspectRatio = m_windowWidth / m_windowHeight;

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, m_aspectRatio, 0.01f, 1000);

            Position = new Vector3(0, 0, Distance);
            Translation = Vector3.Zero;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            bool right_edge = (mouse.X > m_windowWidth - m_edgeSize - 1 && mouse.X <= m_windowWidth) ? true : false;
            bool left_edge = (mouse.X < m_edgeSize + 1 && mouse.X >= 0) ? true : false;
            bool top_edge = (mouse.Y < m_edgeSize + 1 && mouse.Y >= 0) ? true : false;
            bool bottom_edge = (mouse.Y > m_windowHeight - m_edgeSize - 1 && mouse.Y <= m_windowHeight) ? true : false;


            // Zoom with a scroll wheel
            if (mouse.ScrollWheelValue < m_prevMouse.ScrollWheelValue)
                Position.Z = ++Distance;
            else if (mouse.ScrollWheelValue > m_prevMouse.ScrollWheelValue)
                Position.Z = --Distance;

            // Tilt the camera with Shift+Up and Shift+Down
            if ((keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift)) && keyboard.IsKeyDown(Keys.Up))
                Tilt += m_scrollSpeed;
            else if ((keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift)) && keyboard.IsKeyDown(Keys.Down))
                Tilt -= m_scrollSpeed;

            // Pan with cursor on edges and arrows
            else
            {
                if (keyboard.IsKeyDown(Keys.Up) || top_edge)
                    Translation.Y -= m_scrollSpeed;
                if (keyboard.IsKeyDown(Keys.Down) || bottom_edge)
                    Translation.Y += m_scrollSpeed;
                if (keyboard.IsKeyDown(Keys.Left) || left_edge)
                    Translation.X += m_scrollSpeed;
                if (keyboard.IsKeyDown(Keys.Right) || right_edge)
                    Translation.X -= m_scrollSpeed;
            }

            m_prevMouse = mouse;


            // Do the actual translation
            View =
                Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation))
                * Matrix.CreateTranslation(Translation)
                * Matrix.CreateRotationX(MathHelper.ToRadians(Tilt))
                * Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Up);








            base.Update(gameTime);
        }



        /// <summary>
        /// Control camera distance from the map. Capped by min and max values.
        /// </summary>
        public float Distance
        {
            get { return m_distance; }
            set
            {
                if (value > m_maxDistance) m_distance = m_maxDistance;
                else if (value < m_minDistance) m_distance = m_minDistance;
                else m_distance = value;
            }
        }

        /// <summary>
        /// Control the tilt of the camera relative to the map. Capped by min and max values.
        /// </summary>
        public float Tilt
        {
            get { return m_tilt; }
            set
            {
                if (value > m_maxTilt) m_tilt = m_maxTilt;
                else if (value < m_minTilt) m_tilt = m_minTilt;
                else m_tilt = value;
            }
        }

        /// <summary>
        /// Control the rotation of the camera relative to the map.
        /// </summary>
        public float Rotation
        {
            get { return m_rotation; }
            set
            {
                if (value > 360) m_rotation = 0;
                else if (value < 0) m_rotation = 360;
                else m_rotation = value;
            }
        }



    }
}
