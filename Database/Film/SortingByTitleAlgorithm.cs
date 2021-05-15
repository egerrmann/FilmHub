using System.Collections.Generic;

namespace Database.Film
{
    public class SortingByTitleAlgorithm : IComparer<Film>
    {
        public int Compare(Film x, Film y)
        {
            int s = 0;
            if (x.Title.Length <= y.Title.Length)
            {
                s = x.Title.Length;
            }
            else
            {
                s = y.Title.Length;
            }
            for (int i = 0; i < s; i++)
            {
                if (x.Title[i] < y.Title[i])
                {
                    return -1;
                }

                if (x.Title[i] > y.Title[i])
                {
                    return 1;
                }
            }

            if (s == x.Title.Length)
            {
                return -1;
            }
            
            if (s == y.Title.Length)
            {
                return 1;
            }

            return 0;
        }
    }
}