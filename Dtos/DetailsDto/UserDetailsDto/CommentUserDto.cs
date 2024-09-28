using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.DetailsDto.UserDetailsDto
{
    public class CommentUserDto
    {
        public string Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public int Rating { get; set; }
    }
}