
using BreakOut.Entities.Static;
using BreakOut.EntityMana;
using Microsoft.Xna.Framework;
using System;
using TileBreakerGame29.Entities.Dynamic;
using TileBreakerGame29.Entities.Dynamic.PowerUps;

namespace BreakOut.Controller
{
    
    class ComputerController : IController
    {
        const int NUMBER_OF_BALLS = 8;
        private Vector2[] pos;
        private Vector2[] vel;
        private float[] impactX;
        private int[] impactCount;
        private bool[] isImpact;        

        private int timer = 0;

        public ComputerController()
        {
            pos = new Vector2[NUMBER_OF_BALLS];
            vel = new Vector2[NUMBER_OF_BALLS];
            impactX = new float[NUMBER_OF_BALLS];
            impactCount = new int[NUMBER_OF_BALLS];
            isImpact = new bool[NUMBER_OF_BALLS];            

        }               

        public void Update(EntityManager entityManager)
        {           

            entityManager.GetBat().targetPosition.Y = 1920 - 400;
            bool blastWithLaser = true;

            int ind=0;
            for (int counter = 0; counter < entityManager.GetBallsOfDoom().GetBallArray().Length; counter++)
            {
                if (entityManager.GetBallsOfDoom().GetBallArray()[counter].Active)
                {
                    vel[ind] = entityManager.GetBallsOfDoom().GetBallArray()[counter].velocity * 2.0f;
                    pos[ind] = entityManager.GetBallsOfDoom().GetBallArray()[counter].position;
                    isImpact[ind] = false;
                    impactX[ind] = 540;
                    impactCount[ind] = 0;
                    ind++;
                }
            }
            
            int smallestImpact = int.MaxValue;
            if (entityManager.GetBallsOfDoom().GetNumberOfActiveBalls() > 0)  // temp
            { 
                for (int counter = 0; counter < 1000; counter++)
                {
                    for (int i = 0; i < entityManager.GetBallsOfDoom().GetNumberOfActiveBalls(); i++)
                    {
                        pos[i] += vel[i];
                        if (pos[i].X < (7 + Balls.radie) || pos[i].X > (1080 - 7 - Balls.radie))
                        {
                            vel[i].X = -vel[i].X;
                        }

                        if (!isImpact[i] && pos[i].Y > entityManager.GetBat().targetPosition.Y - 40 && pos[i].Y< entityManager.GetBat().targetPosition.Y)
                        {
                            isImpact[i] = true;
                            impactCount[i] = counter;
                            impactX[i] = pos[i].X;
                            if (counter < smallestImpact)
                            {
                                smallestImpact = counter;
                            }
                            blastWithLaser = false;
                        }                        
                    }                    
                }                

                // find the closest ball
                int closestCount = int.MaxValue;
                float nextClosestX = entityManager.GetBat().targetPosition.X;
                float closestX = entityManager.GetBat().targetPosition.X; 
                for (int i = 0; i < entityManager.GetBallsOfDoom().GetNumberOfActiveBalls(); i++)
                {                    
                    if (isImpact[i] && impactCount[i] < closestCount)
                    {
                        nextClosestX = closestX;
                        closestCount = impactCount[i];
                        closestX = impactX[i];
                    }                    
                }

                if ((Math.Abs(closestX - nextClosestX)) < (entityManager.GetBat().collisionRect.Width - 40))
                {
                    closestX = (closestX + nextClosestX) / 2;
                    float delta = closestX - entityManager.GetBat().targetPosition.X;
                    entityManager.GetBat().targetPosition.X += delta * 0.2f;
                }
                else
                {
                    if ((closestX < (entityManager.GetBat().collisionRect.Left + 40)) || (closestX > (entityManager.GetBat().collisionRect.Right - 40)))
                    {
                        float delta = closestX - entityManager.GetBat().targetPosition.X;
                        entityManager.GetBat().targetPosition.X += delta * 0.2f;
                    }
                }
               
            }

            if (smallestImpact>500)
            {
                int closestPowerUpX = Int32.MaxValue;
                float closestPowerUp = float.MaxValue;
                bool powerUpInRange = false;
                foreach (PowerUp pu in PowerUp.powerUpList)
                {
                    if (pu.Active && entityManager.WantPowerUp(pu.GetPowerUpType()))
                    {                        
                        Vector2 powerPosition = pu.GetPosition();
                        if (powerPosition.Y > 1080 && powerPosition.Y < entityManager.GetBat().collisionRect.Bottom)
                        {
                            float distance = Vector2.Distance(powerPosition, entityManager.GetBat().collisionRect.Center.ToVector2());
                            if (distance < closestPowerUp)
                            {
                                closestPowerUp = distance;
                                closestPowerUpX = (int)powerPosition.X;
                                powerUpInRange = true;
                                blastWithLaser = false;
                            }                            
                        }
                    }
                }
                if (powerUpInRange)
                {
                    if ((closestPowerUpX < (entityManager.GetBat().collisionRect.Left + 40)) || (closestPowerUpX > (entityManager.GetBat().collisionRect.Right - 40)))
                    {
                        float delta = closestPowerUpX - entityManager.GetBat().targetPosition.X;
                        entityManager.GetBat().targetPosition.X += delta * 0.1f;
                    }
                }
            }

            if (blastWithLaser && entityManager.isBlasterOn())
            {                
                foreach (AbstractStaticEntity entity in entityManager.GetStaticEntities())
                {
                    if (entity is Tile)
                    {
                        float delta = entity.collisionRect.Center.X - entityManager.GetBat().targetPosition.X;
                        entityManager.GetBat().targetPosition.X += delta * 0.1f;
                    }
                }
            }


            if (entityManager.GetBat().isReadyToServe)
            {                

                if (timer >= 50)
                {
                    timer = 0;
                    Vector2 aim = new Vector2(540, 0);
                    int r = Globals.rnd.Next();
                    for (int i = 0; i < entityManager.GetStaticEntities().Count; i++)
                    //foreach (AbstractStaticEntity entity in entityManager.GetStaticEntities())
                    {
                        int ss = (i + r) % entityManager.GetStaticEntities().Count;
                        if (entityManager.GetStaticEntities()[ss].isKillable && entityManager.GetStaticEntities()[ss].isAlive)
                        {
                            aim = entityManager.GetStaticEntities()[ss].collisionRect.Center.ToVector2();
                            break;
                        }
                    }

                    entityManager.GetBallsOfDoom().SetServeDirection(aim - entityManager.GetBat().position);
                    entityManager.GetBat().IsServed = true;

                }
                else
                {
                    timer++;
                }
            }


        }
    }
}