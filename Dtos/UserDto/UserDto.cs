using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace efcoremongodb.Dtos.UserDto
{
    public class UserDto
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<string> CommentIds { get; set; }
        public List<string> OrderIds { get; set; }
    }
}