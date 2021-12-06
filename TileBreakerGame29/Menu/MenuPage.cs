using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BreakOut.Menu
{
    class MenuPage
    {
        private List<IMenuItem> itemList;

        public MenuPage()
        {
            itemList = new List<IMenuItem>();
        }

        public void AddItem(IMenuItem item)
        {
            itemList.Add(item);
        }

        public void UpdateTouch(TouchLocation location)
        {
            foreach (IMenuItem item in itemList)
            {
                item.UpdateTouch(location);
            }
        }

        public void UpdateGesture(GestureSample gesture)
        {
            foreach (IMenuItem item in itemList)
            {
                item.UpdateGesture(gesture);
            }
        }

        public void GenerateNormalTexture(SpriteBatch spriteBatch)
        {
            foreach (IMenuItem item in itemList)
            {
                item.GenerateNormalTexture(spriteBatch);
            }
        }

        public void GenerateDiffuseTexture(SpriteBatch spriteBatch)
        {
            foreach (IMenuItem item in itemList)
            {
                item.GenerateDiffuseTexture(spriteBatch);
            }
        }

        public void DrawNormal(SpriteBatch spriteBatch)
        {
            foreach (IMenuItem item in itemList)
            {
                item.DrawNormal(spriteBatch);
            }
        }

        public void DrawDiffuse(SpriteBatch spriteBatch)
        {
            foreach(IMenuItem item in itemList)
            {
                item.DrawDiffuse(spriteBatch);
            }
        }

        public void DisposeTextures()
        {
            foreach (IMenuItem item in itemList)
            {
                item.DisposeTextures();
            }
        }

    }
}