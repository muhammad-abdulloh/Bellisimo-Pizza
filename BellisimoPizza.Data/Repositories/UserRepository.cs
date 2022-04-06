using BellisimoPizza.Data.Contexts;
using BellisimoPizza.Data.IRepositories;
using BellisimoPizza.Domain.Entities.Users;
using Serilog;

namespace BellisimoPizza.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(BellisimoDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
