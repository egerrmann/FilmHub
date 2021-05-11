using System;
using System.Collections.Generic;
using System.Linq;
using Database.DbModels;
using Database.Film;
using Microsoft.EntityFrameworkCore;

namespace Database.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IAppContext _dbContext;
        

        public UserRepository(IAppContext data)
        {
            _dbContext = data;
        }
        

        public int Add(User model)
        {
            var dbModel = new UserDbModel()
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Favourite = new List<FilmDbModel>(),
                /*Bookmarks = new List<FilmDbModel>()*/
            };
            _dbContext.FilmHubUsers.Add(dbModel);
            _dbContext.SaveChanges();
            return dbModel.Id;
        }

        public int Count()
        {
            var count = _dbContext.FilmHubUsers.Count();
            return count;
        }

        public bool Find(User logInModel)
        {
            foreach (var user in _dbContext.FilmHubUsers)
            {
                if (logInModel.Name == user.Name && logInModel.Password == user.Password)
                {
                    return true;
                }
            }
            return false;
        }

        public int CurrentUser_Id(User curr_user)
        {
            foreach (var user in _dbContext.FilmHubUsers)
            {
                if (curr_user.Name == user.Name && curr_user.Password == user.Password)
                {
                    return user.Id;
                }
            }

            return -1;
        }

        public User FindById(int id)
        {
            UserDbModel user = _dbContext.FilmHubUsers.Include(u => u.Favourite)
                .Include(u => u.Comments)
                /*.Include(u => u.Bookmarks)*/
                .FirstOrDefault(u => u.Id == id);
            var u = new User
            {
                Email = user.Email, 
                Name = user.Name, 
                Password = user.Password,
                Favourite = user.Favourite.Select(f => new Film.Film()
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
                }).ToList(),
                /*Bookmarks = user.Bookmarks.Select(f => new Film.Film()
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
                }).ToList()*/
            }; 
            return u;
        }

        public void AddToFavourite(string image, int id)
        {
            var currentUser = _dbContext.FilmHubUsers.Include(u => u.Favourite).
                Include(u => u.Comments)
                /*.Include(u => u.Bookmarks)*/.FirstOrDefault(u => u.Id == id);
            var currentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments).
                FirstOrDefault(f => f.Image == image);
            currentUser.Favourite.Add(currentFilm);
            _dbContext.SaveChanges();
        }

        public List<Film.Film> RecommendedFilmsDirector(int id)
        {
            List<Film.Film> recommendedFilmsDirector = new List<Film.Film>();
            User user = FindById(id);
            foreach (var favouriteFilm in user.Favourite)
            {
                foreach (var dbFilm in _dbContext.FilmHubFilms)
                {
                    if (dbFilm.Director == favouriteFilm.Director && user.Favourite.TrueForAll(f => f.Image != dbFilm.Image))
                    {
                        Film.Film film = new Film.Film
                        {
                            Title = dbFilm.Title,
                            Year = dbFilm.Year,
                            Genre = dbFilm.Genre,
                            Director = dbFilm.Director,
                            Summary = dbFilm.Summary,
                            Time = dbFilm.Time,
                            Age = dbFilm.Age,
                            Country = dbFilm.Country,
                            Actors = dbFilm.Actors,
                            Image = dbFilm.Image,
                            Comments = dbFilm.Comments.Select(c => new Comment()).ToList()
                        };
                        recommendedFilmsDirector.Add(film);
                    }
                }
            }

            return recommendedFilmsDirector;
        }
        
        public List<Film.Film> RecommendedFilmsGenre(int id)
        {
            List<Film.Film> recommendedFilmsGenre = new List<Film.Film>();
            User user = FindById(id);
            foreach (var favouriteFilm in user.Favourite)
            {
                foreach (var dbFilm in _dbContext.FilmHubFilms)
                {
                    if (dbFilm.Genre == favouriteFilm.Genre && user.Favourite.TrueForAll(f => f.Image != dbFilm.Image))
                    {
                        Film.Film film = new Film.Film
                        {
                            Title = dbFilm.Title,
                            Year = dbFilm.Year,
                            Genre = dbFilm.Genre,
                            Director = dbFilm.Director,
                            Summary = dbFilm.Summary,
                            Time = dbFilm.Time,
                            Age = dbFilm.Age,
                            Country = dbFilm.Country,
                            Actors = dbFilm.Actors,
                            Image = dbFilm.Image,
                            Comments = dbFilm.Comments.Select(c => new Comment()).ToList()
                        };
                        recommendedFilmsGenre.Add(film);
                    }
                }
            }
            return recommendedFilmsGenre;
        }

        public void ChangeUserPassword(int id, string newPassword)
        {
            UserDbModel currentUser = _dbContext.FilmHubUsers.Find(id);
            currentUser.Password = newPassword;
            _dbContext.SaveChanges();
        }

        /*public List<User> UsersWithSimilarFavourites(int currentUserId)
        {
            throw new NotImplementedException();
        }*/
        
        
    }
}