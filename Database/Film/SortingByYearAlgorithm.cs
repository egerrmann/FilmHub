using System.Collections.Generic;

namespace Database.Film
{
    public class SortingByYearAlgorithm : IComparer<Film>
    {
        public int Compare(Film x, Film y)
        {
            if (y != null && x != null && x.Year < y.Year)
            {
                return 1;
            }

            if (y != null && x != null && x.Year > y.Year)
            {
                return -1;
            }

            return 0;
        }
    }
}