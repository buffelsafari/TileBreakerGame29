using BreakOut;
using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBreakerGame29.Editor
{
    class ListSelector
    {
        private Rectangle textureRect = new Rectangle(2, 2, 160, 80);
        private Rectangle leftTextureRect = new Rectangle(166, 2, 80, 80);
        private Rectangle rightTextureRect = new Rectangle(249, 2, 80, 80);

        private Rectangle forwardRectangle;
        private Rectangle backwardRectangle;
        private Rectangle middleRectangle;

        private Rectangle forwardDrawRectangle;
        private Rectangle backwardDrawRectangle;
        private Rectangle middleDrawRectangle;

        private Vector2 textPosition;

        private int yOffset;
        private int index;        
        private string[] names;

        public ListSelector(string[] names, Rectangle rect, int yOffset)
        {
            this.yOffset = yOffset;
            this.names = names;
            index = 0;
            forwardRectangle = new Rectangle(rect.Right-rect.Height, rect.Top , rect.Height, rect.Height);
            backwardRectangle = new Rectangle(rect.Left,rect.Top, rect.Height, rect.Height);
            middleRectangle = new Rectangle(rect.Left+rect.Height,rect.Top  ,rect.Width-rect.Height*2, rect.Height);


            forwardDrawRectangle = new Rectangle(forwardRectangle.Left, forwardRectangle.Top + yOffset, forwardRectangle.Width, forwardRectangle.Height);
            backwardDrawRectangle = new Rectangle(backwardRectangle.Left, backwardRectangle.Top + yOffset, backwardRectangle.Width + 2, backwardRectangle.Height);
            middleDrawRectangle = new Rectangle(middleRectangle.Left, middleRectangle.Top + yOffset, middleRectangle.Width, middleRectangle.Height);

            SetTextPosition();
        }
        public bool OnTap(Vector2 tapPosition)
        {
            bool retValue = false;            
            if (forwardRectangle.Contains(tapPosition))
            {                
                retValue = true;
                index++;
            }
            if (backwardRectangle.Contains(tapPosition))
            {                
                retValue = true;
                index--;
            }

            if (index >= names.Length)
            {
                index = names.Length - 1;
            }
            else if (index < 0)
            {
                index = 0;
            }
            else if (retValue)
            { 
                SoundManager.Play(SoundId.tap);
            }
            
            SetTextPosition();
            return retValue;
        }

        public int GetIndex()
        {
            return index;
        }

        private void SetTextPosition()
        {
            Vector2 textSize = Globals.smallFont.MeasureString(names[index]);
            textPosition = middleRectangle.Center.ToVector2();
            textPosition -= textSize / 2;
            textPosition.Y += yOffset;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, forwardDrawRectangle,rightTextureRect, Color.Red);
            spriteBatch.Draw(texture, backwardDrawRectangle, leftTextureRect, Color.Red);
            spriteBatch.Draw(texture, middleDrawRectangle,textureRect, Color.Yellow);
            spriteBatch.DrawString(Globals.smallFont, names[index], textPosition, Color.Black);
        }
    }
}