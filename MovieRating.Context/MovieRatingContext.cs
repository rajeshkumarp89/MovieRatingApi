using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieRatings.Context.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieRatings.Context
{
    public class MovieRatingContext : DbContext
    {
        private readonly IConfiguration config;

        public MovieRatingContext(DbContextOptions<MovieRatingContext> options)
         : base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MovieRating> MovieRatings { get; set; }
        public virtual DbSet<MovieGenre> MovieGenres { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieRating>().HasKey(s => new { s.MovieId, s.UserId });
            modelBuilder.Entity<MovieGenre>().HasKey(s => new { s.MovieId, s.GenreId });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("IsActive").CurrentValue = true;
                    entry.Property("DateCreated").CurrentValue = DateTime.Now;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
