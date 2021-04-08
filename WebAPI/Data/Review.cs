using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Data
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int CustomerId { get; set; }
        public int ModelId { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public int? Likes { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ProductModel Model { get; set; }
    }
}
