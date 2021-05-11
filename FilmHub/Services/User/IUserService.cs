using System.Collections.Generic;
using Database.DbModels;
using FilmHub.Models;

namespace FilmHub.Services.User
{
    public interface IUserService
    {
        int Add(Database.User.User model);
        int Count();
        bool Find(Database.User.User logInModel);
        int CurrentUser_Id(Database.User.User current_user);
        Database.User.User FindById(int id);
        void AddToFavourite(string image, int id);
        public List<Database.Film.Film> RecommendedFilmsDirector(int id);
        public List<Database.Film.Film> RecommendedFilmsGenre(int id);
        public void ChangeUserPassword(int id, string newPassword);
    }
}