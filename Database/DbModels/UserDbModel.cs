using System.Collections.Generic;

namespace Database.DbModels
{
    public class UserDbModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<FilmDbModel> Favourite { get; set; }
    }
}