using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Man.Combat;
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
        // General Stuff
        private Dictionary<Direction, Animation> animations;
        private Direction facing;

        // Combat                       // Examples:
        public int Initiative { get; set; } // 10 is high, 1 is low, highest initiative acts first each round
        public int Movement { get; set; } // 3 movement means this character can move up to three tiles in one round.
        public int Health { get; set; } // 10 health
        public int MaxHealth { get; set; } // 25 max health
        public int MinMeleeDmg { get; set; } // minimum 1 damage (before armor deduction)
        public int MaxMeleeDmg { get; set; } // maximum 10 damage (before armor deduction)
        public float Armor { get; set; } // 0.25f armor (25% damage reduction
        public Collection<Ability> Abilities { get; private set; } 

        public Creature(TileSet animationTile)
        {
            animations = new Dictionary<Direction, Animation>(4);
            facing = Direction.Down;

            animations.Add(Direction.Up, new Animation(animationTile, 500, 2,6,10));
            animations.Add(Direction.Down, new Animation(animationTile, 500, 0, 4, 8));
            animations.Add(Direction.Left, new Animation(animationTile, 500, 1, 5, 9));
            animations.Add(Direction.Right, new Animation(animationTile, 500, 3, 7, 11));

            Abilities = new Collection<Ability>();
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

        // Load from file
        public static Creature LoadCreature(string name, ContentManager content)
        {
            XElement creatures = XElement.Load("Data/creatures.xml");

            XElement hit = creatures.Elements("Creature").First(n => (string) n.Attribute("name") == name);
            
            Creature result = new Creature(new TileSet((string)hit.Attribute("tileset"),content));
            result.Initiative = tal(hit, "initiative");
            result.Movement = tal(hit, "movement");
            result.Health = tal(hit, "health");
            result.MaxHealth = result.Health;
            int[] melee = talArray(hit, "melee");
            result.MinMeleeDmg = melee[0];
            result.MaxMeleeDmg = melee[1];
            result.Armor = tal(hit, "armor");
            // todo load rest

            return result;
        }

        private static int tal(XElement source, string attributeName)
        {
            return int.Parse((string) source.Attribute(attributeName));
        }

        private static int[] talArray(XElement source, string attributeName)
        {
            return ((string) source.Attribute(attributeName)).Split(',').Select(int.Parse).ToArray();
        }
    }
}
