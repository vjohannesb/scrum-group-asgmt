using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Data
{
    public partial class Wishlist
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
