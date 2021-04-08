﻿using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Data
{
    public partial class Brand
    {
        public Brand()
        {
            ProductModels = new HashSet<ProductModel>();
        }

        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandDescription { get; set; }

        public virtual ICollection<ProductModel> ProductModels { get; set; }
    }
}
