using BellisimoPizza.Domain.Commons;
using BellisimoPizza.Domain.Enums;
using System;

namespace BellisimoPizza.Domain.Entities.Orders
{
    public class Order : IAuditable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PizzaId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DisCount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public ItemState State { get; set; }

        public void Update()
        {
            UpdatedAt = DateTime.Now;
            State = ItemState.Updated;
        }

        public void Create()
        {
            CreatedAt = DateTime.Now;
            State = ItemState.Created;
        }

        public void Delete()
        {
            State = ItemState.Deleted;
        }

    }
}
