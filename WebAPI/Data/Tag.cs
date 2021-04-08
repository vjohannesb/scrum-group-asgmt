using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Data
{
    public partial class Tag
    {
        public int TagId { get; set; }
        public int ModelId { get; set; }
        public string TagName { get; set; }
    }
}
