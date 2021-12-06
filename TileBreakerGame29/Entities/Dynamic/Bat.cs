using System.Collections.Generic;
using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BreakOut
{

    class Bat
    {
        static public DynamicVertexBuffer dynamicVertexBuffer;
        static public List<VertexPositionColorTexture> dynamicVertexList = new List<VertexPositionColorTexture>();
        static public IndexBuffer dynamicIndexBuffer;
        
        static public List<short> dynamicIndexList = new List<short>();

        private float expand = 0;
        private float expandVelocity = 0;
        private float expandAcceleration = 0;
        private int targetExpand = 0;        

        const int BAT_CENTER_OFFSET_X = 2;
        const int BAT_CENTER_OFFSET_Y = 2;

        const int BAT_LEFT_OFFSET_X = 166;
        const int BAT_LEFT_OFFSET_Y = 2;

        const int BAT_RIGHT_OFFSET_X = 249;
        const int BAT_RIGHT_OFFSET_Y = 2;

            
        public Vector2 targetPosition;
        public Vector2 offset;        

        public Vector2 oldPosition;

        public bool IsServing = false;
        public bool IsServed = false;
        public bool isReadyToServe = false;
        public bool justServed = false;

        public Vector2 position;
        public Vector2 velocity;
        public int radie = 40;        
        public short[] indices;

        private Rectangle textureRect;
        public Rectangle collisionRect;

        public const int SPRITESHEET_SIZE_X = 640;
        public const int SPRITESHEET_SIZE_Y = 640;

        private Color leftColor;
        private Color rightColor;
        private Color middleColor;

        private Rectangle leftTextureRect;
        private Rectangle rightTextureRect;
        private Rectangle middleTextureRect;

        private Color shadowColor;

        public Bat(GraphicsDevice graphics)
        {
            leftTextureRect = new Rectangle(166, 2, 80, 80);
            rightTextureRect = new Rectangle(249, 2, 80, 80);
            middleTextureRect = new Rectangle(2, 2, 160, 80);

            shadowColor = new Color(0, 0, 0, 0.1f);

            position = new Vector2(500, 1700);
            oldPosition = position;

            targetPosition = new Vector2(500, 1700);
            offset = new Vector2(0, 0);
            velocity = new Vector2(0, 0);            
            this.radie = 40;
            
            dynamicVertexBuffer = new DynamicVertexBuffer(graphics, typeof(VertexPositionColorTexture), 18, BufferUsage.WriteOnly);
            dynamicIndexBuffer = new DynamicIndexBuffer(graphics, typeof(short), 18, BufferUsage.WriteOnly);
            
            leftColor = Color.Yellow;
            rightColor = Color.Green;
            middleColor = Color.Pink;

        }

        public void Dispose()
        {
            dynamicVertexBuffer?.Dispose();
            dynamicIndexBuffer?.Dispose();
        }

        public void NewGame()
        {
            targetExpand = 0;
            expand = 0;            
        }

        public void SetLeftColor(Color color)
        {
            leftColor = color;
        }

        public void SetRightColor(Color color)
        {
            rightColor = color; 
        }

        public void SetMiddleColor(Color color)
        {
            middleColor = color;
        }
        public void ExpandBat()
        {
            if (targetExpand < 300)
            {
                SoundManager.Play(SoundId.extender);
                targetExpand += 100;
            }
            else
            {
                SoundManager.Play(SoundId.denial);
            }
        }
        public void ResetExpand()
        {            
            targetExpand = 0;
        }
        public void AddVerticesToList()
        {
            indices = new short[6+6+6];
            textureRect = new Rectangle(0, 0, 160, 80);
            textureRect.Offset(BAT_CENTER_OFFSET_X, BAT_CENTER_OFFSET_Y);

            float l = textureRect.Left / (float)SPRITESHEET_SIZE_X;
            float r = textureRect.Right / (float)SPRITESHEET_SIZE_X;

            float t = textureRect.Top / (float)SPRITESHEET_SIZE_Y;
            float b = textureRect.Bottom / (float)SPRITESHEET_SIZE_Y;            

            Rectangle innerRect = collisionRect;
            innerRect.Inflate(-radie*2+10, 0);

            indices[0] = AddVertex(new VertexPositionColorTexture(new Vector3(innerRect.Left, innerRect.Top, 0), middleColor, new Vector2(l, t)));
            indices[1] = AddVertex(new VertexPositionColorTexture(new Vector3(innerRect.Right, innerRect.Top, 0), middleColor, new Vector2(r, t)));
            indices[2] = AddVertex(new VertexPositionColorTexture(new Vector3(innerRect.Left, innerRect.Bottom, 0), middleColor, new Vector2(l, b)));

            indices[3] = AddVertex(new VertexPositionColorTexture(new Vector3(innerRect.Right, innerRect.Top, 0), middleColor, new Vector2(r, t)));
            indices[4] = AddVertex(new VertexPositionColorTexture(new Vector3(innerRect.Right, innerRect.Bottom, 0), middleColor, new Vector2(r, b)));
            indices[5] = AddVertex(new VertexPositionColorTexture(new Vector3(innerRect.Left, innerRect.Bottom, 0), middleColor, new Vector2(l, b)));

            textureRect = new Rectangle(0, 0, 80, 80);
            textureRect.Offset(BAT_LEFT_OFFSET_X, BAT_LEFT_OFFSET_Y);
            Rectangle leftRect = collisionRect;            
            leftRect.Width = (int)radie*2;

            l = textureRect.Left / (float)SPRITESHEET_SIZE_X;
            r = textureRect.Right / (float)SPRITESHEET_SIZE_X;

            t = textureRect.Top / (float)SPRITESHEET_SIZE_Y;
            b = textureRect.Bottom / (float)SPRITESHEET_SIZE_Y;
                        
            indices[6] = AddVertex(new VertexPositionColorTexture(new Vector3(leftRect.Left, leftRect.Top, 0), leftColor, new Vector2(l, t)));
            indices[7] = AddVertex(new VertexPositionColorTexture(new Vector3(leftRect.Right, leftRect.Top, 0), leftColor, new Vector2(r, t)));
            indices[8] = AddVertex(new VertexPositionColorTexture(new Vector3(leftRect.Left, leftRect.Bottom, 0), leftColor, new Vector2(l, b)));

            indices[9] = AddVertex(new VertexPositionColorTexture(new Vector3(leftRect.Right, leftRect.Top, 0), leftColor, new Vector2(r, t)));
            indices[10] = AddVertex(new VertexPositionColorTexture(new Vector3(leftRect.Right, leftRect.Bottom, 0), leftColor, new Vector2(r, b)));
            indices[11] = AddVertex(new VertexPositionColorTexture(new Vector3(leftRect.Left, leftRect.Bottom, 0), leftColor, new Vector2(l, b)));

            textureRect = new Rectangle(0, 0, 80, 80);
            textureRect.Offset(BAT_RIGHT_OFFSET_X, BAT_RIGHT_OFFSET_Y);
            Rectangle rightRect = collisionRect;
            rightRect.Offset(collisionRect.Width - 80 , 0);
            rightRect.Width = 80;

            l = textureRect.Left / (float)SPRITESHEET_SIZE_X;
            r = textureRect.Right / (float)SPRITESHEET_SIZE_X;

            t = textureRect.Top / (float)SPRITESHEET_SIZE_Y;
            b = textureRect.Bottom / (float)SPRITESHEET_SIZE_Y;
                       
            indices[12] = AddVertex(new VertexPositionColorTexture(new Vector3(rightRect.Left, rightRect.Top, 0), rightColor, new Vector2(l, t)));
            indices[13] = AddVertex(new VertexPositionColorTexture(new Vector3(rightRect.Right, rightRect.Top, 0), rightColor, new Vector2(r, t)));
            indices[14] = AddVertex(new VertexPositionColorTexture(new Vector3(rightRect.Left, rightRect.Bottom, 0), rightColor, new Vector2(l, b)));

            indices[15] = AddVertex(new VertexPositionColorTexture(new Vector3(rightRect.Right, rightRect.Top, 0), rightColor, new Vector2(r, t)));
            indices[16] = AddVertex(new VertexPositionColorTexture(new Vector3(rightRect.Right, rightRect.Bottom, 0), rightColor, new Vector2(r, b)));
            indices[17] = AddVertex(new VertexPositionColorTexture(new Vector3(rightRect.Left, rightRect.Bottom, 0), rightColor, new Vector2(l, b)));

        }
        public void AddIndicesToList()
        {
            if (indices != null)
            {
                dynamicIndexList.AddRange(indices);
            }
        }

        protected static short AddVertex(VertexPositionColorTexture vertex)
        {
            for (int counter = 0; counter < dynamicVertexList.Count; counter++)
            {
                if (dynamicVertexList[counter].Equals(vertex))
                {
                    return (short)counter;
                }
            }
            dynamicVertexList.Add(vertex);
            return (short)(dynamicVertexList.Count - 1);
        }

        public void Update()
        {
            expandAcceleration = (targetExpand - expand);
            expandVelocity += expandAcceleration * 0.009f;
            expandVelocity *= 0.997f;
            expand += expandVelocity * 0.01f;
            collisionRect.X = 0;
            collisionRect.Y = 0;
            collisionRect.Width = 300 + (int)expand;
            collisionRect.Height = 80 - (int)(expandAcceleration * 0.1f);            

            velocity = targetPosition - position;
            if (!velocity.Equals(new Vector2(0, 0)))
            {
                velocity.Normalize();
            }
            if ((targetPosition - position).Length() > 5.0f)
            {
                velocity *= 5.0f;
            }

            position += velocity;            

            if (position.X < 7+collisionRect.Width/2)
            {
                position.X = 7 + collisionRect.Width / 2;               
            }
            else if (position.X > (1080-7) - collisionRect.Width / 2)
            {
                position.X = (1080-7) - collisionRect.Width / 2;
            }            
            collisionRect.Offset(position.X - collisionRect.Width / 2.0f, position.Y);
        }

        public void DrawDirectShadow(SpriteBatch spriteBatch, Vector2 offset, Texture2D texture)
        {
            spriteBatch.Draw(texture, new Rectangle((int)(collisionRect.X+offset.X)+80, (int)(collisionRect.Y+offset.Y), collisionRect.Width-160, collisionRect.Height), middleTextureRect, shadowColor);
            spriteBatch.Draw(texture, new Rectangle((int)(collisionRect.X + offset.X), (int)(collisionRect.Y + offset.Y), 80, collisionRect.Height), leftTextureRect, shadowColor);
            spriteBatch.Draw(texture, new Rectangle((int)(collisionRect.Right-80 + offset.X), (int)(collisionRect.Y + offset.Y), 80, collisionRect.Height), rightTextureRect, shadowColor);

        } 
    }
}