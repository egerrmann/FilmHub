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
    }
}