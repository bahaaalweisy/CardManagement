
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Core.Models.Users
{
    public class UpdateUserDetailModel
    {
        [Required]
        [ValidGuid]
        public string Id { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string IdNumber { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [ValidGuid]
        [Required]
        public string CityId { get; set; } = null!;

        [ValidGuid]
        [Required]
        public string RegionId { get; set; } = null!;
        [Required]
        [ValidGuid]
        public string UserTypeId { get; set; } = null!;
 
        [Required]
        public bool IsActive { get; set; }

      


     
    }
}
