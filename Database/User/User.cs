using System.Collections.Generic;
using Database.DbModels;

namespace Database.User
{
    public class User
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Film.Film> Favourite { get; set; }
    }
}