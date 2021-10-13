using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.ViewModels
{
    public class CouponViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string CouponCode { get; set; }
    }
}
