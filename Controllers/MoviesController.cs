using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NetflixRepository;

namespace NetflixBackendExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly CloudBlobContainer _container;

        public MoviesController(IConfiguration configuration, IMovieRepository movieRepository)
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
        }

        [HttpGet]
        public IActionResult GetAllMovies()
        {
            try
            {
                IEnumerable<Movie> movies = _movieRepository.GetAllMovies();
                return Ok(movies);
            }
            catch (Exception ex)
            {
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
                var sasToken = GetMovieSasToken(movie.FileName);

                // Create a URI with the SAS token for the movie file
                var movieUri = $"{movie.FileName}{sasToken}";

                return Ok(movieUri);
            }
            catch (Exception ex)
            {
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
