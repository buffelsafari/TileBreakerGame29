using System;
using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BreakOut.Menu
{
    class Button:IMenuItem
    {
        private Game game;
        private Rectangle rectangle;
        private string str;
        private Vector2 textPosition;

        private event EventHandler onTap;
        private event EventHandler onTouch;
        private MenuPage childPage;
        private RenderTarget2D normalTexture;
        private RenderTarget2D diffuseTexture;
        private Color textColor;

        public bool isGreyed { get; set; } = false;

        public Button(Game game, Rectangle rectangle, string str, EventHandler onTap, EventHandler onTouch, MenuPage childPage)
        {
            this.game = game;
            this.rectangle = rectangle;
            this.str = str;
            this.onTap = onTap;
            this.onTouch = onTouch;
            this.childPage = childPage;
            Vector2 textSize = Globals.bigFont.MeasureString(str);
            textPosition = new Vector2(rectangle.Width/2-textSize.X/2.0f, rectangle.Height/2-textSize.Y/2);
            textColor=new Color(1.0f, 0.8f, 0.2f, 1.0f);


        }

        public Rectangle GetRectangle()
        {
            return rectangle;
        }
        public void UpdateTouch(TouchLocation location)
        {
            if (rectangle.Contains(ConvertToGameSpace(location.Position)))
            {
                onTouch?.Invoke(this, new EventArgs());                
            }            
        }

        public void UpdateGesture(GestureSample gesture)
        {
            if (!isGreyed && gesture.GestureType == GestureType.Tap)
            {
                if (rectangle.Contains(ConvertToGameSpace(gesture.Position)))
                {
                    SoundManager.Play(SoundId.tap);
                    onTap?.Invoke(this, new EventArgs());
                }
            }            
        }

        private Vector2 ConvertToGameSpace(Vector2 position)
        {
            position.X *= (1080.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
            position.Y *=  (1920.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            return position;
        }

        public MenuPage GetChildPage()
        {
            return childPage;            
        }

        public void GenerateDiffuseTexture(SpriteBatch spriteBatch)
        {            
            diffuseTexture = new RenderTarget2D(game.GraphicsDevice, rectangle.Width, rectangle.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            game.GraphicsDevice.SetRenderTarget(diffuseTexture);
            game.GraphicsDevice.Clear(Color.White);
        }

        public void GenerateNormalTexture(SpriteBatch spriteBatch)
        {
            normalTexture = new RenderTarget2D(game.GraphicsDevice, rectangle.Width, rectangle.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);            
            game.GraphicsDevice.SetRenderTarget(normalTexture);            
            spriteBatch.Begin();
            game.GraphicsDevice.Clear(new Color(0.5f, 0.5f, 1.0f, 1.0f));
            
            for (int counter = 0; counter < 4; counter++)
            {
                spriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Top + counter), new Vector2(rectangle.Right, rectangle.Top + counter), new Color(0.5f, counter * 0.1f, 1.0f, 1.0f));
                spriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Bottom - counter), new Vector2(rectangle.Right, rectangle.Bottom - counter), new Color(0.5f, 1.0f - counter * 0.1f, 1.0f, 1.0f));

                spriteBatch.DrawLine(new Vector2(rectangle.Left + counter, rectangle.Top + counter), new Vector2(rectangle.Left + counter, rectangle.Bottom - counter), new Color(counter * 0.1f, 0.5f, 1.0f, 1.0f));
                spriteBatch.DrawLine(new Vector2(rectangle.Right - counter, rectangle.Top + counter), new Vector2(rectangle.Right - counter, rectangle.Bottom - counter), new Color(1.0f - counter * 0.1f, 0.5f, 1.0f, 1.0f));
            }

            for (double angle = 0; angle < 2 * Math.PI; angle += 0.1f)
            {
                float R = ((float)Math.Cos(angle) + 1) / 2.0f;
                float G = ((float)Math.Sin(angle) + 1) / 2.0f;
                spriteBatch.DrawString(Globals.bigFont, str, new Vector2(textPosition.X + (float)Math.Cos(angle) * 3.0f, textPosition.Y + (float)Math.Sin(angle) * 3.0f), new Color(R, G, 0.5f, 1.0f));
            }
            spriteBatch.DrawString(Globals.bigFont, str, textPosition, new Color(0.5f, 0.5f, 1.0f, 1.0f));                       
            spriteBatch.End();
        }

        public void DrawNormal(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(normalTexture, rectangle, Color.White);            
        }

        public void DrawDiffuse(SpriteBatch spriteBatch)
        {
            if (isGreyed)
            {
                spriteBatch.Draw(diffuseTexture, rectangle, Color.DimGray);
            }
            else
            {
                spriteBatch.Draw(diffuseTexture, rectangle, Color.White);
            }

            if (!isGreyed)
            {
                spriteBatch.DrawString(Globals.bigFont, str, new Vector2(textPosition.X+rectangle.Left, textPosition.Y+rectangle.Top), textColor);
            }
        }

        public void DisposeTextures()
        {
            diffuseTexture?.Dispose();
            normalTexture?.Dispose();
            
        }
    }
}