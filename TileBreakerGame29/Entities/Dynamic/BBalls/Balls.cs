using System.Collections.Generic;
using BreakOut;
using BreakOut.Entities.Static;
using BreakOut.EntityMana;
using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileBreakerGame29.Entities.Dynamic.BBalls;
using TileBreakerGame29.Light;


namespace TileBreakerGame29.Entities.Dynamic
{
    class Balls:ILightRequester
    {
        const float adjustDelta = 0.05f;

        const int MAX_BALLS=8;
        const int SPRITESHEET_SIZE_X = 640;
        const int SPRITESHEET_SIZE_Y = 640;

        public static float ballSpeed=1.0f;

        private DynamicVertexBuffer dynamicVertexBuffer;                
        private DynamicIndexBuffer dynamicIndexBuffer;

        private VertexPositionColorTexture[] vertexArray;
        private short[] indexArray;
        private LightData lightData;
        private Rectangle textureRect;
        private int numberOfActiveBalls = 0;

        public const int radie = 35;

        private Vector2[] textureVectors;
        private BallObject[] ballArray;
        
        private Collisions.StaticCollisionDelegate staticCollision;
        private Collisions.BallToBallCollisionDelegate ballCollision;
        private Collisions.BallBatCollisionDelegate batCollision;
        
        private Collisions.OnBallCollisionDelegate ballToBallEffect;
        private Collisions.OnBallCollisionDelegate ballToBatEffect;
                

        public Balls(Game1 game, EntityManager entityManager)
        {           
           
            dynamicVertexBuffer = new DynamicVertexBuffer(game.GraphicsDevice, typeof(VertexPositionColorTexture), 6*MAX_BALLS, BufferUsage.WriteOnly);
            dynamicIndexBuffer = new DynamicIndexBuffer(game.GraphicsDevice, typeof(short), 6 * MAX_BALLS, BufferUsage.WriteOnly);

            vertexArray = new VertexPositionColorTexture[6 * MAX_BALLS];
            indexArray = new short[6 * MAX_BALLS];
            
            ballArray = new BallObject[8];
            for (int i = 0; i < ballArray.Length; i++)
            {
                ballArray[i] = new BallObject(radie);
                ballArray[i].position.Y = 100 * i;
            }            

            textureRect = new Rectangle(524, 275, 80, 80);

            float texL = textureRect.Left / (float)SPRITESHEET_SIZE_X;
            float texR = textureRect.Right / (float)SPRITESHEET_SIZE_X;

            float texT = textureRect.Top / (float)SPRITESHEET_SIZE_Y;
            float texB = textureRect.Bottom / (float)SPRITESHEET_SIZE_Y;

            textureVectors = new Vector2[6];
            textureVectors[0] = new Vector2(texL, texT);
            textureVectors[1] = new Vector2(texR, texT);
            textureVectors[2] = new Vector2(texL, texB);

            textureVectors[3] = new Vector2(texR, texT);
            textureVectors[4] = new Vector2(texR, texB);
            textureVectors[5] = new Vector2(texL, texB);

            for (int i = 0; i < 1; i++)  
            {
                int id = AddBall(new Vector2(Globals.rnd.Next(0, 1080), 1000 + i * 100), new Vector2(0, -ballSpeed), Color.White, 1);            
            }

            staticCollision = Collisions.BallToStaticEntities;
            ballCollision = Collisions.BallToBall;
            batCollision = Collisions.BallBat;
            
            ballToBallEffect = entityManager.OnBallCollision;
            ballToBatEffect = entityManager.OnBallCollision;

            lightData = new LightData();
        }

        public void Dispose()
        {
            dynamicIndexBuffer?.Dispose();
            dynamicVertexBuffer?.Dispose();            
        }

        public int GetNumberOfActiveBalls()
        {
            return numberOfActiveBalls;
        }

        public void NewGame()
        {
            numberOfActiveBalls = 0;
            for (int i = 0; i < 8; i++)
            {
                ballArray[i].Active = false;
            }            
        }

        public void SetBuffers()
        {
            dynamicVertexBuffer.SetData<VertexPositionColorTexture>(vertexArray);
            dynamicIndexBuffer.SetData<short>(indexArray);
        }

        public void AddVerticesToArray(int index, ref Rectangle rect)
        {            
            vertexArray[0 + 6 * index] = new VertexPositionColorTexture(new Vector3(rect.Left, rect.Top, 0), ballArray[index].color, textureVectors[0]);
            vertexArray[1 + 6 * index] = new VertexPositionColorTexture(new Vector3(rect.Right, rect.Top, 0), ballArray[index].color, textureVectors[1]);
            vertexArray[2 + 6 * index] = new VertexPositionColorTexture(new Vector3(rect.Left, rect.Bottom, 0), ballArray[index].color, textureVectors[2]);

            vertexArray[3 + 6 * index] = new VertexPositionColorTexture(new Vector3(rect.Right, rect.Top, 0), ballArray[index].color, textureVectors[3]);
            vertexArray[4 + 6 * index] = new VertexPositionColorTexture(new Vector3(rect.Right, rect.Bottom, 0), ballArray[index].color, textureVectors[4]);
            vertexArray[5 + 6 * index] = new VertexPositionColorTexture(new Vector3(rect.Left, rect.Bottom, 0), ballArray[index].color, textureVectors[5]);            
        }

        public int AddBall(Vector2 position, Vector2 Velocity, Color color, int dmg)
        {
            for (int i = 0; i < 8; i++)
            {
                if (!ballArray[i].Active)
                {
                    ballArray[i].Active = true;
                    ballArray[i].position = position;
                    
                    ballArray[i].velocity = Velocity;
                    
                    ballArray[i].color = color;
                    ballArray[i].dmg = dmg;
                    ballArray[i].lifeTime = 0;

                    return i;
                }
            }
            return -1;
        }

        public void RemoveBall(int id)
        {
            ballArray[id].Active = false;
        }

        public ref BallObject[] GetBallArray()
        {
            return ref ballArray;
        }

        public void SetServeDirection(Vector2 direction)
        {
            direction.Normalize();
            direction *= Balls.ballSpeed;
            ballArray[0].velocity=direction;
        }
        public void Update(List<AbstractStaticEntity> staticEntities, Bat bat, bool isServing)
        {
            numberOfActiveBalls = 0;

            if (isServing)
            {
                ballArray[0].Active = true;
                ballArray[0].position.X = bat.position.X;
                ballArray[0].position.Y = bat.position.Y- ballArray[0].radie;
                ballArray[0].lifeTime++;
                ballArray[0].collisionRect.X = (int)(ballArray[0].position.X - radie);
                ballArray[0].collisionRect.Y = (int)(ballArray[0].position.Y - radie);
                numberOfActiveBalls = 0;
            }
            else
            {
                for (int i = 0; i < ballArray.Length; i++)
                {
                    if (ballArray[i].Active)
                    {
                        // update position
                        ballArray[i].lifeTime++;
                        ballArray[i].collisionRect.X = (int)(ballArray[i].position.X - radie);
                        ballArray[i].collisionRect.Y = (int)(ballArray[i].position.Y - radie);
                       
                        staticCollision.Invoke(ref ballArray[i].position, ref ballArray[i].velocity, ref ballArray[i].collisionRect, ref ballArray[i].radie, ref ballArray[i].dmg, staticEntities);

                        for (int j = 0; j < ballArray.Length; j++)
                        {
                            if (ballArray[j].Active && j != i)
                            {
                                ballCollision.Invoke(ref ballArray[i].position, ref ballArray[i].velocity, ref ballArray[i].radie, ref ballArray[i].collisionRect, ref ballArray[j].position, ref ballArray[j].velocity, ref ballArray[j].radie, ref ballArray[j].collisionRect, ballToBallEffect, SoundId.ballToBall);
                            }
                        }

                        batCollision.Invoke(ballArray[i], bat, ballToBatEffect, SoundId.ballToBat);
                        
                        if (ballArray[i].position.Y > 1920)
                        {
                            ballArray[i].Active = false;                            
                        }
                        if (ballArray[i].position.Y < 0)
                        {
                            ballArray[i].Active = false;
                        }
                        if (ballArray[i].position.X > 1080)
                        {
                            ballArray[i].Active = false;
                        }
                        if (ballArray[i].position.X < 0)
                        {
                            ballArray[i].Active = false;
                        }

                        ballArray[i].position += ballArray[i].velocity;


                        // adjust angle
                        bool adjusted = false;
                        float xv = ballArray[i].velocity.X;
                        if (xv < adjustDelta && xv > -adjustDelta)
                        {
                            int sign=xv.CompareTo(0);
                            if (sign == 0)
                            {
                                sign++;
                            }
                            ballArray[i].velocity.X= adjustDelta *sign;
                            adjusted = true;
                        }
                        float yv = ballArray[i].velocity.Y;
                        if (yv < adjustDelta && yv > -adjustDelta)
                        {
                            int sign = yv.CompareTo(0);
                            if (sign == 0)
                            {
                                sign++;
                            }
                            ballArray[i].velocity.Y = adjustDelta * sign;
                            adjusted = true;
                        }

                        if (adjusted)
                        {                            
                            ballArray[i].velocity.Normalize();
                            ballArray[i].velocity *= Balls.ballSpeed;
                        }



                    }
                }
            }
        }
        public void UpdateBuffer()
        {
            for (int i = 0; i < ballArray.Length; i++)
            {
                if (ballArray[i].Active)
                {
                    AddVerticesToArray(i, ref ballArray[i].collisionRect);

                    indexArray[0 + numberOfActiveBalls * 6] = (short)(0 + i * 6);
                    indexArray[1 + numberOfActiveBalls * 6] = (short)(1 + i * 6);
                    indexArray[2 + numberOfActiveBalls * 6] = (short)(2 + i * 6);
                    indexArray[3 + numberOfActiveBalls * 6] = (short)(3 + i * 6);
                    indexArray[4 + numberOfActiveBalls * 6] = (short)(4 + i * 6);
                    indexArray[5 + numberOfActiveBalls * 6] = (short)(5 + i * 6);
                    numberOfActiveBalls++;
                }

                if (numberOfActiveBalls > 0)
                {
                    SetBuffers();
                }
            }
        }

        public void Draw(GraphicsDevice graphics)
        {
            if (numberOfActiveBalls > 0)
            {
                graphics.SetVertexBuffer(dynamicVertexBuffer);
                graphics.Indices = dynamicIndexBuffer;                
                graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2 * numberOfActiveBalls);
            }
        }

        public void DrawDirectShadow(SpriteBatch spriteBatch, Vector2 offset, Texture2D texture)
        {
            for (int i = 0; i < ballArray.Length; i++)
            {
                if (ballArray[i].Active)
                {
                    spriteBatch.Draw(texture, new Rectangle((int)(ballArray[i].position.X-radie+offset.X) ,(int)(ballArray[i].position.Y-radie+offset.Y), radie*2,radie*2) , textureRect, new Color(0,0,0,0.1f));
                }
            }
        }

        public void GetLightData(SortedList<int, LightData> list)
        {
            for (int i = 0; i < ballArray.Length; i++)
            {
                if (ballArray[i].Active)
                {                    
                    lightData.position = ballArray[i].position;
                    lightData.color = ballArray[i].color*0.1f;
                    list.Add(Globals.ballLightPriority, lightData);
                }
            }
            
        }
    }
}