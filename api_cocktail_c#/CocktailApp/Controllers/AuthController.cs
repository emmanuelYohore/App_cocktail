using CocktailApp.Models;
using CocktailApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CocktailApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;

        public AuthController(AuthService authService, UserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<dynamic>> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await _authService.AuthenticateAsync(loginRequest);

            if (token == null)
            {
                return Unauthorized(new { message = "Email ou mot de passe incorrect" });
            }

            var user = await _userService.GetByEmailAsync(loginRequest.Email);

            return new
            {
                user.UserId,
                user.FirstName,
                user.LastName,
                user.Email,
                token
            };
        }

        //logout
        [HttpPost("logout")]
        public async Task<ActionResult<dynamic>> Logout([FromBody] LogoutRequest logoutRequest)
        {
            var result = await _authService.LogoutAsync(logoutRequest);
            
            if (!result)
            {
                return BadRequest(new { message = "Logout failed" });
            }

            return Ok(new { message = "Logout successful" });
        }

    }
}