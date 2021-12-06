using BreakOut;
using BreakOut.Entities.Static;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBreakerGame29.Editor
{
    class InformationBar
    {
        private Rectangle textureRect = new Rectangle(2, 2, 160, 80);
        private Rectangle rect;
        private Tile tile;
        private Vector2 tileOffset;
        private string shapeInfo="";
        private string colorTypeInfo = "";
        private string effectTypeInfo = "";
        private string hitPointInfo = "";
        private string colorInfo = "";
        private string positionInfo = "";
        public InformationBar(int yOffset)
        {
            rect = new Rectangle(0, 0, 1080, yOffset);
        }

        public void UpdateTileInformation(Tile tile)
        {
            this.tile = tile;

            if (tile != null)
            {
                tileOffset = -tile.position;
                tileOffset.X += 20;
                tileOffset.Y += 10;

                shapeInfo = tile.shape.ToString();
                colorTypeInfo = "ColorT: " + tile.colorType.ToString();
                effectTypeInfo = "EffectT: " + tile.effectType.ToString();
                hitPointInfo = "HitPoints:" + tile.hp;
                colorInfo = "RGB(" + tile.color.R + ", " + tile.color.G + ", " + tile.color.B + ")";
                positionInfo = "X=" + tile.position.X + ", Y=" + tile.position.Y;
            }
        }

        public void UpdateColorInformation(Color color)
        {
            colorInfo = "RGB(" + color.R + ", " + color.G + ", " + color.B + ")";
        }
       
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        { 
            spriteBatch.Draw(texture, rect, textureRect, Color.Beige);

            if (tile != null)
            {
                spriteBatch.DrawString(Globals.smallFont, shapeInfo, new Vector2(160, 10), Color.Black, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(Globals.smallFont, colorTypeInfo, new Vector2(160, 50), Color.Black, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(Globals.smallFont, effectTypeInfo, new Vector2(160, 90), Color.Black, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);

                spriteBatch.DrawString(Globals.smallFont, hitPointInfo, new Vector2(700, 10), Color.Black, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(Globals.smallFont, colorInfo, new Vector2(700, 50), Color.Black, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(Globals.smallFont, positionInfo, new Vector2(700, 90), Color.Black, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.DrawString(Globals.smallFont, colorInfo, new Vector2(250, 30), Color.Black, 0, new Vector2(0, 0), 1.2f, SpriteEffects.None, 0);
            }
        }

        public void DrawTile(SpriteBatch spriteBatch, Texture2D texture, Effect effect)
        {            
            if (tile != null)
            {
                effect.Parameters["TopLeftColor"].SetValue(tile.colorFunction(tile.color, new Vector2(tile.collisionRect.Left, tile.collisionRect.Top)).ToVector4());
                effect.Parameters["TopRightColor"].SetValue(tile.colorFunction(tile.color, new Vector2(tile.collisionRect.Right, tile.collisionRect.Top)).ToVector4());
                effect.Parameters["BottomLeftColor"].SetValue(tile.colorFunction(tile.color, new Vector2(tile.collisionRect.Left, tile.collisionRect.Bottom)).ToVector4());
                effect.Parameters["BottomRightColor"].SetValue(tile.colorFunction(tile.color, new Vector2(tile.collisionRect.Right, tile.collisionRect.Bottom)).ToVector4());
                effect.Parameters["CenterTexCoord"].SetValue(new Vector2(tile.textureRect.Center.X / 640.0f, (tile.textureRect.Center.Y / 640.0f)));
                tile.EditorDraw(spriteBatch, tileOffset, texture);
            }
        }
    }
}