using efcoremongodb.Dtos.BranchProductDto;
using efcoremongodb.Interfaces;
using efcoremongodb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace efcoremongodb.Controllers
{
    [Route("api/branch-products")]
    [ApiController]
    public class BranchProductController : ControllerBase
    {
        private readonly IBranchProductRepository _branchProductRepository;

        public BranchProductController(IBranchProductRepository branchProductRepository)
        {
            _branchProductRepository = branchProductRepository;
        }

        [HttpPost("{branchId}/products")]
        public async Task<IActionResult> AddProductToBranch(string branchId, [FromBody] BranchProductDto branchProductDto)
        {
            var branchProduct = await _branchProductRepository.AddProductToBranch(branchId, branchProductDto);
            if (branchProduct == null)
                return NotFound("Branch or Product not found.");
            
            return Ok(branchProduct);
        }

        [HttpGet("{branchId}/products")]
        public async Task<IActionResult> GetAllProductsInBranch(string branchId)
        {
            var products = await _branchProductRepository.GetAllProductsInBranch(branchId);
            if (products == null)
                return NotFound("Branch not found.");

            return Ok(products);
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> RemoveProductFromBranch(string id)
        {
            var removedProduct = await _branchProductRepository.RemoveProductFromBranch(id);
            if (removedProduct == null)
                return NotFound("Branch product not found.");

            return Ok(removedProduct);
        }

        [HttpPut("products/{id}")]
        public async Task<IActionResult> UpdateProductInBranch(string id, [FromBody] UpdateProductInBranchDto productToUpdate)
        {
            var updatedProduct = await _branchProductRepository.UpdateProductInBranch(id, productToUpdate.Quantity);
            if (updatedProduct == null)
                return NotFound("Branch product not found or invalid quantity.");

            return Ok(updatedProduct);
        }
    }
}
