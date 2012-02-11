using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Man.Utilities
{
    public class TileSet
    {
        public int TileSize { get; private set; }
        public int WidthCount { get; private set; }
        public int HeightCount { get; private set; }
        public int Count { get { return WidthCount*HeightCount; } }

        public TileSet(Texture2D image, int tileSize = 50)
        {
            Texture = image;
            this.TileSize = tileSize;
            this.WidthCount = image.Width/tileSize;
            this.HeightCount = image.Height/tileSize;
        }

        public Rectangle this[int x, int y]
        {
            get { return new Rectangle(x*TileSize, y*TileSize, TileSize,TileSize); }
        }

        public Rectangle this[int index]
        {
            get { return this[index % WidthCount, index / WidthCount]; }
        }

        public Texture2D Texture { get; private set; }

        public void Draw(int index, Rectangle destination, SpriteBatch draw)
        {
            draw.Draw(this.Texture, destination, this[index], Color.White);
        }

        public void Draw(int index, Point destination, SpriteBatch draw)
        {
            this.Draw(index,new Rectangle(destination.X,destination.Y,this.TileSize,this.TileSize),draw );
        }
    }
}
