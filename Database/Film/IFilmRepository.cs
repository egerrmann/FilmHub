using System.Collections.Generic;

namespace Database.Film
{
    public interface IFilmRepository
    {
        List<Film> GetAllFilms();
        int GetCurrentFilmId(string image);
        Film GetCurrentFilmInfo(int currentFilmId);
        List<Film> CurrentCategoryFilms(string category);
        List<string> AllCategories();
    }
}