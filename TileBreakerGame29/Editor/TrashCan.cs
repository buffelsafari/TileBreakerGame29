using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBreakerGame29.Editor
{
    class TrashCan
    {
        private const float MAX_ANGLE = 1.6f;
        private const float DROP_RESET_ANGLE = 0.5f;
        private const float MIN_ANGLE = 0;

        private Rectangle rect;

        private Rectangle seatDrawRect;
        private Rectangle lidDrawRect;

        private Rectangle seatTextureRect;
        private Rectangle lidTextureRect;

        private float angle=0;
        private float targetAngle;
        private float angleVelocity=0;
        private float angleAcceleration=0;

        private Vector2 origo;

        private bool isLidDropped = true;
        
        public TrashCan(Rectangle rectangle, int yOffset)
        {
            this.rect = rectangle;
            seatDrawRect = new Rectangle(rect.Left, rect.Top + yOffset, rect.Width, rect.Height);
            lidDrawRect = new Rectangle(rect.Left + (int)(rect.Width * 0.350f), rect.Top + yOffset + (int)(rect.Height * 0.49f), (int)(rect.Width * 0.5f), (int)(rect.Height * 0.1f));
            
            seatTextureRect = new Rectangle(0,0, 160,160);
            lidTextureRect = new Rectangle(160, 10, 80, 20);

            origo = new Vector2(3, 12);
        }

        public bool OnDrop(Vector2 dropPosition)
        {
            if (rect.Contains(dropPosition))
            {
                SoundManager.Play(SoundId.flush);
                targetAngle = MIN_ANGLE;
                return true;
            }
            return false;
        }

        public bool OnOver(Vector2 overPosition)
        {
            if (rect.Contains(overPosition))
            {
                targetAngle = MAX_ANGLE;                
                return true;
            }
            targetAngle = 0;
            return false;
        }

        public void Update()
        {
            angleAcceleration = (targetAngle-angle)*0.05f;
            angleVelocity += angleAcceleration;
            angle += angleVelocity;
            
            if (angle > MAX_ANGLE)
            {                
                angleVelocity = -angleVelocity*0.2f;
                angle = MAX_ANGLE;
            }

            if (angle > DROP_RESET_ANGLE)
            {
                isLidDropped = false;
            }

            if (angle < MIN_ANGLE)
            {
                if (!isLidDropped)
                {
                    SoundManager.Play(SoundId.lid);
                    isLidDropped = true;
                }
                angleVelocity = -angleVelocity * 0.2f;
                angle = MIN_ANGLE;
            }       
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        { 
            spriteBatch.Draw(texture, seatDrawRect, seatTextureRect, Color.White);
            spriteBatch.Draw(texture, lidDrawRect, lidTextureRect, Color.White, -angle, origo, SpriteEffects.None, 0);
        }
    }
}