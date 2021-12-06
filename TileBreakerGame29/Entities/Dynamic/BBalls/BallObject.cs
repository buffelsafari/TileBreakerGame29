using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Media;
using Microsoft.Xna.Framework;

namespace TileBreakerGame29.Entities.Dynamic.BBalls
{
    class BallObject
    { 
        public Vector2 position;
        public Vector2 velocity;
        public Color color;
        public Rectangle collisionRect;
        public int radie;
        public int dmg;
        public int lifeTime = 0;
        public bool Active { get; set; }
        public BallObject(int radie)
        {           
            dmg = 1;
            Active = false;
            position = new Vector2();
            velocity = new Vector2();
            this.radie = radie;
            collisionRect = new Rectangle(0, 0, radie*2, radie*2);            
        }
    }
}