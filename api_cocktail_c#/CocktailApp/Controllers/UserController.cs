using CocktailApp.Models;
using CocktailApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get() =>
            await _userService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
         
            user.SetPassword(user.Password);

            await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User userIn)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

         
            if (!string.IsNullOrEmpty(userIn.Password))
            {
                userIn.SetPassword(userIn.Password);
            }

            userIn.UserId = id;
            await _userService.UpdateAsync(id, userIn);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            await _userService.RemoveAsync(id);
            return NoContent();
        }
    }
}