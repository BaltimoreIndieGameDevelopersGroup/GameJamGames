using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zombie.Components.Entities
{
    /// <summary>
    /// Defines the basic structures needed for all "gameplay objects" in the game
    /// </summary>
    public class Actor
    {
        public Guid ID { get; set; }
        public int HP { get; set; }

        // Speed will be a value from 0 (static) to 100?
        public int Speed { get; set; }
        Rectangle CollisionBox { get; set; }

        public Actor()
        {
            ID = Guid.NewGuid();
            HP = 100;
            Speed = 10; //default
            //collisionbox will be set on the fly according to IRenderable texture
        }
    }
            
}
