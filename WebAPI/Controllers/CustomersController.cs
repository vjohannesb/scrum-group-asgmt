using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SharedLibrary.Models;
using SharedLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Filters;
using WebAPI.Services;
using WebAPI.Models;
using SharedLibrary.Models.ProductModels;
using SharedLibrary.Models.OrderModels;
using SharedLibrary.Models.CustomerModels;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Authorize]
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
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model)
        {
            var response = await _identity.SignInAsync(model);
            return response.Succeeded
                ? Ok(response)
                : Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<ResponseModel>> RegisterCustomer(RegisterCustomer model)
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

                    return Ok(new ResponseModel(true, customer.CustomerId.ToString()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unhandled error. {ex.Message}\n{ex}");
                }
            }
            return BadRequest();
        }

        // Returnerar en IEnumerable<int> med produkt-id om det hittas något,
        // annars passande http-kod
        [VerifyToken]
        [HttpGet("wishlist")]
        public async Task<ActionResult<IEnumerable<int>>> GetCustomersWishlist()
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var bearer);
            if (string.IsNullOrEmpty(bearer))
                return Unauthorized();

            var customerId = _identity.GetCustomerIdFromToken(bearer);
            if (customerId == 0)
                return Unauthorized();

            var wishlist = await _context.Wishlists
                .Where(w => w.CustomerId == customerId)
                .ToListAsync();

            if (wishlist.Count < 1)
                return NotFound();

            return Ok(wishlist.Select(w => w.ProductId));
        }

        /* 
         * [Authorize] kontrollerar token gentemot SecretKey (useAuthentication m. JWT i Startup.cs) 
         * [VerifyToken] kontrollerar token gentemot den som finns i databasen (Filters/VerifyToken.cs)
         * Funktionen körs inte om inte båda dessa "godkänns", och gör de de så returneras Ok()
         */
        [VerifyToken]
        [HttpGet("validate")]
        public IActionResult ValidateToken()
            => Ok();
    }
}