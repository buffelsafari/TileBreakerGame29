using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TileBreakerGame29.Light;

namespace TileBreakerGame29
{
    class FreeLight : ILightRequester
    {
        private int nextIndex = 0;
        const int MAX_FREELIGHT = 8;
        private FreeLightObject[] lightArray;        
        private LightData lightData;
        public FreeLight()
        {
            lightArray = new FreeLightObject[MAX_FREELIGHT];
            for (int i = 0; i < lightArray.Length; i++)
            {
                lightArray[i] = new FreeLightObject();
            }
            lightData = new LightData();
        }

        public void NewGame()
        {
            
        }

        public void Add(Vector2 position, Color color, int lifeTime, int priority)
        {
            for (int i = 0; i < lightArray.Length; i++)
            {
                if (lightArray[(i + nextIndex) % MAX_FREELIGHT].lifeTime <= 0)
                {
                    int n = (i + nextIndex) % MAX_FREELIGHT;
                    lightArray[n].lifeTime = lifeTime;
                    lightArray[n].position = position;
                    lightArray[n].color = color;
                    lightArray[n].priority = priority;

                    nextIndex++;
                    return;

                }
            }            
        }        

        public void GetLightData(SortedList<int, LightData> list)
        {
            for (int i = 0; i < lightArray.Length; i++)
            {
                lightArray[i].lifeTime--;

                if (lightArray[i].lifeTime >= 0)
                {
                    lightData.position = lightArray[i].position;
                    lightData.color = lightArray[i].color;
                    list.Add(lightArray[i].priority, lightData);
                }
            }
        }
    }
}