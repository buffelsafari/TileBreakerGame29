using System;
using BreakOut;
using Microsoft.Xna.Framework;

namespace TileBreakerGame29.Menu
{
    class Wisp
    {
        public Vector2 position;
        public Color color;
        Vector2 velocity;
        public float height;
        float baseLine;
        double angle;
        float amplitude;
        float frequency;
        public Wisp(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
            velocity = new Vector2(Globals.rnd.Next(10,100)/10.0f, Globals.rnd.Next(10, 100) / 10.0f);
            baseLine = Globals.rnd.Next(10, 100);
            angle = 0;
            amplitude = Globals.rnd.Next(10,50);
            frequency = Globals.rnd.Next(0, 100)/100.0f;

        }

        public void Update()
        {
            position += velocity;

            if (position.X < -100 || position.X > 1180)
            {
                velocity.X = -velocity.X;
            }

            if (position.Y < -100 || position.Y > 2020)
            {
                velocity.Y = -velocity.Y;
            }
            angle += 0.1f;
            height = baseLine + amplitude*(float)(Math.Sin(angle*frequency));
            
        }


    }
}