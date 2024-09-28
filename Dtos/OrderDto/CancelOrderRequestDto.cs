using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.OrderDto
{
    public class CancelOrderRequestDto
    {
        [Required]
        public string OrderId { get; set; }
    }
}