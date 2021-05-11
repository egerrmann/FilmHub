using System.Collections.Generic;
using System.Linq;
using Database.DbModels;
using Database.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Database.Film.SortingByYearAlgorithm;

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
                Image = f.Image,
                Comments = f.Comments.Select(c => new Comment()).ToList()
            }).ToList();
        }


        public int GetCurrentFilmId(string image)
        {
            var currentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments)
                .FirstOrDefault(f => f.Image == image);
            int currentFilmId = currentFilm.Id;
            return currentFilmId;
        }

        public Film GetCurrentFilmInfo(int currentFilmId)
        {
            FilmDbModel dbCurrentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments)
                .FirstOrDefault(f => f.Id == currentFilmId);
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
                Image = dbCurrentFilm.Image,
                Comments = dbCurrentFilm.Comments.Select(c => new Comment()).ToList()
            };
            return currentFilm;
        }

        public List<Film> CurrentCategoryFilms(string category)
        {
            return _dbContext.FilmHubFilms.Where(film => film.Genre == category)
                .Include(f => f.Comments).Select(f => new Film
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
                Image = f.Image,
                Comments = f.Comments.Select(c => new Comment()).ToList()
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

        public List<Film> SortFilmsByYear()
        {
            List<Film> sortedByYearFilms = GetAllFilms();
            SortingByYearAlgorithm yearComparer = new SortingByYearAlgorithm();
            sortedByYearFilms.Sort(yearComparer);
            return sortedByYearFilms;
        }

        public List<Film> SearchAndGetFilms(string parameter)
        {
            List<Film> filmsFoundByParameter = new List<Film>();
            foreach (var film in _dbContext.FilmHubFilms.Include(f => f.Comments))
            {
                if (film.Director == parameter || film.Genre == parameter || film.Title == parameter || film.Actors.Contains(parameter))
                {
                    Film foundFilm = new Film
                    {
                        Title = film.Title,
                        Year = film.Year,
                        Genre = film.Genre,
                        Director = film.Director,
                        Summary = film.Summary,
                        Time = film.Time,
                        Age = film.Age,
                        Country = film.Country,
                        Actors = film.Actors,
                        Image = film.Image,
                        Comments = film.Comments.Select(c => new Comment()).ToList()
                    };
                    filmsFoundByParameter.Add(foundFilm);
                }
            }

            return filmsFoundByParameter;
        }

        public void LeaveComment(string comment, int userId, int filmId)
        {
            UserDbModel currentUser = _dbContext.FilmHubUsers.Include(u => u.Favourite)
                .Include(u => u.Comments)
                /*.Include(u => u.Bookmarks)*/
                .FirstOrDefault(u => u.Id == userId);
            FilmDbModel currentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments)
                .FirstOrDefault(f => f.Id == filmId);
            CommentDbModel newComment = new CommentDbModel
            {
                User = currentUser,
                Film = currentFilm,
                Text = comment
            };
            _dbContext.Comments.Add(newComment);
            _dbContext.SaveChanges();
        }

        public List<Comment> GetAllComments(int filmId)
        {
            List<CommentDbModel> dbComment = _dbContext.Comments.Include(c => c.User)
                .Where(c => c.Film.Id == filmId).ToList();
            List<Comment> comments = dbComment.Select(c => new Comment()
            {
                User = c.User,
                Film = c.Film,
                Text = c.Text
            }).ToList();
            return comments;
        }
        
        /*public void AddToBookmarks(int filmId, int userId)
        {
            var currentUser = _dbContext.FilmHubUsers.Include(u => u.Favourite)
                .Include(u => u.Comments)
                .Include(u => u.Bookmarks).FirstOrDefault(u => u.Id == userId);
            var currentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments)
                .FirstOrDefault(f => f.Id == filmId);
            currentUser.Bookmarks.Add(currentFilm);
            _dbContext.SaveChanges();
        }*/
    }
}