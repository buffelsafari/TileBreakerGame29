using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


using BreakOut;
using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBreakerGame29.Editor
{
    class EditorButton
    {
        private Rectangle textureRect;
        private Rectangle rect;
        private Rectangle drawRect;
        private string str;
        private Vector2 textPosition;
        
        public EditorButton(Rectangle rect, string str, int yOffset)
        {
            this.rect = rect;
            this.str = str;
            drawRect = new Rectangle(rect.Left, rect.Top + yOffset, rect.Width, rect.Height);                        
            Vector2 size=Globals.bigFont.MeasureString(str);
            textPosition = new Vector2(rect.Center.X-size.X/2, rect.Center.Y-size.Y/2 +yOffset);
            textureRect = new Rectangle(2, 2, 160, 80);
        }

        public bool OnTap(Vector2 tapPosition)
        {
            if (rect.Contains(tapPosition))
            {
                SoundManager.Play(SoundId.tilehit);
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, drawRect, textureRect, Color.Beige);
            spriteBatch.DrawString(Globals.bigFont, str, textPosition, Color.Black);
        }
    }
}