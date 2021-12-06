using System;
using System.Collections.Generic;
using BreakOut;
using BreakOut.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace TileBreakerGame29.Menu
{
    class RadioButtons : IMenuItem
    {
        public bool isGreyed { get; set; } = false;
        private List<Button> buttonList;
        private Texture2D spriteTextureDiffuse;
        private Texture2D spriteTextureNormal;
        private Rectangle textureRect;
        private EventHandler onTap;
        private Rectangle rect;
        private int index = 0;

        public RadioButtons(EventHandler onTap)
        {
            this.onTap = onTap;

            buttonList = new List<Button>();
            spriteTextureDiffuse = (Texture2D) TextureContainer.GetTexture(TextureContainer.TextureReference.spriteSheetDiffuse);
            spriteTextureNormal = (Texture2D) TextureContainer.GetTexture(TextureContainer.TextureReference.spriteSheetNormal);
            textureRect = new Rectangle(524, 275, 80, 80);

        }

        public void Add(Button button)
        {
            buttonList.Add(button);
        }

        public void SetIndex(int i)
        {
            index = i;
            Rectangle r = buttonList[i].GetRectangle();
            rect = new Rectangle(r.Left - r.Height * 3 / 4, r.Top + r.Height / 4, r.Height / 2, r.Height / 2);
            
        }

        public int GetIndex()
        {
            return index;
        }
        public void DisposeTextures()
        {
            foreach (Button b in buttonList)
            {
                b.DisposeTextures();
            }            
        }
        public void DrawDiffuse(SpriteBatch spriteBatch)
        {
            foreach (Button b in buttonList)
            {
                if (isGreyed)
                {
                    b.isGreyed = true;
                    spriteBatch.Draw(spriteTextureDiffuse, rect, textureRect, Color.DimGray);
                }
                else
                {
                    b.isGreyed = false;
                    spriteBatch.Draw(spriteTextureDiffuse, rect, textureRect, Color.White);
                }
                b.DrawDiffuse(spriteBatch);
            }             
        }

        public void DrawNormal(SpriteBatch spriteBatch)
        {
            foreach (Button b in buttonList)
            {
                b.DrawNormal(spriteBatch);
            }
            spriteBatch.Draw(spriteTextureNormal, rect, textureRect, Color.White);
        }

        public void GenerateDiffuseTexture(SpriteBatch spriteBatch)
        {
            foreach (Button b in buttonList)
            {                
                b.GenerateDiffuseTexture(spriteBatch);
            }
        }

        public void GenerateNormalTexture(SpriteBatch spriteBatch)
        {
            foreach (Button b in buttonList)
            {
                b.GenerateNormalTexture(spriteBatch);
            }
        }

        public MenuPage GetChildPage()
        {            
            return null;
        }

        public void UpdateGesture(GestureSample gesture)
        {            
            if (!isGreyed && gesture.GestureType == GestureType.Tap)
            {
                for(int i=0;i<buttonList.Count ;i++)                
                {
                    if (index!=i && buttonList[i].GetRectangle().Contains(ConvertToGameSpace(gesture.Position)))
                    {
                        index = i;                        
                        buttonList[i].UpdateGesture(gesture);
                        Rectangle r = buttonList[i].GetRectangle();
                        rect = new Rectangle(r.Left-r.Height*3/4, r.Top+r.Height/4, r.Height/2, r.Height/2);
                        onTap?.Invoke(this, new EventArgs());
                        break;
                    }
                }
            }            
        }

        private Vector2 ConvertToGameSpace(Vector2 position)
        {
            position.X *= (1080.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
            position.Y *= (1920.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            return position;
        }

        public void UpdateTouch(TouchLocation location)
        {
            foreach (Button b in buttonList)
            {
                b.UpdateTouch(location);
            }
        }
    }
}