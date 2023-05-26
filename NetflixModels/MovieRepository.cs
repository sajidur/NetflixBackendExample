using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetflixRepository.Entity;

namespace NetflixRepository
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAllMovies(int userId);
        Movie GetMovieById(int id);
    }
    internal class MovieRepository : IMovieRepository
    {
        private readonly MovieDBContext _context;
        public MovieRepository(MovieDBContext context)
        {
            _context = context;
        }
        public IEnumerable<Movie> GetAllMovies(int userId)
        {
            var movies = _context.Movies
     .Join(
         _context.UserMovies,
         m => m.Id,
         um => um.MovieId,
         (m, um) => new { Movie = m, UserMovie = um }
     )
     .Where(x => x.UserMovie.UserId == userId)
     .Select(x => x.Movie);
            return movies;
        }

        public Movie GetMovieById(int id)
        {
            return _context.Movies.Find(id);
        }
    }
}
