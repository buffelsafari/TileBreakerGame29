using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BreakOut.Menu
{
    class HighScore:IMenuItem
    {
        private Game game;
        private Rectangle rectangle;
        private string str;
        private string highScoreStr;
        private Vector2 textPosition;
        private RenderTarget2D normalTexture;
        private RenderTarget2D diffuseTexture;
        
        private int score  = 0;
        private float progress  = 0;

        private Color miniTextColor;

        public HighScore(Game game, Rectangle rectangle, string highScoreStr)
        {
            this.highScoreStr = highScoreStr;
            this.game = game;
            this.rectangle = rectangle;            
            str = score.ToString();
            Vector2 textSize = Globals.bigFont.MeasureString(str);
            textPosition = new Vector2(rectangle.Width / 2 - textSize.X / 2.0f, rectangle.Height / 2 - textSize.Y / 2);
            miniTextColor = new Color(0.4f, 0.2f, 0, 0.5f);
        }

        public Rectangle GetRectangle()
        {
            return rectangle;
        }

        public void Update(int score, float progress)
        {
            this.score = score;
            this.progress = progress;
            
            str = score.ToString();
            Vector2 textSize = Globals.bigFont.MeasureString(str);
            textPosition = new Vector2(rectangle.Width / 2 - textSize.X / 2.0f, rectangle.Height / 2 - textSize.Y / 2);

        }      

        public MenuPage GetChildPage()
        {
            return null;            
        }

        public void GenerateDiffuseTexture(SpriteBatch spriteBatch)
        {            
            diffuseTexture = new RenderTarget2D(game.GraphicsDevice, rectangle.Width, rectangle.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            game.GraphicsDevice.SetRenderTarget(diffuseTexture);
            game.GraphicsDevice.Clear(Color.White);
            
            spriteBatch.Begin();
            
            Vector2 start = new Vector2(5, rectangle.Height - 15);
            Vector2 stop = new Vector2(rectangle.Width - 5, rectangle.Height - 15);

            float length=Vector2.Distance(start, stop);
            Vector2 progressStop = new Vector2(start.X+length*progress, start.Y);

            spriteBatch.DrawLine(start, stop, Color.Black, 20);
            spriteBatch.DrawLine(start, progressStop, Color.Green, 18);
            spriteBatch.End();
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
            spriteBatch.Draw(diffuseTexture, rectangle, Color.Gold);
            spriteBatch.DrawString(Globals.bigFont, str, new Vector2(textPosition.X+rectangle.Left, textPosition.Y+rectangle.Top), Color.CornflowerBlue);
            spriteBatch.DrawString(Globals.smallFont, highScoreStr, new Vector2(rectangle.Left, rectangle.Top), miniTextColor);
        }

        public void DisposeTextures()
        {
            diffuseTexture?.Dispose();
            normalTexture?.Dispose();            
        }

        public void UpdateTouch(TouchLocation location)
        {
            
        }

        public void UpdateGesture(GestureSample gesture)
        {
            
        }
    }
}