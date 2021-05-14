using System;
using System.Collections.Generic;

namespace Database.DbModels
{
    public class FilmDbModel
    {
        public int Id { get; set; }
        public List<UserDbModel> UsersWhoAddToFavouritesList { get; set; }
        public List<UserDbModel> UsersWhoAddToBookmarksList { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Summary { get; set; }
        public string Time { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public List<string> Actors { get; set; }
        public string Image { get; set; }
        public List<CommentDbModel> Comments { get; set; } 
    }
}
