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
    public class IngredientsController : ControllerBase
    {
        private readonly IngredientService _ingredientService;

        public IngredientsController(IngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Ingredient>>> Get() =>
            await _ingredientService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> Get(string id)
        {
            var ingredient = await _ingredientService.GetByIdAsync(id);
            if (ingredient == null) return NotFound();
            return ingredient;
        }

        

        [HttpPost]
        public async Task<ActionResult<Ingredient>> Create(Ingredient ingredient)
        {
           
            if (ingredient.Name == null || !ingredient.Name.Any())
                return BadRequest("Au moins un nom d'ingrédient est obligatoire.");

            if (string.IsNullOrWhiteSpace(ingredient.Category))
                return BadRequest("La catégorie est obligatoire.");

            if (ingredient.Quantity <= 0)
                return BadRequest("La quantité doit être supérieure à 0.");

            if (string.IsNullOrWhiteSpace(ingredient.Unit))
                return BadRequest("L'unité est obligatoire.");

            
            ingredient.CreatedAt = DateTime.UtcNow;
            ingredient.UpdatedAt = DateTime.UtcNow;

            await _ingredientService.CreateAsync(ingredient);
            return CreatedAtAction(nameof(Get), new { id = ingredient.IngredientId }, ingredient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Ingredient ingredientIn)
        {
            var ingredient = await _ingredientService.GetByIdAsync(id);
            if (ingredient == null) return NotFound();

            
            if (ingredientIn.Name == null || !ingredientIn.Name.Any())
                return BadRequest("Au moins un nom d'ingrédient est obligatoire.");

            if (string.IsNullOrWhiteSpace(ingredientIn.Category))
                return BadRequest("La catégorie est obligatoire.");

            if (ingredientIn.Quantity <= 0)
                return BadRequest("La quantité doit être supérieure à 0.");

            if (string.IsNullOrWhiteSpace(ingredientIn.Unit))
                return BadRequest("L'unité est obligatoire.");

           
            ingredientIn.IngredientId = id;
            ingredientIn.CreatedAt = ingredient.CreatedAt; 
            ingredientIn.UpdatedAt = DateTime.UtcNow; 

            await _ingredientService.UpdateAsync(id, ingredientIn);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ingredient = await _ingredientService.GetByIdAsync(id);
            if (ingredient == null) return NotFound();
            
            await _ingredientService.RemoveAsync(id);
            return NoContent();
        }

        

        
    }
}