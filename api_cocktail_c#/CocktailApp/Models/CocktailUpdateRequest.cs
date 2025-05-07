using Microsoft.AspNetCore.Http;

namespace CocktailApp.Models
{
    public class CocktailUpdateRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile CocktailImage { get; set; }
    }
}