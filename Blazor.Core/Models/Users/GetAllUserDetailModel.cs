using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Core.Models.Users
{
    public class GetAllUserDetailModel
    {
        public Guid Id { get; set; }
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
        [Required]
        public Guid CityId { get; set; }
        [Required]
        public Guid SpecilitySectorId { get; set; }
        [Required]
        public Guid ExperienceLevelId { get; set; }
        public string ExperienceLevelName { get; set; }
        public string SpecilitySectorName { get; set; }

        public string CityName { get; set; }

        public string? ProfileImage { get; set; }
        public string RoleName { get; set; }
        public Guid? userTypeId { get; set; }

        // public string UserReadableId { get; set; } = null!;
        public bool IsActive { get; set; }

        public Guid? ProjectManagerId { get; set; }
        public Guid? SupervisorId { get; set; }
        public Guid? InspectorId { get; set; }
    }
}
