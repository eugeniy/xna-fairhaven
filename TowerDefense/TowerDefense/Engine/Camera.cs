using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense
{
    public class Camera
    {
        private const float m_defaultDistance = 20;
        private const float m_minDistance = 10;
        private const float m_maxDistance = 50;
        private const float m_defaultRotation = 0;
        private MouseState m_prevMouseState;

        public float Distance;
        public float Rotation;
        public Vector3 Position;

        public Camera()
        {
            Distance = m_defaultDistance;
            Rotation = m_defaultRotation;
            Position = new Vector3(0, 0, Distance);
            m_prevMouseState = Mouse.GetState();
        }

        public void Update(GameTime gameTime, out Matrix view, MouseState mouseState)
        {

            if (mouseState != m_prevMouseState)
            {
                if (mouseState.ScrollWheelValue < m_prevMouseState.ScrollWheelValue && Distance < m_maxDistance)
                    Distance++;

                else if (mouseState.ScrollWheelValue > m_prevMouseState.ScrollWheelValue && Distance > m_minDistance)
                    Distance--;

                else if (mouseState.MiddleButton == ButtonState.Pressed)
                    Distance = m_defaultDistance;


                Position = new Vector3(0, 0, Distance);
                m_prevMouseState = mouseState;
            }

            

            //Rotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.05f;

            view = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)) *
                Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Up);
        }
    }
}
