using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.ViewModels
{
    public class ReviewModel
    {
        public int ModelId { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public int? Likes { get; set; }
    }
}
