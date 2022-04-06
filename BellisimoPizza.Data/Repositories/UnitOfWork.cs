using BellisimoPizza.Data.Contexts;
using BellisimoPizza.Data.IRepositories;
using Serilog;
using System;
using System.Threading.Tasks;

namespace BellisimoPizza.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BellisimoDbContext context;
        private readonly ILogger logger;

        public IOrderRepository Orders { get; set; }

        public IPizzaRepository Pizzas { get; set; }

        public IUserRepository Users { get; set; }

        public UnitOfWork(BellisimoDbContext context)
        {
            this.context = context;

            //Object initializing for repository
            Orders = new OrderRepository(context, logger);
            Pizzas = new PizzaRepository(context, logger);
            Users = new UserRepository(context, logger);

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
