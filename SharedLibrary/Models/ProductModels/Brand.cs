using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable

namespace SharedLibrary.Models.ProductModels
{
    [DataContract]
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        [DataMember]
        public int BrandId { get; set; }

        [DataMember]
        public string BrandName { get; set; }

        [DataMember]
        public string BrandDescription { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Product> Products { get; set; }
    }
}
