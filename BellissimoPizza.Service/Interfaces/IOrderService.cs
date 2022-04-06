using BellisimoPizza.Domain.Commons;
using BellisimoPizza.Domain.Configurations;
using BellisimoPizza.Domain.Entities.Orders;
using BellisimoPizza.Service.DTOs.Orders;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BellisimoPizza.Service.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponse<Order>> CreateAsync(OrderForCreationDto orderDto);
        Task<BaseResponse<Order>> GetAsync(Expression<Func<Order, bool>> expression);
        Task<BaseResponse<IQueryable<Order>>> GetAllAsync(PaginationParams @params, Expression<Func<Order, bool>> expression = null);
        Task<BaseResponse<bool>> DeleteAsync(Expression<Func<Order, bool>> expression);
        Task<BaseResponse<Order>> UpdateAsync(Guid id, OrderForCreationDto orderDto);
    }
}
