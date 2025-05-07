using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CocktailApp.Models
{
    public class Ingredient
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IngredientId { get; set; }

        [BsonElement("name")]
        [BsonRequired]
       
        public List<string> Name { get; set; } = new List<string>();

        [BsonElement("category")]
        [BsonRequired]
        public string Category { get; set; }

        [BsonElement("quantity")]
        [BsonRequired]
        public double Quantity { get; set; }

        [BsonElement("unit")]
        [BsonRequired]
        public string Unit { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
