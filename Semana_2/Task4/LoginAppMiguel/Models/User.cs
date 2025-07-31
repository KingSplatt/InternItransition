using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LoginAppMiguel.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        public DateTime? LastLoginTime { get; set; }

        public bool IsBlocked { get; set; } = false;

        public DateTime RegistrationTime { get; set; } = DateTime.UtcNow;
    }
}
