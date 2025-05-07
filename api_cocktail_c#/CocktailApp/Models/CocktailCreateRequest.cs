using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CocktailApp.Models
{
    public class CocktailCreateRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }

        public IFormFile CocktailImage { get; set; }
    }
}