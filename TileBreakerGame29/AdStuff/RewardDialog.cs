using System;
using BreakOut;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace TileBreakerGame29.AdStuff
{
    class RewardDialog
    {
        private bool btnLock;
        private Vector2 touchPosition;
        private Rectangle yesRectangle;
        private Rectangle noRectangle;
        private Vector2 qPosition;
        private Vector2 yesPosition;
        private Vector2 noPosition;
        private string qString;
        private string yesString;
        private string noString;

        private Rectangle textureRect;
        private Rectangle heartRect;
        private Rectangle buttonFillRect;
        private Rectangle textureRect2;

        private Color buttonFillColor;

        public int selection { get; set; }

        public RewardDialog()
        { 
            qString= Game.Activity.Resources.GetString(Resource.String.RewardQ);
            yesString = Game.Activity.Resources.GetString(Resource.String.Yes);
            noString= Game.Activity.Resources.GetString(Resource.String.No);
            btnLock = false;
            
        }
        public void Init()
        { 
            touchPosition = new Vector2();
            selection = 0;
            Vector2 qSize= Globals.smallFont.MeasureString(qString); 
            Vector2 yesSize = Globals.bigFont.MeasureString(yesString);
            Vector2 noSize = Globals.bigFont.MeasureString(noString);
            
            Vector2 size = new Vector2(Math.Max(yesSize.X, noSize.X)+10, Math.Max(yesSize.Y, noSize.Y));

            yesRectangle = new Rectangle(540-(int)size.X-32, 500, (int)size.X, (int)size.Y);
            noRectangle = new Rectangle(540+32, 500, (int)size.X, (int)size.Y);

            qPosition = new Vector2(540-qSize.X/2, 500-200);

            yesPosition = new Vector2(yesRectangle.Center.X-yesSize.X/2, yesRectangle.Center.Y-yesSize.Y/2);
            noPosition = new Vector2(noRectangle.Center.X-noSize.X/2, noRectangle.Center.Y-noSize.Y/2);

            heartRect = new Rectangle(540 - 40, 400, 80, 80);
            textureRect = new Rectangle(80 * 0, 80 * 6, 80, 80);
            
            textureRect2=new Rectangle(2, 2, 160, 80);
            buttonFillRect = new Rectangle();

            buttonFillColor = new Color(1.0f, 0.5f, 0, 0.2f);

        }

        public void FlushTouch()
        {
            while (TouchPanel.IsGestureAvailable)
            {
                TouchPanel.ReadGesture();
            }
        }

        public void Update()
        {
            buttonFillRect = new Rectangle();
            TouchCollection tc = TouchPanel.GetState();

            if (tc.Count > 0)
            {
                touchPosition = tc[0].Position;
                touchPosition.X *= (1080.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
                touchPosition.Y *= (1920.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

                if (!btnLock && yesRectangle.Contains(touchPosition))
                {
                    buttonFillRect = yesRectangle;

                }
                else if (!btnLock && noRectangle.Contains(touchPosition))
                {
                    buttonFillRect = noRectangle;

                }
                else
                {
                    btnLock = true;                    
                }

            }
            else
            {
                btnLock = false;
            }

            GestureSample gesture = default(GestureSample);

            while (TouchPanel.IsGestureAvailable)
            {                
                gesture = TouchPanel.ReadGesture();

                if (gesture.GestureType == GestureType.Tap)
                {
                    touchPosition = gesture.Position;
                    touchPosition.X *= (1080.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
                    touchPosition.Y *= (1920.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

                    if (yesRectangle.Contains(touchPosition))
                    {
                        selection = 1;                        
                    }

                    if (noRectangle.Contains(touchPosition))
                    {
                        selection = 2;                        
                    }
                    
                }

            }
            
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, heartRect, textureRect, Color.White);

            spriteBatch.DrawString(Globals.smallFont, qString, qPosition, Color.Yellow);
            spriteBatch.DrawString(Globals.bigFont, yesString, yesPosition, Color.Yellow);
            spriteBatch.DrawString(Globals.bigFont, noString, noPosition, Color.Yellow);

            spriteBatch.Draw(texture, buttonFillRect, textureRect2, buttonFillColor);

            spriteBatch.DrawRectangle(yesRectangle, Color.Yellow, 5);
            spriteBatch.DrawRectangle(noRectangle, Color.Yellow, 5);

        }
    }
}