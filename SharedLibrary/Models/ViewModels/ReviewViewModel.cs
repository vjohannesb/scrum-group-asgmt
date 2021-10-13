using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.ViewModels
{
    public class ReviewViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [StringLength(500, MinimumLength = 10, ErrorMessage = "Your review has to be between 10 and 500 characters long.")]
        public string ReviewText { get; set; }

        [Required(ErrorMessage = "A rating is required.")]
        public int Rating { get; set; }
    }
}
