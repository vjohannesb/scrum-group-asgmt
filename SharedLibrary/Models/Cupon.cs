#nullable disable

namespace SharedLibrary.Models
{
    public partial class Cupon
    {
        public int CuponId { get; set; }
        public string Code { get; set; }
        public decimal Discount { get; set; }
    }
}
