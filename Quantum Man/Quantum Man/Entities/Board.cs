using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Man.Utilities;

namespace Quantum_Man.Entities
{

    public class Board
    {
        public TileSet TileSet { get; private set; }
        public int TileSize { get; set; }

        public int[,] Layer0 { get; private set; }
        public int[,] Layer1 { get; private set; }
        public int[,] Layer2 { get; private set; }

        public Obstacle[,] Obstacles { get; private set; }
        public Creature[,] Creatures { get; private set; }

        public int Width { get { return Layer0.GetLength(0); } }
        public int Height { get { return Layer0.GetLength(1); } }

        public Board(TileSet tileSet, int width, int height, int tileSize = 50)
        {
            TileSet = tileSet;
            this.TileSize = tileSize;
            Initialize(width,height);

            // random images
            Random rand = new Random();
            for(int y = 0; y < Width; y++)
            {
                for(int x = 0; x < Height; x++)
                {
                    Layer0[x, y] = 1;
                    Layer1[x, y] = rand.Next(0, TileSet.Count - 1);
                }
            }
        }

        public Board(string file, ContentManager content, int tileSize = 50)
        {
            this.TileSize = tileSize;

            StreamReader reader = File.OpenText(file + ".txt");

            int layerNumber = 0;
            String line;
            String[] row = new string[0];
            List<int[]> layer = new List<int[]>();
            while((line = reader.ReadLine()) != null)
            {
                if(line.StartsWith("TILESET"))
                {
                    TileSet = new TileSet(content.Load<Texture2D>(line.Split('=')[1]));

                }else if(line.StartsWith("LAYER"))
                {
                    // save last layer
                    if(layer.Count > 0)
                    {
                        if (Layer0 == null) Initialize(row.Length, layer.Count);
                        switch (layerNumber)
                        {
                            case 0: { AddAll(layer, Layer0); break; }
                            case 1: { AddAll(layer, Layer1); break; }
                            case 2: { AddAll(layer, Layer2); break; }
                        }
                        layer.Clear();
                    }

                    // new
                    layerNumber = int.Parse(line.Split('=')[1]);

                }else if((row = line.Split(',')).Length > 0)
                {
                    layer.Add(row.Select(n => int.Parse(n)).ToArray());
                }
            }
        }

        public void Draw(int x, int y, Point destination, GameTime time, SpriteBatch draw)
        {
            Rectangle dest = new Rectangle(destination.X, destination.Y, TileSize, TileSize);

            TileSet.Draw(Layer0[x, y], dest, draw);
            TileSet.Draw(Layer1[x, y], dest, draw);

            if(Obstacles[x,y] != null) Obstacles[x,y].Draw(dest, time,draw);
            if(Creatures[x,y] != null) Creatures[x,y].Draw(dest,time,draw);

            TileSet.Draw(Layer2[x, y], dest, draw);
        }

        public void Draw(int xStart, int width, int yStart, int height, Point origin, GameTime time, SpriteBatch draw)
        {
            for(int y = yStart; y <= yStart + height; y++)
            {
                for(int x = xStart; x <= xStart + width; x++)
                {
                    this.Draw(x,y, new Point(origin.X + (x-xStart)*TileSize, origin.Y + (y-yStart)*TileSize),time,draw);
                }
            }
        }

        public Point GetPosition(Entity entity)
        {
            if(entity is Obstacle)
            {
                return Query(entity, Obstacles);
            }else if(entity is Creature)
            {
                return Query(entity, Creatures);
            }

            // entity not found
            return new Point(-1,-1);
        }

        public void MoveCreature(Creature c, Direction dir)
        {
            Point p = GetPosition(c);
            switch(dir)
            {
                case Direction.Up:
                    {
                        if(WithinBounds(p.X,p.Y-1))
                        {
                            Creatures[p.X, p.Y] = null;
                            Creatures[p.X, p.Y - 1] = c;
                        }
                        break;
                    }
                case Direction.Down:
                    {
                        if(WithinBounds(p.X, p.Y + 1))
                        {
                            Creatures[p.X, p.Y] = null;
                            Creatures[p.X, p.Y + 1] = c; 
                        }
                        break;
                    }
                case Direction.Left:
                    {
                        if(WithinBounds(p.X - 1, p.Y))
                        {
                            Creatures[p.X, p.Y] = null;
                            Creatures[p.X - 1, p.Y] = c;
                        }
                        break;
                    }
                case Direction.Right:
                    {
                        if(WithinBounds(p.X + 1, p.Y))
                        {
                            Creatures[p.X, p.Y] = null;
                            Creatures[p.X + 1, p.Y] = c;
                        }
                        break;
                    }
            }
            c.Face(dir);
        }

        #region private methods

        private void Initialize(int width, int height)
        {
            Obstacles = new Obstacle[width, height];
            Creatures = new Creature[width, height];
            Layer0 = new int[width, height];
            Layer1 = new int[width, height];
            Layer2 = new int[width, height];
        }

        private bool WithinBounds(int X, int Y)
        {
            if (X >= 0 && X < Width && Y >= 0 && Y < Height) return true;
            return false;
        }

        private Point Query<T>(Entity entity, T[,] array) where T : Entity
        {
            for(int x = 0; x < array.GetLength(0); x++)
            {
                for(int y = 0; y < array.GetLength(1); y++)
                {
                    if(array[x,y] != null && array[x,y].Equals(entity)) return new Point(x,y);
                }
            }

            // entity not found
            return new Point(-1, -1);
        }

        private void AddAll(List<int[]> input, int[,] output)
        {
            for (int y = 0; y < input.Count; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    output[x, y] = input[y][x];
                }
            }
        }

        #endregion
    }
}
