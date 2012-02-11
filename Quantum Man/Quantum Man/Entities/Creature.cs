using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Man.Utilities;

namespace Quantum_Man.Entities
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Creature : Entity
    {
        private Dictionary<Direction, Animation> animations;
        private Direction facing;

        public Creature(TileSet animationTile)
        {
            animations = new Dictionary<Direction, Animation>(4);
            facing = Direction.Down;

            animations.Add(Direction.Up, new Animation(animationTile, 500, 2,6,10));
            animations.Add(Direction.Down, new Animation(animationTile, 500, 0, 4, 8));
            animations.Add(Direction.Left, new Animation(animationTile, 500, 1, 5, 9));
            animations.Add(Direction.Right, new Animation(animationTile, 500, 3, 7, 11));
        }

        public void Face(Direction newDir)
        {
            facing = newDir;
        }

        public override void Draw(Rectangle destination, GameTime time, SpriteBatch draw)
        {
            animations[facing].Draw(time,destination,draw);
        }

        public override void Update(GameTime time)
        {
            
        }
    }
}
