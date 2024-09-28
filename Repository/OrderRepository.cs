using efcoremongodb.Models;
using efcoremongodb.Interfaces;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.ProductDto;
using efcoremongodb.Dtos.OrderDto;
using efcoremongodb.Services;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using efcoremongodb.Dtos.DetailsDto.OrderDetailsDto;
using efcoremongodb.Mappers;

namespace efcoremongodb.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RestaurantDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderRepository(RestaurantDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Order?> CreateOrderAsync(string userId)
        {
            var orderPending = await _context.Orders.Find(x => x.Status == "Pending").FirstOrDefaultAsync();
            if (orderPending == null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {

                    var order = new Order
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        UserId = user.Id,
                        Status = "Pending"
                    };

                    user.OrderIds.Add(order.Id);
                    await _userManager.UpdateAsync(user);
                    await _context.Orders.InsertOneAsync(order);
                    return order;
                }
                Console.WriteLine("User is not in database");
                return null;
            }
            Console.WriteLine("Can't create new empty order becasuse there is order pending");
            return null;
        }



        public async Task<OrderDetailsDto> GetOrderByIdAsync(string orderId)
        {
            var order = await _context.Orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
            if(order == null)
            {
                throw new Exception("Order not found");
            }
            var branch = await _context.Branches.Find(x => x.Id == order.BranchId).FirstOrDefaultAsync();
            var user = await _userManager.FindByIdAsync(order.UserId.ToString());
            if(user == null)
            {
                throw new Exception("User not found");
            }
            var productsInOrder = new List<OrderItemDto>();
            foreach (var item in order.OrderItems)
            {
                var productInOrder = await _context.Products.Find(p => p.Id == item.ProductId).FirstOrDefaultAsync();
                if (productInOrder != null)
                {
                    productsInOrder.Add(new OrderItemDto
                    {
                        Id = productInOrder.Id,
                        Name = productInOrder.Name,
                        Image = productInOrder.Image,
                        Price = productInOrder.Price,
                        Quantity = item.Quantity
                    });
                }
            }

            // Map the order to OrderDetailsDto using the GetOrderInfo method
            var orderDetails = order.GetOrderInfo(branch, user, productsInOrder);

            return orderDetails;
        }

        public async Task<OrderDetailsDto> ConfirmOrderAsync(string orderId, ConfirmOrderDto confirmOrderDto)
        {
            var branch = await _context.Branches.Find(x => x.Id == confirmOrderDto.BranchId).FirstOrDefaultAsync();
            
            if (branch == null)
            {
                return null;
            }
            var order = await _context.Orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
            if (order == null || order.Status != "Pending")
            {
                return null;
            }
            var user = await _userManager.FindByIdAsync(order.UserId.ToString());
            if(user == null)
            {
                throw new Exception("User not found");
            }
            // Confirm the order and assign a branch
            order.Status = "Confirmed";
            order.BranchId = confirmOrderDto.BranchId;
            order.PayType = confirmOrderDto.PayType;
            order.ConfirmedAt = DateTime.UtcNow;
            branch.OrderIds.Add(orderId);
            await _context.Branches.ReplaceOneAsync(x => x.Id == branch.Id, branch);
            // Update the order in the repository
            await UpdateOrderAsync(order);

            var productsInOrder = new List<OrderItemDto>();
            foreach (var item in order.OrderItems)
            {
                var productInOrder = await _context.Products.Find(p => p.Id == item.ProductId).FirstOrDefaultAsync();
                if (productInOrder != null)
                {
                    productsInOrder.Add(new OrderItemDto
                    {
                        Id = productInOrder.Id,
                        Name = productInOrder.Name,
                        Image = productInOrder.Image,
                        Price = productInOrder.Price,
                        Quantity = item.Quantity
                    });
                }
            }

            // Map the order to OrderDetailsDto using the GetOrderInfo method
            var orderDetails = order.GetOrderInfo(branch, user, productsInOrder);

            return orderDetails;
        }

        public async Task<OrderDetailsDto> AddProductToOrderAsync(string userId, AddProductToOrderDto productDto)
        {
            // Check if the user has any pending orders
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Find an existing pending order
            var pendingOrder = await _context.Orders
                .Find(o => o.UserId.ToString() == userId && o.Status == "Pending")
                .FirstOrDefaultAsync();

            if (pendingOrder == null)
            {
                // No pending orders found, create a new order automatically
                pendingOrder = new Order
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    UserId = user.Id,
                    Status = "Pending",
                    OrderItems = new List<OrderItem>() // Initialize the order items list
                };

                user.OrderIds.Add(pendingOrder.Id);
                await _userManager.UpdateAsync(user);
                await _context.Orders.InsertOneAsync(pendingOrder);
            }

            // Add the product to the order
            var existingProduct = pendingOrder.OrderItems.FirstOrDefault(p => p.ProductId == productDto.ProductId);
            if (existingProduct != null)
            {
                throw new Exception("Product aleardy existing in order");
            }
            else
            {
                // Otherwise, add the product as a new order item
                pendingOrder.OrderItems.Add(new OrderItem
                {
                    ProductId = productDto.ProductId,
                    Quantity = productDto.Quantity
                });
            }

            // Update the order in the database
            await UpdateOrderAsync(pendingOrder);


            // Retrieve all product details for order items
            var productsInOrder = new List<OrderItemDto>();
            foreach (var item in pendingOrder.OrderItems)
            {
                var product = await _context.Products.Find(p => p.Id == item.ProductId).FirstOrDefaultAsync();
                if (product != null)
                {
                    productsInOrder.Add(new OrderItemDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Image = product.Image,
                        Price = product.Price,
                        Quantity = item.Quantity
                    });
                }
            }

            // Map the order to OrderDetailsDto using the GetOrderInfo method
            var orderDetails = pendingOrder.GetOrderInfo(null, user, productsInOrder);

            return orderDetails;
        }


        public async Task<OrderDetailsDto> UpdateProductInOrderAsync(string orderId, UpdateProductInOrderDto productDto)
        {
            var order = await _context.Orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
            var user = await  _userManager.FindByIdAsync(order.UserId.ToString());
            if(user == null)
            {
                throw new Exception("User not found");
            }
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            if (order.Status != "Pending")
            {
                throw new Exception("Cannot modify the order. It is not in a pending state.");
            }

            // Find and update the product in the order
            var product = order.OrderItems.FirstOrDefault(p => p.ProductId == productDto.ProductId);
            
            if (product == null)
            {
                throw new Exception("Product not found in the order.");
            }

            product.Quantity = productDto.Quantity;
            await UpdateOrderAsync(order);
            var productsInOrder = new List<OrderItemDto>();
            foreach (var item in order.OrderItems)
            {
                var productInOrder = await _context.Products.Find(p => p.Id == item.ProductId).FirstOrDefaultAsync();
                if (productInOrder != null)
                {
                    productsInOrder.Add(new OrderItemDto
                    {
                        Id = productInOrder.Id,
                        Name = productInOrder.Name,
                        Image = productInOrder.Image,
                        Price = productInOrder.Price,
                        Quantity = item.Quantity
                    });
                }
            }

            // Map the order to OrderDetailsDto using the GetOrderInfo method
            var orderDetails = order.GetOrderInfo(null, user, productsInOrder);

            return orderDetails;
        }

        public async Task<bool> CancelOrderAsync(string orderId)
        {
            var order = await _context.Orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
            if (order == null)
            {
                return false;
            }

            if (order.Status == "Completed")
            {
                return false;
            }
            if (order.Status == "Confirmed")
            {
                var branchOrderCancel = await _context.Branches.Find(x => x.Id == order.BranchId).FirstOrDefaultAsync();
                branchOrderCancel.OrderIds.Remove(order.Id);
                await _context.Branches.ReplaceOneAsync(x => x.Id == branchOrderCancel.Id, branchOrderCancel);
            }
            var user = await _userManager.FindByIdAsync(order.UserId.ToString());
            user.OrderIds.Remove(order.Id);
            await _userManager.UpdateAsync(user);

            await _context.Orders.DeleteOneAsync(x => x.Id == order.Id);

            return true;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, order.Id);
            await _context.Orders.ReplaceOneAsync(filter, order);
        }
    }
}
