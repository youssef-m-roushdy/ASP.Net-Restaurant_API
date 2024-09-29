using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsClient.Protocol;
using efcoremongodb.Dtos.BranchDto;
using efcoremongodb.Dtos.DetailsDto.BranchDetailsDto;
using efcoremongodb.Dtos.DetailsDto.OrderDetailsDto;
using efcoremongodb.Interfaces;
using efcoremongodb.Mappers;
using efcoremongodb.Models;
using efcoremongodb.Services;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace efcoremongodb.Repository
{
    public class BranchService : IBranchService
    {
        private readonly RestaurantDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BranchService(RestaurantDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<BranchDetailsDto?> AddBranch(CreateBranchDto branch)
        {
            var newBranch = branch.ToBranchFromCreateDto();
            await _context.Branches.InsertOneAsync(newBranch);

            var branchDetails = await GetBranchDetails(newBranch);

            return branchDetails;
        }

        public async Task<Branch?> DeleteBranch(string id)
        {
            var branchToDelete = await _context.Branches.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (branchToDelete == null)
                return null;

            var ordersInBranch = await _context.Orders.Find(x => x.BranchId == branchToDelete.Id).ToListAsync();
            foreach (var order in ordersInBranch)
            {
                await _context.Orders.DeleteOneAsync(c => c.Id == order.Id);


                var user = await _userManager.FindByIdAsync(order.UserId.ToString());  // Assuming comment.UserId is linked to the User
                if (user != null && user.OrderIds.Contains(order.Id))
                {
                    user.CommentIds.Remove(order.Id);
                    await _userManager.UpdateAsync(user);  // Save updated user
                }
            }
            var branchProducts = await _context.BranchProducts.Find(x => x.BranchId == branchToDelete.Id).ToListAsync();
            foreach (var productBranch in branchProducts)
            {
                await _context.BranchProducts.DeleteOneAsync(c => c.Id == productBranch.Id);

                // Remove the comment from the corresponding product's CommentIds list
                await _context.Products.UpdateOneAsync(
                    p => p.Id == productBranch.ProductId,
                    Builders<Product>.Update.Pull(p => p.BranchProductIds, productBranch.Id)
                );
            }
            await _context.Branches.DeleteOneAsync(x => x.Id == id);
            return branchToDelete;
        }

        public async Task<BranchDetailsDto?> EditBranch(string id, UpdateBranchDto branch)
        {
            var branchToUpdate = await _context.Branches.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (branchToUpdate == null)
                return null;

            branchToUpdate.Address = branch.Address;
            branchToUpdate.ContactNumber = branch.ContactNumber;
            branchToUpdate.Name = branch.Name;
            branchToUpdate.OpeningHours = branch.OpeningHours;
            await _context.Branches.ReplaceOneAsync(x => x.Id == id, branchToUpdate);

            var branchDetails = await GetBranchDetails(branchToUpdate);

            return branchDetails;
        }

        public async Task<IEnumerable<BranchDetailsDto>> GetAllBranchs()
        {
            var branches = await _context.Branches.Find(_ => true).ToListAsync();
            var branchesDetails = new List<BranchDetailsDto>();
            foreach (var branch in branches)
            {
                var branchDetails = await GetBranchDetails(branch);
                if(branchDetails != null)
                {
                    branchesDetails.Add(branchDetails);
                }
            }
            return branchesDetails;
        }

        public async Task<BranchDetailsDto?> GetBranchById(string id)
        {
            var branch = await _context.Branches.Find(x => x.Id == id).FirstOrDefaultAsync();
            var branchDetails = await GetBranchDetails(branch);
            return branchDetails;
        }

        public async Task<BranchDetailsDto?> GetBranchDetails(Branch branch)
        {
            if (branch == null)
            {
                return null;
            }
            var orders = new List<OrdersInBranchDto>();
            var branchProducts = new List<BranchInfoProductDto>();
            foreach (var orderId in branch.OrderIds)
            {
                var order = await _context.Orders.Find(x => x.Id == orderId).FirstOrDefaultAsync();
                if (order != null)
                {
                    var user = await _userManager.FindByIdAsync(order.UserId.ToString());
                    var orderItems = new List<OrderItemDto>();
                    foreach (var item in order.OrderItems)
                    {
                        var product = await _context.Products.Find(x => x.Id == item.ProductId).FirstOrDefaultAsync();
                        if (product != null)
                        {
                            orderItems.Add(new OrderItemDto
                            {
                                Id = product.Id,
                                Name = product.Name,
                                Price = product.Price,
                                Image = product.Image,
                                Quantity = item.Quantity
                            });
                        }
                    }
                    if (user != null)
                    {
                        orders.Add(new OrdersInBranchDto
                        {
                            Id = order.Id,
                            User = user.FullName,
                            Address = user.Address,
                            PhoneNumber = user.PhoneNumber,
                            Status = order.Status,
                            PayType = order.PayType,
                            CreatedAt = order.CreatedAt,
                            ConfirmedAt = order.ConfirmedAt,
                            OrderItems = orderItems
                        });
                    }
                }
            }
            foreach (var BranchproductId in branch.BranchProductIds)
            {
                var branchProduct = await _context.BranchProducts.Find(x => x.Id == BranchproductId).FirstOrDefaultAsync();
                if (branchProduct != null)
                {
                    var product = await _context.Products.Find(x => x.Id == branchProduct.ProductId).FirstOrDefaultAsync();
                    if (product != null)
                    {
                        branchProducts.Add(new BranchInfoProductDto
                        {
                            Id = branchProduct.Id,
                            Product = product.Name,
                            Quantity = branchProduct.Quantity
                        });
                    }
                }
            }
            return branch.GetBranchInfo(orders, branchProducts);
        }
    }
}