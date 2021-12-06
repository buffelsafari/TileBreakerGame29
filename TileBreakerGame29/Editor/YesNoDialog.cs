using BreakOut;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBreakerGame29.Editor
{
    class YesNoDialog
    {
        public bool isActive=false;
        private Rectangle textureRect;
        private Rectangle rect;        
        private string str;
        private Vector2 textPosition;        

        private EditorButton yesButton;
        private EditorButton noButton;

        private Rectangle fullScreenRect;
        private Color fullScreenColor;
        
        public YesNoDialog(string str, int yOffset)
        {            
            this.str = str;
            rect = new Rectangle(540 - 450, 540 - 400+yOffset, 900, 600);                                
            Vector2 size=Globals.bigFont.MeasureString(str);
            textPosition = new Vector2(rect.Center.X-size.X/2, rect.Center.Y-150-size.Y/2);
            textureRect = new Rectangle(2, 2, 160, 80);
            yesButton = new EditorButton(new Rectangle(200,rect.Center.Y+50-yOffset,250,200), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Yes), yOffset);
            noButton = new EditorButton(new Rectangle(640, rect.Center.Y+50-yOffset,250, 200), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.No), yOffset);

            fullScreenRect = new Rectangle(0, 0, 1080, 1920);
            fullScreenColor = new Color(0, 0, 0.1f, 0.7f);
        }

        public int OnTap(Vector2 tapPosition)
        {
            if(yesButton.OnTap(tapPosition))
            {
                return 1;
            }
            if (noButton.OnTap(tapPosition))
            {
                return 2;
            }
            return 0;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, fullScreenRect, textureRect, fullScreenColor);
            spriteBatch.Draw(texture, rect,textureRect ,Color.Beige);
            spriteBatch.DrawString(Globals.bigFont, str, textPosition, Color.Black);
            
            yesButton.Draw(spriteBatch, texture);
            noButton.Draw(spriteBatch, texture);            
        }
    }
}