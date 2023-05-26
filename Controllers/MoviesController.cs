using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NetflixRepository;
using NetflixRepository.Entity;

namespace NetflixBackendExample.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly CloudBlobContainer _container;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IConfiguration configuration, IMovieRepository movieRepository, ILogger<MoviesController> logger)
        {
            _movieRepository = movieRepository;

            // Retrieve Azure Storage connection string from configuration
            string storageConnectionString = configuration.GetConnectionString("AzureStorage");

            // Create a CloudStorageAccount object from the connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            // Create a CloudBlobClient object to interact with the Blob storage
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get reference to the container where movie files are stored
            _container = blobClient.GetContainerReference("movies");
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllMovies(int userId)
        {
            try
            {
                IEnumerable<Movie> movies = _movieRepository.GetAllMovies(userId);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetMovieById(int id)
        {
            try
            {
                Movie movie = _movieRepository.GetMovieById(id);

                if (movie == null)
                {
                    return NotFound();
                }

                // Generate a SAS (Shared Access Signature) token for the movie file
                var sasToken = GetMovieSasToken(movie.Title);

                // Create a URI with the SAS token for the movie file
                var movieUri = $"{movie.Title}{sasToken}";

                return Ok(movieUri);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                return StatusCode(500, ex.Message);
            }
        }

        private string GetMovieSasToken(string fileName)
        {
            // Get reference to the movie blob
            CloudBlockBlob blob = _container.GetBlockBlobReference(fileName);

            // Generate a SAS (Shared Access Signature) token with read permission
            var sasToken = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1) // Set the expiry time for the token
            });

            return sasToken;
        }
    }
}
