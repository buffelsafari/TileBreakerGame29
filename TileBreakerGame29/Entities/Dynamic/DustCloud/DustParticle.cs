using Microsoft.Xna.Framework;

namespace TileBreakerGame29.Entities.Dynamic.DustCloud
{
    class DustParticle
    {
        public Vector2 velocity;
        public Color color;
        public float dustSpeed;
        public Rectangle rect;
        public int lifeTime;

        /// <summary> 
        /// a simple dust particle.
        /// </summary>        
        public DustParticle()
        {
            rect = new Rectangle(0, 0, 0, 0);
            velocity = new Vector2();
            color = Color.White;
            lifeTime = 0;            
        }
        public void Update()
        {
            lifeTime--;
            rect.Inflate(dustSpeed, dustSpeed);
            rect.Offset(velocity);
            dustSpeed *= 0.95f;
            velocity *= 0.98f;
            color *= 0.98f;
        }
    }
}