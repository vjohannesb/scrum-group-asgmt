using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable

namespace SharedLibrary.Models.ProductModels
{

    [DataContract]
    public partial class Color
    {
        public Color()
        {
            ProductColors = new HashSet<ProductColor>();
        }

        [DataMember]
        public int ColorId { get; set; }
        [DataMember]
        public string ColorName { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<ProductColor> ProductColors { get; set; }
    }
}
