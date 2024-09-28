using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.CommentDto;
using efcoremongodb.Models;

namespace efcoremongodb.Mappers
{
    public static class CommentMapper
    {
        public static Comment ToCommentFromCreateCommentDto(this CreateCommentDto productComment)
        {
            return new Comment
            {
                Rating = productComment.Rating,
                Content = productComment.Content,
                DatePosted = DateTime.UtcNow,
            };
        }

    
    }
}