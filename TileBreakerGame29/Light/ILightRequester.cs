using System.Collections.Generic;

namespace TileBreakerGame29.Light
{
    interface ILightRequester
    {        
        void GetLightData(SortedList<int, LightData> list);
    }
}