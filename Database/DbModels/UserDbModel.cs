using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.DbModels
{
    public class UserDbModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        [InverseProperty("UsersWhoAddToFavouritesList")]
        public List<FilmDbModel> Favourite { get; set; }
        
        [InverseProperty("UsersWhoAddToBookmarksList")]
        public List<FilmDbModel> Bookmarks { get; set; }
        public List<CommentDbModel> Comments { get; set; }
        //public List<UserDbModel> SimilarUsers { get; set; }
    }
}