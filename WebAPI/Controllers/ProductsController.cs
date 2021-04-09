﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using SharedLibrary.Models;

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


        // GET Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
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

    }
}