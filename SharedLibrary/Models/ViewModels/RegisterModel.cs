using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models.ViewModels
{
    public class RegisterModel
    {
        [Required]
        [MinLength(4, ErrorMessage = "A minimum of 4 characters is required")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "A minimum of 4 characters is required")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "A minimum of 8 characters is required")]
        public string Password { get; set; }
    }
}
