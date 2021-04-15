#nullable disable

using System.Collections.Generic;

namespace SharedLibrary.Models
{
    public partial class Tag
    {
        public Tag()
        {
            ModelTags = new HashSet<ModelTag>();
        }

        public int TagId { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<ModelTag> ModelTags { get; set; }
    }
}
