using SharedLibrary.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.ViewModels
{
    public class OrderViewModel : Order
    {
        [Required(ErrorMessage = "A first name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "A first name of at least 2 characters is required.")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "A last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "A last name of at least 2 characters is required.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "A valid email address is required.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to our terms and conditions to proceed.")]
        public bool AgreedToTerms { get; set; }

        public bool CreateAccount { get; set; }

        // "Create account" - implementera?
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "A minimum of 8 characters is required")]
        public string Password { get; set; }

        public IEnumerable<ProductViewModel> Products { get;set; }
    }
}
