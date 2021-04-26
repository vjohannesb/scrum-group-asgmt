using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Models.CustomerModels;
using SharedLibrary.Models.ViewModels;

#nullable disable

namespace SharedLibrary.Models.OrderModels
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public Order(OrderViewModel ovm)
        {
            CompanyName = ovm.CompanyName;
            Country = ovm.Country;
            Address = ovm.Address;
            SecondaryAddress = ovm.SecondaryAddress;
            City = ovm.City;
            State = ovm.State;
            ZipCode = ovm.ZipCode;
            PhoneNumber = ovm.PhoneNumber;
            TotalPrice = ovm.TotalPrice;
            ShippingMethodId = ovm.ShippingMethodId;
            PaymentMethodId = ovm.PaymentMethodId;
            OrderProducts = ovm.OrderProducts;
        }

        [Required]
        public int OrderId { get; set; }

        public int? CustomerId { get; set; }

        public string CompanyName { get; set; }
        
        [Required]
        public string Country { get; set; }
        
        [Required]
        public string Address { get; set; }
        public string SecondaryAddress { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string State { get; set; }
        
        [Required]
        public string ZipCode { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }

        public decimal TotalPrice { get; set; }
        
        [Required]
        public int ShippingMethodId { get; set; }
        
        [Required]
        public int PaymentMethodId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual ShippingMethod ShippingMethod { get; set; }

        [Required]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
