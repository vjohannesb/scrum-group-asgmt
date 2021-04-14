using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.ViewModels
{
    public class ContactModel
    {
        [Display(Name = "Category")]
        [Required]
        public string ProductCategory { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "A minimum of 4 characters is required")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "A minimum of 4 characters is required")]
        public string Subject { get; set; }
        [Required]
        [MinLength(30, ErrorMessage = "A minimum of 30 characters is required")]
        public string Message { get; set; }


    }
}
