using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixRepository.Entity
{
    public class UserMovie
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public DateTime WatchedDate { get; set; } // Date when the user watched the movie
                                                  // Additional user-movie relationship properties (e.g., rating, review, etc.)
    }
}
