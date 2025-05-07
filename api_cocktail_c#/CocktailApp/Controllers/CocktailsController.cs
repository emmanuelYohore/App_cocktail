using CocktailApp.Models;
using CocktailApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CocktailsController : ControllerBase
    {
        private readonly CocktailService _cocktailService;
        private readonly ImageService _imageService;

        public CocktailsController(CocktailService cocktailService, ImageService imageService)
        {
            _cocktailService = cocktailService;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cocktail>>> Get() =>
            await _cocktailService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Cocktail>> Get(string id)
        {
            var cocktail = await _cocktailService.GetByIdAsync(id);
            if (cocktail == null) return NotFound();
            return cocktail;
        }

        [HttpPost]
        public async Task<ActionResult<Cocktail>> Create([FromForm] CocktailCreateRequest request)
        {
            try
            {
                var cocktail = new Cocktail
                {
                    Name = request.Name,
                    Description = request.Description,
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Traiter l'image si elle existe
                if (request.CocktailImage != null && request.CocktailImage.Length > 0)
                {
                    string imagePath = await _imageService.SaveImageAsync(request.CocktailImage);
                    cocktail.CocktailImage = imagePath;
                }

                await _cocktailService.CreateAsync(cocktail);
                return CreatedAtAction(nameof(Get), new { id = cocktail.CocktailId }, cocktail);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] CocktailUpdateRequest request)
        {
            var cocktail = await _cocktailService.GetByIdAsync(id);
            if (cocktail == null) return NotFound();

            try
            {
                
                cocktail.Name = request.Name ?? cocktail.Name;
                cocktail.Description = request.Description ?? cocktail.Description;
                cocktail.UpdatedAt = DateTime.UtcNow;

               
                if (request.CocktailImage != null && request.CocktailImage.Length > 0)
                {
                    // Supprimer l'ancienne image
                    if (!string.IsNullOrEmpty(cocktail.CocktailImage))
                    {
                        _imageService.DeleteImage(cocktail.CocktailImage);
                    }

                    // Enregistrer la nouvelle image
                    string imagePath = await _imageService.SaveImageAsync(request.CocktailImage);
                    cocktail.CocktailImage = imagePath;
                }

                await _cocktailService.UpdateAsync(id, cocktail);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var cocktail = await _cocktailService.GetByIdAsync(id);
            if (cocktail == null) return NotFound();

            
            if (!string.IsNullOrEmpty(cocktail.CocktailImage))
            {
                _imageService.DeleteImage(cocktail.CocktailImage);
            }

            await _cocktailService.RemoveAsync(id);
            return NoContent();
        }
    }
}
