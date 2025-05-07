using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CocktailApp.Models
{
    public class Rating
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? RatingId { get; set; }

        [BsonElement("userId")]
        [BsonRequired]
        public string UserId { get; set; }

        [BsonElement("cocktailId")]
        [BsonRequired]
        public string? CocktailId { get; set; }

        [BsonElement("rating")]
        [BsonRequired]
        public int Value { get; set; }

        [BsonElement("comment")]
        public string? Comment { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
