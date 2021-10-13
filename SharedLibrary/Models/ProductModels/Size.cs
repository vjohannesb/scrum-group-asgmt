using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable

namespace SharedLibrary.Models.ProductModels
{

    [DataContract]
    public partial class Size
    {
        public Size()
        {
            ProductSizes = new HashSet<ProductSize>();
        }

        [DataMember]
        public int SizeId { get; set; }
        [DataMember]
        public string SizeName { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
    }
}
