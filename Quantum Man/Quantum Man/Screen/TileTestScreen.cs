using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Quantum_Man.Entities;
using Quantum_Man.Utilities;

namespace Quantum_Man.Screen
{
    public class TileTestScreen : GameScreen
    {
        private Board board;
        private Creature player;

        public TileTestScreen(ScreenManager manager) : base(manager)
        {
            //board = new Board("level1",manager.Game.Content);
            TileSet tileSet = new TileSet(manager.Game.Content.Load<Texture2D>("tileset1"));
            board = new Board(tileSet,100,100);
            //board.Obstacles[10,5] = new Obstacle(10,5,tileSet,9);
            player = new Creature(new TileSet(manager.Game.Content.Load<Texture2D>("playersprite")));
        }

        public override void Draw(GameTime time, SpriteBatch draw)
        {
            // print stuff
            for (int y = 0; y < board.Height; y++ )
            {
                for(int x = 0; x < board.Width; x++)
                {
                    board.Draw(x,y,new Point(x * board.TileSize, y * board.TileSize), time, draw);
                }
            }

            player.Draw(new Rectangle(0,0,board.TileSize,board.TileSize), time, draw);

            base.Draw(time, draw);
        }

        private bool upDown = false;
        private bool downDown = false;
        private bool f2Down = false;

        public override void Update(GameTime time)
        {
            base.Update(time);

            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Up) && !upDown){board.TileSize += 10;}
            if (ks.IsKeyDown(Keys.Down) && !downDown) board.TileSize -= 10;

            if (ks.IsKeyDown(Keys.F2) && !f2Down) ((Game1)this.ScreenManager.Game).graphics.ToggleFullScreen();

            upDown = ks.IsKeyDown(Keys.Up);
            downDown = ks.IsKeyDown(Keys.Down);
            f2Down = ks.IsKeyDown(Keys.F2);

            player.Update(time);
        }
    }
}
