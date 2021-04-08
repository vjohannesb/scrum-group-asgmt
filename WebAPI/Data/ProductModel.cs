using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Data
{
    public partial class ProductModel
    {
        public ProductModel()
        {
            Products = new HashSet<Product>();
            Reviews = new HashSet<Review>();
        }

        public int ModelId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public string ProductDescription { get; set; }
        public string AdditionalInfo { get; set; }
        public string ProductImg { get; set; }
        public string Category { get; set; }
        public string ProductGroup { get; set; }
        public int? Rating { get; set; }
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
