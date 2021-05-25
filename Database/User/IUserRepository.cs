using System.Collections.Generic;
using Database.DbModels;

namespace Database.User
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        int Add(User model);
        int Count();
        bool Find(User model);
        int CurrentUser_Id(User user);
        User FindById(int id);
        void AddToFavourite(string image, int id);
        List<Film.Film> RecommendedFilmsDirector(int id);
        List<Film.Film> RecommendedFilmsGenre(int id);
        void EditProfile(string firstName, string lastName, string email, string dateOfBirth,
            string country, User currentUser);
        void ChangeUserPassword(int id, string newPassword);
        int ChangedPasswordIsCorrect (int id, string oldPassword, string newPassword, string newPasswordRepeat);
        List<User> SimilarUsers(int currentUserId);
        User FindByEmail(string userEmail);
        bool IsExpert(int currentUserId);
        void MakeAnExpert(int currentUserId);
        void AddToAdvised(int currentFilmId, int userIdToAdvise);
    }
}