using System;
using System.Collections.Generic;
using Database.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public sealed class AppContext : DbContext, IAppContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
            
        }
        
        public DbSet<UserDbModel> FilmHubUsers { get; set; }
        public DbSet<FilmDbModel> FilmHubFilms { get; set; }
        public DbSet<CommentDbModel> Comments { get; set; }
        public void SaveChanges()
        {
            base.SaveChanges();
        }

        public AppContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Filmhub;Username=postgres;Password=5432");
        }
    }
}