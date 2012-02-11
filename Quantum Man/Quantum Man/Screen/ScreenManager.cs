using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Man.Screen
{
    public class ScreenManager : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D greyed;

        public SpriteFont Font { get; private set; }

        private List<GameScreen> screens;

        public ScreenManager(Game game, SpriteBatch draw)
            : base(game)
        {
            screens = new List<GameScreen>();
            spriteBatch = draw;

            this.Initialize();
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Font = Game.Content.Load<SpriteFont>("standard");
            greyed = Game.Content.Load<Texture2D>("greyed");
            Button.Texture = Game.Content.Load<Texture2D>("blank");
        }

        public override void Draw(GameTime time)
        {
            base.Draw(time);

            spriteBatch.Begin();
            for (int i = 0; i < screens.Count; i++)
            {
                if (i == screens.Count - 1 && screens.Count > 1 && !screens[i].NoShadow)
                {
                    spriteBatch.Draw(greyed, GraphicsDevice.Viewport.Bounds, Color.Black);
                }
                screens[i].Draw(time, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (screens.Count > 0)
            {
                for (int i = screens.Count - 1; i >= 0; i-- )
                {
                    screens[i].Update(time);
                    if (screens[i].BlocksUpdate) break;
                }
            }
        }

        public GameScreen GetTopScreen()
        {
            return screens.Count > 0 ? screens[screens.Count - 1] : null;
        }

        public void Add(GameScreen screen)
        {
            screens.Add(screen);
        }

        public void RemoveTopScreen()
        {
            if (screens.Count > 0)
            {
                screens.RemoveAt(screens.Count - 1);
            }
        }

        public void Clear()
        {
            screens.Clear();
        }
    }
}
