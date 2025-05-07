using CocktailApp.Models;
using CocktailApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CocktailApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly RatingService _ratingService;

        public RatingsController(RatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Rating>>> Get([FromQuery] string? cocktailId = null)
        {
            if (!string.IsNullOrEmpty(cocktailId))
            {
               
                var allRatings = await _ratingService.GetAsync();
                var filteredRatings = allRatings.Where(r => r.CocktailId == cocktailId).ToList();
                return filteredRatings;
            }

            
            return await _ratingService.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetById(string id)
        {
            var rating = await _ratingService.GetByIdAsync(id);
            if (rating == null) return NotFound();
            return rating;
        }

        [HttpPost]
        public async Task<ActionResult<Rating>> Create(Rating rating)
        {
          
            if (string.IsNullOrEmpty(rating.UserId))
                return BadRequest("L'identifiant utilisateur est requis.");

            if (string.IsNullOrEmpty(rating.CocktailId))
                return BadRequest("L'identifiant du cocktail est requis.");

            if (rating.Value < 1 || rating.Value > 5)
                return BadRequest("La note doit être comprise entre 1 et 5.");

           
            if (rating.CreatedAt == default)
                rating.CreatedAt = DateTime.UtcNow;

            if (rating.UpdatedAt == default)
                rating.UpdatedAt = DateTime.UtcNow;

            await _ratingService.CreateAsync(rating);
            return CreatedAtAction(nameof(GetById), new { id = rating.RatingId }, rating);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Rating ratingIn)
        {
            var rating = await _ratingService.GetByIdAsync(id);
            if (rating == null) return NotFound();
            ratingIn.RatingId = id;
            await _ratingService.UpdateAsync(id, ratingIn);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var rating = await _ratingService.GetByIdAsync(id);
            if (rating == null) return NotFound();
            await _ratingService.RemoveAsync(id);
            return NoContent();
        }
    }
}
