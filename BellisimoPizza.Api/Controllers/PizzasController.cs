using BellisimoPizza.Domain.Commons;
using BellisimoPizza.Domain.Configurations;
using BellisimoPizza.Domain.Entities.Pizzas;
using BellisimoPizza.Domain.Enums;
using BellisimoPizza.Service.DTOs.Pizzas;
using BellisimoPizza.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BellisimoPizza.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        private readonly IPizzaService pizzaService;


        public PizzasController(IPizzaService pizzaService)
        {
            this.pizzaService = pizzaService;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<Pizza>>> Create([FromForm] PizzaForCreationDto pizzatDto)
        {
            var result = await pizzaService.CreateAsync(pizzatDto);

            return StatusCode(result.Code ?? result.Error.Code.Value, result);

        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IQueryable<Pizza>>>> GetAll([FromQuery] PaginationParams @params)
        {
            var result = await pizzaService.GetAllAsync(@params);

            return StatusCode(result.Code ?? result.Error.Code.Value, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<Pizza>>> Get([FromRoute] Guid id)
        {
            var result = await pizzaService.GetAsync(p => p.Id == id && p.State != ItemState.Deleted);

            return StatusCode(result.Code ?? result.Error.Code.Value, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse<Pizza>>> Update(Guid id, [FromForm] PizzaForCreationDto pizzaDto)
        {
            var result = await pizzaService.UpdateAsync(id, pizzaDto);

            return StatusCode(result.Code ?? result.Error.Code.Value, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id)
        {
            var result = await pizzaService.DeleteAsync(p => p.Id == id && p.State != ItemState.Deleted);

            return StatusCode(result.Code ?? result.Error.Code.Value, result);
        }
    }
}
