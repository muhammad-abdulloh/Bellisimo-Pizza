using AutoMapper;
using BellisimoPizza.Data.IRepositories;
using BellisimoPizza.Domain.Commons;
using BellisimoPizza.Domain.Configurations;
using BellisimoPizza.Domain.Entities.Pizzas;
using BellisimoPizza.Domain.Enums;
using BellisimoPizza.Service.DTOs.Pizzas;
using BellisimoPizza.Service.Extensions;
using BellisimoPizza.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BellisimoPizza.Service.Services
{
    public class PizzaService : IPizzaService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration config;

        public PizzaService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment env, IConfiguration config)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.env = env;
            this.config = config;
        }

        public async Task<BaseResponse<Pizza>> CreateAsync(PizzaForCreationDto pizzaDto)
        {
            var response = new BaseResponse<Pizza>();

            // create after checking success
            var mappedPizza = mapper.Map<Pizza>(pizzaDto);

            // save image from dto model to wwwroot
            mappedPizza.ImageUrl = await SaveFileAsync(pizzaDto.ImageUrl.OpenReadStream(), pizzaDto.ImageUrl.FileName);

            var result = await unitOfWork.Pizzas.CreateAsync(mappedPizza);

            result.ImageUrl = config.GetSection("FileUrl:ImageUrl").Value + result.ImageUrl;

            await unitOfWork.SaveChangesAsync();

            response.Data = result;

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Expression<Func<Pizza, bool>> expression)
        {
            var response = new BaseResponse<bool>();

            // check for exist pizza
            var existPizza = await unitOfWork.Pizzas.GetAsync(expression);
            if (existPizza is null)
            {
                response.Error = new ErrorResponse(404, "Pizza not found");
                return response;
            }
            existPizza.Delete();

            await unitOfWork.Pizzas.UpdateAsync(existPizza);

            await unitOfWork.SaveChangesAsync();

            response.Data = true;

            return response;
        }

        public async Task<BaseResponse<IQueryable<Pizza>>> GetAllAsync(PaginationParams @params, Expression<Func<Pizza, bool>> expression = null)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var response = new BaseResponse<IQueryable<Pizza>>();

            var pizza = await unitOfWork.Pizzas.GetAllAsync(expression => expression.State != ItemState.Deleted);

            response.Data = pizza.ToPagedList(@params);

            if (response.Data is null)
            {
                response.Error = new ErrorResponse(404, "Pizza not found");
            }

            return response;
        }

        public async Task<BaseResponse<Pizza>> GetAsync(Expression<Func<Pizza, bool>> expression)
        {
            var response = new BaseResponse<Pizza>();

            var pizza = await unitOfWork.Pizzas.GetAsync(expression);
            if (pizza is null)
            {
                response.Error = new ErrorResponse(404, "Pizza not found");
                return response;
            }

            response.Data = pizza;

            return response;
        }

        public async Task<BaseResponse<Pizza>> UpdateAsync(Guid id, PizzaForCreationDto pizzaDto)
        {
            var response = new BaseResponse<Pizza>();

            // check for exist pizza
            var pizza = await unitOfWork.Pizzas.GetAsync(p => p.Id == id && p.State != ItemState.Deleted);
            if (pizza is null)
            {
                response.Error = new ErrorResponse(404, "Pizza not found");
                return response;
            }

            pizza.Name = pizzaDto.Name;
            pizza.Description = pizzaDto.Description;
            pizza.Size = pizzaDto.Size;
            pizza.Category = pizzaDto.Category;
            pizza.Price = pizzaDto.Price;

            string imagePath = await SaveFileAsync(pizzaDto.ImageUrl.OpenReadStream(), pizzaDto.ImageUrl.FileName);
            pizza.ImageUrl = config.GetSection("FileUrl:ImageUrl").Value + imagePath;

            pizza.Update();

            var result = await unitOfWork.Pizzas.UpdateAsync(pizza);

            await unitOfWork.SaveChangesAsync();

            response.Data = result;

            return response;
        }


        private async Task<string> SaveFileAsync(Stream file, string fileName)
        {
            fileName = Guid.NewGuid().ToString("N") + "_" + fileName;
            string storagePath = config.GetSection("Storage:ImageUrl").Value;
            string filePath = Path.Combine(env.WebRootPath, $"{storagePath}/{fileName}");

            FileStream mainFile = File.Create(filePath);
            await file.CopyToAsync(mainFile);
            mainFile.Close();
            return fileName;
        }


    }
}
