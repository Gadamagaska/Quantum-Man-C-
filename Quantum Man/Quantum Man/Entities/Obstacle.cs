using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Man.Utilities;

namespace Quantum_Man.Entities
{
    public class Obstacle : Entity
    {
        private TileSet tileSet;
        private int index;

        public Obstacle(TileSet tileSet, int index)
        {
            this.tileSet = tileSet;
            this.index = index;
        }

        public override void Draw(Rectangle destination, GameTime time, SpriteBatch draw)
        {
            tileSet.Draw(index, destination,draw);
        }

        public override void Update(GameTime time)
        {
            
        }
    }
}
