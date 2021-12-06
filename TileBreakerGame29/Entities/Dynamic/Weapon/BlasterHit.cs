using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBreakerGame29.Entities.Dynamic.Weapon
{
    class BlasterHit
    {
        private const int duration = 7; 
        public bool Active { get; set; }
        private Rectangle rect;
        public int aliveTime { get; set; }
        private static Rectangle[] textureRect =  { new Rectangle(80*0, 80*7, 80, 80),
                                                            new Rectangle(80*1, 80*7, 80, 80),
                                                            new Rectangle(80*2, 80*7, 80, 80),
                                                            new Rectangle(80*3, 80*7, 80, 80),
                                                            new Rectangle(80*4, 80*7, 80, 80),
                                                            new Rectangle(80*5, 80*7, 80, 80),
                                                            };
        /// <summary>
        /// animates the hit from the blaster weapon
        /// </summary>
        public BlasterHit()
        {
            Active = false;
            this.rect = new Rectangle(0,0,80,80);                   
            this.aliveTime = 0;
        }

        public void SetData(Vector2 position)
        {            
            this.Active = true;
            this.aliveTime = 0;            
            rect.X = (int)(position.X - 40);
            rect.Y = (int)(position.Y - 40);            
        }        

        public void Update()
        {
            aliveTime++;
            if (aliveTime > duration)
            {
                Active= false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, rect,textureRect[(aliveTime-1)%textureRect.Length], Color.White);
        }
    }
}