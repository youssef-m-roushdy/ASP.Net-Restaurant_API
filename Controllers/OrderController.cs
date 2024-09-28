using efcoremongodb.Dtos.OrderDto;
using efcoremongodb.Models;
using efcoremongodb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace efcoremongodb.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        // POST: api/Order
        [HttpPost]
        [Authorize] // Ensure the user is authenticated
        public async Task<IActionResult> CreateOrder()
        {
            if (!ModelState.IsValid)
                return BadRequest("Order cannot be null.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = userIdClaim.Value;

            var createdOrder = await _orderRepository.CreateOrderAsync(userId);
            if (createdOrder == null)
                return BadRequest("There is a order still not confirmed");
            return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(string orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(order);
        }

        [HttpPost("/add-product")]
        [Authorize]
        public async Task<IActionResult> AddProductToOrder(
       [FromBody] AddProductToOrderDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Order cannot be null.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = userIdClaim.Value;
            try
            {
                var order = await _orderRepository.AddProductToOrderAsync(userId, productDto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }

        [HttpPut("{orderId}/update-product")]
        public async Task<IActionResult> UpdateProductInOrder(string orderId, [FromBody] UpdateProductInOrderDto productDto)
        {
            if (productDto == null || string.IsNullOrEmpty(productDto.ProductId))
            {
                return BadRequest("Invalid product details.");
            }

            try
            {
                var updatedOrder = await _orderRepository.UpdateProductInOrderAsync(orderId, productDto);
                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{orderId}/confirm")]
        [Authorize]
        public async Task<IActionResult> ConfirmOrder(
       [FromRoute] string orderId,
       [FromBody] ConfirmOrderDto confirmOrderDto)
        {
            var order = await _orderRepository.ConfirmOrderAsync(orderId, confirmOrderDto);
            if (order == null)
                return NotFound("Order not found or not in a pending state.");

            return Ok(order);
        }

        [HttpDelete("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            try
            {
                var success = await _orderRepository.CancelOrderAsync(orderId);

                if (success)
                {
                    return Ok("Order cancelled successfully.");
                }

                return BadRequest("Failed to cancel the order.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
