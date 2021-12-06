using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileBreakerGame29.Bomb;

namespace TileBreakerGame29.Refraction
{
    class ShockRefraction
    {        
        private Rectangle blasterTextureRect;
        private const float aspect = 1080 / 1920.0f;
        private const float radie = 75;
        private static List<ShockWave> shockList;
        private List<ShockWave> shockRemove;
        private Color clearColor = new Color(0.5f, 0.5f, 1.0f, 1.0f);

        public ShockRefraction()
        {
            blasterTextureRect = new Rectangle(524, 275, 80, 80);
            shockList = new List<ShockWave>();
            shockRemove = new List<ShockWave>();            
        }

        public void Dispose()
        { 
            
        }

        public void NewGame()
        {
            shockList.Clear();
            shockRemove.Clear();
        }

        public static void Add(Vector2 position, int size)
        { 
            shockList.Add(new ShockWave(position, size));
        }

        public void Update()
        {
            foreach (ShockWave sw in shockList)
            {
                sw.Update();
                if (!sw.Active)
                {
                    shockRemove.Add(sw);
                }
            }

            foreach (ShockWave sw in shockRemove)
            {
                shockList.Remove(sw);
            }
            shockRemove.Clear();            

        }

        public void DrawToRefractionMap(GraphicsDevice graphics, SpriteBatch spriteBatch, RenderTarget2D RefractionMap, Texture2D texture, List<Vector2> blasterBolts, Stack<Vector2> eyes, List<Bubble> bubbles, Vector2 shake)
        {
            graphics.SetRenderTarget(RefractionMap);
            graphics.Clear(clearColor);
            spriteBatch.Begin();

            foreach (ShockWave sw in shockList)
            {
                sw.Draw(spriteBatch, texture);
            }

            foreach (Vector2 p in blasterBolts)
            {
                Vector2 position = new Vector2((p.X / 1080.0f) * 512.0f, (p.Y / 1920.0f) * 512.0f);

                spriteBatch.Draw(texture, new Rectangle((int)(position.X - radie*0.5f), (int)(position.Y - (radie*0.5f) * aspect), (int)(radie), (int)((radie * 2) * aspect)), blasterTextureRect, Color.White*0.5f);
            }

            //googly eye lenses
            float rad = 20;
            foreach (Vector2 v in eyes)
            {
                Vector2 pos = new Vector2(((v.X-shake.X) / 1080.0f) * 512.0f, ((v.Y-shake.Y) / 1920.0f) * 512.0f);
                spriteBatch.Draw(texture, new Rectangle((int)(pos.X - rad * 0.5f), (int)(pos.Y - (rad * 0.5f) * aspect), (int)(rad), (int)((rad) * aspect)), blasterTextureRect, Color.White * 0.25f);
            }

            // detonate bubbles
            foreach (Bubble b in bubbles)
            {                 
                Vector2 pos = new Vector2(((b.position.X) / 1080.0f) * 512.0f, ((b.position.Y) / 1920.0f) * 512.0f);
                spriteBatch.Draw(texture, new Rectangle((int)(pos.X - b.radie*0.5f), (int)(pos.Y - (b.radie*0.5f) * aspect), (int)(b.radie), (int)((b.radie) * aspect)), blasterTextureRect, Color.White );
            }
            spriteBatch.End();
        }
    }
}