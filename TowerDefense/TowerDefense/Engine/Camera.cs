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
        private Matrix m_translation;
        private Matrix m_rotation;

        private Vector3 m_position = new Vector3(0, 0, 30);
        private Vector3 m_angle = Vector3.Zero;

        private float m_windowWidth;
        private float m_windowHeight;
        private float m_aspectRatio;

        private const float m_scrollSpeed = 0.25f;
        private const int m_edgeSize = 20;

        private MouseState m_prevMouse;


        /// <summary>
        /// Creates the instance of the camera.
        /// </summary>
        public Camera(Game game) : base(game)
        {
            m_translation = Matrix.Identity;
            m_rotation = Matrix.Identity;

            m_windowWidth = (float)Game.Window.ClientBounds.Width;
            m_windowHeight = (float)Game.Window.ClientBounds.Height;
            m_aspectRatio = m_windowWidth / m_windowHeight;

            // Create default camera transformations
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, m_aspectRatio, 0.01f, 1000);
            View = Matrix.CreateLookAt(m_position, Vector3.Zero, Vector3.Up);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }


        /// <summary>
        /// Handle the camera movement using user input.
        /// </summary>
        protected void ProcessInput()
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();


            // Zoom with a scroll wheel
            if (mouse.ScrollWheelValue < m_prevMouse.ScrollWheelValue)
                Move(y: -1);

            else if (mouse.ScrollWheelValue > m_prevMouse.ScrollWheelValue)
                Move(y: 1);


            if (keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift))
            {
                // Tilt the camera with Shift+Up and Shift+Down
                if (keyboard.IsKeyDown(Keys.Up)) 
                    Rotate(x: m_scrollSpeed/2);

                else if (keyboard.IsKeyDown(Keys.Down))
                    Rotate(x: -m_scrollSpeed/2);

                // Rotate the camera with Shift+Left and Shift+Right
                else if (keyboard.IsKeyDown(Keys.Left))
                    Rotate(y: -m_scrollSpeed / 2);

                else if (keyboard.IsKeyDown(Keys.Right))
                    Rotate(y: m_scrollSpeed / 2);
            }


            // Scroll with arrows or cursor on screen edges
            if (keyboard.IsKeyDown(Keys.Up)
                || mouse.Y < m_edgeSize + 1 && mouse.Y >= 0)
                Move(z: m_scrollSpeed);

            if (keyboard.IsKeyDown(Keys.Down)
                || mouse.Y > m_windowHeight - m_edgeSize - 1 
                && mouse.Y <= m_windowHeight)
                Move(z: -m_scrollSpeed);

            if (keyboard.IsKeyDown(Keys.Left)
                || mouse.X < m_edgeSize + 1 && mouse.X >= 0)
                Move(x: m_scrollSpeed);

            if (keyboard.IsKeyDown(Keys.Right)
                || mouse.X > m_windowWidth - m_edgeSize - 1
                && mouse.X <= m_windowWidth)
                Move(x: -m_scrollSpeed);


            m_prevMouse = mouse;
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            m_translation = Matrix.Identity;
            m_rotation = Matrix.Identity;

            // Handle camera movement
            ProcessInput();

            View = m_translation * View * m_rotation;

            base.Update(gameTime);
        }


        /// <summary>
        /// Updates the translation matrix.
        /// </summary>
        /// <param name="x">Distance to move along the X-axis.</param>
        /// <param name="y">Distance to move along the Y-axis.</param>
        /// <param name="z">Distance to move along the Z-axis.</param>
        protected void Move(float x = 0, float y = 0, float z = 0)
        {
            m_position.X += x;
            m_position.Y += y;
            m_position.Z += z;

            m_translation *= Matrix.CreateTranslation(x, y, z);
        }

        /// <summary>
        /// Updates the rotation matrix.
        /// </summary>
        /// <param name="x">Radians to rotate the camera upon the X-axis.</param>
        /// <param name="y">Radians to rotate the camera upon the Y-axis.</param>
        /// <param name="z">Radians to rotate the camera upon the Z-axis.</param>
        protected void Rotate(float x = 0, float y = 0, float z = 0)
        {
            m_angle.X += x;
            m_angle.Y += y;
            m_angle.Z += z;

            m_rotation *= Matrix.CreateFromYawPitchRoll(y, x, z);
        }

        /// <summary>
        /// View matrix accessor.
        /// </summary>
        public Matrix View { get; protected set; }

        /// <summary>
        /// Projection matrix accessor.
        /// </summary>
        public Matrix Projection { get; protected set; }

    }
}
