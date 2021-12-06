using BreakOut.Entities.Static;
using BreakOut.EntityMana;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace BreakOut
{
    class Cracks
    {
        private List<ValueTuple<Vector2, Tile>> crackList;
        private List<Tile> deleteList;        
        private RenderTarget2D renderTarget;        

        private DynamicVertexBuffer vertexBuffer;
        private BasicEffect basicEffect;
        private Matrix world = Matrix.CreateTranslation(0, 0, 0);
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        private Matrix projection = Matrix.CreateOrthographicOffCenter(0, 1080, 1080, 0, 0, 100);

        private RasterizerState rasterizerState = new RasterizerState();
        private VertexPosition[] vertigo = new VertexPosition[3];

        private DepthStencilState drawState;
        private DepthStencilState maskState;

        private Rectangle fullCubicRect = new Rectangle(0, 0, 1080, 1080);
        private Rectangle crackTextureRect = new Rectangle(160 * 0, 160 * 3, 160, 160);
        private Color crackColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
        private Vector2 origo = new Vector2(80,80);


        public Cracks(GraphicsDevice graphics)
        {
            basicEffect = new BasicEffect(graphics);
            vertexBuffer = new DynamicVertexBuffer(graphics, typeof(VertexPosition), 3*5, BufferUsage.None);            
            basicEffect.VertexColorEnabled = true;

            vertigo[0] = new VertexPosition(new Vector3(300, 300, 0));
            vertigo[1] = new VertexPosition(new Vector3(400, 300, 0));
            vertigo[2] = new VertexPosition(new Vector3(300, 400, 0));


            renderTarget = new RenderTarget2D(graphics, 1080, 1080, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PreserveContents);
            int[] data=new int[1080*1080];
            renderTarget.SetData(data); 
            crackList = new List<ValueTuple<Vector2,Tile>>();
            deleteList = new List<Tile>();

            maskState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
                StencilMask = 1,
            };

            drawState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Equal,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
                StencilMask = 1,
            };
        }

        public void Dispose()
        {
            renderTarget?.Dispose();
            drawState?.Dispose();
            maskState?.Dispose();
            rasterizerState?.Dispose();
            basicEffect?.Dispose();
            vertexBuffer?.Dispose();
        }

        public void Add(Tile tile, Vector2 position)
        {
            crackList.Add(new ValueTuple<Vector2, Tile>(position, tile));
        }

        public void Sub(Tile tile)
        {
            deleteList.Add(tile);
        }

        public void NewGame()
        {
            crackList.Clear();
            deleteList.Clear();
            int[] data = new int[1080 * 1080];
            renderTarget.SetData(data);
        }       
       
        private void DrawMask(GraphicsDevice graphics, int numberOfTriangles, float alpha, Effect effect, Texture2D texture)
        {
            effect.Parameters["WorldViewProjection"].SetValue(world * view * projection);
            graphics.SetVertexBuffer(vertexBuffer);
            rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            rasterizerState.FillMode = FillMode.Solid;
            graphics.RasterizerState = rasterizerState;
            effect.Techniques[0].Passes[0].Apply();             
            graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, numberOfTriangles);
        }        

        public void DrawToCrackMap(GraphicsDevice graphics, SpriteBatch spriteBatch, EntityManager entityManager, Texture2D texture, Effect effect)
        {
            graphics.SetRenderTarget(renderTarget);
            graphics.DepthStencilState = maskState;

            foreach (ValueTuple<Vector2, Tile> tu in crackList)
            { 
                graphics.Clear(ClearOptions.Stencil | ClearOptions.DepthBuffer, Color.Transparent, 0f, 0);
                vertexBuffer.SetData<VertexPosition>(tu.Item2.GetTriangles());                
                DrawMask(graphics, tu.Item2.GetNumberOfTriangles(), 0.0f, effect, texture);                

                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, drawState, null, null, null);
                spriteBatch.Draw(texture, tu.Item1, crackTextureRect, crackColor,(float)Globals.rnd.NextDouble()*2*3.14f,origo, 1.5f,SpriteEffects.None,0);  
                spriteBatch.End();
            }
                       
            crackList.Clear();
  
            graphics.DepthStencilState = maskState;
            graphics.BlendState = BlendState.Opaque;
            foreach (Tile tile in deleteList)
            { 
                vertexBuffer.SetData<VertexPosition>(tile.GetTriangles());                
                DrawMask(graphics, tile.GetNumberOfTriangles(), 0.0f, effect, texture);
            }
            deleteList.Clear();            
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch spriteBatch)
        { 
            spriteBatch.Begin();
            spriteBatch.Draw(renderTarget, fullCubicRect, Color.White);
            spriteBatch.End();
        }
    }
}