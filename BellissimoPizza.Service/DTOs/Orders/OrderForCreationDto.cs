using System;

namespace BellisimoPizza.Service.DTOs.Orders
{
    public class OrderForCreationDto
    {
        public Guid PizzaId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DisCount { get; set; }
    }
}
