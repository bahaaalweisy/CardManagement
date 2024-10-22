using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CardManagement.Core.Models.Users
{
    public class UserDetailModel
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
    
        [Required]
        public Guid UserTypeId { get; set; }
     

        [JsonIgnore]
        public List<string> Roles { get; set; }= new List<string>();

        public bool IsActive { get; set; }
    }
    
   
   
}