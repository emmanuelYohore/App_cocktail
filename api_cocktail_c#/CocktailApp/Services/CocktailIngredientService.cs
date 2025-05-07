using CocktailApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailApp.Services
{
    public class CocktailIngredientService
    {
        private readonly IMongoCollection<CocktailIngredient> _cocktailIngredients;

        public CocktailIngredientService(IOptions<CocktailDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _cocktailIngredients = database.GetCollection<CocktailIngredient>(settings.Value.CocktailIngredientsCollectionName);
        }

        public async Task<List<CocktailIngredient>> GetAsync() =>
            await _cocktailIngredients.Find(_ => true).ToListAsync();

        public async Task<CocktailIngredient> GetByIdAsync(string id) =>
            await _cocktailIngredients.Find(ci => ci.CocktailIngredientId == id).FirstOrDefaultAsync();

        public async Task<CocktailIngredient> CreateAsync(CocktailIngredient cocktailIngredient)
        {
            await _cocktailIngredients.InsertOneAsync(cocktailIngredient);
            return cocktailIngredient;
        }

        public async Task UpdateAsync(string id, CocktailIngredient cocktailIngredientIn) =>
            await _cocktailIngredients.ReplaceOneAsync(ci => ci.CocktailIngredientId == id, cocktailIngredientIn);

        public async Task RemoveAsync(string id) =>
            await _cocktailIngredients.DeleteOneAsync(ci => ci.CocktailIngredientId == id);
    }
}
