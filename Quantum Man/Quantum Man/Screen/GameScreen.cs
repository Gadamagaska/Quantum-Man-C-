using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Man.Screen
{
    public class GameScreen
    {
        public GameScreen(ScreenManager manager)
        {
            ScreenManager = manager;
            Buttons = new Collection<Button>();
        }

        public ScreenManager ScreenManager { get; private set; }
        private Collection<Button> Buttons { get; set; }

        public virtual void Update(GameTime time)
        {
            foreach (Button b in Buttons)
            {
                b.Update(time);
            }
        }

        public virtual void Draw(GameTime time, SpriteBatch draw)
        {
            foreach (Button b in Buttons)
            {
                b.Draw(time, draw);
            }
        }

        public virtual void AddButton(Button button)
        {
            Buttons.Add(button);
        }

        protected void DrawCentered(SpriteBatch draw, string text, SpriteFont font, Point position, Color color)
        {
            Vector2 measures = font.MeasureString(text);
            draw.DrawString(font, text, new Vector2(position.X - measures.X / 2, position.Y - measures.Y / 2), color);
        }
    }
}
