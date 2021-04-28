using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models.CustomerModels;
using SharedLibrary.Models.ProductModels;
using SharedLibrary.Models.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Filters;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Authorize]
    [VerifyToken]
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly SqlDbContext _context;
        private readonly IIdentityService _identity;

        public WishlistController(SqlDbContext context, IIdentityService identity)
        {
            _context = context;
            _identity = identity;
        }

        // GET: api/Wishlist (hämta kundens wishlist)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetWishlist()
        {
            // Hämta CustomerId från Token
            HttpContext.Request.Headers.TryGetValue("Authorization", out var bearer);
            var customerId = _identity.GetCustomerIdFromToken(bearer);
            if (customerId == 0)
                return Unauthorized();

            var wishlistProduct = _context.Wishlists
                .Where(w => w.CustomerId == customerId)
                .Select(w => w.Product);

            // Hämta ProductColors, -Sizes, -Tags
            await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Reviews)
                    .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                    .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                    .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                    .LoadAsync();

            var productViewModels = await wishlistProduct.Select(wp => new ProductViewModel(wp)).ToListAsync();

            return productViewModels;
        }

        // GET: api/Wishlist/{productId}
        [HttpGet("{productId:int}")]
        public ActionResult<ResponseModel> IsInWishlist(int productId)
        {
            // Hämta CustomerId från Token
            HttpContext.Request.Headers.TryGetValue("Authorization", out var bearer);
            var customerId = _identity.GetCustomerIdFromToken(bearer);
            if (customerId == 0)
                return Unauthorized();

            // Requesten lyckas men produkten finns inte i wishlist
            if (!_context.Wishlists.Any(w => w.ProductId == productId && w.CustomerId == customerId))
                return Ok(new ResponseModel(false, null));

            // Returnera produktid i ResponseModel
            return Ok(new ResponseModel(true, productId.ToString()));
        }

        // POST: api/Wishlist/{productId}
        [HttpPost("{productId:int}")]
        public async Task<ActionResult<Wishlist>> AddToWishlist(int productId)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var bearer);
            var customerId = _identity.GetCustomerIdFromToken(bearer);
            if (customerId == 0)
                return Unauthorized();

            if (_context.Wishlists.Any(w => w.ProductId == productId && w.CustomerId == customerId))
                return BadRequest();

            var wishlistItem = new Wishlist { CustomerId = customerId, ProductId = productId };

            try
            {
                _context.Wishlists.Add(wishlistItem);
                await _context.SaveChangesAsync();

                return Ok(wishlistItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception.{ex.Message}\n{ex}");
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Wishlist/{productId}
        [HttpDelete("{productId:int}")]
        public async Task<ActionResult<Wishlist>> DeleteWishlistItem(int productId)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var bearer);
            if (string.IsNullOrEmpty(bearer))
                return Unauthorized();

            var customerId = _identity.GetCustomerIdFromToken(bearer);
            if (customerId == 0)
                return Unauthorized();

            var wishlistItem = _context.Wishlists.FirstOrDefault(w => w.ProductId == productId && w.CustomerId == customerId);
            if (wishlistItem == null)
                return NotFound();

            try
            {
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();

                return Ok(wishlistItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception.{ex.Message}\n{ex}");
                return BadRequest(ex.Message);
            }

        }
    }
}
