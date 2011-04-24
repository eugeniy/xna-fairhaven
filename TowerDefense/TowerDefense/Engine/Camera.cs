using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense
{
    public class Camera
    {
        public Vector3 Position;
        public float Rotation = 0;
        public Vector3 Translation;

        private float m_distance = 20;
        private const float m_minDistance = 15;
        private const float m_maxDistance = 40;

        private const float m_scrollSpeed = 0.25f;
        private const int m_edgeSize = 20;

        private MouseState m_prevMouse;

        
        public Camera()
        {
            Position = new Vector3(0, 0, Distance);
            Translation = Vector3.Zero;
        }


        /// <summary>
        /// Handle zooming and panning of the camera.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="graphics"></param>
        /// <param name="view">The view matrix used for camera transformation. Output only.</param>
        public void Update(GameTime gameTime, GraphicsDeviceManager graphics, out Matrix view)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            bool right_edge = (mouse.X > graphics.PreferredBackBufferWidth - m_edgeSize - 1 && mouse.X <= graphics.PreferredBackBufferWidth) ? true : false;
            bool left_edge = (mouse.X < m_edgeSize + 1 && mouse.X >= 0) ? true : false;
            bool top_edge = (mouse.Y < m_edgeSize + 1 && mouse.Y >= 0) ? true : false;
            bool bottom_edge = (mouse.Y > graphics.PreferredBackBufferHeight - m_edgeSize - 1 && mouse.Y <= graphics.PreferredBackBufferHeight) ? true : false;


            // Zoom with a scroll wheel
            if (mouse.ScrollWheelValue < m_prevMouse.ScrollWheelValue)
                Position.Z = ++Distance;
            else if (mouse.ScrollWheelValue > m_prevMouse.ScrollWheelValue)
                Position.Z = --Distance;


            // Pan with cursor on edges and arrows
            if (keyboard.IsKeyDown(Keys.Up) || top_edge)
                Translation.Y -= m_scrollSpeed;
            if (keyboard.IsKeyDown(Keys.Down) || bottom_edge)
                Translation.Y += m_scrollSpeed;
            if (keyboard.IsKeyDown(Keys.Left) || left_edge)
                Translation.X += m_scrollSpeed;
            if (keyboard.IsKeyDown(Keys.Right) || right_edge)
                Translation.X -= m_scrollSpeed;
                
            m_prevMouse = mouse;
            

            // Do the actual translation
            view = Matrix.CreateTranslation(Translation) 
                * Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Up);
        }

        /// <summary>
        /// Camera distance accessor. Capped by min and max values.
        /// </summary>
        public float Distance {
            get { return m_distance; }
            set {
                if (value > m_maxDistance) m_distance = m_maxDistance;
                else if (value < m_minDistance) m_distance = m_minDistance;
                else m_distance = value;
            }
        }

    }
}
