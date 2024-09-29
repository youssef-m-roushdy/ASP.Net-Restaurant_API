using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.DetailsDto.OrderDetailsDto;

namespace efcoremongodb.Dtos.DetailsDto.BranchDetailsDto
{
    public class OrdersInBranchDto
    {
        public string Id { get; set; }
        public string User { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }  // List of order items
        public string Status { get; set; }  // Pending, Confirmed, Cancelled
        public string PayType { get; set; }  // Pending, Confirmed, Cancelled
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }  // Time when the or
    }
}