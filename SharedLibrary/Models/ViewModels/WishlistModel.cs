﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Models.CustomerModels;
using SharedLibrary.Models.ProductModels;
using SharedLibrary.Models;

namespace SharedLibrary.Models.ViewModels
{
    public class WishlistModel
    {
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
