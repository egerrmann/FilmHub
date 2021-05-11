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
        List<Film.Film> RecommendedFilmsDirector(int id);
        List<Film.Film> RecommendedFilmsGenre(int id);
        void ChangeUserPassword(int id, string newPassword);
        //List<User> UsersWithSimilarFavourites(int currentUserId);
    }
}