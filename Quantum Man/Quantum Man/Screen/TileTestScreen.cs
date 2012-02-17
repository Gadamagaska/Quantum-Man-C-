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
        private static int GhostChaseDistX = 3;
        private static int GhostChaseDistY = 3;

        private KeyboardState lastState;
        private KeyboardState curState;

        private Board board;
        private Creature player;
        private Point ghost;
        private Point mapArea;

        public TileTestScreen(ScreenManager manager) : base(manager)
        {
            board = new Board("level1",manager.Game.Content);
            //board = new Board(new TileSet(manager.Game.Content.Load<Texture2D>("tileset1")),100,100 );

            player = new Creature(new TileSet("playersprite",manager.Game.Content));
            board.Creatures[6, 6] = player;
            ghost = new Point(6,6);
            mapArea = new Point(14,10);
            GhostChaseDistX = (mapArea.X - 6)/2;
            GhostChaseDistY = (mapArea.Y - 6) / 2;

            Creature.LoadCreature("Sand Monster",manager.Game.Content);
        }

        public override void Draw(GameTime time, SpriteBatch draw)
        {
            int left = GhostSpaceTowards(Direction.Left);
            int up = GhostSpaceTowards(Direction.Up);
            board.Draw(ghost.X - left,mapArea.X,ghost.Y - up,mapArea.Y,new Point(10,10),time,draw );

            base.Draw(time, draw);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            curState = Keyboard.GetState();
            if (KeyPressed(Keys.Up) && board.MoveCreature(player, Direction.Up)) UpdateGhost();
            if (KeyPressed(Keys.Down) && board.MoveCreature(player, Direction.Down)) UpdateGhost();
            if (KeyPressed(Keys.Right) && board.MoveCreature(player, Direction.Right)) UpdateGhost();
            if (KeyPressed(Keys.Left) && board.MoveCreature(player, Direction.Left)) UpdateGhost();

            if (KeyPressed(Keys.F2)) ((Game1)this.ScreenManager.Game).graphics.ToggleFullScreen();

            lastState = curState;
        }

        private void UpdateGhost()
        {
            Point distance = Distance(ghost, board.GetPosition(player));
            if(Math.Abs(distance.X) > GhostChaseDistX)
            {
                if(distance.X > 0) // player running left
                {
                    if(ghost.X - GhostSpaceTowards(Direction.Left) > 0)
                    {
                        ghost.X -= 1;
                    }
                }
                else if ((ghost.X + GhostSpaceTowards(Direction.Right)) < board.Width) // player running right
                {
                    ghost.X += 1;
                }
            }
            if(Math.Abs(distance.Y) > GhostChaseDistY)
            {
                if(distance.Y > 0) // player running up
                {
                    if (ghost.Y - GhostSpaceTowards(Direction.Up) > 0)
                    {
                        ghost.Y -= 1;
                    }
                }
                else if ((ghost.Y + GhostSpaceTowards(Direction.Down)) < board.Height) // player running down
                {
                    ghost.Y += 1;
                }
            }
        }

        private int GhostSpaceTowards(Direction dir)
        {
            switch(dir)
            {
                case Direction.Left: return (mapArea.X - 1) / 2;
                case Direction.Right:
                    return mapArea.X - GhostSpaceTowards(Direction.Left);
                case Direction.Up: return (mapArea.Y - 1) / 2;
                case Direction.Down:
                    return mapArea.Y - GhostSpaceTowards(Direction.Up);
            }
            return 1337;
        }

        private Point Distance(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        private bool GhostAtBorder()
        {
            return ghost.X == 0 || ghost.Y == 0 || (ghost.X + mapArea.X) == board.Width ||
                   (ghost.Y + mapArea.Y) == board.Height;
        }

        public bool KeyPressed(Keys key)
        {
            return curState.IsKeyDown(key) && !lastState.IsKeyDown(key);
        }
    }
}
