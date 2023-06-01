using System.ComponentModel.DataAnnotations;

namespace ArtisanELearningSystem.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

       
        public string? Category { get; set; }

        [Required]
        public string AboutMe { get; set; }
    }
}
