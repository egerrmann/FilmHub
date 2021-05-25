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


        public List<User> GetAllUsers()
        {
            return _dbContext.FilmHubUsers.Select(u => new User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                Email = u.Email,
                Password = u.Password,
                LastName = u.LastName,
                Country = u.Country,
                DateOfBirth = u.DateOfBirth,
                Favourite = u.Favourite.Select(f => new Film.Film()).ToList(),
                Bookmarks = u.Bookmarks.Select(f => new Film.Film()).ToList(),
                Ratings = u.Ratings.Select(f => new Rating()).ToList(),
                //AdvisedFilms = u.AdvisedFilms.Select(f=>new Film.Film()).ToList()

            }).ToList();
        }

        public int Add(User model)
        {
            var dbModel = new UserDbModel()
            {
                FirstName = model.FirstName,
                Email = model.Email,
                Password = model.Password,
                LastName = model.LastName,
                Country = model.Country,
                DateOfBirth = model.DateOfBirth,
                Favourite = new List<FilmDbModel>(),
                Bookmarks = new List<FilmDbModel>(),
                Ratings = new List<RatingDbModel>(),
                AdvisedFilms =new List<FilmDbModel>()
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
                if (logInModel.Email == user.Email && logInModel.Password == user.Password)
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
                if (curr_user.Email == user.Email && curr_user.Password == user.Password)
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
                .Include(u => u.Bookmarks)
                .Include(u=>u.Ratings)
                .Include(u=>u.AdvisedFilms)
                .FirstOrDefault(u => u.Id == id);
            var u = new User
            {
                Email = user.Email, 
                FirstName = user.FirstName, 
                Password = user.Password,
                LastName = user.LastName,
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
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
                    Trailer = f.Trailer,
                    Rating = f.Rating,
                    
                }).ToList(),
                Bookmarks = user.Bookmarks.Select(f => new Film.Film()
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
                    Rating = f.Rating
                }).ToList(),
                AdvisedFilms = user.AdvisedFilms.Select(f => new Film.Film()
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
                    Rating = f.Rating
                }).ToList(),
                /*Ratings = user.Ratings.Select(r=>new Film.Rating()
                {
                    Scenario = r.Scenario,
                    
                    
                }).ToList()*/
            }; 
            return u;
        }

        public void AddToFavourite(string image, int id)
        {
            var currentUser = _dbContext.FilmHubUsers.Include(u => u.Favourite).
                Include(u => u.Comments)
                .Include(u => u.Bookmarks)
                .Include(u => u.Ratings)
                .Include(u=>u.AdvisedFilms)
                .FirstOrDefault(u => u.Id == id);
            var currentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments)
                .Include(f => f.Ratings).
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
                            Comments = dbFilm.Comments.Select(c => new Film.Comment()).ToList()
                            /*Ratings = dbFilm.Ratings.Select(r=>new Rating()).ToList()*/
                            Rating = dbFilm.Rating
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
                            Trailer = dbFilm.Trailer,
                            Comments = dbFilm.Comments.Select(c => new Film.Comment()).ToList()
                            /*Ratings = dbFilm.Ratings.Select(r=>new Rating()).ToList()*/
                            Rating = dbFilm.Rating
                        };
                        recommendedFilmsGenre.Add(film);
                    }
                }
            }
            return recommendedFilmsGenre;
        }

        public void EditProfile(string firstName, string lastName, string email, string dateOfBirth,
            string country, User currentUser)
        {
            int currentUserId = CurrentUser_Id(currentUser);
            UserDbModel user = _dbContext.FilmHubUsers.Find(currentUserId);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.DateOfBirth = dateOfBirth;
            user.Country = country;
            _dbContext.SaveChanges();
        }

        public void ChangeUserPassword(int id, string newPassword)
        {
            UserDbModel currentUser = _dbContext.FilmHubUsers.Find(id);
            currentUser.Password = newPassword;
            _dbContext.SaveChanges();
        }

        public int ChangedPasswordIsCorrect(int id, string oldPassword, string newPassword, string newPasswordRepeat)
        {
            UserDbModel currentUser = _dbContext.FilmHubUsers.Find(id);
            for (int i = 0; i < newPassword.Length; i++)
            {
                if (Char.IsDigit(newPassword[i]))
                {
                    if (newPassword.Length >= 8)
                    {
                        if (newPassword == newPasswordRepeat)
                        {
                            if (newPassword != oldPassword )
                            {
                                if (currentUser.Password == oldPassword)
                                {
                                    return 1;
                                }
                            }
                            return 2;    
                        }
                        return 3;
                    }
                    return 4;
                }   
            }
            
            return 5;
        }

        public List<User> SimilarUsers(int currentUserId)
        {
            List<User> similarUsers = new List<User>();
            UserDbModel currentUser = _dbContext.FilmHubUsers.Find(currentUserId);
            int sameFilms = 0;
            foreach (var anotherUser in _dbContext.FilmHubUsers.Include(u => u.Bookmarks)
                .Include(u => u.Comments)
                .Include(u => u.Favourite)
                .Include(u => u.Ratings)
                /*.Include(u=>u.AdvisedFilms)*/)
                
            {
                sameFilms = 0;
                if (anotherUser.Id != currentUserId)
                {
                    foreach (var currentUserFav in currentUser.Favourite)
                    {
                        foreach (var anotherUserFav in anotherUser.Favourite)
                        {
                            if (currentUserFav == anotherUserFav)
                            {
                                sameFilms += 1;
                            }
                        }
                    }

                    if (sameFilms >= 2)
                    {
                        User similarUser = new User
                        {
                            Email = anotherUser.Email,
                            FirstName = anotherUser.FirstName,
                            Password = anotherUser.Password,
                            LastName = anotherUser.LastName,
                            Country = anotherUser.Country,
                            DateOfBirth = anotherUser.DateOfBirth,
                            Favourite = anotherUser.Favourite.Select(f => new Film.Film()
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
                                Rating = f.Rating
                            }).ToList(),
                            Bookmarks = anotherUser.Bookmarks.Select(f => new Film.Film()
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
                                Rating = f.Rating

                            }).ToList()
                            /*Ratings = user.Ratings.Select(r=>new Film.Rating()
           {
               Scenario = r.Scenario,
               
           }).ToList()*/
                        };
                        similarUsers.Add(similarUser);
                    }
                }
                else
                {
                    continue;
                }
            }

            return similarUsers;
        }

        public User FindByEmail(string userEmail)
        {
            UserDbModel user = _dbContext.FilmHubUsers.Include(u => u.Favourite)
                .Include(u => u.Comments)
                .Include(u => u.Bookmarks)
                .Include(u => u.Ratings)
                .Include(u=>u.AdvisedFilms)
                .FirstOrDefault(u => u.Email == userEmail);
            var u = new User
            {
                Email = user.Email, 
                FirstName = user.FirstName, 
                Password = user.Password,
                LastName = user.LastName,
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
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
                    Trailer = f.Trailer,
                    Rating = f.Rating

                }).ToList(),
                Bookmarks = user.Bookmarks.Select(f => new Film.Film()
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
                    Rating = f.Rating
                }).ToList(),
                  AdvisedFilms = user.AdvisedFilms.Select(f => new Film.Film()
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
                                    Rating = f.Rating
                                }).ToList(),

            Ratings = user.Ratings.Select(r=>new Rating()
           {
               GeneralImpression=r.GeneralImpression,
               Scenario = r.Scenario,
               ActorPlay = r.ActorPlay,
               Filming = r.Filming
               /*User = new UserDbModel()
               {
                   FirstName = r.User.FirstName
               }*/
           }).ToList()
            }; 
            return u;
        }

        public bool IsExpert(int currentUserId)
        {
            
            UserDbModel currentUser = _dbContext.FilmHubUsers.Find(currentUserId);
            if (currentUser.IsExpert)
            {
                return true;
            }
            return false;
        }

        public void MakeAnExpert(int currentUserId)
        {
            List<int> amountOfComments=new List<int>();
            foreach (var user in _dbContext.FilmHubUsers)
            {
                amountOfComments.Add(user.Comments.Count);
            }

            int averageCommentsAmount = (int) amountOfComments.Average();
            UserDbModel currentUser = _dbContext.FilmHubUsers.Find(currentUserId);

            if (currentUser.Comments.Count >= averageCommentsAmount)
            {
                currentUser.IsExpert = true;
            }
            _dbContext.SaveChanges();

        }

        public void AddToAdvised(int currentFilmId, int userIdToAdvise)
        {
            UserDbModel currentUser = _dbContext.FilmHubUsers.Include(u => u.Favourite)
                .Include(u => u.Comments)
                .Include(u => u.Bookmarks)
                .Include(u => u.Ratings)
                .Include(u=>u.AdvisedFilms).FirstOrDefault(u => u.Id == userIdToAdvise);
            FilmDbModel currentFilm = _dbContext.FilmHubFilms.Include(f => f.Comments)
                .Include(f=>f.Ratings)
                .FirstOrDefault(f => f.Id == currentFilmId);
            currentUser.AdvisedFilms.Add(currentFilm);
            _dbContext.SaveChanges();
        }
    }
}