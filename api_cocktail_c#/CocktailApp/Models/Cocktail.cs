using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CocktailApp.Models
{
    public class Cocktail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CocktailId { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("cocktailImage")]
        public string? CocktailImage { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("userId")]
        [BsonRequired]
        public string UserId { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
