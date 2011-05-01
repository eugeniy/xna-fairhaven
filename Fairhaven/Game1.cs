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

namespace Fairhaven
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


        


        // Particles
        List<ParticleExplosion> explosions = new List<ParticleExplosion>();
        ParticleExplosionSettings particleExplosionSettings = new ParticleExplosionSettings();
        ParticleSettings particleSettings = new ParticleSettings();
        Texture2D explosionTexture;
        Texture2D explosionColorsTexture;
        Effect explosionEffect;

        TimeSpan elapsedTime = TimeSpan.Zero;



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

            map = new Grid(20, 14);

            map.Randomize();
            
            // Calculate shortest path
            map.Path = map.FindPath(new Point(0, 0), new Point(19, 13));

            // TODO: Do something when map.Path is null
            // Toggle path flag for cells on the path
            for (int i = 0; i < map.Path.Count; i++)
                    map.Path[i].Status |= Cell.Type.Path;


            camera = new Camera(this);
            Components.Add(camera);

            Components.Add(new Statistics(this));


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

            



            // Load explosion textures and effect
            explosionTexture = Content.Load<Texture2D>(@"Textures\Particle");
            explosionColorsTexture = Content.Load<Texture2D>(@"Textures\Sun");
            explosionEffect = Content.Load<Effect>(@"Effects\Particle");

            // Set effect parameters that don't change per particle
            explosionEffect.CurrentTechnique = explosionEffect.Techniques["Technique1"];
            explosionEffect.Parameters["theTexture"].SetValue(explosionTexture);





            map.LoadContent(Content, GraphicsDevice);
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


            elapsedTime += gameTime.ElapsedGameTime;


            // Allows the game to exit
            if (gamepad.Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            // Enter fullscreen
            else if (keyboard.IsKeyDown(Keys.RightAlt) && keyboard.IsKeyDown(Keys.Enter))
                graphics.ToggleFullScreen();


            // TODO: Add your update logic here


            else if (keyboard.IsKeyDown(Keys.X) && elapsedTime > TimeSpan.FromSeconds(1))
            {
                explosions.Add(
                new ParticleExplosion(GraphicsDevice, new Vector3(0, 0, -200),
                    ParticleExplosion.rnd.Next(
                        particleExplosionSettings.minLife,
                        particleExplosionSettings.maxLife),
                    ParticleExplosion.rnd.Next(
                        particleExplosionSettings.minRoundTime,
                        particleExplosionSettings.maxRoundTime),
                    ParticleExplosion.rnd.Next(
                        particleExplosionSettings.minParticlesPerRound,
                        particleExplosionSettings.maxParticlesPerRound),
                    ParticleExplosion.rnd.Next(
                        particleExplosionSettings.minParticles,
                        particleExplosionSettings.maxParticles),
                         explosionColorsTexture, particleSettings,
                    explosionEffect));

                elapsedTime = TimeSpan.Zero;
            }

            // Loop through and update explosions
            for (int i = 0; i < explosions.Count; ++i)
            {
                explosions[i].Update(gameTime);
                // If explosion is finished, remove it
                if (explosions[i].IsDead)
                {
                    explosions.RemoveAt(i);
                    --i;
                }
            }
                        


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here




            

            // Use wireframe mode for debugging
            //RasterizerState rs = new RasterizerState();
            //rs.CullMode = CullMode.None;
            //rs.FillMode = FillMode.WireFrame;
            //GraphicsDevice.RasterizerState = rs;







            spriteBatch.Begin();


            


            map.Draw(GraphicsDevice, camera);
            //spriteBatch.End();


            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            // Loop through and draw each particle explosion
            foreach (ParticleExplosion pe in explosions)
            {
                pe.Draw(camera);
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
