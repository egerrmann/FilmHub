using System;
using Database.DbModels;

namespace Database.Film
{
    public class Comment
    {
        public int Id { get; set; }
        public UserDbModel User { get; set; }
        public FilmDbModel Film { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }
        public int Likes { get; set; }
    }
}