using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Quantum_Man.Screen
{
    public delegate void ClickHandler();

    public class Button
    {
        public static Texture2D Texture { get; set; }

        public static Button CreateCenteredToPoint(string title, Color buttonColor, SpriteFont font, Point position, int margin = 5)
        {
            Vector2 measure = font.MeasureString(title);
            Rectangle rect = new Rectangle(position.X - margin - (int)(measure.X / 2), position.Y - margin - (int)(measure.Y / 2),
                                           (int)measure.X + 2 * margin, (int)measure.Y + 2 * margin);

            return new Button(title, buttonColor, rect, new Vector2(rect.X + margin, rect.Y + margin), font);
        }

        private static readonly int BORDER = 5;
        private static readonly int COLOR_DIFFERENCE = 64;

        //---------------------------------------------------------//

        // view fields
        private string title;
        private Color color;
        private Color outerColor;

        private Vector2 titlePos;

        public event ClickHandler ButtonClicked;
        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; innerBounds = new Rectangle(value.X + BORDER, value.Y + BORDER, value.Width - BORDER * 2, value.Height - BORDER * 2); }
        }
        private Rectangle bounds;
        private Rectangle innerBounds;
        private readonly SpriteFont font;

        // click fields
        private bool lastClicked = false;

        private Button(string title, Color color, Rectangle bounds, Vector2 titlePos, SpriteFont font)
        {
            this.title = title;
            this.color = color;
            this.titlePos = titlePos;
            this.font = font;
            Bounds = bounds;

            outerColor = new Color(Math.Max(0, color.R - COLOR_DIFFERENCE),
                                    Math.Max(0, color.G - COLOR_DIFFERENCE),
                                    Math.Max(0, color.B - COLOR_DIFFERENCE),
                                    color.A);
        }

        public void Update(GameTime time)
        {

            MouseState ms = Mouse.GetState();
            bool leftPressed = ms.LeftButton == ButtonState.Pressed;
            Point mouse = new Point(ms.X, ms.Y);

            if (Bounds.Contains(mouse))
            {
                if (lastClicked && !leftPressed)
                {
                    ButtonClicked();
                }
                lastClicked = ms.LeftButton == ButtonState.Pressed;
            }
            else
            {
                lastClicked = false;
            }
        }

        public void Draw(GameTime time, SpriteBatch draw)
        {
            draw.Draw(Texture, Bounds, lastClicked ? color : outerColor);
            draw.Draw(Texture, innerBounds, lastClicked ? outerColor : color);
            draw.DrawString(font, title, titlePos, Color.Black);
        }
    }
}
