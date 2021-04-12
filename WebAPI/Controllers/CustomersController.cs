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
using WebAPI.Filters;
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
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model)
        {
            var response = await _identity.SignInAsync(model);
            return response.Succeeded
                ? Ok(response)
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

        // POST Color
        [HttpPost("wish")]
        public async Task<ActionResult<Wishlist>> wish(Wishlist model)
        {

            var customer = _context.Customers.Find(model.CustomerId);
            var product = _context.Products.Find(model.ProductId);

            var wishlistItem = new Wishlist()
            {
                CustomerId = customer.CustomerId,
                ProductId = product.ProductId
            };
            _context.Wishlists.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return Ok(wishlistItem);

        }


        [HttpPost("wishlist")]
        public async Task<ActionResult<Wishlist>> AddWhislistItem(Wishlist model)
        {
            if (!_context.Wishlists.Any(w => w.ProductId == model.ProductId))
            {
                try
                {
                    //var wishlistItem =
                    //   from customer in _context.Customers
                    //   join product in _context.Products on customer.CustomerId equals product.ProductId
                    //   where customer.CustomerId == model.CustomerId && product.ProductId == model.ProductId
                    //   select new { CustomerId = customer.CustomerId, ProductId = product.ProductId };

                    var wishlistItem = new Wishlist()
                    {
                        CustomerId = model.CustomerId,
                        ProductId = model.ProductId
                    };
                    _context.Wishlists.Add(wishlistItem);
                    await _context.SaveChangesAsync();

                    return Ok(wishlistItem);
                }
                catch { }
            }
            return BadRequest();
        }


        /* 
         * [Authorize] kontrollerar token gentemot SecretKey (useAuthentication m. JWT i Startup.cs) 
         * [VerifyToken] kontrollerar token gentemot den som finns i databasen (Filters/VerifyToken.cs)
         * Funktionen körs inte om inte båda dessa "godkänns", och gör de de så returneras Ok()
         */
        [Authorize]
        [VerifyToken]
        [HttpPost("validate")]
        public IActionResult ValidateToken()
            => Ok();

    }
}