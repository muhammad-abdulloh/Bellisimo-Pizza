using BellisimoPizza.Data.Contexts;
using BellisimoPizza.Data.IRepositories;
using BellisimoPizza.Domain.Entities.Orders;
using Serilog;

namespace BellisimoPizza.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(BellisimoDbContext context, ILogger logger) : base(context, logger)
        {
        }

    }
}
