using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Data
{
    public partial class Size
    {
        public Size()
        {
            Products = new HashSet<Product>();
        }

        public int SizeId { get; set; }
        public string SizeName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
