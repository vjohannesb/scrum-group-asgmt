using SharedLibrary.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedLibrary.Models.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public string ProductDescription { get; set; }
        public string AdditionalInfo { get; set; }
        public string ProductImage { get; set; }
        public string Category { get; set; }
        public int? Rating { get; set; }
        public int BrandId { get; set; }
        public int InStock { get; set; }
        public DateTime DateTimeCreated { get; set; }

        public bool IsInWishlist { get; set; }

        public Brand Brand { get; set; }
        public IEnumerable<Color> Colors { get; set; }
        public IEnumerable<Size> Sizes { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

        // JSON-serializing m.m.
        public ProductViewModel() { }

        public ProductViewModel(Product p)
        {
            ProductId = p.ProductId;
            ProductName = p.ProductName;
            Price = p.Price;
            Discount = p.Discount;
            ProductDescription = p.ProductDescription;
            AdditionalInfo = p.AdditionalInfo;
            ProductImage = p.ProductImage;
            Category = p.Category;
            Rating = p.Rating;
            BrandId = p.BrandId;
            Brand = p.Brand;
            InStock = p.InStock;
            DateTimeCreated = p.DateTimeCreated;
            Reviews = p.Reviews;

            Colors = p.ProductColors.Select(pc => pc.Color);
            Sizes = p.ProductSizes.Select(ps => ps.Size);
            Tags = p.ProductTags.Select(pt => pt.Tag);
        }

        public int Quantity { get; set; } = 1;
    }
}
