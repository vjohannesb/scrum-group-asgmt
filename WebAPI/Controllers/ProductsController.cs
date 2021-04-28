using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using SharedLibrary.Models.ViewModels;
using SharedLibrary.Models.ProductModels;
using Microsoft.EntityFrameworkCore.Query;
using SharedLibrary.Models;
using SharedLibrary.Models.CustomerModels;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SqlDbContext _context;

        public ProductsController(SqlDbContext context, IIdentityService identity)
        {
            _context = context;
        }

        // Går att använda istället för _context.Products, latmanlösning
        private IIncludableQueryable<Product, Tag> ProductContext()
            => _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Reviews)
                    .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                    .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                    .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag);

        // GET: api/Products m. valfria query-parametrar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts(
            string categories = null,
            string colors = null,
            string sizes = null,
            string brands = null,
            bool inStock = false,
            int take = 9,
            int from = 0)
        {
            IEnumerable<ProductViewModel> products = await ProductContext().Select(p => new ProductViewModel(p)).ToListAsync();

            if (categories?.Length + colors?.Length + sizes?.Length == 0 && !inStock)
                return await ProductContext().Select(p => new ProductViewModel(p)).ToListAsync();

            // Filtrera produkter
            var categoryList = categories?.ToLower().Split(",");
            var colorList = colors?.ToLower().Split(",");
            var sizeList = sizes?.ToLower().Split(",");
            var brandList = brands?.ToLower().Split(",");

            if (categoryList?.Length > 0)
                products = products.Where(p => categoryList.Contains(p.Category.ToLower()));

            if (colorList?.Length > 0)
                products = products
                    .Where(p => p.Colors.Select(c => c.ColorName.ToLower())
                        .Intersect(colorList)
                        .Any());

            if (sizeList?.Length > 0)
                products = products
                    .Where(p => p.Sizes.Select(s => s.SizeName.ToLower())
                        .Intersect(sizeList)
                        .Any());

            if (brandList?.Length > 0)
                products = products.Where(p => brandList.Contains(p.BrandId.ToString()));

            if (inStock)
                products = products.Where(p => p.InStock > 0);

            return products.Skip(from).Take(take).ToList();
        }

        // GET: api/Products/id
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                await ProductContext().LoadAsync();
                return Ok(new ProductViewModel(product));
            }

            return NotFound();
        }

        // GET: Top Rated Products
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetTopProducts(int take)
        {
            var products = await ProductContext()
                .OrderByDescending(p => p.Rating)
                .Select(p => new ProductViewModel(p))
                .Take(take)
                .ToListAsync();
            return Ok(products);
        }

        // GET: Most Sold Products (räknar inte kvantitet utan bara antal ordrar m. produkt i)
        [HttpGet("popular")]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetPopularProducts(int take)
        {
            var productIds = await _context.OrderProducts
                .Select(op => op.ProductId)
                .GroupBy(i => i)
                .OrderByDescending(i => i.Count())
                .Take(take)
                .Select(i => i.Key)
                .ToListAsync();

            var products = await ProductContext()
                .Where(p => productIds.Contains(p.ProductId))
                .Select(p => new ProductViewModel(p))
                .ToListAsync();

            return products;
        }

        // GET Products api/Products/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(string searchString)
        {
            return await _context.Products.Where(p => p.ProductName.ToLower().Contains(searchString.ToLower())).ToListAsync();
        }


        // GET Products api/Category/filter <Göran>
        [HttpGet("FilterCategory")]
        public async Task<ActionResult<IEnumerable<Product>>> FilterCategory(string FilterStringCategory)
        {
            return await _context.Products.Where(p => p.Category.StartsWith(FilterStringCategory)).ToListAsync();
        }
        // GET Products api/Price/filter <Göran>
        [HttpGet("FilterPrice")]
        public async Task<ActionResult<IEnumerable<Product>>> FilterPrice(int priceMin, int priceMax)
        {
            var direction = "Asc";
            var result = _context.Products.Where(x => x.Price >= priceMin && x.Price <= priceMax).ToList();
            if (direction == "Asc")
                result.OrderBy(x => x.Price);
            else
                result.OrderByDescending(x => x.Price);
            return result;
        }

        [HttpGet("fetchDiscount")]
        public async Task<IEnumerable<Coupon>> GetCoupons( string couponCode )
        {
            List<Coupon> list = new List<Coupon>();
            var couponList = await _context.Coupons.ToListAsync();
            foreach (var coupon in couponList)
            {
                if (coupon.CouponCode == couponCode)
                {
                    list.Add(coupon);
                }
            }

            return list;
        }


        // GET Products api/Price/filter <Göran>
        //[HttpGet("FilterColor")]
        //public async Task<ActionResult<IEnumerable<Product>>> FilterColor(string FilterStringColor)
        //{
        //  var direction = "Asc";
        // var result = _context.Products.Where(x => x.Price >= min && x.Price <= max).ToList();
        // if (direction == "Asc")
        //  result.OrderBy(x => x.Price);
        // else
        //    result.OrderByDescending(x => x.Price);
        // return result;
        // }

        // Hämta flera produkter samtidigt, från ShoppingCart
        [HttpPost("multi")]
        public async Task<ActionResult<List<ProductViewModel>>> GetMultipleProducts([FromBody] List<ShoppingCartItem> cart)
        {
            var ids = cart.Select(sci => sci.ProductId).ToArray();
            if (ids.Length < 1)
                return NotFound();

            var products = await _context.Products
                .Where(pm => ids.Contains(pm.ProductId))
                .Select(pm => new ProductViewModel(pm))
                .ToListAsync();

            if (products.Count < 1)
                return NotFound();

            foreach (var product in products)
                product.Quantity = cart.FirstOrDefault(sci => sci.ProductId == product.ProductId)?.Quantity ?? 1;

            return Ok(products);
        }

        [HttpGet("{id}/related")]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetRelatedProducts(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product = await ProductContext().FirstAsync(p => p.ProductId == id);

            var tags = product.ProductTags.Select(pt => pt.TagId);
            var products = await ProductContext().ToListAsync();
            products = products
                    .Where(p => tags
                                .Intersect(p.ProductTags.Select(p => p.TagId))
                                .Any())
                    .OrderByDescending(p => tags
                                .Intersect(p.ProductTags.Select(p => p.TagId))
                                .Count())
                    .ToList();
            products.Remove(product);

            var productViewModels = products.Select(p => new ProductViewModel(p));

            return Ok(productViewModels);
        }




        // POST Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProducts(Product product)
        {
            product.DateTimeCreated = DateTime.Now;
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return Ok(product);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex);
            }

            return BadRequest();
        }


        /**** Product-info (colors, sizes, brands, tags) ****/
        /**** Product-info (colors, sizes, brands, tags) ****/
        // POST Color
        [HttpPost("color")]
        public async Task<ActionResult<Color>> PostColor(Color color)
        {
            if (!_context.Colors.Any(c => c.ColorName == color.ColorName))
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
        public async Task<ActionResult<ProductTag>> PostTags(Tag tag)
        {
            if (!_context.Tags.Any(t => t.TagName == tag.TagName))
            {
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();

                return Ok(tag);
            }
            return BadRequest();
        }

        // POST: Coupon - Skapa nytt rabattkod och spara i databasen
        [HttpPost("createCoupon")]
        public async Task<ActionResult<Coupon>> PostCoupon( Coupon coupon )
        {
            if (!_context.Coupons.Any(c => c.CouponCode == coupon.CouponCode))
            {
                _context.Coupons.Add(coupon);
                await _context.SaveChangesAsync();
                return Ok(coupon);
            }
            return BadRequest();

        }

    }
}
