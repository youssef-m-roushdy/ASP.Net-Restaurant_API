using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsClient.Protocol;
using efcoremongodb.Dtos.BranchDto;
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
         public async Task<Branch?> AddBranch(CreateBranchDto branch)
        {
            var newBranch = branch.ToBranchFromCreateDto();
            await _context.Branches.InsertOneAsync(newBranch);
            return newBranch;
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

        public async Task<Branch?> EditBranch(string id, UpdateBranchDto branch)
        {
            var branchToUpdate = await _context.Branches.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (branchToUpdate == null)
                return null;

            branchToUpdate.Address = branch.Address;
            branchToUpdate.ContactNumber = branch.ContactNumber;
            branchToUpdate.Name = branch.Name;
            branchToUpdate.OpeningHours = branch.OpeningHours;
            await _context.Branches.ReplaceOneAsync(x => x.Id == id, branchToUpdate);
            return branchToUpdate;
        }

        public async Task<IEnumerable<Branch>> GetAllBranchs()
        {
            return await _context.Branches.Find(_ => true).ToListAsync();
        }

        public async Task<Branch?> GetBranchById(string id)
        {
            return await _context.Branches.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}