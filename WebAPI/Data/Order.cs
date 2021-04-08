using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Data
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string SecondaryAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public int Shipping { get; set; }
        public decimal TotalPrice { get; set; }
        public int PaymentMethod { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual PaymentMethod PaymentMethodNavigation { get; set; }
        public virtual Shipping ShippingNavigation { get; set; }
    }
}
