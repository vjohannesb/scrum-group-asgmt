using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SharedLibrary.Models;
using SharedLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SqlDbContext _context;
        private readonly IIdentityService _identity;

        public CustomersController(SqlDbContext context, IIdentityService identity)
        {
            _context = context;
            _identity = identity;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model)
        {
            var response = await _identity.SignInAsync(model);
            return response.Succeeded
                ? Ok(Response)
                : Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterCustomer>> RegisterCustomer(RegisterCustomer model)
        {
            if (!_context.Customers.Any(c => c.Email == model.Email))
            {
                try
                {
                    var customer = new Customer()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email
                    };
                    customer.CreatePasswordWithHash(model.Password);
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();

                    return Ok(customer);
                }
                catch { }
            }
            return BadRequest();
        }

    }
}