using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.DetailsDto.OrderDetailsDto;

namespace efcoremongodb.Dtos.DetailsDto.UserDetailsDto
{
    public class OrderUserDto
    {
        public string Id { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public string Status { get; set; }  
        public string PayType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ConfirmedAt { get; set; }
    }
}