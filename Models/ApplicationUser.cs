using System;
using System.ComponentModel.DataAnnotations;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace efcoremongodb.Models
{
    [CollectionName("users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Image { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> CommentIds { get; set; } = new List<string>();
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> OrderIds { get; set; } = new List<string>();
    }
}

