using System;

using Microsoft.Xna.Framework;

namespace TileBreakerGame29
{
    class ColorFunctions
    {        
        public delegate Color ColorFunctionDelegate(Color c, Vector2 v, int time=0);

        public static Color HorizontalRainbow(Color c, Vector2 v, int time = 0)
        {
            float f = (v.X / 128.0f);
            if (f < 0)
            {
                f = 0;
            }
            int h = (int)f;
            f = f - h;

            Vector3[] rainbow = new Vector3[7];
            rainbow[0] = new Vector3(1, 0, 0);
            rainbow[1] = new Vector3(1, 0.6470f, 0);
            rainbow[2] = new Vector3(1, 1, 0);
            rainbow[3] = new Vector3(0, 0.5f, 0);
            rainbow[4] = new Vector3(0, 0, 1);
            rainbow[5] = new Vector3(0.3f, 0, 0.51f);
            rainbow[6] = new Vector3(0.93f, 0.51f, 0.93f);

            Vector3 color = Vector3.LerpPrecise(rainbow[h % rainbow.Length], rainbow[(h + 1) % rainbow.Length], f);

            return new Color(color * c.ToVector3());
        }

        public static Color Rainbow(Color c, Vector2 v, int time=0)  
        {
            float f = (v.Y / 128.0f);
            if (f < 0)
            {
                f = 0;
            }
            int h = (int)f;
            f = f - h;

            Vector3[] rainbow = new Vector3[7];  
            rainbow[0] = new Vector3(1, 0, 0);
            rainbow[1] = new Vector3(1, 0.6470f, 0);
            rainbow[2] = new Vector3(1, 1, 0);
            rainbow[3] = new Vector3(0, 0.5f, 0);
            rainbow[4] = new Vector3(0, 0, 1);
            rainbow[5] = new Vector3(0.3f, 0, 0.51f);
            rainbow[6] = new Vector3(0.93f, 0.51f, 0.93f);

            Vector3 color = Vector3.LerpPrecise(rainbow[h % rainbow.Length], rainbow[(h + 1) % rainbow.Length], f);

            return new Color(color*c.ToVector3());
        }

        public static Color ReverseRainbow(Color c, Vector2 v, int time = 0)
        {
            float f = (v.Y / 128.0f);
            if (f < 0)
            {
                f = 0;
            }
            int h = (int)f;
            f = f - h;

            Vector3[] rainbow = new Vector3[7];
            rainbow[6] = new Vector3(1, 0, 0);
            rainbow[5] = new Vector3(1, 0.6470f, 0);
            rainbow[4] = new Vector3(1, 1, 0);
            rainbow[3] = new Vector3(0, 0.5f, 0);
            rainbow[2] = new Vector3(0, 0, 1);
            rainbow[1] = new Vector3(0.3f, 0, 0.51f);
            rainbow[0] = new Vector3(0.93f, 0.51f, 0.93f);

            Vector3 color = Vector3.LerpPrecise(rainbow[h % rainbow.Length], rainbow[(h + 1) % rainbow.Length], f);

            return new Color(color * c.ToVector3());
        }

        public static Color Normal(Color c, Vector2 v, int time=0)
        {
            return c;
        }

        public static Color Rune(Color c, Vector2 v, int time = 0)
        {            
            Color returnColor = new Color(255-c.R, 255-c.G, 255-c.B, 255);
            float dist = Vector2.Distance(new Vector2(540, 360), v);
            returnColor =returnColor*(float)(Math.Sin(dist*0.1f+time*0.05f));
            return returnColor;
        }
    }
}