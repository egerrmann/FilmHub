using System.Collections.Generic;

namespace Database.Film
{
    public interface IFilmRepository
    {
        List<Film> GetAllFilms();
        List<Film> GetNewFilms();
        int GetCurrentFilmId(string image);
        Film GetCurrentFilmInfo(int currentFilmId);
        List<Film> CurrentCategoryFilms(string category);
        List<string> AllCategories();
        List<Film> SortFilmsByYear();
        List<Film> SortFilmsByTitle();
        List<Film> SortFilmsByDuration();
        List<Film> SearchAndGetFilms(string parameter);
        void LeaveComment(string comment, int userId, int filmId);
        List<Comment> GetAllComments(int filmId);
        void AddToBookmarks(int film, int userId);
        
        
        
        
        Database.User.User FindById(int id);
    }
}