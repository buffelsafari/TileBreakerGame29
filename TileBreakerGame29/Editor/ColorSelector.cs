using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BreakOut.Editor
{
    class ColorSelector
    {
        private Rectangle textureRect = new Rectangle(160, 80, 80, 80);
        
        private Rectangle[] recentColorRect;
        private Rectangle[] recentColorDrawRect;
        private Color[] recentColors;

        private Slider sliderR;
        private Slider sliderG;
        private Slider sliderB;

        public ColorSelector(int yOffset)
        {
            sliderR = new Slider(new Rectangle(550, 960, 80, 500), new Color(255,0,0,255), yOffset);
            sliderG = new Slider(new Rectangle(680, 960, 80, 500), new Color(0,255,0,255), yOffset);
            sliderB = new Slider(new Rectangle(810, 960, 80, 500), new Color(0,0,255,255), yOffset);

            recentColors = new Color[5];
            recentColorRect = new Rectangle[5];
            recentColorDrawRect = new Rectangle[5];
            for (int counter = 0; counter < 5; counter++)
            {
                recentColorRect[counter] = new Rectangle(970, 960+counter*100, 100, 100);
                recentColorDrawRect[counter] = new Rectangle(recentColorRect[counter].X, recentColorRect[counter].Y + yOffset, recentColorRect[counter].Width, recentColorRect[counter].Height);
                recentColors[counter]=new Color(counter * 50, 127, 0);
            }           

        }

        public void AddRecentColor(Color color)
        {
            for (int counter = 0; counter < 5; counter++)
            {
                if (recentColors[counter].Equals(color))
                {
                    return;
                }
            }

            for (int counter = 4;counter!=0 ; counter--)
            {
                recentColors[counter] = recentColors[counter-1];
            }
            

            recentColors[0]=color;
            
        }

        public void UpdateSliders(Vector2 position)
        {
            sliderR.OnDown(position);
            sliderG.OnDown(position);
            sliderB.OnDown(position);
        }

        public bool OnTap(Vector2 position)
        {
            for (int counter = 0; counter < 5; counter++)
            {
                if (recentColorRect[counter].Contains(position))
                {
                    SoundManager.Play(SoundId.tap);
                    SetColor(recentColors[counter]);
                    return true;
                }
            }
            return false;

        }

        public Color GetColor()
        {
            return new Color(sliderR.GetValue(), sliderG.GetValue(), sliderB.GetValue());
        }

        public void SetColor(Color color)
        {
            Vector3 c= color.ToVector3();
            sliderR.SetValue(c.X);
            sliderG.SetValue(c.Y);
            sliderB.SetValue(c.Z);
        }


        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {

            sliderR.Draw(spriteBatch, texture);
            sliderG.Draw(spriteBatch, texture);
            sliderB.Draw(spriteBatch, texture);

            for(int counter=0;counter< recentColorRect.Length ;counter++)
            {
                spriteBatch.Draw(texture, recentColorDrawRect[counter],textureRect, recentColors[counter]);
            
            }
           
        }
    }
}