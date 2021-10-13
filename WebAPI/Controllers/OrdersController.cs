using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models.OrderModels;
using SharedLibrary.Models.ViewModels;
using System;
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

        // Lägg till token-check + hämta userid om inloggad?
        [HttpPost]
        public async Task<IActionResult> PostOrder(OrderViewModel orderViewModel)
        {
            var order = new Order(orderViewModel);
            order.CustomerId = null;
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("shipping")]
        public async Task<IEnumerable<ShippingMethod>> GetShippingMethods()
            => await _context.ShippingMethods.ToListAsync();


        [HttpGet("payment")]
        public async Task<IEnumerable<PaymentMethod>> GetPaymentMethods()
            => await _context.PaymentMethods.ToListAsync();

        // POST: ShippingMethod
        [HttpPost("shipping")]
        public async Task<ActionResult<ShippingMethod>> PostBrand(ShippingMethod shipping)
        {
            if (!_context.ShippingMethods.Any(b => b.ShippingMethodName == shipping.ShippingMethodName))
            {
                try
                {
                    _context.ShippingMethods.Add(shipping);
                    await _context.SaveChangesAsync();

                    return Ok(shipping);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return BadRequest();
        }

        // POST: PaymentMethods
        [HttpPost("paymentmethods")]
        public async Task<ActionResult<PaymentMethod>> PostTags(PaymentMethod paymentMethod)
        {
            if (!_context.PaymentMethods.Any(p => p.PaymentMethodName == paymentMethod.PaymentMethodName))
            {
                try
                {
                    _context.PaymentMethods.Add(paymentMethod);
                    await _context.SaveChangesAsync();

                    return Ok(paymentMethod);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return BadRequest();
        }
    }
}
