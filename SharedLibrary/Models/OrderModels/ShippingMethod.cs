using System;
using System.Collections.Generic;

#nullable disable

namespace SharedLibrary.Models.OrderModels
{
    public partial class ShippingMethod
    {
        public ShippingMethod()
        {
            Orders = new HashSet<Order>();
        }

        public int ShippingMethodId { get; set; }
        public string ShippingMethodName { get; set; }
        public decimal Price { get; set; }
        public string AdditionalInfo { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
