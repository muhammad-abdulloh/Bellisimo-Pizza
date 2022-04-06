using BellisimoPizza.Data.IRepositories;
using BellisimoPizza.Data.Repositories;
using BellisimoPizza.Service.Interfaces;
using BellisimoPizza.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BellisimoPizza.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPizzaService, PizzaService>();
        }
    }
}
