using System.Collections.Generic;
using System.Linq;
using Database.User;
using FilmHub.Models;

namespace FilmHub.Services.User
{
    public class UserService : IUserService
    {
        public List<Database.User.User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int Add(Database.User.User model)
        {
            return _userRepository.Add(model);
        }

        public int Count()
        {
            return _userRepository.Count();
        }

        public bool Find(Database.User.User logInModel)
        {
            return _userRepository.Find(logInModel);
        }

        public int CurrentUser_Id(Database.User.User current_user)
        {
            return _userRepository.CurrentUser_Id(current_user);
        }

        public Database.User.User FindById(int id)
        {
            return _userRepository.FindById(id);
        }

        public void AddToFavourite(string image, int id)
        {
            _userRepository.AddToFavourite(image, id);
        }

        public List<Database.Film.Film> RecommendedFilmsDirector(int id)
        {
            return _userRepository.RecommendedFilmsDirector(id);
        }
        public List<Database.Film.Film> RecommendedFilmsGenre(int id)
        {
            return _userRepository.RecommendedFilmsGenre(id);
        }

        public void EditProfile(string firstName, string lastName, string email, string dateOfBirth,
            string country, Database.User.User currentUser)
        {
            _userRepository.EditProfile(firstName, lastName, email, dateOfBirth, country,
                currentUser);
        }

        public void ChangeUserPassword(int id, string newPassword)
        {
            _userRepository.ChangeUserPassword(id, newPassword);
        }

        public List<Database.User.User> SimilarUsers(int currentUserId)
        {
            return _userRepository.SimilarUsers(currentUserId);
        }

        public Database.User.User FindByEmail(string userEmail)
        {
            return _userRepository.FindByEmail(userEmail);
        }

        public bool IsExpert(int currentUserId)
        {
            return _userRepository.IsExpert(currentUserId);
        }

        public int ChangedPasswordIsCorrect(int id, string oldPassword, string newPassword, string newPasswordRepeat)
        {
            return _userRepository.ChangedPasswordIsCorrect(id, oldPassword, newPassword,  newPasswordRepeat);
        }

        public void AddToAdvised(int currentFilmId, int userIdToAdvise)
        {
            _userRepository.AddToAdvised(currentFilmId,userIdToAdvise);
        }

        public void MakeAnExpert(int currentUserId)
        {
            _userRepository.MakeAnExpert(currentUserId);
        }

        static string ErrorMessage;
    }
}