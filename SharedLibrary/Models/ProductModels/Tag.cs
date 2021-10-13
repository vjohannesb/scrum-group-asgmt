using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable

namespace SharedLibrary.Models.ProductModels
{

    [DataContract]
    public partial class Tag
    {
        public Tag()
        {
            ProductTags = new HashSet<ProductTag>();
        }


        [DataMember]
        public int TagId { get; set; }

        [DataMember]
        public string TagName { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<ProductTag> ProductTags { get; set; }
    }
}
