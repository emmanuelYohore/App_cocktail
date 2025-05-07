using CocktailApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailApp.Services
{
    public class CocktailService
    {
        private readonly IMongoCollection<Cocktail> _cocktails;

        public CocktailService(IOptions<CocktailDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _cocktails = database.GetCollection<Cocktail>(settings.Value.CocktailsCollectionName);
        }

        public async Task<List<Cocktail>> GetAsync() =>
            await _cocktails.Find(_ => true).ToListAsync();

        public async Task<Cocktail> GetByIdAsync(string id) =>
            await _cocktails.Find(c => c.CocktailId == id).FirstOrDefaultAsync();

        public async Task<Cocktail> CreateAsync(Cocktail cocktail)
        {
            await _cocktails.InsertOneAsync(cocktail);
            return cocktail;
        }

        public async Task UpdateAsync(string id, Cocktail cocktailIn) =>
            await _cocktails.ReplaceOneAsync(c => c.CocktailId == id, cocktailIn);

        public async Task RemoveAsync(string id) =>
            await _cocktails.DeleteOneAsync(c => c.CocktailId == id);
    }
}
