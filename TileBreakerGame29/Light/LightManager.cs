using BreakOut;
using BreakOut.Entities.Static;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using TileBreakerGame29.Entities.Dynamic.FragParticles;
using TileBreakerGame29.Entities.Dynamic.BBalls;
using Color = Microsoft.Xna.Framework.Color;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace TileBreakerGame29.Light
{
    class LightManager
    {
        
        const float distanceMultiplicator = 1024;

        private Matrix world = Matrix.CreateTranslation(0, 0, 0);
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        private Matrix projection = Matrix.CreateOrthographicOffCenter(0, 1080, 1920, 0, 0, 100);
                
        private RasterizerState rasterizerState;        

        private SortedList<int, LightData> sortedLightList;
        private List<ILightRequester> lightRequestorList;              
        private int numberOfActiveLights;
        
        public RenderTarget2D[] shadowMaps;

        private int[] quadCounter;

        const int bufferCapacity = 256;
        private VertexBuffer vertexBuffer;
        private VertexPosition[] vertigo = new VertexPosition[bufferCapacity * 6 * 8];

        private Vector3[] lightPosition;  
        private Vector4[] lightColor;

        private Rectangle fullScreenRect = new Rectangle(0,0,1080,1920);
        private Color shadowColor = new Color(0.100f, 0.100f, 0.100f, 0.0f);

        public LightManager(GraphicsDevice graphics)
        {
            quadCounter = new int[8];
            lightPosition = new Vector3[8];  
            lightColor = new Vector4[8];

            vertexBuffer = new VertexBuffer(graphics, typeof(VertexPosition), bufferCapacity * 6 * 8, BufferUsage.None);            
            rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;            
            numberOfActiveLights = 0;
            sortedLightList = new SortedList<int, LightData>(new Comparer<int>());
            lightRequestorList = new List<ILightRequester>();                     

            shadowMaps = new RenderTarget2D[8];
            for (int i = 0; i < 8; i++)
            { 
                shadowMaps[i]=new RenderTarget2D(graphics, 256, 256, false,SurfaceFormat.Color,DepthFormat.None,0,RenderTargetUsage.PreserveContents);
            }
        }

        public void Dispose()
        {
            vertexBuffer.Dispose();
            
            foreach (RenderTarget2D map in shadowMaps)
            {
                map?.Dispose();
            }
            rasterizerState?.Dispose();
        }
        
        public void AddLightRequestor(ILightRequester lr)
        {
            lightRequestorList.Add(lr);
        }
        public void RemoveLightRequestor(ILightRequester lr)
        {
            lightRequestorList.Remove(lr);
        }
        public ref Vector3[] GetLightPosition()
        {
            return ref lightPosition;
        }
        public ref Vector4[] GetLightColor()
        {
            return ref lightColor;
        }        

        private void AddShadowQuad(int index, Vector2 p1, Vector2 p2, Vector2 n1, Vector2 n2)
        {
            if (quadCounter[index] < bufferCapacity)
            {
                vertigo[quadCounter[index] * 6     + bufferCapacity * index*6] = new VertexPosition(new Vector3(p1, 0));
                vertigo[quadCounter[index] * 6 + 1 + bufferCapacity * index*6] = new VertexPosition(new Vector3(p2, 0));
                vertigo[quadCounter[index] * 6 + 2 + bufferCapacity * index*6] = new VertexPosition(new Vector3(n1, 0));
                vertigo[quadCounter[index] * 6 + 3 + bufferCapacity * index*6] = new VertexPosition(new Vector3(p2, 0));
                vertigo[quadCounter[index] * 6 + 4 + bufferCapacity * index*6] = new VertexPosition(new Vector3(n1, 0));
                vertigo[quadCounter[index] * 6 + 5 + bufferCapacity * index*6] = new VertexPosition(new Vector3(n2, 0));
                quadCounter[index]++;
            }
        }

        public void NAddShadowQuad(int index, ref Vector2 position, ref Vector2 lightPosition, ref int radie)
        {
            
            Vector2 vespQ = (position - lightPosition);
            vespQ = new Vector2(vespQ.Y, -vespQ.X);
            vespQ.Normalize();

            Vector2 p1Q = vespQ * radie + position;
            Vector2 p2Q = -vespQ * radie + position;

            Vector2 n1Q = (p1Q - lightPosition);
            Vector2 n2Q = (p2Q - lightPosition);            

            n1Q *= distanceMultiplicator;
            n2Q *= distanceMultiplicator;

            n1Q += p1Q;
            n2Q += p2Q;

            if (quadCounter[index] < bufferCapacity)
            {
                vertigo[quadCounter[index] * 6 + bufferCapacity * index * 6] = new VertexPosition(new Vector3(p1Q, 0));
                vertigo[quadCounter[index] * 6 + 1 + bufferCapacity * index * 6] = new VertexPosition(new Vector3(p2Q, 0));
                vertigo[quadCounter[index] * 6 + 2 + bufferCapacity * index * 6] = new VertexPosition(new Vector3(n1Q, 0));
                vertigo[quadCounter[index] * 6 + 3 + bufferCapacity * index * 6] = new VertexPosition(new Vector3(p2Q, 0));
                vertigo[quadCounter[index] * 6 + 4 + bufferCapacity * index * 6] = new VertexPosition(new Vector3(n1Q, 0));
                vertigo[quadCounter[index] * 6 + 5 + bufferCapacity * index * 6] = new VertexPosition(new Vector3(n2Q, 0));
                quadCounter[index]++;
            }
        }
        
        public void Update(List<AbstractStaticEntity> entityList,BallObject[] ballArray, FragParticle[] frag, Bat bat)  
        {           

            sortedLightList.Clear();

            foreach (ILightRequester lr in lightRequestorList)
            {
                lr.GetLightData(sortedLightList);
            }

            numberOfActiveLights = sortedLightList.Count;
            if (numberOfActiveLights > 8)
            {
                numberOfActiveLights = 8;
            }

            for (int i = numberOfActiveLights + 1; i < 8; i++) // some filling for the shader
            {
                lightPosition[i] = new Vector3(540, 0, 2000);
                lightColor[i] = new Vector4(0, 0, 0, 0);
            }
            for (int i = 0; i < numberOfActiveLights; i++)
            {
                lightPosition[i] = new Vector3(sortedLightList.Values.ToArray()[i].position, 200);
                lightColor[i] = sortedLightList.Values.ToArray()[i].color.ToVector4();
            }

            int batRadie = 40;
            int fragRadie = 8;
            Vector2 leftPosition = new Vector2(bat.collisionRect.Left + batRadie, bat.collisionRect.Center.Y);
            Vector2 rightPosition = new Vector2(bat.collisionRect.Right - batRadie, bat.collisionRect.Center.Y);
                        
            for (int i = 0; i < numberOfActiveLights; i++)
            {
                quadCounter[i] = 0;
                Vector2 lightPosition = sortedLightList.Values.ToArray()[i].position;

                for (int b = 0; b < ballArray.Length; b++)
                {
                    if (ballArray[b].Active)
                    {
                        NAddShadowQuad(i, ref ballArray[b].position, ref lightPosition, ref ballArray[b].radie);
                    }
                }
                // the bat
                NAddShadowQuad(i, ref leftPosition, ref lightPosition, ref batRadie);
                // right side 
                NAddShadowQuad(i, ref rightPosition, ref lightPosition, ref batRadie);

                Vector2 normal1 = (leftPosition - lightPosition);
                Vector2 normal2 = (rightPosition - lightPosition);

                normal1 *= distanceMultiplicator;
                normal2 *= distanceMultiplicator;

                normal1 += leftPosition;
                normal2 += rightPosition;

                AddShadowQuad(i, leftPosition, rightPosition, normal1, normal2);

                // static entitys
                foreach (AbstractStaticEntity entity in entityList)
                {
                    foreach (LineSegment lineSeg in entity.segments)
                    {
                        normal1 = (lineSeg.point1 - lightPosition);

                        //-- cull
                        if (Vector2.Dot(normal1, lineSeg.normal) <= 0)
                        {
                            normal2 = (lineSeg.point2 - lightPosition);

                            normal1 *= distanceMultiplicator;
                            normal2 *= distanceMultiplicator;

                            normal1 += lineSeg.point1;
                            normal2 += lineSeg.point2;

                            AddShadowQuad(i, lineSeg.point1, lineSeg.point2, normal1, normal2);
                        }

                    }
                }

                for (int j = 0; j < frag.Length; j++)
                {
                    if (frag[j].Active)
                    {
                        NAddShadowQuad(i, ref frag[j].position, ref lightPosition, ref fragRadie);

                    }
                }
            }
            vertexBuffer.SetData<VertexPosition>(vertigo);
        }

        public void DrawShadowsToShadowMaps(GraphicsDevice graphics, Effect effect, SpriteBatch spriteBatch, Texture2D texture)
        {

            for (int i = 0; i < numberOfActiveLights; i++)
            {
                graphics.SetRenderTarget(shadowMaps[i]);                
                graphics.Clear(Color.Black);
                spriteBatch.Begin();
                int radie = 1080;
                float x = ((sortedLightList.Values.ToArray()[i].position.X)/1080.0f)*256;
                float y = ((sortedLightList.Values.ToArray()[i].position.Y)/1920)*256;
                float w= (radie / 1080.0f) * 256*2;
                float h = (radie / 1920.0f) * 256 * 2;
                spriteBatch.Draw(texture, new Rectangle((int)x-(int)(w/2.0f),(int)y - (int)(h / 2.0f), (int)w,(int)h), sortedLightList.Values.ToArray()[i].color);
                spriteBatch.End();
            }

            effect.Parameters["WorldViewProjection"].SetValue(world*view*projection);
            graphics.RasterizerState = rasterizerState;
            graphics.SetVertexBuffer(vertexBuffer);

            effect.CurrentTechnique.Passes[0].Apply();
            
            for (int i = 0; i < numberOfActiveLights; i++)
            {
                graphics.SetRenderTarget(shadowMaps[i]);
                graphics.DrawPrimitives(PrimitiveType.TriangleList, bufferCapacity*i*6, quadCounter[i] *2);                                
            }
        }

        public void DrawShadows(SpriteBatch spriteBatch)
        {            
            spriteBatch.Begin();
            for (int i = 0; i < numberOfActiveLights; i++)
            {
                spriteBatch.Draw(shadowMaps[i], fullScreenRect, shadowColor);
            }
            spriteBatch.End();
        }        
    }
}