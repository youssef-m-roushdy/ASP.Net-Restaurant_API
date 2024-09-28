using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.BranchProductDto;
using efcoremongodb.Dtos.DetailsDto.BranchProductDetailsDto;
using efcoremongodb.Interfaces;
using efcoremongodb.Mappers;
using efcoremongodb.Models;
using efcoremongodb.Services;
using MongoDB.Driver;

namespace efcoremongodb.Repository
{
    public class BranchProductRepository : IBranchProductRepository
    {
        private readonly RestaurantDbContext _context;
        public BranchProductRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<BranchProductDetailsDto?> AddProductToBranch(string branchId, BranchProductDto branchProductDto)
        {
            var checkProduct = await _context.BranchProducts.Find(x => x.Id == branchProductDto.ProductId).FirstOrDefaultAsync();
            if(checkProduct != null)
                return null;
            var branch = await _context.Branches.Find(x => x.Id == branchId).FirstOrDefaultAsync();
            var product = await _context.Products.Find(x => x.Id == branchProductDto.ProductId).FirstOrDefaultAsync();
            if (branch != null && product != null)
            {
                var newBranchProduct = branchProductDto.ToBranchProductFromDto();
                newBranchProduct.BranchId = branchId;
                await _context.BranchProducts.InsertOneAsync(newBranchProduct);

                branch.BranchProductIds.Add(newBranchProduct.Id);
                product.BranchProductIds.Add(newBranchProduct.Id);

                await _context.Branches.ReplaceOneAsync(x => x.Id == branch.Id, branch);
                await _context.Products.ReplaceOneAsync(x => x.Id == product.Id, product);

                var branchProductDetail = newBranchProduct.GetBranchProductInfo(branch, product);

                return branchProductDetail; // Return the newly added product
            }
            return null;
        }

        public async Task<IEnumerable<BranchProductDetailsDto?>> GetAllProductsInBranch(string branchId)
        {
            var branch = await _context.Branches.Find(x => x.Id == branchId).FirstOrDefaultAsync();
            var productInBranchInfos = new List<BranchProductDetailsDto>();
            if (branch != null)
            {
                var productsInBranch = await _context.BranchProducts.Find(x => x.BranchId == branchId).ToListAsync();
                foreach (var productInBranch in productsInBranch)
                {
                    var product = await _context.Products.Find(x => x.Id == productInBranch.ProductId).FirstOrDefaultAsync();
                    if (product == null)
                        return null;
                    var productInBranchInfo = productInBranch.GetBranchProductInfo(branch, product);
                    productInBranchInfos.Add(productInBranchInfo);
                }
                return productInBranchInfos;
            }
            return null;
        }

        public async Task<BranchProduct?> RemoveProductFromBranch(string id)
        {
            var removedBranchProduct = await _context.BranchProducts.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (removedBranchProduct == null)
                return null;

            var branch = await _context.Branches.Find(x => x.Id == removedBranchProduct.BranchId).FirstOrDefaultAsync();
            var product = await _context.Products.Find(x => x.Id == removedBranchProduct.ProductId).FirstOrDefaultAsync();

            if (branch != null && product != null)
            {
                branch.BranchProductIds.Remove(removedBranchProduct.Id); // Fix reference here
                product.BranchProductIds.Remove(removedBranchProduct.Id); // Fix reference here

                await _context.Branches.ReplaceOneAsync(x => x.Id == branch.Id, branch);
                await _context.Products.ReplaceOneAsync(x => x.Id == product.Id, product);
                await _context.BranchProducts.DeleteOneAsync(x => x.Id == id);
                return removedBranchProduct;
            }
            return null;
        }

        public async Task<BranchProductDetailsDto?> UpdateProductInBranch(string id, decimal quantity)
        {
            var updatedBranchProduct = await _context.BranchProducts.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (updatedBranchProduct == null)
                return null;

            if (quantity != 0)
            {
                if (quantity < 0 && updatedBranchProduct.Quantity < (quantity * (-1)))
                    return null;
                updatedBranchProduct.Quantity += quantity; // Correctly update the quantity
            }
            else
            {
                return null;
            }

            await _context.BranchProducts.ReplaceOneAsync(x => x.Id == id, updatedBranchProduct);
            var product = await _context.Products.Find(x => x.Id == updatedBranchProduct.ProductId).FirstOrDefaultAsync();
            var branch = await _context.Branches.Find(x => x.Id == updatedBranchProduct.BranchId).FirstOrDefaultAsync();
            if (product == null || branch == null)
                return null;
            var productInBranchInfo = updatedBranchProduct.GetBranchProductInfo(branch, product);
            return productInBranchInfo;
        }
    }
}
