using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.CommentDto;
using efcoremongodb.Dtos.DetailsDto.CommentDetailsDto;
using efcoremongodb.Models;

namespace efcoremongodb.Interfaces
{
    public interface ICommentService
    {
        public Task<IEnumerable<CommentDetailsDto?>> GetCommentsOnProduct(string productId);
        public Task<CommentDetailsDto?> AddComment(string id, string userId,CreateCommentDto comment);
        public Task<Comment?> DeleteComment(string id);
    }
}