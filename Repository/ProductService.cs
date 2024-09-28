using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.ProductDto;
using efcoremongodb.Interfaces;
using efcoremongodb.Mappers;
using efcoremongodb.Models;
using efcoremongodb.Services;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace efcoremongodb.Repository
{
    public class ProductService : IProductService
    {
        private readonly RestaurantDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProductService(RestaurantDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Product> AddProduct(CreateProductDto productDto)
        {
            var product = productDto.ToProductFromCreateDto();

            await _context.Products.InsertOneAsync(product);

            return product;
        }

        public async Task<Product?> DeleteProduct(string id)
        {
            var productToDelete = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (productToDelete == null)
            {
                return null;
            }

            // Find related branch products
            var relatedBranchProducts = await _context.BranchProducts.Find(bp => bp.ProductId == id).ToListAsync();

            // Find related comments
            var relatedComments = await _context.Comments.Find(c => c.ProductId == id).ToListAsync();

            // Delete related branch products from BranchProducts collection
            foreach (var branchProduct in relatedBranchProducts)
            {
                await _context.BranchProducts.DeleteOneAsync(bp => bp.Id == branchProduct.Id);

                // Remove the branch product reference from the Branch collection
                var branch = await _context.Branches.Find(b => b.BranchProductIds.Contains(branchProduct.Id)).FirstOrDefaultAsync();
                if (branch != null)
                {
                    branch.BranchProductIds.Remove(branchProduct.Id);
                    await _context.Branches.ReplaceOneAsync(b => b.Id == branch.Id, branch);
                }
            }

            // Delete related comments and update user collection if necessary
            foreach (var comment in relatedComments)
            {
                // Delete comment from Comments collection
                await _context.Comments.DeleteOneAsync(c => c.Id == comment.Id);

                // Find the user who made the comment and remove it from their CommentIds
                var user = await _userManager.FindByIdAsync(comment.UserId.ToString());  // Assuming comment.UserId is linked to the User
                if (user != null && user.CommentIds.Contains(comment.Id))
                {
                    user.CommentIds.Remove(comment.Id);
                    await _userManager.UpdateAsync(user);  // Save updated user
                }
                
            }

            // Finally, delete the product from Products collection
            await _context.Products.DeleteOneAsync(p => p.Id == id);

            return productToDelete;
        }


        public async Task<Product?> EditProduct(string id, UpdateProductDto product)
        {
            var productToUpdate = await _context.Products.Find(r => r.Id == id).FirstOrDefaultAsync();
            if (productToUpdate == null)
            {
                return null;
            }
            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Price = product.Price;
            productToUpdate.Image = product.Image;

            await _context.Products.ReplaceOneAsync(r => r.Id == id, productToUpdate);
            return productToUpdate;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.Find(_ => true).ToListAsync();
        }

        public async Task<Product?> GetProductById(string id)
        {
            return await _context.Products.Find(r => r.Id == id).FirstOrDefaultAsync();
        }
    }
}