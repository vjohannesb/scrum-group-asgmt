#nullable disable

using System.Collections.Generic;

namespace SharedLibrary.Models
{
    public partial class Product
    {
        public Product()
        {
            Wishlists = new HashSet<Wishlist>();
        }

        public int ProductId { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int InStock { get; set; }

        public virtual Color Color { get; set; }
        public virtual ProductModel Model { get; set; }
        public virtual Size Size { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
