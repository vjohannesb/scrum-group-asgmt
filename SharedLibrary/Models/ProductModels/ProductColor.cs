#nullable disable

namespace SharedLibrary.Models.ProductModels
{
    public partial class ProductColor
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }

        public virtual Color Color { get; set; }

        public virtual Product Product { get; set; }
    }
}
