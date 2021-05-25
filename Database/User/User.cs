using System.Collections.Generic;
using Database.DbModels;
using Database.Film;

namespace Database.User
{
    public class User
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string DateOfBirth { get; set; }
        public List<Film.Film> Favourite { get; set; }
        public List<Film.Film> Bookmarks { get; set; }
        public List<Film.Comment> Comments { get; set; }
    }
}