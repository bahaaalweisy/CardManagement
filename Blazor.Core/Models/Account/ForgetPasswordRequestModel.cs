using System.ComponentModel.DataAnnotations;

namespace CardManagement.Core.Models.Account
{
    public class ForgotPasswordRequestModel
    {
        [Required]
        public string Email { get; set; } = null!;
    }
}
