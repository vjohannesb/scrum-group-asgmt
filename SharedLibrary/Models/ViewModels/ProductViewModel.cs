using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.ViewModels
{
    public class ProductViewModel : ProductModel
    {
        // JSON-serializing m.m.
        public ProductViewModel() { }

        public ProductViewModel(ProductModel pm)
        {
            ModelId = pm.ModelId;
            ProductName = pm.ProductName;
            Price = pm.Price;
            Discount = pm.Discount;
            ProductDescription = pm.ProductDescription;
            AdditionalInfo = pm.AdditionalInfo;
            ProductImg = pm.ProductImg;
            Category = pm.Category;
            ProductGroup = pm.ProductGroup;
            Rating = pm.Rating;
            BrandId = pm.BrandId;
        }

        public int Quantity { get; set; } = 1;
    }
}
