using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Man.Entities
{
    public abstract class Entity
    {
        public abstract void Draw(Rectangle destTile, GameTime time, SpriteBatch draw);
        public abstract void Update(GameTime time);
    }
}
