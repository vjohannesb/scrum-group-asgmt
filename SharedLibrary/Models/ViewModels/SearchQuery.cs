using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.ViewModels
{
    public class SearchQuery
    {
        [DataType(DataType.Text)]
        public string Query { get; set; }

        public string Category { get; set; }
    }
}
