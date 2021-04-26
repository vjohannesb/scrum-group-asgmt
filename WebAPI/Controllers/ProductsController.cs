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

namespace WebAPI.Controllers
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

        // Använd denna istället för _context.Products!
        // ex: var products = await ProductContext().ToListAsync();
        // eller som jag gjort i GetProducts
        private IIncludableQueryable<Product, Tag> ProductContext()
            => _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                    .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                    .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag);

        // GET: api/Products
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

        [HttpGet("getWishlist")]
        public async Task<IEnumerable<Product>> GetProductsWishlist()
        {
            List<Product> list = new List<Product>();
            var test = await _context.Products.ToListAsync();
            foreach (var product in test)
            {
                try
                {
                    if (_context.Wishlists.Any(w => w.ProductId == product.ProductId))
                         list.Add(product);
                }
                catch { }
            }
            
            return list;
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


        /* Dessa kan nu ersättas med _context.Products.Include(p => p.ProductColors)...
        [HttpGet("getProductById")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            if (_context.Products.Any())
            {
                var productModel = await _context.Products.FindAsync(id);
                if (productModel != null)
                {
                    return productModel;
                }
            }

            return null;
        }
        
        [HttpGet("getTagsById")]
        public async Task<ActionResult<List<ProductTag>>> getTagsById(int id)
        {
            if (_context.Products.Any() && _context.Tags.Any())
            {
                var list = new List<ProductTag>();
                var modelTagList = new List<ProductTag>();

                foreach (var mtag in _context.ProductTags)
                {
                    if (mtag.ProductId == id)
                    {
                        modelTagList.Add(mtag);
                    }
                }
                foreach (var mtagItem in modelTagList)
                {
                    if (mtagItem.ProductId == id)
                    {
                        ProductTag tag = new ProductTag();
                        foreach (Tag t in _context.Tags)
                        {
                            if (t.TagId == mtagItem.TagId)
                            {
                                tag = t;
                            }
                        }
                        var addTag = new Tag()
                        {
                            TagName = tag.TagName
                        };

                        list.Add(addTag);
                    }
                }

                if (list != null)
                {
                    return list;
                }
            }

            return null;
        }

        [HttpGet("getReviewsById")]
        public async Task<ActionResult<List<ReviewModel>>> getReviewsById(int id)
        {
            if (_context.Products.Any() && _context.Reviews.Any())
            {
                var list = new List<ReviewModel>();
                var reviewList = new List<Review>();

                foreach (Review rev in _context.Reviews)
                {
                    if (rev.ProductId == id)
                    {
                        reviewList.Add(rev);
                    }
                }
                foreach (Review revitem in reviewList)
                {
                    if (revitem.ProductId == id)
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
                            ProductId = id,
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

        [HttpGet("getProductsByProductId")]
        public async Task<ActionResult<List<Product>>> GetProducts(int id)
        {
            if (_context.Products.Any() && _context.Products.Any())
            {
                var list = new List<Product>();
                foreach (Product prod in _context.Products)
                {
                    if (prod.ProductId == id)
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

        */

     

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

        //POST Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProducts(Product product)
        {
            product.DateTimeCreated = DateTime.Now;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
    }

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

        // POST: Reviews
        [HttpPost("registerReview")]
        public async Task<ActionResult<ReviewModel>> PostReview(ReviewModel reviewModel)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == reviewModel.Email);

            Review review = new Review()
            {
                CustomerId = customer.CustomerId,
                ProductId = reviewModel.ProductId,
                ReviewText = reviewModel.Text,
                Rating = reviewModel.Rating,
                Likes = 0
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(review);
        }

        [HttpPut("ChangeNameCustomer")]
        public async Task<IActionResult> ChangeNameCustomer(ReviewModel revmodel)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(u => u.Email == revmodel.Email);
            user.FirstName = revmodel.Name;
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
