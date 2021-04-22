using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models.OrderModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly SqlDbContext _context;

        public OrdersController(SqlDbContext context)
        {
            _context = context;
        }

        [HttpGet("shipping")]
        public async Task<IEnumerable<ShippingMethod>> GetShippingMethods()
            => await _context.ShippingMethods.ToListAsync();

        // POST: ShippingMethod
        [HttpPost("shipping")]
        public async Task<ActionResult<ShippingMethod>> PostBrand(ShippingMethod shipping)
        {
            if (!_context.ShippingMethods.Any(b => b.ShippingMethodName == shipping.ShippingMethodName))
            {
                _context.ShippingMethods.Add(shipping);
                await _context.SaveChangesAsync();

                return Ok(shipping);
            }
            return BadRequest();
        }

        // POST: PaymentMethods
        [HttpPost("paymentmethods")]
        public async Task<ActionResult<PaymentMethod>> PostTags(PaymentMethod paymentMethod)
        {
            if (!_context.PaymentMethods.Any(p => p.PaymentMethodName == paymentMethod.PaymentMethodName))
            {
                _context.PaymentMethods.Add(paymentMethod);
                await _context.SaveChangesAsync();

                return Ok(paymentMethod);
            }
            return BadRequest();

        }
    }
}
