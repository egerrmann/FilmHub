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
        List<Film> SortFilmsByYear();
        List<Film> SearchAndGetFilms(string parameter);
        void LeaveComment(string comment, int userId, int filmId);
        List<Comment> GetAllComments(int filmId);
        void AddToBookmarks(int film, int userId);
    }
}