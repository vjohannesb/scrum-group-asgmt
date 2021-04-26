using System;
using System.Collections.Generic;

#nullable disable

namespace SharedLibrary.Models.OrderModels
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Orders = new HashSet<Order>();
        }

        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public string AdditionalInfo { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
