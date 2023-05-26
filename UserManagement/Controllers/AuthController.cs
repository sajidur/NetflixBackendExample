using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementRepository.Entity;
using UserManagementService;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login(User model)
        {
            var user = _userService.ValidateUser(model.Username, model.Password);

            if (user == null)
            {
                return Unauthorized(); // Invalid credentials
            }

            var token = _userService.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }
    }
}
