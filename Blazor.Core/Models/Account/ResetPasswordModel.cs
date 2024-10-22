using System.ComponentModel.DataAnnotations;

namespace CardManagement.Core.Models.Account
{
    public class ResetPasswordModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = null!;
     

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
