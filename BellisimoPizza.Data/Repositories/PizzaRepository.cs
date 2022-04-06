using BellisimoPizza.Data.Contexts;
using BellisimoPizza.Data.IRepositories;
using BellisimoPizza.Domain.Entities.Pizzas;
using Serilog;

namespace BellisimoPizza.Data.Repositories
{
    public class PizzaRepository : GenericRepository<Pizza>, IPizzaRepository
    {
        public PizzaRepository(BellisimoDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
