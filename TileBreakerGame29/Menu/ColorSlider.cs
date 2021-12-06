using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;


namespace BreakOut.Menu
{
    class ColorSlider:IMenuItem
    {
        private Game game;
        private Rectangle rectangle;
        private string str;
        private Vector2 textPosition;

        private event EventHandler onTap;
        private event EventHandler onTouch;               
        private RenderTarget2D normalTexture;
        private RenderTarget2D diffuseTexture;

        private Texture2D spriteTextureDiffuse;
        private Texture2D spriteTextureNormal;
        private Rectangle knobTextureRect;
        
        private int knobRadie;        

        private Rectangle[] slitRectangle;
        private Rectangle[] collisionRect;
        private Vector2[] leverPosition;

        private float[] value;
        private Color colorValue;

        public ColorSlider(Game game, Rectangle rectangle, string str, EventHandler onTap, EventHandler onTouch)
        {
            this.game = game;
            this.rectangle = rectangle;
            this.str = str;
            this.onTap = onTap;
            this.onTouch = onTouch;            

            spriteTextureDiffuse = (Texture2D)TextureContainer.GetTexture(TextureContainer.TextureReference.spriteSheetDiffuse);
            spriteTextureNormal = (Texture2D)TextureContainer.GetTexture(TextureContainer.TextureReference.spriteSheetNormal);
            knobTextureRect = new Rectangle(524, 275, 80, 80);
            knobRadie = (int)(rectangle.Height *0.11f); // sligly bigger then 0.25f

            Vector2 textSize = Globals.bigFont.MeasureString(str);            
            textPosition = new Vector2(rectangle.Width/2.0f - textSize.X / 2.0f, 0);

            slitRectangle= new Rectangle[3];
            collisionRect = new Rectangle[3];
            leverPosition = new Vector2[3];


            Rectangle rexi = new Rectangle(rectangle.Height / 8, rectangle.Height * 5 / 16, rectangle.Width - rectangle.Height / 4, rectangle.Height / 8);
            for (int j = 0; j < 3; j++)
            {
                slitRectangle[j] = rexi;
                collisionRect[j] = rexi;//--------------------------------------------
                collisionRect[j].Offset(rectangle.X, rectangle.Y);
                collisionRect[j].Inflate(rexi.Height, rexi.Height / 4);                
                rexi.Offset(0, rectangle.Height / 4);
            }

            for (int i = 0;i<3 ; i++)
            {
                leverPosition[i] = new Vector2(rectangle.Left + rectangle.Height / 8, (rectangle.Top + rectangle.Height * 3 / 8)+i*(rectangle.Height / 4));
            }
            value = new float[3];
        }

        public void UpdateTouch(TouchLocation location)
        {
            Vector2 position = ConvertToGameSpace(location.Position);
            
            for (int i = 0; i < 3; i++)
            {
                if (collisionRect[i].Contains(position))
                { 
                    if (position.X < collisionRect[i].Left + slitRectangle[i].Height)
                    {
                        position.X = collisionRect[i].Left + slitRectangle[i].Height;
                    }
                    if (position.X > collisionRect[i].Right - slitRectangle[i].Height)
                    {
                        position.X = collisionRect[i].Right - slitRectangle[i].Height;
                    }
                    leverPosition[i].X = position.X;

                    float slideLength = collisionRect[i].Width - slitRectangle[i].Height * 2;
                    float sliderStart = collisionRect[i].Left + slitRectangle[i].Height;
                    value[i] = position.X - sliderStart;
                    value[i] = value[i] / slideLength;
                }
            }
            colorValue = new Color(value[0], value[1], value[2]);
            onTouch?.Invoke(this, new EventArgs());            
        }

        public void UpdateGesture(GestureSample gesture)
        {
            if (gesture.GestureType == GestureType.Tap)
            {
                if (rectangle.Contains(ConvertToGameSpace(gesture.Position)))
                {                    
                    onTap?.Invoke(this, new EventArgs());
                }
            }            
        }

        public void SetValue(Color color)
        {
            colorValue = color;
            value[0] = color.ToVector3().X;
            value[1] = color.ToVector3().Y;
            value[2] = color.ToVector3().Z;

            for (int i = 0; i < 3; i++)
            {
                float slideLength = collisionRect[i].Width - slitRectangle[i].Height * 2;
                float sliderStart = collisionRect[i].Left + slitRectangle[i].Height;
                leverPosition[i].X = slideLength * value[i] + sliderStart;
            }            
        }

        public Color GetValue()
        {
            return colorValue;
        }

        private static Vector2 ConvertToGameSpace(Vector2 position)
        {
            position.X *= (1080.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
            position.Y *=  (1920.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            return position;
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
            spriteBatch.DrawString(Globals.bigFont, str, textPosition, new Color(1.0f, 0.5f, 0.0f, 1.0f));
                        
            for (int j = -1; j < 2; j++)
            {                
                Vector2 v1= new Vector2(slitRectangle[j + 1].Left, slitRectangle[j + 1].Top);
                Vector2 v2 = new Vector2(slitRectangle[j + 1].Left, slitRectangle[j + 1].Bottom);
                for (int i = 0; i <= slitRectangle[j + 1].Width; i++)
                {
                    float c = (i*1.0f) / (slitRectangle[j + 1].Width*1.0f);
                    spriteBatch.DrawLine(v1, v2, new Color(c*(j-1)*(j), c*(1-j)*(j+1), c*(1 + j)*(j),1.0f));  //KISS
                    v1.X++;
                    v2.X++;
                    
                }
                spriteBatch.DrawRectangle(slitRectangle[j + 1], Color.Black, 2);                
            }
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

            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(spriteTextureNormal, new Rectangle((int)leverPosition[i].X - knobRadie, (int)leverPosition[i].Y - knobRadie, knobRadie * 2, knobRadie * 2), knobTextureRect, Color.White);

            }
        }
        public void DrawDiffuse(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(diffuseTexture, rectangle, Color.White);
            
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(spriteTextureDiffuse, new Rectangle((int)leverPosition[i].X - knobRadie, (int)leverPosition[i].Y - knobRadie, knobRadie * 2, knobRadie * 2), knobTextureRect, Color.White);
            }            
        }

        public void DisposeTextures()
        {
            diffuseTexture.Dispose();
            normalTexture.Dispose();            
        }
    }
}