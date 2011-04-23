using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public abstract class GameObject
    {

        public void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.2f, 0.5f, 0.2f);
                    effect.DirectionalLight0.Direction = new Vector3(4, 1, -6);
                    effect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }

    }
}
