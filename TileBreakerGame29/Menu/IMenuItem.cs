using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BreakOut.Menu
{
    interface IMenuItem
    {
        MenuPage GetChildPage();
        void UpdateTouch(TouchLocation location);
        void UpdateGesture(GestureSample gesture);
        void GenerateNormalTexture(SpriteBatch spriteBatch);
        void GenerateDiffuseTexture(SpriteBatch spriteBatch);
        void DisposeTextures();
        void DrawNormal(SpriteBatch spriteBatch);
        void DrawDiffuse(SpriteBatch spriteBatch);
    }
}