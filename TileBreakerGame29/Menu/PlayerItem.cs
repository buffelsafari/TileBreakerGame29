using BreakOut;
using BreakOut.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace TileBreakerGame29.Menu
{
    class PlayerItem : IMenuItem
    {
        private Texture2D spriteTextureDiffuse;
        private Texture2D spriteTextureNormal;
        private Rectangle middleTextureRect;
        private Rectangle leftTextureRect;
        private Rectangle rightTextureRect;

        private Rectangle rectangle;

        private Rectangle middleRect;
        private Rectangle leftRect;
        private Rectangle rightRect;

        private Color leftColor;
        private Color rightColor;
        private Color middleColor;

        public PlayerItem(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            spriteTextureDiffuse = (Texture2D) TextureContainer.GetTexture(TextureContainer.TextureReference.spriteSheetDiffuse);
            spriteTextureNormal = (Texture2D) TextureContainer.GetTexture(TextureContainer.TextureReference.spriteSheetNormal);
            middleTextureRect = new Rectangle(2, 2, 160, 80);
            leftTextureRect = new Rectangle(166, 2, 80, 80);
            rightTextureRect = new Rectangle(249, 2, 80, 80);

            middleRect = rectangle;
            middleRect.Inflate(-rectangle.Height/2,0);
            leftRect = new Rectangle(rectangle.Left, rectangle.Top, rectangle.Height, rectangle.Height);
            rightRect = new Rectangle(rectangle.Right-rectangle.Height, rectangle.Top, rectangle.Height, rectangle.Height);

            leftColor = Color.Red;
            rightColor = Color.Red;
            middleColor = Color.White;
        
        }

        public void SetLeftColor(Color color)
        {
            leftColor = color;            
        }

        public void SetRightColor(Color color)
        {
            rightColor = color;
        }

        public void SetMiddleColor(Color color)
        {
            middleColor = color;
        }


        public void DisposeTextures()
        {
          
        }

        public void DrawDiffuse(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTextureDiffuse, middleRect, middleTextureRect, middleColor);
            spriteBatch.Draw(spriteTextureDiffuse, leftRect, leftTextureRect, leftColor);
            spriteBatch.Draw(spriteTextureDiffuse, rightRect, rightTextureRect, rightColor);            
        }

        public void DrawNormal(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTextureNormal, middleRect, middleTextureRect, Color.White);
            spriteBatch.Draw(spriteTextureNormal, leftRect, leftTextureRect, Color.White);
            spriteBatch.Draw(spriteTextureNormal, rightRect, rightTextureRect, Color.White);            
        }

        public void GenerateDiffuseTexture(SpriteBatch spriteBatch)
        {
            
        }

        public void GenerateNormalTexture(SpriteBatch spriteBatch)
        {
            
        }

        public MenuPage GetChildPage()
        {            
            return null;
        }

        public void UpdateGesture(GestureSample gesture)
        {
            
        }

        public void UpdateTouch(TouchLocation location)
        {
            
        }
    }
}