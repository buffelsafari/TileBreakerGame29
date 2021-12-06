using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace BreakOut
{
    static class DebugFunctions
    {
        private static Texture2D _texture;
        private static Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _texture.SetData(new[] { Color.White });
            }
            return _texture;
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            float distance = Vector2.Distance(point1, point2);
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            Vector2 origin = new Vector2(0f, 0.5f);
            Vector2 scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle boundingBox, Color color, float thikness = 1f)
        {
            DebugFunctions.DrawLine(spriteBatch, new Vector2(boundingBox.Left, boundingBox.Top), new Vector2(boundingBox.Right, boundingBox.Top), color, thikness);
            DebugFunctions.DrawLine(spriteBatch, new Vector2(boundingBox.Left, boundingBox.Top), new Vector2(boundingBox.Left, boundingBox.Bottom), color, thikness);
            DebugFunctions.DrawLine(spriteBatch, new Vector2(boundingBox.Left, boundingBox.Bottom), new Vector2(boundingBox.Right, boundingBox.Bottom), color, thikness);
            DebugFunctions.DrawLine(spriteBatch, new Vector2(boundingBox.Right, boundingBox.Top), new Vector2(boundingBox.Right, boundingBox.Bottom), color, thikness);
        }
    }
}