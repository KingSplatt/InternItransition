using System.ComponentModel.DataAnnotations;

namespace LoginAppMiguel.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "The email is required")]
        [EmailAddress(ErrorMessage = "The email format is invalid")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "The password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
