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

        
        [HttpPost("addWishlist")]
        public async Task<ActionResult<Wishlist>> AddWhislistItem([FromBody] int productId)
        {
            var authorized = HttpContext.Request.Headers.TryGetValue("Authorization", out var bearer);
            var token = bearer.ToString().Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var tokenId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            var customerId = int.Parse(tokenId);

            if (!_context.Wishlists.Any(w => w.ProductId == productId && w.CustomerId == customerId))
            {
                try
                {       
                    var wishlistItem = new Wishlist()
                    {
                        CustomerId = customerId,
                        ProductId = productId
                    };
                    _context.Wishlists.Add(wishlistItem);
                    await _context.SaveChangesAsync();

                    return Ok(wishlistItem);
                }
                catch { }
            }
            return BadRequest();

        }
        [HttpPost("checkWishlist")]
        public async Task<ActionResult<Wishlist>> CheckWhislistItem([FromBody] int productId)
        {
            if (!_context.Wishlists.Any(w => w.ProductId == productId))
            {
                return BadRequest();    
            }
            return Ok();
        }

        [HttpDelete("deleteWishlist")]
        public async Task<ActionResult<Wishlist>> DeleteWhislistItem([FromBody] int productId)
        {
            var authorized = HttpContext.Request.Headers.TryGetValue("Authorization", out var bearer);
            var token = bearer.ToString().Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var tokenId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            var customerId = int.Parse(tokenId);

            if (_context.Wishlists.Any(w => w.ProductId == productId && w.CustomerId == customerId))
            {
                try
                {
                    var wishlistItem = new Wishlist()
                    {
                        CustomerId = customerId,
                        ProductId = productId
                    };
                    _context.Wishlists.Remove(wishlistItem);
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