using System.Collections.Generic;
using System.Linq;
using Database.Film;
using FilmHub.Models;

namespace FilmHub.Services.Film
{
    public class FilmService : IFilmService
    {
        private readonly IFilmRepository _filmRepository;
        
        static int currentFilmId;

        public FilmService(IFilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }
        public List<Database.Film.Film> GetAllFilms()
        {
            return _filmRepository.GetAllFilms();
        }

        public int GetCurrentFilmId(string image)
        {
            return _filmRepository.GetCurrentFilmId(image);
        }

        public Database.Film.Film GetCurrentFilmInfo(int currentFilmId)
        {
            var currentFilm = _filmRepository.GetCurrentFilmInfo(currentFilmId);
            currentFilm.Comments = _filmRepository.GetAllComments(currentFilmId);
            return currentFilm;
        }

        public List<Database.Film.Film> CurrentCategoryFilms(string category)
        {
            return _filmRepository.CurrentCategoryFilms(category);
        }

        public List<string> GetAllCategories()
        {
            return _filmRepository.AllCategories();
        }

        public List<Database.Film.Film> SortFilmsByYear()
        {
            return _filmRepository.SortFilmsByYear();
        }

        public List<Database.Film.Film> SearchAndGetFilms(string parameter)
        {
            return _filmRepository.SearchAndGetFilms(parameter);
        }

        public void LeaveComment(string comment, int userId, int filmId)
        {
            _filmRepository.LeaveComment(comment, userId, filmId);
        }

        /*public void AddToBookmarks(int filmId, int userId)
        {
            _filmRepository.AddToBookmarks(filmId, userId);
        }*/
    }
}
