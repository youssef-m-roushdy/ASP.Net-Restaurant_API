using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace efcoremongodb.Dtos.OrderDto
{
    public class CreateOrderDto
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        [Range(1,10)]
        public decimal Quantity { get; set; }
    }
}