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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Grid map;
        Camera camera;




        VertexPositionColor[] verts;
        int[] indices;

        VertexBuffer vertexBuffer;
        BasicEffect effect;




        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 1440;
            graphics.PreferredBackBufferHeight = 900;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            map = new Grid(4, 3);

            //map.Randomize();
            
            // Calculate shortest path
            //map.Path = map.FindPath(new Point(0, 0), new Point(19, 13));

            // TODO: Do something when map.Path is null
            // Toggle path flag for cells on the path
            //for (int i = 0; i < map.Path.Count; i++)
            //        map.Path[i].Status |= Cell.Type.Path;


            camera = new Camera(this);
            Components.Add(camera);


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here




            // Setup vertices



            verts = new VertexPositionColor[map.Width * map.Height];

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    verts[x + y * map.Width].Position = new Vector3(x, 0, -y);
                    verts[x + y * map.Width].Color = (x%2==0) ? Color.White : Color.Red;
                }
            }



            // Setup indices








            indices = new int[(map.Width - 1) * (map.Height - 1) * 6];
            int counter = 0;
            for (int y = 0; y < map.Height - 1; y++)
            {
                for (int x = 0; x < map.Width - 1; x++)
                {
                    int lowerLeft = x + y * map.Width;
                    int lowerRight = (x + 1) + y * map.Width;
                    int topLeft = x + (y + 1) * map.Width;
                    int topRight = (x + 1) + (y + 1) * map.Width;
 
                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;
 
                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }







            // Set vertex data in VertexBuffer
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor),
            verts.Length, BufferUsage.None);
            vertexBuffer.SetData(verts);

            // Initialize the BasicEffect
            effect = new BasicEffect(GraphicsDevice);







            map.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);

            // Allows the game to exit
            if (gamepad.Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            // Enter fullscreen
            else if (keyboard.IsKeyDown(Keys.RightAlt) && keyboard.IsKeyDown(Keys.Enter))
                graphics.ToggleFullScreen();


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SteelBlue);

            // TODO: Add your drawing code here




            GraphicsDevice.SetVertexBuffer(vertexBuffer);


            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;



            //Set object and camera info
            effect.World = Matrix.Identity;
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.VertexColorEnabled = true;
            // Begin effect and draw for each pass
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives <VertexPositionColor>
                    (PrimitiveType.TriangleList, verts, 0, verts.Length, indices, 0, indices.Length / 3, VertexPositionColor.VertexDeclaration);
            }






            map.Draw3D(camera);


            base.Draw(gameTime);
        }
    }
}
