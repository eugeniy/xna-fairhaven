using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Fairhaven
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Statistics : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected ContentManager m_content;
        protected SpriteBatch m_spriteBatch;
        protected SpriteFont m_font;

        protected int m_frameRate = 0;
        protected int m_frameCounter = 0;
        protected TimeSpan m_elapsedTime = TimeSpan.Zero;


        public Statistics(Game game) : base(game)
        {
            m_content = new ContentManager(game.Services);

            // TODO: Ehh, need to get this from the config or the game class.
            m_content.RootDirectory = "Content";
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
        /// LoadContent will be called once component is started.
        /// </summary>
        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            m_font = m_content.Load<SpriteFont>(@"Fonts\Statistics");

            base.LoadContent();
        }


        /// <summary>
        /// UnloadContent will be called when the component is disabled.
        /// </summary>
        protected override void UnloadContent()
        {
            m_content.Unload();

            base.UnloadContent();
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            m_elapsedTime += gameTime.ElapsedGameTime;

            if (m_elapsedTime > TimeSpan.FromSeconds(1))
            {
                m_elapsedTime -= TimeSpan.FromSeconds(1);
                m_frameRate = m_frameCounter;
                m_frameCounter = 0;
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game component should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            m_frameCounter++;
            string fps = string.Format("fps: {0}", m_frameRate);

            m_spriteBatch.Begin();
            m_spriteBatch.DrawString(m_font, fps, new Vector2(10, 10), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            m_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
