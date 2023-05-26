using Microsoft.EntityFrameworkCore;
using NetflixRepository.Entity;

namespace NetflixRepository
{
    public class MovieDBContext:DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<UserMovie> UserMovies { get; set; }


        public MovieDBContext(DbContextOptions<MovieDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optional: Configure entity relationships, constraints, etc.
            base.OnModelCreating(modelBuilder);
        }
    }
}
