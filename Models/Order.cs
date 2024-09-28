using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using MongoDbGenericRepository.Attributes;

namespace efcoremongodb.Models
{
    [CollectionName("orders")]
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string BranchId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();  // List of order items
        public string Status { get; set; }  // Pending, Confirmed, Cancelled
        public string PayType { get; set; }  // Pending, Confirmed, Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ConfirmedAt { get; set; }  // Time when the or
    }
}
