using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;


namespace BreakOut.Menu
{
    class Slider:IMenuItem
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
        private Vector2 knobPosition;
        private int knobRadie;
        private Rectangle sliderRect;

        public float value { get; private set; }

        public Slider(Game game, Rectangle rectangle, string str, EventHandler onTap, EventHandler onTouch)
        {
            this.game = game;
            this.rectangle = rectangle;
            this.str = str;
            this.onTap = onTap;
            this.onTouch = onTouch;            

            spriteTextureDiffuse = (Texture2D)TextureContainer.GetTexture(TextureContainer.TextureReference.spriteSheetDiffuse);
            spriteTextureNormal = (Texture2D)TextureContainer.GetTexture(TextureContainer.TextureReference.spriteSheetNormal);
            knobTextureRect = new Rectangle(524, 275, 80, 80);
            knobRadie = (int)(rectangle.Height *0.30f); // sligly bigger then 0.25f
            Vector2 textSize = Globals.bigFont.MeasureString(str);
            textPosition = new Vector2(rectangle.Width/2.0f - textSize.X / 2.0f, 0);
            knobPosition = new Vector2(rectangle.Left+rectangle.Height*0.25f ,rectangle.Center.Y + rectangle.Height * 0.25f);
            sliderRect = new Rectangle(rectangle.Left, rectangle.Center.Y, rectangle.Width, rectangle.Height / 2);

            
        }



        public void UpdateTouch(TouchLocation location)
        {
            Vector2 position = ConvertToGameSpace(location.Position);            
            
            if (sliderRect.Contains(position))
            {                
                if (position.X < sliderRect.Left + sliderRect.Height/2)
                {
                    position.X = sliderRect.Left + sliderRect.Height/2;
                }
                if (position.X > sliderRect.Right - sliderRect.Height/2)
                {
                    position.X = sliderRect.Right - sliderRect.Height / 2;
                }
                knobPosition.X = position.X;

                float slideLength = sliderRect.Width - sliderRect.Height;
                float sliderStart = sliderRect.Left + sliderRect.Height / 2;
                value =position.X- sliderStart;
                value = value / slideLength;
                
                onTouch?.Invoke(this, new EventArgs());
            }            
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

        public void SetValue(float value)
        {
            this.value = value;

            float slideLength = sliderRect.Width - sliderRect.Height;
            float sliderStart = sliderRect.Left + sliderRect.Height / 2;            
            knobPosition.X = slideLength*value +sliderStart;            
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

            Rectangle rexi = new Rectangle(rectangle.Height/8, rectangle.Height*5/8,rectangle.Width-rectangle.Height/4, rectangle.Height/4);
            for (int i = 0; i <= rectangle.Height / 8; i++)
            {
                spriteBatch.DrawRectangle(rexi, Color.Black, 2);
                rexi.Inflate(-1, -1);
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
            spriteBatch.Draw(spriteTextureNormal, new Rectangle((int)knobPosition.X - knobRadie, (int)knobPosition.Y - knobRadie, knobRadie * 2, knobRadie * 2), knobTextureRect, Color.White);
        }

        public void DrawDiffuse(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(diffuseTexture, rectangle, Color.White);              
            spriteBatch.Draw(spriteTextureDiffuse, new Rectangle((int)knobPosition.X-knobRadie, (int)knobPosition.Y-knobRadie, knobRadie*2, knobRadie*2), knobTextureRect, Color.White);            
        }

        public void DisposeTextures()
        {
            diffuseTexture.Dispose();
            normalTexture.Dispose();            
        }
    }
}