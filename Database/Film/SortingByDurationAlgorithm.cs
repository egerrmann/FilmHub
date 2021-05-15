using System;
using System.Collections.Generic;

namespace Database.Film
{
    public class SortingByDurationAlgorithm : IComparer<Film>
    {
        public int Compare(Film x, Film y)
        {
            int xd = 0;
            int yd = 0;
            int[] xNums = new int[3];
            int[] yNums = new int[3];
            xNums[0] = x.Time[0];
            xNums[1] = x.Time[3];
            xNums[2] = x.Time[4];
            yNums[0] = y.Time[0];
            yNums[1] = y.Time[3];
            yNums[2] = y.Time[4];

            xd = xNums[0] * 60 + xNums[1] * 10 + xNums[2];
            yd = yNums[0] * 60 + yNums[1] * 10 + yNums[2];
            
            if (xd <= yd)
            {
                return 1;
            }
            
            if (xd > yd)
            {
                return -1;
            }
            
            return 0;
        }
    }
}