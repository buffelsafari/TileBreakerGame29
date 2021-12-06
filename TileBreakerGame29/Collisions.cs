using System.Collections.Generic;
using BreakOut;
using BreakOut.Entities.Static;
using BreakOut.Sound;
using Microsoft.Xna.Framework;
using TileBreakerGame29.Entities.Dynamic.BBalls;
using TileBreakerGame29.Entities.Static.Effects;

namespace TileBreakerGame29
{
    class Collisions
    {
        public delegate void StaticCollisionDelegate(ref Vector2 position, ref Vector2 velocity, ref Rectangle collisionRect, ref int radie, ref int dmg, List<AbstractStaticEntity> staticEntities);
        public delegate void BallToBallCollisionDelegate(ref Vector2 position1, ref Vector2 velocity1, ref int radie1, ref Rectangle collisionRect1, ref Vector2 position2, ref Vector2 velocity2, ref int radie2, ref Rectangle collisionRect2, OnBallCollisionDelegate onCollision, SoundId sound);
        public delegate void BallBatCollisionDelegate(BallObject ball, Bat bat, OnBallCollisionDelegate onCollision, SoundId sound);        
        public delegate void OnBallCollisionDelegate(Vector2 position, Vector2 normal, SoundId sound);        

        public static void BallToStaticEntities(ref Vector2 position, ref Vector2 velocity, ref Rectangle collisionRect, ref int radie, ref int dmg, List<AbstractStaticEntity> staticEntities)
        {
            if (position.Y > 1080 && position.X > 50 && position.X < 1030)
            {
                return;
            }

            AbstractStaticEntity entity = null;
            Vector2 combo = new Vector2(0, 0);
            bool hasCorner = true;
            Vector2 cornerPoint = new Vector2(0, 0);
            Vector2 hitPoint = new Vector2(0, 0);

            int bitX = 0;
            int bitY = 0;

            bitX |= 1 << ((int)position.X / 135);  // set the bit
            bitY |= 1 << ((int)position.Y / 135);  // set the bit

            foreach (AbstractStaticEntity e in staticEntities)
            {
                if ((bitX & e.collisionBitRegionX) != 0 && (bitY & e.collisionBitRegionY) != 0)
                {
                    foreach (LineSegment lineSeg in e.segments)
                    {
                        if (e.collisionRect.Intersects(collisionRect))
                        {
                            if (lineSeg.box.Intersects(collisionRect))
                            {
                                Vector2 toStart = lineSeg.point1 - position;
                                if (Vector2.Dot(lineSeg.normal, toStart) > 0)
                                {
                                    continue;
                                }

                                float dist = Vector2.Dot(toStart, lineSeg.biNormal);

                                bool corner = false;
                                if (dist > 0)
                                {
                                    dist = 0;
                                    corner = true;
                                }
                                if (dist < -lineSeg.length)
                                {
                                    dist = -lineSeg.length;
                                    corner = true;
                                }

                                Vector2 closestPoint = lineSeg.point1 - lineSeg.biNormal * dist;
                                Vector2 toPoint = closestPoint - position;

                                float close = Vector2.Distance(position, closestPoint);

                                if (close <= radie)
                                {
                                    entity = e;
                                    hitPoint = closestPoint;

                                    if (!corner)
                                    {
                                        combo += toPoint;
                                        hasCorner = false;
                                    }
                                    else
                                    {
                                        cornerPoint = toPoint;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (hasCorner)
            {
                combo += cornerPoint;
            }

            if (combo.Equals(new Vector2(0, 0)))
            {
                return;                
            }
            combo.Normalize();

            entity.hp -= dmg;
            foreach (EffectFactory.EffectFunctionDelegate h in entity.onHitList)
            {
                h.Invoke(entity, hitPoint, combo * 10);
            }

            position -= combo;

            Vector2 refi;
            refi = Vector2.Reflect(velocity, combo);

            position -= (velocity);

            velocity = refi;

            collisionRect.X = (int)(position.X - radie);
            collisionRect.Y = (int)(position.Y - radie);
        }

        public static void BallToBall(ref Vector2 position1, ref Vector2 velocity1, ref int radie1, ref Rectangle collisionRect1, ref Vector2 position2, ref Vector2 velocity2, ref int radie2, ref Rectangle collisionRect2, OnBallCollisionDelegate onCollision, SoundId sound)
        {
            if (collisionRect1.Intersects(collisionRect2))
            {
                float ls = (position2 - position1).Length();
                if (ls < radie1 + radie2)
                {
                    Vector2 normal = (position2 - position1);
                    normal.Normalize();
                    
                    Vector2 v1 = velocity2;
                    Vector2 v2 = velocity1;

                    velocity1 = v1;
                    velocity2 = v2;

                    float penetration = (radie1 + radie2) - ls;
                    position1 -= normal * (penetration) * 2;
                    position2 += normal * (penetration) * 2;

                    collisionRect1.X = (int)(position1.X - radie1);
                    collisionRect1.Y = (int)(position1.Y - radie1);

                    collisionRect1.X = (int)(position1.X - radie1);
                    collisionRect1.Y = (int)(position1.Y - radie1);

                    onCollision.Invoke(position1, normal, sound);
                }
            }            
        }

        public static void BallBat(BallObject ball, Bat bat, OnBallCollisionDelegate onCollision, SoundId sound) 
        {
            if (ball.collisionRect.Intersects(bat.collisionRect))
            {
                Rectangle innerRect = bat.collisionRect;
                innerRect.Inflate(-80, 0);
                if (innerRect.Intersects(ball.collisionRect))
                {
                    Vector2 normal = ball.velocity;
                    normal.Normalize();
                    if (ball.position.Y <= bat.collisionRect.Center.Y)
                    {
                        ball.position = (ball.position - 2 * ball.velocity);
                        ball.velocity = new Vector2(ball.velocity.X, -ball.velocity.Y);
                        ball.position.Y = bat.position.Y - ball.radie; // test
                    }
                    else
                    {                        
                        ball.position.Y = bat.position.Y+bat.collisionRect.Height + ball.radie; // test
                    }
                    onCollision.Invoke(ball.position, normal, sound);
                    return;
                }
                
                float batRadie = 40;
                Vector2 leftBallPosition = new Vector2(innerRect.Left - batRadie, bat.collisionRect.Center.Y);
                float ls = (ball.position - leftBallPosition).Length();
                if (ls < ball.radie + batRadie)
                {

                    Vector2 normal = (leftBallPosition - ball.position);
                    normal.Normalize();

                    if (ball.position.Y <= bat.collisionRect.Center.Y)
                    {                        
                        ball.position = (ball.position - 2 * ball.velocity);
                        ball.velocity = new Vector2(ball.velocity.X, -ball.velocity.Y);
                        ball.position.Y = bat.position.Y - ball.radie; // test
                    }
                    else
                    {
                        ball.position.Y = bat.position.Y + bat.collisionRect.Height + ball.radie; // test
                    }

                    onCollision.Invoke(ball.position, normal, sound);
                    return;
                }
                
                Vector2 rightBallPosition = new Vector2(innerRect.Right + batRadie, innerRect.Center.Y);
                float rs = (ball.position - rightBallPosition).Length();
                if (rs < ball.radie + batRadie)
                {
                    Vector2 normal = (rightBallPosition - ball.position);
                    normal.Normalize();

                    if (ball.position.Y <= bat.collisionRect.Center.Y)
                    {
                        ball.position = (ball.position - 2 * ball.velocity);
                        ball.velocity = new Vector2(ball.velocity.X, -ball.velocity.Y);
                        ball.position.Y = bat.position.Y - ball.radie; // test
                    }
                    else
                    {
                        ball.position.Y = bat.position.Y + bat.collisionRect.Height + ball.radie; // test
                    }

                    onCollision.Invoke(ball.position, normal, sound);
                    return;
                }
            }
        }
    }
}