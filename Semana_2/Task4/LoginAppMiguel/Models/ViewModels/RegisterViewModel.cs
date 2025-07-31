using System.ComponentModel.DataAnnotations;

namespace LoginAppMiguel.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "The name is required")]
        [StringLength(100, ErrorMessage = "The name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The email is required")]
        [EmailAddress(ErrorMessage = "The email format is invalid")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "The password is required")]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
