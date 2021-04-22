using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models.CustomerModels;
using SharedLibrary.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly SqlDbContext _context;

        public WishlistController(SqlDbContext context)
        {
            _context = context;
        }


        [HttpPost("checkWishlist")]
        public async Task<ActionResult<Wishlist>> CheckWhislistItem([FromBody] int productId)
        {
            if (_context.Wishlists.Any())
            {
                if (_context.Wishlists.Any(w => w.ProductId == productId))
                {
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest();

            //try
            //{
            //    if (!_context.Wishlists.Any())
            //    {
            //        return BadRequest();
            //    }
            //    try
            //    {
            //        var wishlistItems = _context.Wishlists.ToList();
            //        foreach (Wishlist wishlistItem in wishlistItems)
            //        {
            //            if (wishlistItem.ProductId != productId)
            //            {
            //                return BadRequest();
            //            }
            //        }
            //        return Ok();
            //    }
            //    catch { }  
            //}
            //catch { }
            //return BadRequest();
            //List<Product> prodList = new List<Product>();
            //var wishlistItems = _context.Wishlists.ToList();
            //foreach (Wishlist wish in wishlistItems)
            //{
            //    var prod = _context.Products.FirstOrDefault(p => p.ProductId == wish.ProductId);
            //    prodList.Add(prod);
            //}
            //if (!prodList.Any(p => p.ModelId == modelId))
            //{
            //    return BadRequest();
            //}
            //return Ok();
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

        [HttpDelete("deleteWishlist")]
        public async Task<ActionResult<Wishlist>> DeleteWhislistItem([FromBody] int productId)
        {
            var authorized = HttpContext.Request.Headers.TryGetValue("Authorization", out var bearer);
            var token = bearer.ToString().Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var tokenId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            var customerId = int.Parse(tokenId);

            var wishlistItem = _context.Wishlists.FirstOrDefault(w => w.ProductId == productId && w.CustomerId == customerId);
            if (wishlistItem != null)
            {
                try
                {
                    _context.Wishlists.Remove(wishlistItem);
                    await _context.SaveChangesAsync();

                    return Ok(wishlistItem);
                }
                catch { }
            }
            return BadRequest();

        }
    }
}
