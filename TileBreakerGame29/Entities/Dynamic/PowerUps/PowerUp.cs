using System;
using System.Collections.Generic;
using BreakOut;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileBreakerGame29.Light;
using Color = Microsoft.Xna.Framework.Color;

namespace TileBreakerGame29.Entities.Dynamic.PowerUps
{
    class PowerUp : ILightRequester
    {
        public enum PowerUpType
        { 
            Expander,
            Flash,
            Heart,
            Star,
        }
        public static List<PowerUp> powerUpList { get; set; } = new List<PowerUp>();

        protected Vector2 position;
        protected Rectangle collisionRect;
        protected Rectangle textureRect;
        protected Color color;
        protected Color lightColor;
        protected PowerUpType type;

        protected float anim = 0;

        private static LightData lightData=new LightData();

        public bool Active { get; set; } = true;

        public delegate void BatCollisionDelegate(PowerUp powerUp);
        public delegate void DestroyDelegate(PowerUp powerUp);
        public delegate void AnimateDelegate();

        public BatCollisionDelegate batCollision { get; set; }
        public DestroyDelegate destroy { get; set; }
        public AnimateDelegate animate { get; set; }

        protected Tuple<int, LightData>[] lights = new Tuple<int, LightData>[1];


        public PowerUp(PowerUpType type, Vector2 position, BatCollisionDelegate batCollision, DestroyDelegate destroy)
        {
            this.type = type;
            this.position = position;
            this.batCollision = batCollision;
            this.destroy = destroy;
            

            switch (type)
            {
                case PowerUpType.Expander:
                    this.animate = AnimateExpander;
                    textureRect = new Rectangle(80 * 2, 80 * 6, 80, 80);
                    color = Color.White;
                    lightColor = new Color(255, 100, 100, 255);
                    break;

                case PowerUpType.Flash:
                    this.animate = AnimateFlash;
                    textureRect = new Rectangle(80 * 3, 80 * 6, 80, 80);
                    color = Color.White;
                    lightColor = new Color(155, 155, 255, 255);
                    break;

                case PowerUpType.Heart:
                    this.animate = AnimateHeart;
                    textureRect = new Rectangle(80*0,80*6,80,80);
                    color = Color.White;
                    lightColor = new Color(255, 100, 100, 255);
                    break;

                case PowerUpType.Star:
                    this.animate = AnimateStar;
                    textureRect = new Rectangle(80 * 1, 80 * 6, 80, 80);
                    color = Color.White;
                    lightColor = new Color(255, 255, 100, 255);
                    break;
            }
        }

        public static void NewGame()
        {
            foreach (PowerUp p in powerUpList)
            {
                p.position.Y=2000;
            }
        }

        public virtual void Update(Bat bat)
        {
            if (Active && bat.collisionRect.Intersects(collisionRect))
            {
                batCollision.Invoke(this);
                destroy.Invoke(this);
            }

            if (position.Y > 1920)
            {
                destroy.Invoke(this);
            }

            animate.Invoke();
        }
        

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, collisionRect, textureRect, color);
        }

        public Vector2 GetPosition()
        {
            return collisionRect.Center.ToVector2();
        }

        public PowerUpType GetPowerUpType()
        {
            return type;
        }


        private void AnimateExpander()
        {
            position.Y += 10;
            anim += 0.1f;
            int scale = (int)(Math.Sin(anim) * 10.0f);
            collisionRect = new Rectangle((int)position.X - scale - 40, (int)position.Y - 40, 80 + scale * 2, 80);
        }

        private void AnimateFlash()
        {
            position.Y += 10;
            anim += 1.0f;
            float scale = (float)((Math.Sin(anim) + 1.0) / 2.0);
            color = new Color(scale, scale, 1 + scale, 1.0f);
            lightColor = new Color(scale, scale, 1 + scale, scale);
            collisionRect = new Rectangle((int)position.X - 40, (int)position.Y - 40, 80, 80);
        }

        private void AnimateHeart()
        {
            position.Y += 10;
            anim += 0.4f;
            int scale = (int)(Math.Sin(anim) * 10.0f);
            collisionRect = new Rectangle((int)position.X - scale - 40, (int)position.Y - scale - 40, 80 + scale * 2, 80 + scale * 2);

        }

        private void AnimateStar()
        {
            position.Y += 10;
            anim += 0.3f;
            float scale = (float)((Math.Sin(anim) + 1.0) / 2.0);
            color = new Color(scale, scale, scale, 1.0f);
            lightColor = new Color(scale, scale, scale, scale);
            collisionRect = new Rectangle((int)position.X - 40, (int)position.Y - 40, 80, 80);
        }

        public void GetLightData(SortedList<int, LightData> list)
        {            
            lightData.position = position;
            lightData.color = lightColor;
            list.Add(Globals.powerUpLightPriority, lightData);
        }       
    }
}