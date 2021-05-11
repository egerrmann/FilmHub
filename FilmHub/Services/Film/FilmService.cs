using System.Collections.Generic;
using System.Linq;
using Database.Film;
using FilmHub.Models;

namespace FilmHub.Services.Film
{
    public class FilmService : IFilmService
    {
        private readonly IFilmRepository _filmRepository;

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
            return _filmRepository.GetCurrentFilmInfo(currentFilmId);
        }

        public List<Database.Film.Film> CurrentCategoryFilms(string category)
        {
            return _filmRepository.CurrentCategoryFilms(category);
        }

        public List<string> GetAllCategories()
        {
            return _filmRepository.AllCategories();
        }
    }
}