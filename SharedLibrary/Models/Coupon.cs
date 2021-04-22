#nullable disable

namespace SharedLibrary.Models
{
    public partial class Coupon
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public decimal CouponDiscount { get; set; }
    }
}
