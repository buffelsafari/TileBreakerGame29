using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BreakOut.Editor
{
    class Slider
    {
        private Rectangle knobTextureRect = new Rectangle(160, 40, 40, 20);
        private Rectangle slideTextureRect = new Rectangle(255,0,30,160);

        private Rectangle knobDrawRect;
        private Rectangle slideDrawRect;

        private int sliderPosition;
        private Rectangle sliderRect;
        private Rectangle outerRect;
        private int knobHeight = 20;
        private float value;
        private Color color;
        private int yOffset;


        public Slider(Rectangle rect, Color color, int yOffset)
        {
            this.yOffset = yOffset;
            this.color = color;
            sliderRect = rect;            
            outerRect = new Rectangle(sliderRect.Location, sliderRect.Size);
            outerRect.Inflate(0, 20);
            sliderPosition = sliderRect.Top;
            SetValue(1.0f);

            knobDrawRect= new Rectangle(sliderRect.X, sliderPosition + yOffset, sliderRect.Width, knobHeight);
            slideDrawRect= new Rectangle(sliderRect.X, sliderRect.Y + yOffset, sliderRect.Width, sliderRect.Height + knobHeight);

        }

        public void OnDown(Vector2 position)
        {            
            if (outerRect.Contains(position))
            {
                sliderPosition = (int)position.Y;

                if (position.Y < sliderRect.Top)
                {
                    sliderPosition = sliderRect.Top;
                }
                if (position.Y > sliderRect.Bottom)
                {
                    sliderPosition = sliderRect.Bottom;
                }
            }
            knobDrawRect = new Rectangle(sliderRect.X, sliderPosition + yOffset, sliderRect.Width, knobHeight);
        }
        public float GetValue()
        {
            value = (1 - (1.0f / (float)sliderRect.Height) * (float)(sliderPosition - sliderRect.Top));
            return value;
        }

        public void SetValue(float value)
        {
            this.value = value;
            sliderPosition =(int)(sliderRect.Height*(1.0f-value)) +sliderRect.Top;
            knobDrawRect = new Rectangle(sliderRect.X, sliderPosition + yOffset, sliderRect.Width, knobHeight);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {            
            spriteBatch.Draw(texture, slideDrawRect, slideTextureRect, color);
            spriteBatch.Draw(texture, knobDrawRect, knobTextureRect, Color.Orange);            
        }

    }
}