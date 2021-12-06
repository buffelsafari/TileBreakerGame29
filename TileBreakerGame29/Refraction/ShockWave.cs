using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBreakerGame29.Refraction
{
    class ShockWave
    {
        private readonly static Rectangle textureRect = new Rectangle(524, 275, 80, 80);
        private const float aspect = 1080 / 1920.0f;
        private Vector2 position;
        private float radie = 0;        
        public bool Active { get; set; }
        private int size;
        
        public ShockWave(Vector2 position, int size)
        {
            Active = true;
            this.size = size;
            this.position = new Vector2((position.X/1080.0f)*512.0f, (position.Y/1920.0f)*512.0f);             
        }

        public void Update()
        {
            radie += 15;
            if (radie > size)
            {
                Active = false;
                radie = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {            
            spriteBatch.Draw(texture, new Rectangle((int)(position.X-radie),(int)(position.Y-radie * aspect),(int)(radie*2),(int)((radie*2)*aspect)), textureRect, Color.White);
        }
    }
}