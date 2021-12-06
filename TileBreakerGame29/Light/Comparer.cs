using System;
using System.Collections.Generic;


namespace TileBreakerGame29.Light
{
    public class Comparer<TKey>: IComparer<TKey> where TKey : IComparable
    {        
        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
            {
                return 1;   // Handle equality as beeing greater
            }
            else
            {
                return result;
            }
        }
        
    }
}