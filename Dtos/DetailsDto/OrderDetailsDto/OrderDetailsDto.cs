using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.DetailsDto.OrderDetailsDto
{
    public class OrderDetailsDto
    {
        public string Id { get; set; }
        public BranchOrderInfo Branch { get; set; }
        public UserOrderInfo User { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public string Status { get; set; }
        public string PayType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
    }
}