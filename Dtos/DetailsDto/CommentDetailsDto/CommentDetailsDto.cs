using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.DetailsDto.CommentDetailsDto
{
    public class CommentDetailsDto
    {
        public string Id { get; set; }
        public UserCommentDto User { get; set; }
        public ProductCommentDto Product { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public int Rating { get; set; }
    }
}