using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace efcoremongodb.Models
{
    [CollectionName("orderItems")]
    public class OrderItem
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }  // Link to the product
        public decimal Quantity { get; set; }  // Capture quantity of the product ordered
    }
}
