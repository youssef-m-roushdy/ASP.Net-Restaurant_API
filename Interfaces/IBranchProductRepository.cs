using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.BranchProductDto;
using efcoremongodb.Dtos.DetailsDto.BranchProductDetailsDto;
using efcoremongodb.Models;

namespace efcoremongodb.Interfaces
{
    public interface IBranchProductRepository
    {
        public Task<IEnumerable<BranchProductDetailsDto?>> GetAllProductsInBranch(string branchId);
        public Task<BranchProductDetailsDto?> AddProductToBranch(string branchId, BranchProductDto branchProductDto);
        public Task<BranchProduct?> RemoveProductFromBranch(string id);
        public Task<BranchProductDetailsDto?> UpdateProductInBranch(string id, decimal quantity);
    }
}