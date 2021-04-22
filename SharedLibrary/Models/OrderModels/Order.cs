using System;
using System.Collections.Generic;
using SharedLibrary.Models.CustomerModels;

#nullable disable

namespace SharedLibrary.Models.OrderModels
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

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
        public decimal TotalPrice { get; set; }
        public int ShippingMethodId { get; set; }
        public int PaymentMethodId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual ShippingMethod ShippingMethod { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
