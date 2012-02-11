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
        private KeyboardState lastState;
        private KeyboardState curState;

        private Board board;
        private Creature player;

        public TileTestScreen(ScreenManager manager) : base(manager)
        {
            board = new Board("level1",manager.Game.Content);
            //board = new Board(new TileSet(manager.Game.Content.Load<Texture2D>("tileset1")),100,100 );

            player = new Creature(new TileSet(manager.Game.Content.Load<Texture2D>("playersprite")));
            board.Creatures[5, 5] = player;
        }

        public override void Draw(GameTime time, SpriteBatch draw)
        {
            Point p = board.GetPosition(player);
            int x = Math.Max(Math.Min(p.X - 5,board.Width - 11),0);
            int y = Math.Max(Math.Min(p.Y - 5, board.Height - 11), 0);

            board.Draw(x,10,y,10,new Point(10,10),time,draw );

            base.Draw(time, draw);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            curState = Keyboard.GetState();
            if (KeyPressed(Keys.Up)) board.MoveCreature(player,Direction.Up);
            if (KeyPressed(Keys.Down)) board.MoveCreature(player, Direction.Down);
            if (KeyPressed(Keys.Right)) board.MoveCreature(player, Direction.Right);
            if (KeyPressed(Keys.Left)) board.MoveCreature(player, Direction.Left);

            if (KeyPressed(Keys.F2)) ((Game1)this.ScreenManager.Game).graphics.ToggleFullScreen();

            lastState = curState;
        }

        public bool KeyPressed(Keys key)
        {
            return curState.IsKeyDown(key) && !lastState.IsKeyDown(key);
        }
    }
}
