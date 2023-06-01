using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ArtisanELearningSystem.Models
{
    public class ForgotPasswordModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public ForgotPasswordStatus Status { get; set; }
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public enum ForgotPasswordStatus
        {
            None,
            Success,
            Error
        }
    }
}
