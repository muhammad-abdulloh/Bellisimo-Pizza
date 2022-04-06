using BellisimoPizza.Domain.Commons;
using BellisimoPizza.Domain.Configurations;
using BellisimoPizza.Domain.Entities.Users;
using BellisimoPizza.Domain.Enums;
using BellisimoPizza.Service.DTOs.Users;
using BellisimoPizza.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BellisimoPizza.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<User>>> Create(UserForCreationDto userDto)
        {
            var result = await userService.CreateAsync(userDto);

            return StatusCode(result.Code ?? result.Error.Code.Value, result);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IQueryable<User>>>> GetAll([FromQuery] PaginationParams @params)
        {
            var result = await userService.GetAllAsync(@params);

            return StatusCode(result.Code ?? result.Error.Code.Value, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<User>>> Get([FromRoute] Guid id)
        {
            var result = await userService.GetAsync(p => p.Id == id && p.State != ItemState.Deleted);

            return StatusCode(result.Code ?? result.Error.Code.Value, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse<User>>> Update(Guid id, [FromForm] UserForCreationDto userDto)
        {
            var result = await userService.UpdateAsync(id, userDto);

            return StatusCode(result.Code ?? result.Error.Code.Value, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id)
        {
            var result = await userService.DeleteAsync(p => p.Id == id && p.State != ItemState.Deleted);

            return StatusCode(result.Code ?? result.Error.Code.Value, result);
        }
    }
}
