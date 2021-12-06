using BreakOut.Entities.Static;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BreakOut
{
    class LeftWall : AbstractStaticEntity
    {
        const int LEFT_UPPER_WALL_OFFSET_X = 437;
        const int LEFT_UPPER_WALL_OFFSET_Y = 156;
        const int LEFT_LOWER_WALL_OFFSET_X = 521;  
        const int LEFT_LOWER_WALL_OFFSET_Y = 2;
        
        
        public LeftWall(Color color)
        {
            isKillable = false;            
            int sign = 1;
            Vector2 offset = new Vector2(7,7);
            int height = 5;
            Vector2 point6=new Vector2(0,0);
            for (int y = 0; y < height; y++)
            {
                Vector2 point1 = new Vector2(offset.X,              24 + y * 144 + offset.Y);
                Vector2 point2 = new Vector2(offset.X,              72 + y * 144 + offset.Y);
                Vector2 point3 = new Vector2(offset.X + sign * 41,  96 + y * 144 + offset.Y);
                Vector2 point4 = new Vector2(offset.X + sign * 41,  144 + y * 144 + offset.Y);
                Vector2 point5 = new Vector2(offset.X,              168 + y * 144 + offset.Y);          

                segments.Add(new LineSegment(point1, point2));
                segments.Add(new LineSegment(point2, point3));
                segments.Add(new LineSegment(point3, point4));
                segments.Add(new LineSegment(point4, point5));

                point6 = point5;
            }

            Vector2 point7 = new Vector2(point6.X, 1920);
            segments.Add(new LineSegment(point6, point7));

            MakeCollisionRectangle();
            MakeCollisionBitRegions();

            textureRect = new Rectangle(0, 0, 48, 144);
            textureRect.Offset(LEFT_UPPER_WALL_OFFSET_X, LEFT_UPPER_WALL_OFFSET_Y);

            this.color = color;
            isKillable = false;
        }        
                
        public void EditorDraw(SpriteBatch spriteBatch, Vector2 offset, Texture2D texture, Effect effect)
        {
            effect.Parameters["CenterTexCoord"].SetValue(new Vector2(textureRect.Center.X / 640.0f, (textureRect.Center.Y / 640.0f)));

            Rectangle rectum = new Rectangle(0 + (int)offset.X, 79 + (int)offset.Y - 144, 48, 144);
            for (int i = 0; i < 6; i++)
            {
                effect.Parameters["TopLeftColor"].SetValue(colorFunction(color, new Vector2(rectum.Left, rectum.Top - offset.Y)).ToVector4());
                effect.Parameters["TopRightColor"].SetValue(colorFunction(color, new Vector2(rectum.Right, rectum.Top - offset.Y)).ToVector4());
                effect.Parameters["BottomLeftColor"].SetValue(colorFunction(color, new Vector2(rectum.Left, rectum.Bottom - offset.Y)).ToVector4());
                effect.Parameters["BottomRightColor"].SetValue(colorFunction(color, new Vector2(rectum.Right, rectum.Bottom - offset.Y)).ToVector4());

                spriteBatch.Draw(texture, rectum, textureRect, color);
                rectum.Offset(0, 144);
                
            }

            Rectangle rectum2 = new Rectangle(0 + (int)offset.X, 79 + (int)offset.Y + 144 * 5, 48, 1200);
            Rectangle tRex = new Rectangle(textureRect.Left, textureRect.Top + 96, textureRect.Width, textureRect.Height - 96);

            effect.Parameters["CenterTexCoord"].SetValue(new Vector2(tRex.Center.X / 640.0f, (tRex.Center.Y / 640.0f)));

            effect.Parameters["TopLeftColor"].SetValue(colorFunction(color, new Vector2(rectum2.Left, rectum2.Top - offset.Y)).ToVector4());
            effect.Parameters["TopRightColor"].SetValue(colorFunction(color, new Vector2(rectum2.Right, rectum2.Top - offset.Y)).ToVector4());
            effect.Parameters["BottomLeftColor"].SetValue(colorFunction(color, new Vector2(rectum2.Left, rectum2.Bottom - offset.Y)).ToVector4());
            effect.Parameters["BottomRightColor"].SetValue(colorFunction(color, new Vector2(rectum2.Right, rectum2.Bottom - offset.Y)).ToVector4());
            spriteBatch.Draw(texture, rectum2, tRex, color);
            
        }

        public override void AddVerticesToList()
        {
            textureRect = new Rectangle(0, 0, 48, 144);
            textureRect.Offset(LEFT_UPPER_WALL_OFFSET_X, LEFT_UPPER_WALL_OFFSET_Y);
            
            indices = new short[6 * 6 + 6];
            float l = textureRect.Left / (float)SPRITESHEET_SIZE_X;
            float r = textureRect.Right / (float)SPRITESHEET_SIZE_X;

            float t = textureRect.Top / (float)SPRITESHEET_SIZE_Y;
            float b = textureRect.Bottom / (float)SPRITESHEET_SIZE_Y;

            for (int counter = -1; counter < 5; counter++)
            {
                
                indices[(counter+1) * 6 + 0] = AddVertex(new VertexPositionColorTexture(new Vector3(0, counter * 144 + 80, 0), colorFunction(this.color, new Vector2(0,counter * 144 + 80)), new Vector2(l, t)));
                indices[(counter+1) * 6 + 1] = AddVertex(new VertexPositionColorTexture(new Vector3(48, counter * 144 + 80, 0), colorFunction(this.color, new Vector2(48,counter * 144 + 80)), new Vector2(r, t)));
                indices[(counter+1) * 6 + 2] = AddVertex(new VertexPositionColorTexture(new Vector3(0, counter * 144 + 80+144, 0), colorFunction(this.color, new Vector2(0,counter * 144 + 80+144)), new Vector2(l, b)));

                indices[(counter+1) * 6 + 3] = AddVertex(new VertexPositionColorTexture(new Vector3(48, counter * 144 + 80, 0), colorFunction(this.color, new Vector2(48,counter * 144 + 80)), new Vector2(r, t)));
                indices[(counter+1) * 6 + 4] = AddVertex(new VertexPositionColorTexture(new Vector3(48, counter * 144 + 80 + 144, 0), colorFunction(this.color, new Vector2(48,counter * 144 + 80+144)), new Vector2(r, b)));
                indices[(counter+1) * 6 + 5] = AddVertex(new VertexPositionColorTexture(new Vector3(0, counter * 144 + 80 + 144, 0), colorFunction(this.color, new Vector2(0,counter * 144 + 80+144)), new Vector2(l, b)));
            }

            textureRect = new Rectangle(0, 0, 10, 144); // adjuted a bit
            textureRect.Offset(LEFT_LOWER_WALL_OFFSET_X, LEFT_LOWER_WALL_OFFSET_Y);

            l = textureRect.Left / (float)SPRITESHEET_SIZE_X;
            r = textureRect.Right / (float)SPRITESHEET_SIZE_X;

            t = textureRect.Top / (float)SPRITESHEET_SIZE_Y;
            b = textureRect.Bottom / (float)SPRITESHEET_SIZE_Y;

            indices[36] = AddVertex(new VertexPositionColorTexture(new Vector3(0, 800, 0), colorFunction(this.color, new Vector2(0,800)), new Vector2(l, t)));
            indices[37] = AddVertex(new VertexPositionColorTexture(new Vector3(10, 800, 0), colorFunction(this.color, new Vector2(10, 800)), new Vector2(r, t)));
            indices[38] = AddVertex(new VertexPositionColorTexture(new Vector3(0, 1920, 0), colorFunction(this.color, new Vector2(0, 1920)), new Vector2(l, b)));

            indices[39] = AddVertex(new VertexPositionColorTexture(new Vector3(10, 800, 0), colorFunction(this.color, new Vector2(10, 800)), new Vector2(r, t)));
            indices[40] = AddVertex(new VertexPositionColorTexture(new Vector3(10, 1920, 0), colorFunction(this.color, new Vector2(10, 1920)), new Vector2(r, b)));
            indices[41] = AddVertex(new VertexPositionColorTexture(new Vector3(0, 1920, 0), colorFunction(this.color, new Vector2(0, 1920)), new Vector2(l, b)));

        }

        public override void DrawDirectShadow(SpriteBatch spriteBatch, Vector2 offset, Texture2D texture)
        {
            textureRect = new Rectangle(0, 0, 48, 144);
            textureRect.Offset(LEFT_UPPER_WALL_OFFSET_X, LEFT_UPPER_WALL_OFFSET_Y);  

            Rectangle rectum = new Rectangle(0 + (int)offset.X, 79 + (int)offset.Y - 144, 48, 144);
            for (int i = 0; i < 6; i++)
            { 
                spriteBatch.Draw(texture, rectum, textureRect, directShadowColor);
                rectum.Offset(0, 144);
            }

            Rectangle rectum2 = new Rectangle(0 + (int)offset.X, 79 + (int)offset.Y + 144 * 5, 48, 1200);
            Rectangle tRex = new Rectangle(textureRect.Left, textureRect.Top + 96, textureRect.Width, textureRect.Height - 96);

            spriteBatch.Draw(texture, rectum2, tRex, directShadowColor);

        }
    }
}