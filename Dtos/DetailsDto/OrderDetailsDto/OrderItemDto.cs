using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Models;

namespace efcoremongodb.Dtos.DetailsDto.OrderDetailsDto
{
    public class OrderItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public decimal Quantity { get; set; }
    }
}