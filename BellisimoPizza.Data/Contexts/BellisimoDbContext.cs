using BellisimoPizza.Domain.Entities.Orders;
using BellisimoPizza.Domain.Entities.Pizzas;
using BellisimoPizza.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace BellisimoPizza.Data.Contexts
{
    public class BellisimoDbContext : DbContext
    {
        public BellisimoDbContext(DbContextOptions<BellisimoDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Pizza> Pizzas { get; set; }
    }
}
