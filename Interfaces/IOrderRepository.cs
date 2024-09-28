using efcoremongodb.Dtos.DetailsDto.OrderDetailsDto;
using efcoremongodb.Dtos.OrderDto;
using efcoremongodb.Dtos.ProductDto;
using efcoremongodb.Models;
using System.Threading.Tasks;

namespace efcoremongodb.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> CreateOrderAsync(string userId);
        Task<OrderDetailsDto> GetOrderByIdAsync(string orderId);
        Task<OrderDetailsDto> ConfirmOrderAsync(string orderId, ConfirmOrderDto confirmOrderDto);
        Task<OrderDetailsDto> AddProductToOrderAsync(string userId, AddProductToOrderDto productDto);
        Task<OrderDetailsDto> UpdateProductInOrderAsync(string orderId, UpdateProductInOrderDto productDto);
        Task<bool> CancelOrderAsync(string orderId);
        Task UpdateOrderAsync(Order order);
    }
}
