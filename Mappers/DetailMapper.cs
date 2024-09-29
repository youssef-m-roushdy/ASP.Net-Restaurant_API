using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.DetailsDto.BranchDetailsDto;
using efcoremongodb.Dtos.DetailsDto.BranchProductDetailsDto;
using efcoremongodb.Dtos.DetailsDto.CommentDetailsDto;
using efcoremongodb.Dtos.DetailsDto.OrderDetailsDto;
using efcoremongodb.Dtos.DetailsDto.ProductDetailsDto;
using efcoremongodb.Dtos.DetailsDto.UserDetailsDto;
using efcoremongodb.Models;

namespace efcoremongodb.Mappers
{
    public static class DetailMapper
    {
        public static CommentDetailsDto GetCommentInfo(this Comment comment, ApplicationUser user, Product product)
        {
            return new CommentDetailsDto
            {
                Id = comment.Id,
                DatePosted = comment.DatePosted,
                Content = comment.Content,
                Rating = comment.Rating,
                User = new UserCommentDto
                {
                    Id = user.Id.ToString(),
                    FullName = user.FullName,
                    Image = user.Image
                },
                Product = new ProductCommentDto
                {
                    Id = product.Id,
                    Name = product.Name,
                }
            };
        }
        public static BranchProductDetailsDto GetBranchProductInfo(this BranchProduct branchProduct, Branch branch, Product product)
        {
            return new BranchProductDetailsDto
            {
                Id = branchProduct.Id,
                Quantity = branchProduct.Quantity,
                Branch = new BranchInfoDto
                {
                    Id = branch.Id,
                    Name = branch.Name,
                    Address = branch.Address
                },
                Product = new ProductInfoDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.Image
                }
            };
        }

        public static OrderDetailsDto GetOrderInfo(this Order order, Branch branch, ApplicationUser user, List<OrderItemDto> orderItems)
        {
            return new OrderDetailsDto
            {
                Id = order.Id,
                Status = order.Status,
                PayType = order.PayType,
                CreatedAt = order.CreatedAt,
                ConfirmedAt = order.ConfirmedAt,
                Branch = branch == null ? null : new BranchOrderInfo 
                {
                    Id = branch.Id,
                    Name = branch.Name,
                    Address = branch.Address,
                    ContactNumber = branch.ContactNumber
                },
                User = new UserOrderInfo
                {
                    Id = user.Id.ToString(),
                    FullName = user.FullName,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber
                },
                OrderItems = orderItems
            };
        }

        public static UserDetailsDto GetUserInfo(this ApplicationUser user, List<CommentUserDto> comments, List<OrderUserDto> orders)
        {
            return new UserDetailsDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Address = user.Address,
                Image = user.Image,
                PhoneNumber = user.PhoneNumber,
                Comments = comments,
                Orders = orders
            };
        }

        public static ProductDetailsDto GetProductInfo(this Product product, List<CommentOnProductDto> comments, List<ProductInBranchProductDto> branchProducts)
        {
            return new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Image = product.Image,
                Comments = comments,
                BranchProducts = branchProducts
            };
        }

        public static BranchDetailsDto GetBranchInfo(this Branch branch, List<OrdersInBranchDto> orders, List<BranchInfoProductDto> branchProducts)
        {
            return new BranchDetailsDto
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                ContactNumber = branch.ContactNumber,
                OpeningHours = branch.OpeningHours,
                Orders = orders,
                BranchProducts = branchProducts
            };
        }
    }
}