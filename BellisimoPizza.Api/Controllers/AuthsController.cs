using BellisimoPizza.Domain.Entities.Users;
using BellisimoPizza.Service.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable
namespace BellisimoPizza.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        public static UserRegister userreg = new UserRegister();

        public IConfiguration _configuration;

        public AuthsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserRegister>> Register(UserRegisterDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            userreg.UserName = request.UserName;
            userreg.PasswordHash = passwordHash;
            userreg.PasswordSalt = passwordSalt;

            return Ok(userreg);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserRegister>> Login(UserRegisterDto request)
        {
            if (userreg.UserName != request.UserName)
            {
                return BadRequest("Invalid UserName");
            }

            if (!VerifyPasswordHash(request.Password, userreg.PasswordHash, userreg.PasswordSalt))
            {
                return BadRequest("Wrong password! ");
            }

            string token = CreateToken(userreg);

            return Ok(token);
        }

        private string CreateToken(UserRegister userreg)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userreg.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
