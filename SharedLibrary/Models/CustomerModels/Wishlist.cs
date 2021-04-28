using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SharedLibrary.Models.ProductModels;

#nullable disable

namespace SharedLibrary.Models.CustomerModels
{
    [DataContract]
    public partial class Wishlist
    {
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public int ProductId { get; set; }

        [IgnoreDataMember]
        public virtual Customer Customer { get; set; }
        [IgnoreDataMember]
        public virtual Product Product { get; set; }
    }
}
