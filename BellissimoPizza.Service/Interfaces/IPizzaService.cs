using BellisimoPizza.Domain.Commons;
using BellisimoPizza.Domain.Configurations;
using BellisimoPizza.Domain.Entities.Pizzas;
using BellisimoPizza.Service.DTOs.Pizzas;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BellisimoPizza.Service.Interfaces
{
    public interface IPizzaService
    {
        Task<BaseResponse<Pizza>> CreateAsync(PizzaForCreationDto pizzaDto);
        Task<BaseResponse<Pizza>> GetAsync(Expression<Func<Pizza, bool>> expression);
        Task<BaseResponse<IQueryable<Pizza>>> GetAllAsync(PaginationParams @params, Expression<Func<Pizza, bool>> expression = null);
        Task<BaseResponse<bool>> DeleteAsync(Expression<Func<Pizza, bool>> expression);
        Task<BaseResponse<Pizza>> UpdateAsync(Guid id, PizzaForCreationDto pizzaDto);

    }
}
