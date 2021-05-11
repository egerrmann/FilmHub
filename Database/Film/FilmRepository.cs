using System.Collections.Generic;
using System.Linq;
using Database.DbModels;
using Database.User;

namespace Database.Film
{
    public class FilmRepository : IFilmRepository
    {
        private readonly IAppContext _dbContext;

        public FilmRepository(IAppContext data)
        {
            _dbContext = data;
        }

        public List<Film> GetAllFilms()
        {
            return _dbContext.FilmHubFilms.Select(f => new Film
            {
                Title = f.Title,
                Year = f.Year,
                Genre = f.Genre,
                Director = f.Director,
                Summary = f.Summary,
                Time = f.Time,
                Age = f.Age,
                Country = f.Country,
                Actors = f.Actors,
                Image = f.Image
            }).ToList();
        }


        public int GetCurrentFilmId(string image)
        {
            var currentFilm = _dbContext.FilmHubFilms.FirstOrDefault(f => f.Image == image);
            /*foreach (var f in _dbContext.FilmHubFilms)
            {
                if (f.Image == image)
                {
                    currentFilmId = f.Id;
                }
            }*/
            int currentFilmId = currentFilm.Id;
            return currentFilmId;
        }

        public Film GetCurrentFilmInfo(int currentFilmId)
        {
            FilmDbModel dbCurrentFilm = _dbContext.FilmHubFilms.Find(currentFilmId);
            Film currentFilm = new Film
            {
                Title = dbCurrentFilm.Title,
                Year = dbCurrentFilm.Year,
                Genre = dbCurrentFilm.Genre,
                Director = dbCurrentFilm.Director,
                Summary = dbCurrentFilm.Summary,
                Time = dbCurrentFilm.Time,
                Age = dbCurrentFilm.Age,
                Country = dbCurrentFilm.Country,
                Actors = dbCurrentFilm.Actors,
                Image = dbCurrentFilm.Image
            };
            return currentFilm;
        }

        public List<Film> CurrentCategoryFilms(string category)
        {
            return _dbContext.FilmHubFilms.Where(film => film.Genre == category).Select(f => new Film
            {
                Title = f.Title,
                Year = f.Year,
                Genre = f.Genre,
                Director = f.Director,
                Summary = f.Summary,
                Time = f.Time,
                Age = f.Age,
                Country = f.Country,
                Actors = f.Actors,
                Image = f.Image
            }).ToList();
        }

        public List<string> AllCategories()
        {
            var l = new List<string>();
            foreach (var film in _dbContext.FilmHubFilms)
            {
                l.Add(film.Genre);
            }

            var myHash = new HashSet<string>(l).ToList();
            return myHash;
        }
    }
}