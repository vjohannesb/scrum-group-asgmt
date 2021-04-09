using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI.Controller
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

        // POST: Shipping
        [HttpPost("shipping")]
        public async Task<ActionResult<Shipping>> PostBrand(Shipping shipping)
        {
            if (!_context.Shippings.Any(b => b.Name == shipping.Name))
            {
                _context.Shippings.Add(shipping);
                await _context.SaveChangesAsync();

                return Ok(shipping);
            }
            return BadRequest();
        }

        // POST: PaymentMethods
        [HttpPost("paymentmethods")]
        public async Task<ActionResult<PaymentMethod>> PostTags(PaymentMethod paymentMethod)
        {
            if (!_context.PaymentMethods.Any(p => p.Name == paymentMethod.Name))
            {
                _context.PaymentMethods.Add(paymentMethod);
                await _context.SaveChangesAsync();

                return Ok(paymentMethod);
            }
            return BadRequest();

        }
    }
}
