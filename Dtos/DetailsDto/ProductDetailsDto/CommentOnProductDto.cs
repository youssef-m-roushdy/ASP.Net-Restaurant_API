using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.DetailsDto.ProductDetailsDto
{
    public class CommentOnProductDto
    {
        public string Id { get; set; }
        public string User { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public int Rating { get; set; }
    }
}