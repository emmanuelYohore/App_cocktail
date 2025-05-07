using CocktailApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailApp.Services
{
    public class RatingService
    {
        private readonly IMongoCollection<Rating> _ratings;

        public RatingService(IOptions<CocktailDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _ratings = database.GetCollection<Rating>(settings.Value.RatingsCollectionName);
        }

        public async Task<List<Rating>> GetAsync() =>
            await _ratings.Find(_ => true).ToListAsync();

        public async Task<Rating> GetByIdAsync(string id) =>
            await _ratings.Find(r => r.RatingId == id).FirstOrDefaultAsync();

        public async Task<Rating> CreateAsync(Rating rating)
        {
            await _ratings.InsertOneAsync(rating);
            return rating;
        }

        public async Task UpdateAsync(string id, Rating ratingIn) =>
            await _ratings.ReplaceOneAsync(r => r.RatingId == id, ratingIn);

        public async Task RemoveAsync(string id) =>
            await _ratings.DeleteOneAsync(r => r.RatingId == id);
    }
}
