using AutoMapper;
using BellisimoPizza.Data.IRepositories;
using BellisimoPizza.Domain.Commons;
using BellisimoPizza.Domain.Configurations;
using BellisimoPizza.Domain.Entities.Orders;
using BellisimoPizza.Domain.Enums;
using BellisimoPizza.Service.DTOs.Orders;
using BellisimoPizza.Service.Extensions;
using BellisimoPizza.Service.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BellisimoPizza.Service.Services
{
    public class OrderService : IOrderService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<BaseResponse<Order>> CreateAsync(OrderForCreationDto orderDto)
        {
            var response = new BaseResponse<Order>();

            // create after checking success
            var mappedOrder = mapper.Map<Order>(orderDto);

            var result = await unitOfWork.Orders.CreateAsync(mappedOrder);

            await unitOfWork.SaveChangesAsync();

            response.Data = result;

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Expression<Func<Order, bool>> expression)
        {
            var response = new BaseResponse<bool>();

            // check for exist employeeSalary
            var existOrder = await unitOfWork.Orders.GetAsync(expression);
            if (existOrder is null)
            {
                response.Error = new ErrorResponse(404, "Order not found");
                return response;
            }
            existOrder.Delete();

            await unitOfWork.Orders.UpdateAsync(existOrder);

            await unitOfWork.SaveChangesAsync();

            response.Data = true;

            return response;
        }

        public async Task<BaseResponse<IQueryable<Order>>> GetAllAsync(PaginationParams @params, Expression<Func<Order, bool>> expression = null)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var response = new BaseResponse<IQueryable<Order>>();

            var order = await unitOfWork.Orders.GetAllAsync(expression => expression.State != ItemState.Deleted);

            response.Data = order.ToPagedList(@params);

            if (response.Data is null)
            {
                response.Error = new ErrorResponse(404, "Order not found");
            }

            return response;

        }

        public async Task<BaseResponse<Order>> GetAsync(Expression<Func<Order, bool>> expression)
        {
            var response = new BaseResponse<Order>();

            var order = await unitOfWork.Orders.GetAsync(expression);
            if (order is null)
            {
                response.Error = new ErrorResponse(404, "Order not found");
                return response;
            }

            response.Data = order;

            return response;
        }

        public async Task<BaseResponse<Order>> UpdateAsync(Guid id, OrderForCreationDto orderDto)
        {
            var response = new BaseResponse<Order>();

            // check for exist employeeSalary
            var order = await unitOfWork.Orders.GetAsync(p => p.Id == id && p.State != ItemState.Deleted);
            if (order is null)
            {
                response.Error = new ErrorResponse(404, "Order not found");
                return response;
            }

            order.PizzaId = orderDto.PizzaId;
            order.Quantity = orderDto.Quantity;
            order.UnitPrice = orderDto.UnitPrice;
            order.TotalPrice = orderDto.TotalPrice;
            order.DisCount = orderDto.DisCount;

            order.Update();

            var result = await unitOfWork.Orders.UpdateAsync(order);

            await unitOfWork.SaveChangesAsync();

            response.Data = result;

            return response;
        }
    }
}
