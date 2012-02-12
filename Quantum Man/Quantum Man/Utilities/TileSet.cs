using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Man.Utilities
{
    public class TileSet
    {
        public Texture2D Texture { get; private set; }

        public int TileSize { get; private set; }
        public int WidthCount { get; private set; }
        public int HeightCount { get; private set; }
        public int Count { get { return WidthCount*HeightCount; } }

        private bool[,] Walk { get; set; }

        public TileSet(string fileName, ContentManager manager, int tileSize = 50)
        {
            Texture = manager.Load<Texture2D>(fileName);
            this.TileSize = tileSize;
            this.WidthCount = Texture.Width/tileSize;
            this.HeightCount = Texture.Height/tileSize;

            TryLoadWalkable(fileName);
        }

        private void TryLoadWalkable(string name)
        {
            try
            {
                StreamReader reader = File.OpenText(name + ".txt");

                List<bool[]> layer = new List<bool[]>();
                String line;
                String[] splitted;
                while ((line = reader.ReadLine()) != null)
                {
                    splitted = line.Split(',');
                    layer.Add(splitted.Select(n => n == "1").ToArray());
                }

                // save
                Walk = new bool[this.WidthCount, this.HeightCount];
                for (int y = 0; y < Walk.GetLength(1); y++)
                {
                    for (int x = 0; x < Walk.GetLength(0); x++)
                    {
                        Walk[x, y] = layer[y][x];
                    }
                }
            }catch(FileNotFoundException)
            {
                Console.WriteLine("No walkability file found for TileSet = "+name);
            }
        }

        public Rectangle this[int x, int y]
        {
            get { return new Rectangle(x*TileSize, y*TileSize, TileSize,TileSize); }
        }

        public Rectangle this[int index]
        {
            get { return this[index % WidthCount, index / WidthCount]; }
        }

        public bool Walkable(int index)
        {
            try
            {
                return Walk[index % WidthCount, index / WidthCount];
            }catch(IndexOutOfRangeException)
            {
                return false;
            }
        }

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
