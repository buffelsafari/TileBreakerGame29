using System.Collections.Generic;
using BreakOut;

using BreakOut.Entities.Static;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileBreakerGame29.Entities.Dynamic.BBalls;


namespace TileBreakerGame29.Entities.Dynamic.FragParticles
{

    class Frags
    {
        int nextPosition = 0;
        int nextBlasterPosition = 0;
        int nextSparkPosition = 0;

        const int fragArraySize = 1024;
        const int blasterArraySize = 256;
        const int sparkArraySize = 256;
        const int radie = 8;
        
        private FragParticle[] fragArray;
        private FragParticle[] blasterArray;
        private FragParticle[] sparkArray;

        Rectangle fragTextureRect= new Rectangle(524, 275, 80, 80);
        Rectangle blasterTextureRect= new Rectangle(80 * 7, 80 * 6, 80, 160);
        


        public Frags()
        {
            fragArray = new FragParticle[fragArraySize];
            for (int i = 0; i < fragArray.Length; i++)
            {
                fragArray[i] = new FragParticle();
            }

            blasterArray = new FragParticle[blasterArraySize];
            for (int i = 0; i < blasterArray.Length; i++)
            {
                blasterArray[i] = new FragParticle();
            }

            sparkArray = new FragParticle[sparkArraySize];
            for (int i = 0; i < sparkArray.Length; i++)
            {
                sparkArray[i] = new FragParticle();
            }            

        }

        public void Dispose()
        { 
        
        }

        public void NewGame()
        {
            for (int i = 0; i < fragArray.Length; i++)
            {
                fragArray[i].Active = false;
            }

            for (int i = 0; i < blasterArray.Length; i++)
            {
                blasterArray[i].Active = false;
            }

            for (int i = 0; i < sparkArray.Length; i++)
            {
                sparkArray[i].Active = false;
            }

        }

        public ref FragParticle[] getParticleArray()
        {
            return ref fragArray;
        }

        public void AddParticle(Vector2 position, Vector2 velocity, Color color, int lifeTime)
        {
            for (int counter = 0; counter < fragArraySize; counter++)
            {
                if (!fragArray[nextPosition].Active)
                {
                    counter = fragArraySize;
                }
                nextPosition++;
                nextPosition %= fragArraySize;
            }

            fragArray[nextPosition].Active = true;
            fragArray[nextPosition].position = position;
            fragArray[nextPosition].velocity = velocity;
            fragArray[nextPosition].color = color;
            fragArray[nextPosition].lifeTime = lifeTime;

        }

        public void AddBlasterParticle(Vector2 position, Vector2 velocity, int lifeTime)
        {
            for (int counter = 0; counter < blasterArraySize; counter++)
            {
                if (!blasterArray[nextBlasterPosition].Active)
                {
                    counter = blasterArraySize;
                }
                nextBlasterPosition++;
                nextBlasterPosition %= blasterArraySize;
            }

            blasterArray[nextBlasterPosition].Active = true;
            blasterArray[nextBlasterPosition].position = position;
            blasterArray[nextBlasterPosition].velocity = velocity;
            blasterArray[nextBlasterPosition].color = Color.White;
            blasterArray[nextBlasterPosition].lifeTime = lifeTime;

        }

        public void AddSparkParticle(Vector2 position, Vector2 velocity, int lifeTime)
        {
            for (int counter = 0; counter < sparkArraySize; counter++)
            {
                if (!sparkArray[nextSparkPosition].Active)
                {
                    counter = sparkArraySize;
                }
                nextSparkPosition++;
                nextSparkPosition %= sparkArraySize;
            }

            sparkArray[nextSparkPosition].Active = true;
            sparkArray[nextSparkPosition].position = position;
            sparkArray[nextSparkPosition].velocity = velocity;
            sparkArray[nextSparkPosition].color = Color.Yellow;
            sparkArray[nextSparkPosition].lifeTime = lifeTime;

        }



        private void UpdateArray(ref FragParticle[] array, List<AbstractStaticEntity> entityList, ref BallObject[] ballArray, Bat bat)
        {
            int batTop = bat.collisionRect.Top;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Active)
                {
                    array[i].lifeTime--;
                    array[i].velocity.Y += 0.3f;
                    array[i].position += array[i].velocity;


                    array[i].collisionRect.X = (int)array[i].position.X - radie;
                    array[i].collisionRect.Y = (int)array[i].position.Y - radie;

                    ParticleStaticCollision(array[i], entityList);

                    for (int b = 0; b < ballArray.Length; b++)
                    {
                        if (ballArray[b].Active)
                        {
                            ParticleBallCollision(array[i], ballArray[b]);
                        }
                    }


                    if (array[i].position.Y >= batTop)
                    {
                        ParticleBatCollision(array[i], bat);
                    }


                    if ((array[i].position.Y > 1920) || array[i].lifeTime <= 0)
                    {
                        array[i].Active = false;
                    }

                }
            }
        }


        public void Update(List<AbstractStaticEntity> entityList, ref BallObject[] ballArray, Bat bat)
        {            

            UpdateArray(ref fragArray, entityList, ref ballArray, bat);
            UpdateArray(ref blasterArray, entityList, ref ballArray, bat);
            UpdateArray(ref sparkArray, entityList, ref ballArray, bat);            
            
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {            

            spriteBatch.Begin();
            for (int i = 0; i < fragArray.Length; i++)
            {
                if (fragArray[i].Active)
                {
                    spriteBatch.Draw(texture, fragArray[i].collisionRect, fragTextureRect, fragArray[i].color);
                }
            }
                        
            for (int i = 0; i < blasterArray.Length; i++)
            {                 
                if (blasterArray[i].Active)
                {
                    Rectangle rect = blasterArray[i].collisionRect;
                    rect.Inflate(8, 8);
                    spriteBatch.Draw(texture, rect, blasterTextureRect, blasterArray[i].color);
                }
            }

            for (int i = 0; i < sparkArray.Length; i++)
            {
                if (sparkArray[i].Active)
                {
                    Rectangle rect = sparkArray[i].collisionRect;
                    rect.Inflate(8, 8);                   

                    spriteBatch.DrawLine(sparkArray[i].position, sparkArray[i].position+sparkArray[i].velocity, sparkArray[i].color, 2);
                }
            }



            spriteBatch.End();
            
        }

        private void ParticleBatCollision(FragParticle particle, Bat bat)
        {
            if (particle.collisionRect.Intersects(bat.collisionRect))            
            {
                Rectangle innerRect = bat.collisionRect;
                innerRect.Inflate(-80, 0);
                if (particle.collisionRect.Intersects(innerRect))
                {
                    particle.position -= particle.velocity * 2;
                    particle.velocity.Y = -particle.velocity.Y*0.5f;
                    return;
                }

                Vector2 leftBallPosition = new Vector2(innerRect.Left - bat.radie, innerRect.Center.Y);
                float ls = (particle.position - leftBallPosition).Length();
                if (ls < radie + bat.radie)
                {
                    Vector2 normal = (particle.position - leftBallPosition);
                    normal.Normalize();

                    particle.position -= particle.velocity * 2.0f;
                    particle.velocity = Vector2.Reflect(particle.velocity, normal);
                    particle.velocity *= 0.5f;

                    return;
                }

                Vector2 rightBallPosition = new Vector2(innerRect.Right + bat.radie, innerRect.Center.Y);
                float rs = (particle.position - rightBallPosition).Length();
                if (rs < radie + bat.radie)
                {

                    Vector2 normal = (particle.position - rightBallPosition);
                    normal.Normalize();

                    particle.position -= particle.velocity * 2.0f;
                    particle.velocity = Vector2.Reflect(particle.velocity, normal);
                    particle.velocity *= 0.5f;

                    return;
                }
            }
        }

        private void ParticleBallCollision(FragParticle particle, BallObject ball)
        {
            if (particle.collisionRect.Intersects(ball.collisionRect))
            {
                float sqls = (particle.position - ball.position).LengthSquared();

                if (sqls <= (radie + ball.radie) * (radie + ball.radie))
                { 
                    Vector2 normal = (particle.position - ball.position);
                    normal.Normalize();
                    particle.velocity = Vector2.Reflect(particle.velocity, normal);                    
                }
            }
        }

        private void ParticleStaticCollision(FragParticle particle, List<AbstractStaticEntity> entityList)
        {
            int bitX = 0;
            int bitY = 0;

            bitX |= 1 << ((int)particle.position.X / 135);  // set the bit
            bitY |= 1 << ((int)particle.position.Y / 135);  // set the bit


            for (int i = 0; i < entityList.Count; i++)
            {
                if ((bitX & entityList[i].collisionBitRegionX) != 0 && (bitY & entityList[i].collisionBitRegionY) != 0)
                {
                    if (entityList[i].collisionRect.Intersects(particle.collisionRect))
                    {
                        for (int j = 0; j < entityList[i].segments.Count; j++)
                        {
                            if (entityList[i].segments[j].box.Intersects(particle.collisionRect))
                            {
                                Vector2 toStart = entityList[i].segments[j].point1 - particle.position;
                                if (Vector2.Dot(entityList[i].segments[j].normal, toStart) > 0)
                                {
                                    continue;
                                }

                                float dist = Vector2.Dot(toStart, entityList[i].segments[j].biNormal);

                                if (dist > 0)
                                {
                                    dist = 0;
                                }
                                if (dist < -entityList[i].segments[j].length)
                                {
                                    dist = -entityList[i].segments[j].length;
                                }

                                Vector2 closestPoint = entityList[i].segments[j].point1 - entityList[i].segments[j].biNormal * dist;
                                Vector2 toPoint = closestPoint - particle.position;

                                float close = Vector2.DistanceSquared(particle.position, closestPoint);

                                if (close <= radie * radie)
                                {
                                    toPoint.Normalize();                                
                                    particle.velocity = Vector2.Reflect(particle.velocity, toPoint);                                   
                                    return;

                                }
                            }
                        }
                    }
                }
            }
        }        
    }
}