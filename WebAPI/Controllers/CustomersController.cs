using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI.Controllers
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

        //POST Customer
        //[HttpPost]
        //public async Task<ActionResult<Customer>> PostOrder(Customer customer)
        //{
        //    _context.Customers.Add(customer);
        //    await _context.SaveChangesAsync();

        //    return Ok(customer);
        //}


        //public async Task<bool> CreateUserAsync(RegisterCustomer model)
        //{
        //    if (!_context.Customers.Any(c => c.Email == model.Email))
        //    {
        //        try
        //        {
        //            var customer = new Customer()
        //            {
        //                FirstName = model.FirstName,
        //                LastName = model.LastName,
        //                Email = model.Email
        //            };
        //            customer.CreatePasswordWithHash(model.Password);
        //            _context.Customers.Add(customer);
        //            await _context.SaveChangesAsync();

        //            return true;
        //        }
        //        catch { }
        //    }

        //    return false;
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

