using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using MongoDbGenericRepository.Attributes;

namespace efcoremongodb.Models
{
    [CollectionName("products")]
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> CommentIds { get; set; } = new List<string>();
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> BranchProductIds { get; set; } = new List<string>();
    }
}
