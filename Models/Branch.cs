using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System.ComponentModel.DataAnnotations;

namespace efcoremongodb.Models
{
    [CollectionName("branches")]
    public class Branch
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string OpeningHours { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> OrderIds { get; set; } = new List<string>();
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> BranchProductIds { get; set; } = new List<string>();
    }
}
