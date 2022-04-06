using AutoMapper;
using BellisimoPizza.Data.IRepositories;
using BellisimoPizza.Domain.Commons;
using BellisimoPizza.Domain.Configurations;
using BellisimoPizza.Domain.Entities.Users;
using BellisimoPizza.Domain.Enums;
using BellisimoPizza.Service.DTOs.Users;
using BellisimoPizza.Service.Extensions;
using BellisimoPizza.Service.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BellisimoPizza.Service.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<BaseResponse<User>> CreateAsync(UserForCreationDto userDto)
        {
            var response = new BaseResponse<User>();

            // create after checking success
            var mappedUser = mapper.Map<User>(userDto);

            // save image from dto model to wwwroot
            var result = await unitOfWork.Users.CreateAsync(mappedUser);

            await unitOfWork.SaveChangesAsync();

            response.Data = result;

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Expression<Func<User, bool>> expression)
        {
            var response = new BaseResponse<bool>();

            // check for exist user
            var existUser = await unitOfWork.Users.GetAsync(expression);
            if (existUser is null)
            {
                response.Error = new ErrorResponse(404, "User not found");
                return response;
            }
            existUser.Delete();

            await unitOfWork.Users.UpdateAsync(existUser);

            await unitOfWork.SaveChangesAsync();

            response.Data = true;

            return response;
        }

        public async Task<BaseResponse<IQueryable<User>>> GetAllAsync(PaginationParams @params, Expression<Func<User, bool>> expression = null)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var response = new BaseResponse<IQueryable<User>>();

            var user = await unitOfWork.Users.GetAllAsync(expression => expression.State != ItemState.Deleted);

            response.Data = user.ToPagedList(@params);

            if (response.Data is null)
            {
                response.Error = new ErrorResponse(404, "Users not found");
            }

            return response;
        }

        public async Task<BaseResponse<User>> GetAsync(Expression<Func<User, bool>> expression)
        {
            var response = new BaseResponse<User>();

            var user = await unitOfWork.Users.GetAsync(expression);
            if (user is null)
            {
                response.Error = new ErrorResponse(404, "User not found");
                return response;
            }

            response.Data = user;

            return response;
        }

        public async Task<BaseResponse<User>> UpdateAsync(Guid id, UserForCreationDto userDto)
        {
            var response = new BaseResponse<User>();

            // check for exist user
            var user = await unitOfWork.Users.GetAsync(p => p.Id == id && p.State != ItemState.Deleted);
            if (user is null)
            {
                response.Error = new ErrorResponse(404, "User not found");
                return response;
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.ShoppingCard = userDto.ShoppingCard;

            user.Update();

            var result = await unitOfWork.Users.UpdateAsync(user);

            await unitOfWork.SaveChangesAsync();

            response.Data = result;

            return response;
        }
    }
}
