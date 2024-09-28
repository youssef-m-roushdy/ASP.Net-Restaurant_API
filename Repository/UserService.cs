using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.DetailsDto.OrderDetailsDto;
using efcoremongodb.Dtos.DetailsDto.UserDetailsDto;
using efcoremongodb.Dtos.UserDto;
using efcoremongodb.Interfaces;
using efcoremongodb.Mappers;
using efcoremongodb.Models;
using efcoremongodb.Services;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace efcoremongodb.Repository
{
    public class UserService : IUserService
    {
        private readonly RestaurantDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(RestaurantDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ApplicationUser?> DeleteUserAsync(Guid userId)
        {
            // Find the user to delete
            var userToDelete = await _userManager.FindByIdAsync(userId.ToString());

            if (userToDelete == null)
            {
                // Return null if the user is not found
                return null;
            }

            // Find all comments made by the user
            var userComments = await _context.Comments.Find(c => c.UserId.ToString() == userId.ToString()).ToListAsync();
            var userOrders = await _context.Orders.Find(c => c.UserId.ToString() == userId.ToString()).ToListAsync();

            // Loop through each comment and delete it from the Comments collection
            foreach (var order in userOrders)
            {
                // Delete the comment from the Comments collection
                await _context.Orders.DeleteOneAsync(c => c.Id == order.Id);


                // Remove the comment from the corresponding product's CommentIds list
                await _context.Branches.UpdateOneAsync(
                    p => p.Id == order.BranchId,
                    Builders<Branch>.Update.Pull(p => p.OrderIds, order.Id)
                );
            }
            foreach (var comment in userComments)
            {
                // Delete the comment from the Comments collection
                await _context.Comments.DeleteOneAsync(c => c.Id == comment.Id);

                // Remove the comment from the corresponding product's CommentIds list
                await _context.Products.UpdateOneAsync(
                    p => p.Id == comment.ProductId,
                    Builders<Product>.Update.Pull(p => p.CommentIds, comment.Id)
                );
            }

            // Delete the user using UserManager
            var result = await _userManager.DeleteAsync(userToDelete);
            // Return the deleted user
            return userToDelete;
        }

        public async Task<UserDetailsDto?> GetUserProfile(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                // Return null if the user is not found
                throw new Exception("User not found");
            }
            var comments = new List<CommentUserDto>();
            var orders = new List<OrderUserDto>();
            
            foreach (var commentId in user.CommentIds)
            {
                var comment = await _context.Comments.Find(x => x.Id == commentId).FirstOrDefaultAsync();
                if(comment != null)
                {
                    comments.Add(new CommentUserDto
                    {
                        Id = comment.Id,
                        DatePosted = comment.DatePosted,
                        Rating = comment.Rating,
                        Content = comment.Content
                    });
                }
            }

            foreach (var orderId in user.OrderIds)
            {
                var order = await _context.Orders.Find(x => x.Id == orderId).FirstOrDefaultAsync();
                if(order != null)
                {
                    var orderItems = new List<OrderItemDto>();
                    foreach (var item in order.OrderItems)
                    {
                        var product = await _context.Products.Find(x => x.Id == item.ProductId).FirstOrDefaultAsync();
                        if(product != null)
                        {
                            orderItems.Add(new OrderItemDto{
                                Id = product.Id,
                                Quantity = item.Quantity,
                                Name = product.Name,
                                Price = product.Price,
                                Image = product.Image
                            });
                        }
                    }
                    orders.Add(new OrderUserDto
                    {
                        Id = order.Id,
                        OrderItems = orderItems,
                        Status = order.Status,
                        PayType = order.PayType,
                        ConfirmedAt = order.ConfirmedAt,
                        CreatedAt = order.CreatedAt
                    });
                }
            }

            var userInfo = user.GetUserInfo(comments, orders);
             return userInfo;
        }
    }
}