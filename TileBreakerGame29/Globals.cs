using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BreakOut
{
    static class Globals
    {
        public const int numberOfBallUpdates = 32;
        public const float batPushStrength = 5;

        public const int dustLifeTime = 200;
        public const int blasterHitLifeTime = 7;
        public static SpriteFont smallFont;
        public static SpriteFont bigFont;
        public static Rectangle circleRectangle = new Rectangle(524, 275, 80, 80);
        public static float bombRadie = 300;
        public static int adHeight;
        public static Random rnd = new Random();
        
        public const int ballLightPriority = 5;  
        public const int sparkLightPriority = 4;
        public const int blasterLightPriority = 2;
        public const int bomberLightPriority=1;
        public const int powerUpLightPriority = 3;

        public const int maxLives = 10;
        public const int maxExpands=3;
        public const int maxFlash = 4;
        public const int maxStar = 10;
    }
}