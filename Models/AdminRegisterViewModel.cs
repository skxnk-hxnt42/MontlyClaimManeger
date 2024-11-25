using System.ComponentModel.DataAnnotations;

namespace Claiming_System.Models
{
    public class AdminRegisterViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        // You can add more fields as necessary
    }
}
