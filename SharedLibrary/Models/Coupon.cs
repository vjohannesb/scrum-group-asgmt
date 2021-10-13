#nullable disable

using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public partial class Coupon
    {
        [Key]
        public int CouponId { get; set; }

        [DataType(DataType.Text)]
        public string CouponCode { get; set; }
        public decimal CouponDiscount { get; set; }
    }
}
