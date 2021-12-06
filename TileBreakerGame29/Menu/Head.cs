using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BreakOut.Menu
{
    class Head:IMenuItem
    {        
        private Rectangle rectangle;
        private string str;
        private Vector2 textPosition;
        private Vector2[] vertex;
        private Color color;

        public Head(Rectangle rectangle, string str)
        {            
            this.rectangle = rectangle;
            this.str = str;           

            Vector2 textSize = Globals.bigFont.MeasureString(str);
            textPosition = new Vector2(rectangle.Width/2-textSize.X/2.0f, 0);
            vertex = new Vector2[6];
            vertex[0] = new Vector2(rectangle.Center.X+ textSize.X/2, textSize.Y / 2+rectangle.Top);
            vertex[1] = new Vector2(rectangle.Right, textSize.Y / 2 + rectangle.Top);
            vertex[2] = new Vector2(rectangle.Right, rectangle.Bottom);
            vertex[3] = new Vector2(rectangle.Left, rectangle.Bottom);
            vertex[4] = new Vector2(rectangle.Left, textSize.Y/2 + rectangle.Top);
            vertex[5] = new Vector2(rectangle.Center.X-textSize.X/2, textSize.Y/2 + rectangle.Top);

            color = new Color(1.0f, 0.8f, 0.2f, 1.0f);
        }

        public Rectangle GetRectangle()
        {
            return rectangle;
        }

        public void UpdateTouch(TouchLocation location)
        {
                        
        }

        public void UpdateGesture(GestureSample gesture)
        {
           
        }
                
        public MenuPage GetChildPage()
        {
            return null;            
        }

        public void GenerateDiffuseTexture(SpriteBatch spriteBatch)
        {
           
        }

        public void GenerateNormalTexture(SpriteBatch spriteBatch)
        {
           
        }

        public void DrawNormal(SpriteBatch spriteBatch)
        {
           
        }

        public void DrawDiffuse(SpriteBatch spriteBatch)
        {            
            spriteBatch.DrawLine(vertex[0], vertex[1], new Color(1.0f, 0.8f, 0.2f, 1.0f), 5);
            spriteBatch.DrawLine(vertex[1], vertex[2], new Color(1.0f, 0.8f, 0.2f, 1.0f), 5);
            spriteBatch.DrawLine(vertex[2], vertex[3], new Color(1.0f, 0.8f, 0.2f, 1.0f), 5);
            spriteBatch.DrawLine(vertex[3], vertex[4], new Color(1.0f, 0.8f, 0.2f, 1.0f), 5);
            spriteBatch.DrawLine(vertex[4], vertex[5], new Color(1.0f, 0.8f, 0.2f, 1.0f), 5);            

            spriteBatch.DrawString(Globals.bigFont, str, new Vector2(textPosition.X+rectangle.Left, textPosition.Y+rectangle.Top), color);            
        }

        public void DisposeTextures()
        {
                        
        }
    }
}