using Microsoft.Xna.Framework;

namespace TowerDefense
{
    public class Camera
    {
        public Vector3 Position;
        public float Rotation;
        public float Distance;

        public Camera()
        {
            Position = new Vector3(0, 0, 20);
            Rotation = 0;
            Distance = 20;
        }

        public void Update(GameTime gameTime, out Matrix view)
        {
            Rotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.05f;

            view = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)) *
                Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Up);
        }
    }
}
