using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.OrderDto
{
    public class AddProductToOrderDto
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public decimal Quantity { get; set; }
    }
}