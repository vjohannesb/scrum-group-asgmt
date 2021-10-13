#nullable disable

namespace SharedLibrary.Models.ProductModels
{

    public partial class ProductSize
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Size Size { get; set; }
    }
}
