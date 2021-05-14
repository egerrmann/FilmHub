using System.Collections.Generic;
using FilmHub.Models;
using FilmHub.Services.Registration;

namespace FilmHub.Services.Film
{
    public interface IFilmService
    {
        List<Database.Film.Film> GetAllFilms();
        int GetCurrentFilmId(string image);
        static int currentFilmId;
        Database.Film.Film GetCurrentFilmInfo(int currentFilmId);
        List<Database.Film.Film> CurrentCategoryFilms(string category);
        List<string> GetAllCategories();
        List<Database.Film.Film> SortFilmsByYear();
        List<Database.Film.Film> SearchAndGetFilms(string parameter);
        public void LeaveComment(string comment, int userId, int filmId);
        public void AddToBookmarks(int filmId, int userId);
    }
}