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
            var test = await _context.ProductModels.ToListAsync();
            foreach (ProductModel model in test)
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null
                ? NotFound()
                : Ok(product);
        }

        [HttpGet("getProductModelById")]
        public async Task<ActionResult<ProductModel>> GetProductModelById(int id)
        {
            if (_context.ProductModels.Any())
            {
                var productModel = await _context.ProductModels.FindAsync(id);
                if (productModel != null)
                {
                    return productModel;
                }
            }

            return null;
        }


       

        [HttpGet("getReviewsById")]
        public async Task<ActionResult<List<ReviewModel>>> getReviewsById(int id)
        {
            if (_context.ProductModels.Any() && _context.Reviews.Any())
            {
                var list = new List<ReviewModel>();
                var reviewList = new List<Review>();

                foreach (Review rev in _context.Reviews)
                {
                    if (rev.ModelId == id)
                    {
                        reviewList.Add(rev);
                    }
                }
                foreach (Review revitem in reviewList)
                {
                    if (revitem.ModelId == id)
                    {
                        Customer customer = new Customer();
                        foreach (Customer cust in _context.Customers)
                        {
                            if (cust.CustomerId == revitem.CustomerId)
                            {
                                customer = cust;
                            }
                        }
                        var reviewModel = new ReviewModel()
                        {
                            ModelId = id,
                            Name = customer.FirstName,
                            Email = customer.Email,
                            Text = revitem.Text,
                            Rating = revitem.Rating
                        };

                        list.Add(reviewModel);
                    }
                }

                if (list != null)
                {
                    return list;
                }
            }

            return null;
        }


        [HttpGet("getProductsByModelId")]
        public async Task<ActionResult<List<Product>>> GetProducts(int id)
        {
            if (_context.ProductModels.Any() && _context.Products.Any())
            {
                var list = new List<Product>();
                foreach (Product prod in _context.Products)
                {
                    if (prod.ModelId == id)
                    {
                        list.Add(prod);
                    }
                }
                if (list != null)
                {
                    return list;
                }
            }

            return null;
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
        [HttpPost("registerReview")]
        public async Task<ActionResult<ReviewModel>> PostReview(ReviewModel reviewModel)
        {
            var customer = new Customer();
            foreach (Customer cust in _context.Customers)
            {
                if (cust.Email == reviewModel.Email)
                {
                    customer = cust;                   
                }
            }
            Review review = new Review()
            {
                CustomerId = customer.CustomerId,
                ModelId = reviewModel.ModelId,
                Text = reviewModel.Text,
                Rating = reviewModel.Rating,
                Likes = 0
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(review);
        }
    }
}
