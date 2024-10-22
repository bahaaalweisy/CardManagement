using System.ComponentModel.DataAnnotations;

namespace CardManagement.Core.Models.Account
{
    public class VerifyForgotEmailOTPModel
    {
        [Required]
        public string IdNumber { get; set; } = null!;

        //[JsonIgnore]
        //public string? Email { get;set; }=null!;
        [Required]
        public string Otp { get; set; } = null!;
    }
}
