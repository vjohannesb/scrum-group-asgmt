using System;
using System.Collections.Generic;
using SharedLibrary.Models.ProductModels;

#nullable disable

namespace SharedLibrary.Models.CustomerModels
{
    public partial class Wishlist
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
