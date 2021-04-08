using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SqlDbContext _context;
        private IConfiguration _configuration { get; }

        public CustomersController(SqlDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterUser model)
        //{
        //    if (await _identity.CreateUserAsync(model))
        //        return new OkResult();

        //    return new BadRequestResult();
        //}


        //public async Task<bool> CreateCustomerAsync(RegisterCustomer model)
        //{
        //    if (!_context.Customers.Any(customer => customer.Email == model.Email))
        //    {
        //        try
        //        {
        //            var user = new User()
        //            {
        //                FirstName = model.FirstName,
        //                LastName = model.LastName,
        //                Email = model.Email
        //            };
        //            user.CreatePasswordWithHash(model.Password);
        //            _context.Users.Add(user);
        //            await _context.SaveChangesAsync();

        //            return true;
        //        }
        //        catch { }
        //    }

        //    return false;
        //}
    }
}
