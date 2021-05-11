using System.Collections.Generic;
using Database.DbModels;

namespace Database.User
{
    public interface IUserRepository
    {
        int Add(User model);

        int Count();
        bool Find(User model);
        int CurrentUser_Id(User user);
        User FindById(int id);
        void AddToFavourite(string image, int id);
    }
}