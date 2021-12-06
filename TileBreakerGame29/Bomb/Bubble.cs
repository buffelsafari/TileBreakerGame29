using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBreakerGame29.Bomb
{
    class Bubble
    {
        private static Rectangle textureRect = new Rectangle(80 * 5, 80 * 7, 80, 80);
        private static Vector2 origo = new Vector2(40, 40);
        
        private Color color= new Color(0.5f, 0.5f, 0.5f, 0.1f);

        private float angle = 0;
        public float radie { get; private set; }
        private float maxRadie;
        
        
        public Vector2 position { get; private set;}

        private Rectangle rectangle;
        public bool active { get; set; }

        /// <summary>
        /// updates and draw an explosion bubble        
        /// </summary>        
        public Bubble(Vector2 position, float maxRadie)
        {
            this.position = position;
            this.maxRadie = maxRadie;
            active = true;            
            radie = 10;
        }

        public Color GetColor()
        {
            return color;
        }

        public void Update()
        {
            angle+=0.1f;
            radie += 30.0f;            

            if (radie > maxRadie*0.75f)
            {
                color *= 0.75f;
            }

            if (radie > maxRadie)
            {
                active = false;
            }

            
            rectangle.X = (int)position.X;
            rectangle.Y=(int)position.Y;
            rectangle.Width = (int)(radie * 2);
            rectangle.Height = (int)(radie * 2);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        { 
            spriteBatch.Draw(texture, rectangle, textureRect, color, angle, origo, SpriteEffects.None,0);
            spriteBatch.Draw(texture, rectangle, textureRect, color, -angle, origo, SpriteEffects.None, 0);
        }
    }
}