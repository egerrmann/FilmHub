using System.Collections.Generic;
using Database.Film;

namespace FilmHub.Models
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Film> Favourite { get; set; }
    }
}