using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.CommentDto;
using efcoremongodb.Dtos.DetailsDto.CommentDetailsDto;
using efcoremongodb.Interfaces;
using efcoremongodb.Mappers;
using efcoremongodb.Models;
using efcoremongodb.Services;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace efcoremongodb.Repository
{
    public class CommentService : ICommentService
    {
        private readonly RestaurantDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentService(RestaurantDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<CommentDetailsDto?> AddComment(string productId, string userId, CreateCommentDto comment)
        {
            var createdComment = comment.ToCommentFromCreateCommentDto();
            createdComment.Id = ObjectId.GenerateNewId().ToString();
            var user = await _userManager.FindByIdAsync(userId);
            var product = await _context.Products.Find(x => x.Id == productId).FirstOrDefaultAsync();
            if (user != null && product != null)
            {
                createdComment.UserId = user.Id;
                createdComment.ProductId = productId;
                try
                {
                    user.CommentIds.Add(createdComment.Id);
                    await _userManager.UpdateAsync(user);

                    product.CommentIds.Add(createdComment.Id);
                    await _context.Products.ReplaceOneAsync(x => x.Id == product.Id, product);

                    await _context.Comments.InsertOneAsync(createdComment);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                var commentDetail = createdComment.GetCommentInfo(user, product);
                return commentDetail;
            }
            return null;
        }

        public async Task<Comment?> DeleteComment(string id)
        {
            var commentToDelete = await _context.Comments.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (commentToDelete == null)
                return null;

            var user = await _userManager.FindByIdAsync(commentToDelete.UserId.ToString());

            var product = await _context.Products.Find(x => x.Id == commentToDelete.ProductId).FirstOrDefaultAsync();

            if (user != null && product != null)
            {
                user.CommentIds.Remove(commentToDelete.Id);
                await _userManager.UpdateAsync(user);

                product.CommentIds.Remove(commentToDelete.Id);
                await _context.Products.ReplaceOneAsync(x => x.Id == product.Id, product);

                await _context.Comments.DeleteOneAsync(x => x.Id == id);
                return commentToDelete;
            }
            return null;
        }


        public async Task<IEnumerable<CommentDetailsDto?>> GetCommentsOnProduct(string productId)
        {
            var product = await _context.Products.Find(x => x.Id == productId).FirstOrDefaultAsync();
            if(product == null)
                return null;
            var commentsOnProduct = await _context.Comments.Find(x => x.ProductId == productId).ToListAsync();
            var commentDetails = new List<CommentDetailsDto>();
            foreach (var comment in commentsOnProduct)
            {
                var user = await _userManager.FindByIdAsync(comment.UserId.ToString());
                if(user != null)
                {
                    var commentDetail = comment.GetCommentInfo(user, product);
                    commentDetails.Add(commentDetail);
                }
            }
            return commentDetails;
        }
    }
}
