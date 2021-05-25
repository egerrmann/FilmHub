using Database.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public interface IAppContext
    {
        public DbSet<UserDbModel> FilmHubUsers { get; set; }
        public DbSet<FilmDbModel> FilmHubFilms { get; set; }
        
        public DbSet<CommentDbModel> Comments { get; set; }
        public DbSet<RatingDbModel> Ratings { get; set; }
        public void SaveChanges();
    }
}