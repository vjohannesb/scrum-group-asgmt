using SharedLibrary.Models.CustomerModels;
using System.Runtime.Serialization;


#nullable disable

namespace SharedLibrary.Models.ProductModels
{

    [DataContract]
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public int? Likes { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
