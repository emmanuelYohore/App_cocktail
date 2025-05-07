using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CocktailApp.Models
{
    public class CocktailIngredient
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CocktailIngredientId { get; set; }

        [BsonElement("cocktailId")]
        [BsonRequired]
        public string CocktailId { get; set; }

        [BsonElement("ingredientId")]
        [BsonRequired]
        public string IngredientId { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
