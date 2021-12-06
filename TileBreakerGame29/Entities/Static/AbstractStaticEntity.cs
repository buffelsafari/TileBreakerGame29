using System;
using System.Collections.Generic;
using System.IO;
using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileBreakerGame29;
using TileBreakerGame29.Entities.Dynamic.PowerUps;
using TileBreakerGame29.Entities.Static.Effects;
using Math = System.Math;

namespace BreakOut.Entities.Static
{    
    abstract class AbstractStaticEntity 
    {
        public const int SPRITESHEET_SIZE_X = 640;
        public const int SPRITESHEET_SIZE_Y = 640;

        public Rectangle collisionRect;
        public Rectangle textureRect;
        public Vector2 position;
        public Color color;        
        
        public StaticEntityFactory.StaticEntityShape shape;
        public EffectFactory.EffectType effectType;
        public EffectFactory.ColorType colorType;

        public ColorFunctions.ColorFunctionDelegate colorFunction;
        public ColorFunctions.ColorFunctionDelegate runeColorFunction;
        
        public List<EffectFactory.EffectFunctionDelegate> onHitList = new List<EffectFactory.EffectFunctionDelegate>();
        public List<EffectFactory.EffectFunctionDelegate> onKillList = new List<EffectFactory.EffectFunctionDelegate>();

        public SoundId soundId = SoundId.ballToWall;

        public bool isKillable=true;
        public int hp = 3;
        public int score = 5;
        public bool isAlive = true;

        public int collisionBitRegionX;
        public int collisionBitRegionY;

        static public VertexBuffer vertexBuffer;
        static public List<VertexPositionColorTexture> vertexList = new List<VertexPositionColorTexture>();
        static public IndexBuffer indexBuffer;
        static public List<short> indexList = new List<short>();

        public float fingerDelta; // to help picking in editor

        protected short[] indices;    
        public List<LineSegment> segments=new List<LineSegment>();

        public bool showRune = false;
        public Rectangle runeTextureRect;
        public Rectangle runeRect;
        public PowerUp powerUp;
        protected Color directShadowColor = new Color(0, 0, 0, 0.1f);
                
        public abstract void DrawDirectShadow(SpriteBatch spriteBatch, Vector2 offset, Texture2D texture);
        

        protected void MakeCollisionBitRegions()
        {
            collisionBitRegionX = 0;
            collisionBitRegionY = 0;
            int step = 135;
            int margin = 40;
            for (int counterY = 0; counterY < 15; counterY++)
            {
                for (int counterX = 0; counterX < 8; counterX++)
                {
                    Rectangle rectum = new Rectangle(step * (counterX ) - margin, step * (counterY ) - margin, step + margin * 2, step + margin * 2);
                    if (rectum.Intersects(collisionRect))
                    {
                        collisionBitRegionX |= (1 << counterX);
                        collisionBitRegionY |= (1 << counterY);                        
                    }
                }                
            }
        }

        public Rectangle GetCollisionRectangle()
        {
            return collisionRect;
        }

        public static void InitBuffers(GraphicsDevice graphics)
        {
            vertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColorTexture), vertexList.Count, BufferUsage.WriteOnly); 
            vertexBuffer.SetData<VertexPositionColorTexture>(vertexList.ToArray());

            indexBuffer = new IndexBuffer(graphics, typeof(short), indexList.Count, BufferUsage.WriteOnly);
            indexBuffer.SetData<short>(indexList.ToArray());
        }

        public void AddIndicesToList()
        {
            if (indices != null)
            {
                indexList.AddRange(indices);
            }            
        }

        protected static short AddVertex(VertexPositionColorTexture vertex)
        {
            for (int counter=0;counter<vertexList.Count;counter++)
            {
                if (vertexList[counter].Equals(vertex))
                {                    
                    return (short)counter;
                }
                
            }            
            vertexList.Add(vertex);
            return (short)(vertexList.Count-1);            
        }

        public virtual void AddVerticesToList()
        {           
            indices = new short[6];
            float l = textureRect.Left / (float)SPRITESHEET_SIZE_X;
            float r = textureRect.Right / (float)SPRITESHEET_SIZE_X;

            float t = textureRect.Top / (float)SPRITESHEET_SIZE_Y;
            float b = textureRect.Bottom / (float)SPRITESHEET_SIZE_Y;

            indices[0] = AddVertex(new VertexPositionColorTexture(new Vector3(collisionRect.Left, collisionRect.Top, 0), colorFunction(color, new Vector2(collisionRect.Left, collisionRect.Top)), new Vector2(l, t)));
            indices[1] = AddVertex(new VertexPositionColorTexture(new Vector3(collisionRect.Right, collisionRect.Top, 0), colorFunction(color, new Vector2(collisionRect.Right, collisionRect.Top)), new Vector2(r, t)));
            indices[2] = AddVertex(new VertexPositionColorTexture(new Vector3(collisionRect.Left, collisionRect.Bottom, 0), colorFunction(color, new Vector2(collisionRect.Left, collisionRect.Bottom)), new Vector2(l, b)));

            indices[3] = AddVertex(new VertexPositionColorTexture(new Vector3(collisionRect.Right, collisionRect.Top, 0), colorFunction(color, new Vector2(collisionRect.Right, collisionRect.Top)), new Vector2(r, t)));
            indices[4] = AddVertex(new VertexPositionColorTexture(new Vector3(collisionRect.Right, collisionRect.Bottom, 0), colorFunction(color, new Vector2(collisionRect.Right, collisionRect.Bottom)), new Vector2(r, b)));
            indices[5] = AddVertex(new VertexPositionColorTexture(new Vector3(collisionRect.Left, collisionRect.Bottom, 0), colorFunction(color, new Vector2(collisionRect.Left, collisionRect.Bottom)), new Vector2(l, b)));

            color= colorFunction(color, new Vector2(collisionRect.Center.X, collisionRect.Center.Y));
        }

        public void MakeCollisionRectangle()
        {            
            int left = Int32.MaxValue;
            int right = Int32.MinValue;

            int top = Int32.MaxValue;
            int bottom = Int32.MinValue;

            foreach (LineSegment seg in segments)
            {
                left = Math.Min(seg.box.Left, left);
                right = Math.Max(seg.box.Right, right);

                top = Math.Min(seg.box.Top, top);
                bottom = Math.Max(seg.box.Bottom, bottom);
            }
            collisionRect = new Rectangle(left, top, right-left, bottom-top);
        }

        public void DrawRune(SpriteBatch spriteBatch, Texture2D texture, int time)
        {
            if (showRune)
            {                
                spriteBatch.Draw(texture, runeRect, runeTextureRect, runeColorFunction(color, collisionRect.Center.ToVector2(),time));
            }
        }

        public void DrawRune(SpriteBatch spriteBatch, Texture2D texture, int yOffset, int time)
        {
            if (showRune)
            {                
                spriteBatch.Draw(texture, new Rectangle(runeRect.X, runeRect.Y+yOffset, runeRect.Width, runeRect.Height), runeTextureRect, runeColorFunction(color, collisionRect.Center.ToVector2(), time));
            }
        }

        public void Save(BinaryWriter binaryWriter)
        {
            binaryWriter.Write((Int16)position.X);
            binaryWriter.Write((Int16)position.Y);

            binaryWriter.Write((byte)color.R);
            binaryWriter.Write((byte)color.G);
            binaryWriter.Write((byte)color.B);
            binaryWriter.Write((byte)color.A);

            binaryWriter.Write((byte)hp);

            binaryWriter.Write((Int16)shape);
            binaryWriter.Write((Int16)effectType);
            binaryWriter.Write((Int16)colorType);

            binaryWriter.Write((int)0);  // extra pading for unforseen stuff
            binaryWriter.Write((int)0);
            binaryWriter.Write((int)0);
        }
    }
}