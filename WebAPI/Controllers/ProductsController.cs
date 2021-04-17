using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using SharedLibrary.Models;
using SharedLibrary.Models.ViewModels;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SqlDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(SqlDbContext context, 
            ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /* 
         * Products = individuell produkt (Blå klacksko från Bexim i stl 38)    [Fysisk, finns i lager]
         * ProductModels = "Grundmodellen" (Klacksko från Bexim)                [Abstrakt, finns bara i databas]
         *      Flera Products kommer från samma ProductModel
         */

        // GET: api/Products/models
        [HttpGet("models")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProductModels()
            => await _context.ProductModels.ToListAsync();

        [HttpGet("getWishlist")]
        public async Task<IEnumerable<ProductModel>> GetProductModelsWishlist()
        {
            List<ProductModel> list = new List<ProductModel>();
            var modelList = await _context.ProductModels.ToListAsync();
            foreach (ProductModel model in modelList)
            {
                try
                {
                    if (_context.Wishlists.Any(w => w.ProductId == model.ModelId))
                    {
                         list.Add(model);
                    }
                }
                catch { }
            }
            
            return list;
        }
            

        // GET: api/Products/models/id
        [HttpGet("models/{id}")]
        public async Task<IActionResult> GetProductModel(int id)
        {
            var productModel = await _context.ProductModels.FindAsync(id);
            return productModel == null
                ? NotFound()
                : Ok(productModel);
        }

        // GET Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }


        // GET: api/Product/id
        [HttpGet("productsById")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductById([FromBody] int id)
        {
            if (!_context.Products.Any())
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var model = await _context.ProductModels.FindAsync(id);
                    List<Product> products = new List<Product>();
                    foreach (Product prod in _context.Products)
                    {
                        if (prod.ModelId == model.ModelId)
                        {
                            products.Add(prod);
                        }
                    }
                    if (products != null)
                    {
                        return Ok(products);
                    }
                }
                catch { }
                return NotFound();
            }
                    
        }




        // GET: api/Product/id
        [HttpGet("productModelById")]
        public async Task<IActionResult> GetProductModelById(int id)
        {
            var productModel = await _context.ProductModels.FindAsync(id);
            return productModel == null
                ? NotFound()
                : Ok(productModel);
        }

        [HttpPost("multi")]
        public async Task<ActionResult<List<ProductViewModel>>> GetMultipleProducts([FromBody] List<ShoppingCartItem> cart) 
        {
            var ids = cart.Select(sci => sci.ProductId).ToArray();
            if (ids.Length < 1)
                return NotFound();

            var products = await _context.ProductModels
                .Where(pm => ids.Contains(pm.ModelId))
                .Select(pm => new ProductViewModel(pm))
                .ToListAsync();

            if (products.Count < 1)
                return NotFound();

            foreach (var product in products)
                product.Quantity = cart.FirstOrDefault(sci => sci.ProductId == product.ModelId)?.Quantity ?? 1;

            return Ok(products);
        }

        //POST Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostOrder(Product product)
        {
            var productItem = new Product()
            {
                ModelId = product.ModelId,
                ColorId = product.ColorId,
                SizeId = product.SizeId,
                InStock = product.InStock,
            };
            _context.Products.Add(productItem);
            await _context.SaveChangesAsync();

            return Ok(product);
    }

        // POST Color
        [HttpPost("color")]
        public async Task<ActionResult<Color>> PostColor(Color color)
        {  
            if (!_context.Colors.Any(c => c.Name == color.Name))
            {
                _context.Colors.Add(color);
                await _context.SaveChangesAsync();

                return Ok(color);
            }
         
            return BadRequest();
        }  

        // POST Size
        [HttpPost("size")]
        public async Task<ActionResult<Size>> PostSize(Size size)
        {
            if (!_context.Sizes.Any(s => s.SizeName == size.SizeName))
            {
                _context.Sizes.Add(size);
                await _context.SaveChangesAsync();

                return Ok(size);
            }
            return BadRequest();
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
            if (!_context.Brands.Any(b => b.BrandName == brand.BrandName))
            {
                _context.Brands.Add(brand);
                await _context.SaveChangesAsync();

                return Ok(brand);
            }
            return BadRequest();
        }

        // POST: Tags
        [HttpPost("tags")]
        public async Task<ActionResult<Tag>> PostTags(Tag tag)
        {          
            if (!_context.Tags.Any(t => t.TagName == tag.TagName))
            {
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();

                return Ok(tag);
            }
            return BadRequest();
        }

        // POST: Reviews
        [HttpPost("reviews")]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(review);
        }


        [HttpPost("addReview")]
        public async Task<ActionResult<ReviewModel>> AddReview([FromBody] ReviewModel reviewModel)
        {
            var authorized = HttpContext.Request.Headers.TryGetValue("Authorization", out var bearer);
            var token = bearer.ToString().Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var tokenId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            var customerId = int.Parse(tokenId);

            try
            {
                var review = new Review()
                {
                    CustomerId = customerId,
                    ModelId = reviewModel.ModelId,
                    Text = reviewModel.Text,
                    Rating = reviewModel.Rating,
                    Likes = reviewModel.Likes

                };
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                return Ok(review);
            }
            catch {
                return BadRequest();
            }
        }
    }
}
