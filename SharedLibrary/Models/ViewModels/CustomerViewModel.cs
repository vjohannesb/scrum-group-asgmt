using SharedLibrary.Models.CustomerModels;
using SharedLibrary.Models.OrderModels;
using SharedLibrary.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<Wishlist> Wishlists { get; set; }

        public IEnumerable<Product> ProductsInWishlist { get; set; }
    }
}
