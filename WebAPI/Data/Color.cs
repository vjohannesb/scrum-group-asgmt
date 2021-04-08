using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Data
{
    public partial class Color
    {
        public Color()
        {
            Products = new HashSet<Product>();
        }

        public int ColorId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
