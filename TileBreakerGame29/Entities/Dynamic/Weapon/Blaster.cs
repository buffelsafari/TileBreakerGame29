using System.Collections.Generic;
using BreakOut;
using BreakOut.Entities.Static;
using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileBreakerGame29.Entities.Dynamic.FragParticles;
using TileBreakerGame29.Entities.Dynamic.BBalls;
using TileBreakerGame29.Entities.Static.Effects;
using TileBreakerGame29.Light;
using TileBreakerGame29.Refraction;

namespace TileBreakerGame29.Entities.Dynamic.Weapon
{
    class Blaster:ILightRequester
    {
        private static LightData lightData;
        const int maxHits = 32;
        private int nextHit = 0;
        private int timer;
        private int timerInc;
        private int leftRightFlip;
        private int leftRightInc;
        private int centerFlip;
        private int centerInc;
        private int fireDelay;
        private static Rectangle textureRect = new Rectangle(80 * 7, 80 * 6, 80, 160);
        private Rectangle rect=new Rectangle(0,0,80,160);
        
        private BlasterHit[] hitArray;        
        private List<Vector2> boltPosition;
        const int boltRadie=20;
        int weaponLevel = 0;

        /// <summary>
        /// handles the players blaster weapon
        /// </summary>
        public Blaster()
        {
            lightData = new LightData();
            lightData.color = Color.LightBlue;

            boltPosition = new List<Vector2>();            
            hitArray = new BlasterHit[maxHits];

            for (int i = 0; i < hitArray.Length; i++)
            {
                hitArray[i] = new BlasterHit();
            }


            SetWeaponLevel(0);
        }

        public void NewGame()
        {
            SetWeaponLevel(0);
            boltPosition.Clear();
            for (int counter = 0; counter < hitArray.Length; counter++)
            {
                hitArray[nextHit].Active = false;                
            }
        }
        public List<Vector2> GetBlasterBolts()
        {
            return boltPosition;
        }
        public void Activate()
        {
            SetWeaponLevel(++weaponLevel);
        }
        private void SetWeaponLevel(int level)
        {
            weaponLevel = level;
            if (weaponLevel <= 4)
            {
                switch (weaponLevel)
                {
                    case 0:
                        timer = -1;
                        timerInc = 0;
                        leftRightFlip = 0;
                        leftRightInc = 0;
                        centerFlip = 0;
                        centerInc = 0;
                        fireDelay = 30;                        
                        break;
                    case 1:
                        timer = -1;
                        timerInc = 1;
                        leftRightFlip = 0;
                        leftRightInc = 0;
                        centerFlip = 0;
                        centerInc = 0;
                        fireDelay = 30;                        
                        break;
                    case 2:
                        timer = -1;
                        timerInc = 1;
                        leftRightFlip = 1;
                        leftRightInc = 1;
                        centerFlip = 1;
                        centerInc = 0;
                        fireDelay = 15;                        
                        break;
                    case 3:
                        timer = -1;
                        timerInc = 1;
                        leftRightFlip = 1;
                        leftRightInc = 1;
                        centerFlip = 1;
                        centerInc = 1;
                        fireDelay = 8;                        
                        break;
                    case 4:
                        timer = -1;
                        timerInc = 1;
                        leftRightFlip = 1;
                        leftRightInc = 1;
                        centerFlip = 1;
                        centerInc = 1;
                        fireDelay = 5;                        
                        break;
                }
            }            
        }

        public void Reset()
        {
            SetWeaponLevel(0);            
        }        

        private void BallCollision(ref BallObject[] ballArray, Frags frags)
        {            
            for(int b=0;b<ballArray.Length ;b++)
            {
                if (ballArray[b].Active)
                {
                    for(int i=boltPosition.Count-1;i>=0;i--)            
                    {
                
                        if ((Vector2.DistanceSquared(ballArray[b].position, boltPosition[i]) < (Balls.radie * Balls.radie + boltRadie * boltRadie)))
                        {
                            Vector2 n = ballArray[b].position - boltPosition[i];
                            n.Normalize();
                            ballArray[b].velocity = (n * Balls.ballSpeed);  // variable for speed
                      
                            AddHit(boltPosition[i]);
                            blastParticles(boltPosition[i], -n, frags);
                            boltPosition.RemoveAt(i);
                        }
                    }
                }
            }
        }

        private void AddHit(Vector2 position)
        {
            SoundManager.Play(SoundId.blasterhit);
            for (int counter = 0; counter < hitArray.Length; counter++)
            {
                if (!hitArray[nextHit].Active)
                {
                    counter = hitArray.Length;
                }
                nextHit++;
                nextHit %= hitArray.Length;
            }            

            hitArray[nextHit].SetData(position);
            ShockRefraction.Add(position, 200); 
        }

        private void blastParticles(Vector2 position, Vector2 velocity, Frags frags)
        {
            for (int counter = 0;counter<20 ; counter++)
            {
                int lifeTime = Globals.rnd.Next(5,15);                
                Vector2 vel = new Vector2(Globals.rnd.Next(-100, 100) * 0.1f, Globals.rnd.Next(-100, 100) * 0.1f);

                frags.AddBlasterParticle(position, velocity+vel, lifeTime);
            }
        }

        private void StaticCollision(List<AbstractStaticEntity> entities, Frags frags)
        {
            float radie = 10;
            for (int i = boltPosition.Count - 1; i >= 0; i--)            
            {
                Rectangle rc = new Rectangle((int)(boltPosition[i].X - 10), (int)(boltPosition[i].Y -10), 20, 20);
                foreach (AbstractStaticEntity entity in entities)
                {
                    if (rc.Intersects(entity.GetCollisionRectangle()))
                    {
                        foreach (LineSegment lineSeg in entity.segments)
                        {
                            if (lineSeg.box.Intersects(rc))
                            {
                                Vector2 toStart = lineSeg.point1 - boltPosition[i];
                                if (Vector2.Dot(lineSeg.normal, toStart) > 0)
                                {
                                    continue;
                                }

                                float dist = Vector2.Dot(toStart, lineSeg.biNormal);

                                if (dist > 0)
                                {
                                    dist = 0;
                                }

                                if (dist < -lineSeg.length)
                                {
                                    dist = -lineSeg.length;
                                }

                                Vector2 closestPoint = lineSeg.point1 - lineSeg.biNormal * dist;
                                Vector2 toPoint = closestPoint - boltPosition[i];

                                float close = Vector2.DistanceSquared(boltPosition[i], closestPoint);

                                if (close <= radie * radie)
                                {
                                    toPoint.Normalize();
                                    AddHit(boltPosition[i]);

                                    entity.hp--;
                                    foreach (EffectFactory.EffectFunctionDelegate h in entity.onHitList)
                                    {
                                        h.Invoke(entity, boltPosition[i], toPoint * 5);
                                    }
                                    blastParticles(boltPosition[i], -toPoint, frags);

                                    boltPosition.RemoveAt(i);

                                    return;

                                }
                            }
                        }
                    }
                }
            }
        }

        public void Update(Bat bat, ref BallObject[] ballArray, List<AbstractStaticEntity> entities, Frags frags)
        {
            
            // fire shoots
            if (timer % fireDelay == 0)
            {
                SoundManager.Play(SoundId.blastershoot);
                Vector2 p = new Vector2(((bat.collisionRect.Left + 60)*(leftRightFlip%2)+ (bat.collisionRect.Right - 60) * ((leftRightFlip+1) % 2))*(centerFlip%2) + 
                                        (bat.collisionRect.Center.X) * ((centerFlip+1) % 2), 
                                        bat.collisionRect.Top);
                
                boltPosition.Add(p);
                centerFlip+=centerInc;
                leftRightFlip += centerFlip * leftRightInc;               
                
            }
            timer += timerInc;


            // update positions
            for (int counter = 0; counter < 10; counter++)
            {
                for (int i = 0; i < boltPosition.Count; i++)
                {                    
                    boltPosition[i] = new Vector2(boltPosition[i].X, boltPosition[i].Y - 5);                    
                }

                BallCollision(ref ballArray, frags);
                StaticCollision(entities, frags);              
            }

            for (int i=0; i<hitArray.Length ;i++)
            {
                if (hitArray[i].Active)
                {
                    hitArray[i].Update();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {            
            foreach (Vector2 p in boltPosition)
            {
                rect.X = (int)(p.X - 40);
                rect.Y = (int)(p.Y-20);
                spriteBatch.Draw(texture, rect, textureRect, Color.White);
            }            

            for (int i = 0; i < hitArray.Length; i++)
            {
                if (hitArray[i].Active)
                {
                    hitArray[i].Draw(spriteBatch, texture);
                }
            }
        }
       

        public void GetLightData(SortedList<int, LightData> list)
        {            
            for (int i=0; i < boltPosition.Count; i++)
            {                
                lightData.position = boltPosition[i];                
                list.Add(Globals.blasterLightPriority, lightData);                
            }            
        }
    }
}