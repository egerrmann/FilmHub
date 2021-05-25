using System.Collections.Generic;
using Database.Film;
using Database.User;

namespace FilmHub.Models
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public List<Film> Favourite { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string DateOfBirth { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Film> Bookmarks { get; set; }
        public List<Rating> Ratings { get; set; }
        public bool IsExpert { get; set; }
        public List<Film> AdvisedFilms { get; set; }

    }
}