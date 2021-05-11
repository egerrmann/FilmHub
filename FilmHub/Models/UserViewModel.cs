using System.Collections.Generic;
using Database.Film;
using Database.User;

namespace FilmHub.Models
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Film> Favourite { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Film> Bookmarks { get; set; }
    }
}