using System;
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
                Trailer = f.Trailer,
                Comments = f.Comments.Select(c => new Comment()).ToList(),
                Ratings=f.Ratings.Select(r => new Rating()).ToList(),
                Rating = f.Rating

            }).ToList();
        }

        public List<Film> GetNewFilms()
        {
            return _dbContext.FilmHubFilms.Where(film => film.Year == 2021)
                .Include(f => f.Comments)
                .Include(f=>f.Ratings).Select(f => new Film
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
                    Trailer = f.Trailer,
                    Comments = f.Comments.Select(c => new Comment()).ToList(),
                    Ratings=f.Ratings.Select(r => new Rating()).ToList(),
                    Rating = f.Rating
                }).ToList();
        }


        public int GetCurrentFilmId(string image)
        {
            var currentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments)
                .Include(f=>f.Ratings)
                .FirstOrDefault(f => f.Image == image);
            int currentFilmId = currentFilm.Id;
            return currentFilmId;
        }

        public Film GetCurrentFilmInfo(int currentFilmId)
        {
            FilmDbModel dbCurrentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments)
                .Include(f=>f.Ratings)
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
                Trailer = dbCurrentFilm.Trailer,
                Comments = dbCurrentFilm.Comments.Select(c => new Comment()).ToList(),
                Ratings=dbCurrentFilm.Ratings.Select(r => new Rating()).ToList(),
                Rating = dbCurrentFilm.Rating
            };
            return currentFilm;
        }

        public List<Film> CurrentCategoryFilms(string category)
        {
            return _dbContext.FilmHubFilms.Where(film => film.Genre == category)
                .Include(f => f.Comments)
                .Include(f=>f.Ratings).Select(f => new Film
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
                Trailer = f.Trailer,
                Comments = f.Comments.Select(c => new Comment()).ToList(),
                Ratings=f.Ratings.Select(r => new Rating()).ToList(),
                Rating = f.Rating
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

        public List<Film> SortFilmsByTitle()
        {
            List<Film> sortedByTitleFilms = GetAllFilms();
            SortingByTitleAlgorithm titleComparer = new SortingByTitleAlgorithm();
            sortedByTitleFilms.Sort(titleComparer);
            return sortedByTitleFilms;
        }

        public List<Film> SortFilmsByDuration()
        {
            List<Film> sortedByDurationFilms = GetAllFilms();
            SortingByDurationAlgorithm durationComparer = new SortingByDurationAlgorithm();
            sortedByDurationFilms.Sort(durationComparer);
            return sortedByDurationFilms;
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
                        Trailer = film.Trailer,
                        Comments = film.Comments.Select(c => new Comment()).ToList(),
                        Ratings=film.Ratings.Select(r => new Rating()).ToList(),
                        Rating = film.Rating
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
                .Include(u => u.Bookmarks)
                .Include(f=>f.Ratings)
                .FirstOrDefault(u => u.Id == userId);
            FilmDbModel currentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments)
                .Include(f=>f.Ratings)
                .FirstOrDefault(f => f.Id == filmId);
            CommentDbModel newComment = new CommentDbModel
            {
                User = currentUser,
                Film = currentFilm,
                Text = comment,
                Time = $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToShortTimeString()}"
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
                Text = c.Text,
                Time = c.Time
            }).ToList();
            return comments;
        }
        
        public void AddToBookmarks(int filmId, int userId)
        {
            var currentUser = _dbContext.FilmHubUsers.Include(u => u.Favourite)
                .Include(u => u.Comments)
                .Include(u => u.Bookmarks)
                .Include(f=>f.Ratings).FirstOrDefault(u => u.Id == userId);
            var currentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments)
                .Include(f=>f.Ratings)
                .FirstOrDefault(f => f.Id == filmId);
            currentUser.Bookmarks.Add(currentFilm);
            _dbContext.SaveChanges();
        }

        public void AddRating(int filmId, int userId, int generalImpression, int actorPlay, int scenario, int filming)
        {
            UserDbModel currentUser = _dbContext.FilmHubUsers.Include(u => u.Favourite)
                .Include(u => u.Comments)
                .Include(u => u.Bookmarks)
                .Include(f=>f.Ratings)
                .FirstOrDefault(u => u.Id == userId);
            FilmDbModel currentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments)
                .Include(f=>f.Ratings)
                .FirstOrDefault(f => f.Id == filmId);
            RatingDbModel newRating = new RatingDbModel()
            {
                User = currentUser,
                Film = currentFilm,
                GeneralImpression = generalImpression,
                ActorPlay = actorPlay,
                Scenario = scenario,
                Filming = filming
            };
            if (currentFilm.Ratings.Find(r => r.User == currentUser) != null)
            {
                RatingDbModel currentRating = _dbContext.Ratings.FirstOrDefault(rating => rating.User == currentUser && rating.Film == currentFilm);
                currentRating.Filming = newRating.Filming;
                currentRating.Scenario = newRating.Scenario;
                currentRating.ActorPlay = newRating.ActorPlay;
                currentRating.GeneralImpression = newRating.GeneralImpression;
                
                FilmDbModel film = _dbContext.FilmHubFilms.FirstOrDefault(f => f.Id==currentFilm.Id);
                RatingDbModel filmRating =film.Ratings.FirstOrDefault(r => r.User == currentUser);
                filmRating.Filming = newRating.Filming;
                filmRating.Scenario = newRating.Scenario;
                filmRating.ActorPlay = newRating.ActorPlay;
                filmRating.GeneralImpression = newRating.GeneralImpression;
            }
            else
            {
                currentFilm.Ratings.Add(newRating);
                currentUser.Ratings.Add(newRating);
                _dbContext.Ratings.Add(newRating);
            }

            UpdateRating(currentFilm.Id);
            _dbContext.SaveChanges();
        }


        public void UpdateRating(int filmId)
        {
            var currentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments)
                .Include(f=>f.Ratings)
                .FirstOrDefault(f => f.Id == filmId);
            var ratingValues=new List<int>();
            foreach (var rating in currentFilm.Ratings)
            {
                ratingValues.Add(rating.GeneralImpression);
                ratingValues.Add(rating.Scenario);
                ratingValues.Add(rating.ActorPlay);
                ratingValues.Add(rating.Filming);
            }

             double newRatingValue=ratingValues.Average();
             currentFilm.Rating = newRatingValue;
            _dbContext.SaveChanges();
        }

        public User.User FindById(int id)
        {
            UserDbModel user = _dbContext.FilmHubUsers.Include(u => u.Favourite)
                .Include(u => u.Comments)
                .Include(u => u.Bookmarks)
                .Include(f=>f.Ratings)
                .FirstOrDefault(u => u.Id == id);
            var u = new User.User
            {
                Email = user.Email, 
                FirstName = user.FirstName, 
                Password = user.Password,
                LastName = user.LastName,
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
                Favourite = user.Favourite.Select(f => new Database.Film.Film()
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
                    Trailer = f.Trailer,
                    Ratings=f.Ratings.Select(r => new Rating()).ToList(),
                    Rating = f.Rating
                }).ToList(),
                Bookmarks = user.Bookmarks.Select(f => new Database.Film.Film()
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
                    Trailer = f.Trailer,
                    Ratings=f.Ratings.Select(r => new Rating()).ToList(),
                    Rating = f.Rating

                }).ToList()
            }; 
            return u;
        }
    }
}