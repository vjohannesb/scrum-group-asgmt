using SharedLibrary.Models.CustomerModels;
using SharedLibrary.Models.OrderModels;
using System;
using System.Collections.Generic;

#nullable disable

namespace SharedLibrary.Models.ProductModels
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
            ProductColors = new HashSet<ProductColor>();
            ProductSizes = new HashSet<ProductSize>();
            ProductTags = new HashSet<ProductTag>();
            Reviews = new HashSet<Review>();
            Wishlists = new HashSet<Wishlist>();
        }

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

        public virtual Brand Brand { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
