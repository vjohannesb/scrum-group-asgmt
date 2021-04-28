using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models.ProductModels;
using SharedLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Filters;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly SqlDbContext _context;
        private readonly IIdentityService _identity;

        public ReviewsController(SqlDbContext context, IIdentityService identity)
        {
            _context = context;
            _identity = identity;
        }

        [Authorize]
        [VerifyToken]
        [HttpPost]
        public async Task<ActionResult<Review>> AddReview(ReviewViewModel model)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var bearer);
            var customerId = _identity.GetCustomerIdFromToken(bearer);
            if (customerId == 0)
                return Unauthorized();

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer == null)
                return Unauthorized();

            var review = new Review
            {
                ProductId = model.ProductId,
                CustomerId = customerId,
                CustomerName = $"{customer.FirstName} {customer.LastName}",
                ReviewText = model.ReviewText,
                Rating = model.Rating,
                Likes = 0,
                Created = DateTime.Now
            };

            if (_context.Reviews.Any(r => r.CustomerId == customerId && r.ProductId == review.ProductId))
                return Conflict();

            try
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                return Ok(review);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception. {ex.Message}\n{ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
            => await _context.Reviews.ToListAsync();

        [HttpGet("{productId:int}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetProductReviews(int productId)
            => await _context.Reviews.Where(r => r.ProductId == productId).ToListAsync();
    }
}
