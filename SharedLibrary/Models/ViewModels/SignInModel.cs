using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models.ViewModels
{
    public class SignInModel
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "A minimum of 8 characters is required")]
        public string Password { get; set; }
    }
}
