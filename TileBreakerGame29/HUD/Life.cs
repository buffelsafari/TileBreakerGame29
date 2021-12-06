using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileBreakerGame29.Entities.Dynamic;

namespace TileBreakerGame29.HUD
{
    class Life
    {
        private Rectangle textureRect = new Rectangle(524, 275, 80, 80);
        private Rectangle rect;
        private Color color;

        private Rectangle leftEyeRect;
        private Rectangle rightEyeRect;

        private Rectangle leftPupilRect;
        private Rectangle rightPupilRect;

        private Vector2 leftPupilPosition;
        private Vector2 rightPupilPosition;

        private Vector2 leftPupilTargetPosition;
        private Vector2 rightPupilTargetPosition;

        private Vector2 leftPupilVelocity;
        private Vector2 rightPupilVelocity;
        
        private Vector2 leftPupilAcc;
        private Vector2 rightPupilAcc;

        
        private int pRadie = 5;
        private int eRadie = 20;
        private float k = 0.1f;  // a spring constant
        private float d = 0.99f; // damper
        private float shakeAmount=0.5f;

        private Vector2 bodyAcc;
        private Vector2 bodyVelocity;
        private Vector2 bodyPosition;
        private Vector2 orginalPosition;

        public Life(Vector2 position, Color color)
        {
            
            this.color = color;
            orginalPosition = position;
            bodyPosition = position;

            rect = new Rectangle((int)(position.X - 40), (int)(position.Y - 40), 80, 80);


            leftEyeRect = new Rectangle(rect.Center.X-eRadie*2, rect.Center.Y-eRadie*2, eRadie*2,eRadie*2);
            rightEyeRect = new Rectangle(rect.Center.X , rect.Center.Y - eRadie*2, eRadie*2, eRadie*2);

            leftPupilPosition = leftEyeRect.Center.ToVector2();
            rightPupilPosition = rightEyeRect.Center.ToVector2();

            leftPupilRect = new Rectangle((int)(leftPupilPosition.X-pRadie),(int)(leftPupilPosition.Y-pRadie), pRadie*2, pRadie*2);
            rightPupilRect = new Rectangle(rightEyeRect.Center.X - pRadie, rightEyeRect.Center.Y - pRadie, pRadie*2, pRadie*2);

            leftPupilVelocity=new Vector2();
            leftPupilAcc=new Vector2();

            rightPupilVelocity = new Vector2();
            rightPupilAcc = new Vector2();
                        
            leftPupilTargetPosition = leftPupilPosition;            

            rightPupilTargetPosition = rightEyeRect.Center.ToVector2();            
        }

        public Vector2 GetLeftEyePosition()
        {
            return leftEyeRect.Center.ToVector2();
        }

        public Vector2 GetRightEyePosition()
        {
            return rightEyeRect.Center.ToVector2();
        }

        public void Update(Vector2 shake, Balls balls)
        {
            int closestIndex=0;
            float minDistance = 2000; 
            for(int i=0; i<balls.GetBallArray().Length;i++)
            {
                if (balls.GetBallArray()[i].Active)
                {                    
                    float d=Vector2.Distance(bodyPosition, balls.GetBallArray()[i].position);
                    if (d < minDistance)
                    {
                        closestIndex = i;
                        minDistance = d;
                    }
                }
            }

            Vector2 pos= balls.GetBallArray()[closestIndex].position;
            Vector2 dist = pos - GetLeftEyePosition();
            float l=dist.Length();
            dist.Normalize();
            leftPupilTargetPosition = GetLeftEyePosition() + dist * (l *0.01f);

            dist = pos - GetRightEyePosition();
            l = dist.Length();
            dist.Normalize();
            rightPupilTargetPosition = GetRightEyePosition() + dist * (l * 0.01f);

            bodyAcc = orginalPosition-bodyPosition-shake*shakeAmount;
            bodyVelocity += bodyAcc*0.2f;
            bodyPosition += bodyVelocity;
            bodyPosition *= 0.95f;
            rect.X = (int)(bodyPosition.X - 40);
            rect.Y = (int)(bodyPosition.Y - 40);
            
            Vector2 v = bodyPosition - orginalPosition;
            if (v.Length() > 20)
            {
                v.Normalize();
                v *= (20);
                bodyPosition = orginalPosition + v;

            }

            //-------------------------------------------------------------------
            leftPupilAcc = (leftPupilTargetPosition - leftPupilPosition) -bodyAcc*0.5f;
            leftPupilVelocity += leftPupilAcc*k;
            leftPupilPosition += leftPupilVelocity;
            leftPupilPosition *= d;            
                        
            v = leftPupilPosition-leftEyeRect.Center.ToVector2();
            if (v.Length() > (eRadie - pRadie))
            {
                v.Normalize();                
                v *= (eRadie - pRadie);
                leftPupilPosition = leftEyeRect.Center.ToVector2() + v;
                
            }
            //-----------------------------------------------------------------
            rightPupilAcc = (rightPupilTargetPosition - rightPupilPosition)-bodyAcc*0.5f;
            rightPupilVelocity += rightPupilAcc * k;
            rightPupilPosition += rightPupilVelocity;
            rightPupilPosition *= d;

            v = rightPupilPosition - rightEyeRect.Center.ToVector2();
            if (v.Length() > (eRadie - pRadie))
            {
                v.Normalize();
                v *= (eRadie - pRadie);
                rightPupilPosition = rightEyeRect.Center.ToVector2() + v;

            }
            leftPupilRect.X = (int)(leftPupilPosition.X - pRadie);
            leftPupilRect.Y = (int)(leftPupilPosition.Y - pRadie);

            rightPupilRect.X = (int)(rightPupilPosition.X - pRadie);
            rightPupilRect.Y = (int)(rightPupilPosition.Y - pRadie);

        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, rect, textureRect, color);

            spriteBatch.Draw(texture, leftEyeRect, textureRect, Color.White);
            spriteBatch.Draw(texture, rightEyeRect, textureRect, Color.White);

            spriteBatch.Draw(texture, leftPupilRect, textureRect, Color.Black);
            spriteBatch.Draw(texture, rightPupilRect, textureRect, Color.Black);
        }

    }
}