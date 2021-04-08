using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SqlDbContext _context;

        public ProductsController(SqlDbContext context)
        {
            _context = context;
        }


        // GET Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            //return await _context.Products.Include(p => p.Model).Include(p => p.Color).Include(p => p.Size).ToListAsync();
            return await _context.Products.ToListAsync();
        }

        //POST Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostOrder(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        // POST Color
        [HttpPost("color")]
        public async Task<ActionResult<Color>> PostColor(Color color)
        {
            _context.Colors.Add(color);
            await _context.SaveChangesAsync();

            return Ok(color);
        }

        // POST Size
        [HttpPost("size")]
        public async Task<ActionResult<Size>> PostSize(Size size)
        {
            _context.Sizes.Add(size);
            await _context.SaveChangesAsync();

            return Ok(size);
        }

        // POST: Model
        [HttpPost("model")]
        public async Task<ActionResult<ProductModel>> PostModel(ProductModel model)
        {
            _context.ProductModels.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        // POST: Brand
        [HttpPost("brand")]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return Ok(brand);
        }

    }
}
