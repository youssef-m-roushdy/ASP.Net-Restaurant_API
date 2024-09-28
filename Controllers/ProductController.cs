using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.ProductDto;
using efcoremongodb.Interfaces;
using efcoremongodb.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace efcoremongodb.Controllers
{
    [Route("/api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _product;
        public ProductController(IProductService product)
        {
            _product = product;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _product.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var product = await _product.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductDto createproductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _product.AddProduct(createproductDto);
            if(product.Id == null)
            {
                return BadRequest("Product can not be created");
            }
            return CreatedAtAction(nameof(GetById), new {id = product.Id}, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateProductDto updateproduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            

            var product = await _product.EditProduct(id, updateproduct);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var product = await _product.DeleteProduct(id);
            if(product == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
