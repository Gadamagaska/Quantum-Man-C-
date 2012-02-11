using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Man.Utilities
{
    public class Animation
    {

        private TileSet tileSet;
        private int duration;
        private int[] images;

        public Animation(TileSet tileSet, int duration, params int[] images)
        {
            this.tileSet = tileSet;
            this.duration = duration;
            this.images = images;
        }

        public void Draw(GameTime time, Rectangle destination, SpriteBatch draw)
        {
            tileSet.Draw(images[((long)time.TotalGameTime.TotalMilliseconds % duration * images.Length) / duration],destination,draw);
        }

        public void Draw(GameTime time, Point destination, SpriteBatch draw)
        {
            this.Draw(time, new Rectangle(destination.X,destination.Y,tileSet.TileSize, tileSet.TileSize),draw );
        }

        public TileSet TileSet { get { return tileSet; } }
    }
}
