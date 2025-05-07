using CocktailApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailApp.Services
{
    public class UserService
    {
        
        private readonly IMongoCollection<User> _users;

        public UserService(IOptions<CocktailDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>(settings.Value.UsersCollectionName);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }
        public async Task<List<User>> GetAsync() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task<User> GetByIdAsync(string id) =>
            await _users.Find(u => u.UserId == id).FirstOrDefaultAsync();

        public async Task<User> CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task UpdateAsync(string id, User userIn) =>
            await _users.ReplaceOneAsync(u => u.UserId == id, userIn);

        public async Task RemoveAsync(string id) =>
            await _users.DeleteOneAsync(u => u.UserId == id);
    }
}
