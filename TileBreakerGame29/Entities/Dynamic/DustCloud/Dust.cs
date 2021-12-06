using BreakOut;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBreakerGame29.Entities.Dynamic.DustCloud
{
    class Dust
    {
        private Rectangle textureRect = new Rectangle(80 * 4, 80 * 6, 80, 80);
        private int nextIndex=0;        
        const int MAX_DUST=16;
        private DustParticle[] dustArray;
        /// <summary> 
        /// a collection of DustParticle.
        /// </summary>        
        public Dust()
        {            
            dustArray = new DustParticle[MAX_DUST];
            for (int i = 0; i < dustArray.Length; i++)
            {
                dustArray[i] = new DustParticle();
            }
        }

        public void Dispose()
        { 
        
        }
        
        /// <param name="position">Center of the dustcloud.</param>
        /// <param name="velocity">The initial offset velocity, is also affected by area.</param>
        /// <param name="color">Color of the dustcloud.</param>
        /// <param name="area">affects the growthspeed and velocity of the dustcloud.</param>
        public void Add(Vector2 position, Vector2 velocity, Color color, int area)
        {
            for(int i=0;i<dustArray.Length ;i++)
            {
                if (dustArray[(i + nextIndex) % MAX_DUST].lifeTime <= 0)
                {
                    int n = (i+nextIndex)%MAX_DUST;
                    dustArray[n].lifeTime = Globals.dustLifeTime;
                    dustArray[n].rect.X = (int)position.X-5;
                    dustArray[n].rect.Y = (int)position.Y-5;
                    dustArray[n].rect.Width = 10;
                    dustArray[n].rect.Height = 10;
                    dustArray[n].color = color;
                    dustArray[n].dustSpeed = area * 4 * 0.001f;
                    dustArray[n].velocity= velocity * area * 0.0003f;

                    nextIndex++;
                    return;                    
                }
            }
        }

        public void NewGame()
        {
            nextIndex = 0;
            for (int i = 0;i<dustArray.Length ; i++)
            {
                dustArray[i].lifeTime = 0;
            }
            
        }

        public void Update()
        {
            for (int i = 0; i < dustArray.Length; i++)
            {
                if (dustArray[i].lifeTime >=0)
                {
                    dustArray[i].Update();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            for (int i = 0; i < dustArray.Length; i++)
            {
                if (dustArray[i].lifeTime >=0)
                {
                    spriteBatch.Draw(texture, dustArray[i].rect, textureRect, dustArray[i].color);                    
                }
            }
        }
    }
}