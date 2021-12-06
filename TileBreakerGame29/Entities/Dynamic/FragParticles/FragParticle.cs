using Microsoft.Xna.Framework;

namespace TileBreakerGame29.Entities.Dynamic.FragParticles
{
    class FragParticle 
    {
        public Vector2 position;
        public Vector2 velocity;
        public int lifeTime;
        public Color color;
        public Rectangle collisionRect;
        public bool Active { get; set; }

        public FragParticle()
        {
            lifeTime = 0;
            Active = false;            
            collisionRect = new Rectangle(0, 0, 16, 16);
            position = new Vector2();
            velocity = new Vector2();
            color = new Color();
        }        

    }
}