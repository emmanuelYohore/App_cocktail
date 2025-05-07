using CocktailApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailApp.Services
{
    public class IngredientService
    {
        private readonly IMongoCollection<Ingredient> _ingredients;

        public IngredientService(IOptions<CocktailDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _ingredients = database.GetCollection<Ingredient>(settings.Value.IngredientsCollectionName);
        }

        public async Task<List<Ingredient>> GetAsync() =>
            await _ingredients.Find(_ => true).ToListAsync();

        public async Task<Ingredient> GetByIdAsync(string id) =>
            await _ingredients.Find(i => i.IngredientId == id).FirstOrDefaultAsync();

        public async Task<Ingredient> CreateAsync(Ingredient ingredient)
        {
            await _ingredients.InsertOneAsync(ingredient);
            return ingredient;
        }

        public async Task UpdateAsync(string id, Ingredient ingredientIn) =>
            await _ingredients.ReplaceOneAsync(i => i.IngredientId == id, ingredientIn);

        public async Task RemoveAsync(string id) =>
            await _ingredients.DeleteOneAsync(i => i.IngredientId == id);
    }
}
