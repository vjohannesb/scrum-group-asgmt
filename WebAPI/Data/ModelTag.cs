using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Data
{
    public partial class ModelTag
    {
        public int ModelId { get; set; }
        public int TagId { get; set; }

        public virtual ProductModel Model { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
