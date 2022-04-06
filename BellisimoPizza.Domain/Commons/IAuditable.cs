using BellisimoPizza.Domain.Enums;
using System;

namespace BellisimoPizza.Domain.Commons
{
    public interface IAuditable
    {
        Guid Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        Guid? UpdatedBy { get; set; }
        ItemState State { get; set; }
    }
}
