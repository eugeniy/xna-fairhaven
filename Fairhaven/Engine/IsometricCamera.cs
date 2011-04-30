using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fairhaven
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class IsometricCamera : Camera
    {
        public IsometricCamera(Game game) : base(game)
        {
            game.IsMouseVisible = true;
        }


        /// <summary>
        /// Handle the camera movement using user input.
        /// </summary>
        protected override void ProcessInput()
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            // Disregard Y coordinate, since we just want to move parallel to the ground
            Vector3 rotation = new Vector3(m_direction.X, 0, m_direction.Z);

            // Move camera with WASD keys
            if (keyboard.IsKeyDown(Keys.W) || mouse.Y >= 0 && mouse.Y <= m_edgeSize)
                // Move forward and backwards by adding m_position and m_direction vectors
                m_position += rotation * m_speed;

            if (keyboard.IsKeyDown(Keys.S) || mouse.Y >= m_windowHeight - m_edgeSize && mouse.Y <= m_windowHeight)
                m_position -= rotation * m_speed;

            if (keyboard.IsKeyDown(Keys.A) || mouse.X >= 0 && mouse.X <= m_edgeSize)
                // Strafe by adding a cross product of m_up and m_direction vectors
                m_position += Vector3.Cross(m_up, m_direction) * m_speed;

            if (keyboard.IsKeyDown(Keys.D) || mouse.X >= m_windowWidth - m_edgeSize && mouse.X <= m_windowWidth)
                m_position -= Vector3.Cross(m_up, m_direction) * m_speed;


            // Rotate with Q and E
            if (keyboard.IsKeyDown(Keys.Q))
                m_direction = Vector3.Transform(m_direction,
                    Matrix.CreateFromAxisAngle(m_up, m_rotationSpeed)
                );
            if (keyboard.IsKeyDown(Keys.E))
                m_direction = Vector3.Transform(m_direction,
                    Matrix.CreateFromAxisAngle(m_up, -m_rotationSpeed)
                );


            // Pitch with shift + mouse wheel
            if (keyboard.IsKeyDown(Keys.LeftShift))
            {
                // FIXME: Gets stuck at pitch limits
                if (Math.Abs(m_pitch + m_rotationSpeed) < m_pitchLimit)
                {
                    float angle = 0;

                    if (mouse.ScrollWheelValue > m_prevMouse.ScrollWheelValue)
                    {
                        angle = m_rotationSpeed;
                        m_pitch += angle;
                    }
                    else if (mouse.ScrollWheelValue < m_prevMouse.ScrollWheelValue)
                    {
                        angle = -m_rotationSpeed;
                        m_pitch += angle;
                    }

                    m_direction = Vector3.Transform(m_direction,
                        Matrix.CreateFromAxisAngle(Vector3.Cross(m_up, m_direction), angle)
                    );
                }
            }


            // Zoom with a mouse wheel
            else if (mouse.ScrollWheelValue > m_prevMouse.ScrollWheelValue)
                m_position -= m_up * m_speed;

            else if (mouse.ScrollWheelValue < m_prevMouse.ScrollWheelValue)
                m_position += m_up * m_speed;

            m_prevMouse = mouse;
        }

    }
}
