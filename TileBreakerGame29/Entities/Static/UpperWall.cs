using BreakOut.Entities.Static;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BreakOut
{
    class UpperWall : AbstractStaticEntity
    {
        const int UPPER_WALL_OFFSET_X = 222;
        const int UPPER_WALL_OFFSET_Y = 359;

        public UpperWall(Color color)
        { 
            isKillable = false;
           
            for (int xc = 0; xc < 13; xc++)
            {
                int x = xc * 82 + 7;
                Vector2 point1 = new Vector2(82 + x, 24 + 7);
                Vector2 point2 = new Vector2(41 + x, 0 + 7);
                Vector2 point3 = new Vector2(0 + x, 24 + 7);            

                segments.Add(new LineSegment(point1, point2));
                segments.Add(new LineSegment(point2, point3));
            }

            MakeCollisionRectangle();
            MakeCollisionBitRegions();

            textureRect = new Rectangle(0, 0, 82, 32);
            textureRect.Offset(UPPER_WALL_OFFSET_X, UPPER_WALL_OFFSET_Y);
            this.color = color;
            isKillable = false;

        }       

        public void EditorDraw(SpriteBatch spriteBatch, Vector2 offset, Texture2D texture, Effect effect)
        {
            effect.Parameters["CenterTexCoord"].SetValue(new Vector2(textureRect.Center.X / 640.0f, (textureRect.Center.Y / 640.0f)));

            Rectangle rectum = new Rectangle(collisionRect.X + (int)offset.X , (int)offset.Y, 82, 32);
            for (int i = 0; i < 13; i++)
            {
                effect.Parameters["TopLeftColor"].SetValue(colorFunction(color, new Vector2(rectum.Left, rectum.Top - offset.Y)).ToVector4());
                effect.Parameters["TopRightColor"].SetValue(colorFunction(color, new Vector2(rectum.Right, rectum.Top - offset.Y)).ToVector4());
                effect.Parameters["BottomLeftColor"].SetValue(colorFunction(color, new Vector2(rectum.Left, rectum.Bottom - offset.Y)).ToVector4());
                effect.Parameters["BottomRightColor"].SetValue(colorFunction(color, new Vector2(rectum.Right, rectum.Bottom - offset.Y)).ToVector4());

                spriteBatch.Draw(texture, rectum, textureRect, color);
                rectum.Offset(82,0);                
            }
        }

        public override void AddVerticesToList()
        {
            indices = new short[6*13];
            float l = textureRect.Left / (float)SPRITESHEET_SIZE_X;
            float r = textureRect.Right / (float)SPRITESHEET_SIZE_X;

            float t = textureRect.Top / (float)SPRITESHEET_SIZE_Y;
            float b = textureRect.Bottom / (float)SPRITESHEET_SIZE_Y;

            for (int counter = 0; counter < 13; counter++)
            {
                indices[counter * 6 + 0] = AddVertex(new VertexPositionColorTexture(new Vector3(counter * 82 + 7, 0, 0), colorFunction(this.color, new Vector2(counter * 82 + 7, 0)), new Vector2(l, t)));
                indices[counter * 6 + 1] = AddVertex(new VertexPositionColorTexture(new Vector3(counter * 82 + 7 + 82, 0, 0), colorFunction(this.color, new Vector2(counter * 82 + 7+82,0)), new Vector2(r, t)));
                indices[counter * 6 + 2] = AddVertex(new VertexPositionColorTexture(new Vector3(counter * 82 + 7, 32, 0), colorFunction(this.color, new Vector2(counter * 82 + 7,32)), new Vector2(l, b)));

                indices[counter * 6 + 3] = AddVertex(new VertexPositionColorTexture(new Vector3(counter * 82 + 7 + 82, 0, 0), colorFunction(this.color, new Vector2(counter * 82 + 7+82,0)), new Vector2(r, t)));
                indices[counter * 6 + 4] = AddVertex(new VertexPositionColorTexture(new Vector3(counter * 82 + 7 + 82, 32, 0), colorFunction(this.color, new Vector2(counter * 82 + 7+82,32)), new Vector2(r, b)));
                indices[counter * 6 + 5] = AddVertex(new VertexPositionColorTexture(new Vector3(counter * 82 + 7, 32, 0), colorFunction(this.color, new Vector2(counter * 82 + 7,32)), new Vector2(l, b)));
            }
        }

        public override void DrawDirectShadow(SpriteBatch spriteBatch, Vector2 offset, Texture2D texture)
        {
            Rectangle rectum = new Rectangle(collisionRect.X + (int)offset.X, (int)offset.Y, 82, 32);
            for (int i = 0; i < 13; i++)
            {
                spriteBatch.Draw(texture, rectum, textureRect, directShadowColor);                
                rectum.Offset(82, 0);                
            }
            Rectangle rectum2 = new Rectangle(collisionRect.X + (int)offset.X, (int)offset.Y-80, 1080, 80);
            spriteBatch.Draw(texture, rectum2, new Rectangle(2, 2, 160, 80), directShadowColor);
        }
    }
}