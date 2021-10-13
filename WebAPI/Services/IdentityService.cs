using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Models.ViewModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SqlDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public IdentityService(SqlDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<ResponseModel> SignInAsync(SignInModel model)
        {
            if (!EmailRegistered(model.Email))
                return new ResponseModel(false, null);

            var customer = await _context.Customers.FirstOrDefaultAsync(customer => customer.Email == model.Email);

            if (!customer.ValidatePasswordHash(model.Password))
                return new ResponseModel(false, null);

            var _secretKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey"));
            var _tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", customer.CustomerId.ToString()),
                    new Claim("DisplayName", $"{customer.FirstName} {customer.LastName}")
                }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha512Signature)
            };

            var _accessToken = _tokenHandler.WriteToken(_tokenHandler.CreateToken(_tokenDescriptor));
            customer.CreateTokenWithHash(_accessToken);

            try
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled error. {ex.Message}\n{ex}");
            }

            return new ResponseModel(true, _accessToken);
        }

        private bool EmailRegistered(string email) =>
            _context.Customers.Any(customer => customer.Email == email);

        public int GetCustomerIdFromToken(string bearer)
        {
            var token = bearer.Contains(" ") ? bearer.Split(" ")[1] : bearer;
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            var tokenId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            var parsed = int.TryParse(tokenId, out int id);

            return parsed ? id : 0;
        }
    }
}
