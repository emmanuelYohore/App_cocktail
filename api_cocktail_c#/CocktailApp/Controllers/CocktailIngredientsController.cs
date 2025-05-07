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
    public class CocktailIngredientsController : ControllerBase
    {
        private readonly CocktailIngredientService _cocktailIngredientService;
        private readonly IngredientService _ingredientService;

        public CocktailIngredientsController(CocktailIngredientService cocktailIngredientService, IngredientService ingredientService)
        {
            _cocktailIngredientService = cocktailIngredientService;
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CocktailIngredient>>> Get() =>
            await _cocktailIngredientService.GetAsync();

        [HttpGet("cocktail/{cocktailId}")]
        public async Task<ActionResult<List<Ingredient>>> GetIngredientsByCocktailId(string cocktailId)
        {
            var cocktailIngredients = await _cocktailIngredientService.GetAsync();
            var relevantCIs = cocktailIngredients.Where(ci => ci.CocktailId == cocktailId).ToList();

            if (!relevantCIs.Any())
                return new List<Ingredient>();

            var ingredients = new List<Ingredient>();
            foreach (var ci in relevantCIs)
            {
                var ingredient = await _ingredientService.GetByIdAsync(ci.IngredientId);
                if (ingredient != null)
                    ingredients.Add(ingredient);
            }

            return ingredients;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CocktailIngredient>> Get(string id)
        {
            var ci = await _cocktailIngredientService.GetByIdAsync(id);
            if (ci == null) return NotFound();
            return ci;
        }

        [HttpPost]
        public async Task<ActionResult<CocktailIngredient>> Create(CocktailIngredient ci)
        {
            await _cocktailIngredientService.CreateAsync(ci);
            return CreatedAtAction(nameof(Get), new { id = ci.CocktailIngredientId }, ci);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, CocktailIngredient ciIn)
        {
            var ci = await _cocktailIngredientService.GetByIdAsync(id);
            if (ci == null) return NotFound();
            ciIn.CocktailIngredientId = id;
            await _cocktailIngredientService.UpdateAsync(id, ciIn);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ci = await _cocktailIngredientService.GetByIdAsync(id);
            if (ci == null) return NotFound();
            await _cocktailIngredientService.RemoveAsync(id);
            return NoContent();
        }
    }
}
