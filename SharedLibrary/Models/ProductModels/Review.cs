using SharedLibrary.Models.CustomerModels;
using System;
using System.Runtime.Serialization;


#nullable disable

namespace SharedLibrary.Models.ProductModels
{

    [DataContract]
    public partial class Review
    {
        [DataMember]
        public int ReviewId { get; set; }
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string ReviewText { get; set; }
        [DataMember]
        public int Rating { get; set; }
        [DataMember]
        public int? Likes { get; set; }
        [DataMember]
        public DateTime Created { get; set; }

        [IgnoreDataMember]
        public virtual Customer Customer { get; set; }
        [IgnoreDataMember]
        public virtual Product Product { get; set; }
    }
}
