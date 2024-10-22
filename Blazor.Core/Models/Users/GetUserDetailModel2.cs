using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Core.Models.Users
{
    public class GetUserDetailModel2
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
        public Guid? RegionId { get; set; }
        [Required]
        public Guid SpecilitySectorId { get; set; }
        [Required]
        public Guid ExperienceLevelId { get; set; }
        [Required]
        public Guid UserTypeId { get; set; }


        public string? ProfileImage { get; set; }
        // public string UserReadableId { get; set; } = null!;
        public bool IsActive { get; set; }
        public Guid? ProjectManagerId { get; set; }
        public Guid? SupervisorId { get; set; }
        public Guid? InspectorId { get; set; }


        public List<SupervisorDto> Supervisors { get; set; } = new List<SupervisorDto>();
        public List<InspectorDto> Inspectors { get; set; } = new List<InspectorDto>();
        public ProjectManagerDto ProjectManager { get; set; }


    }
    public class SupervisorDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid CityId { get; set; }
        public Guid SpecilitySectorId { get; set; }
        public Guid ExperienceLevelId { get; set; }
        public Guid UserTypeId { get; set; }
        public List<InspectorDto> Inspectors { get; set; }

        // Other supervisor-specific properties can be added here
    }

    public class InspectorDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid CityId { get; set; }
        public Guid SpecilitySectorId { get; set; }
        public Guid ExperienceLevelId { get; set; }
        public Guid UserTypeId { get; set; }

        // Other inspector-specific properties can be added here
    }

    // Assuming you also need ProjectManagerDto
    public class ProjectManagerDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid CityId { get; set; }
        public Guid SpecilitySectorId { get; set; }
        public Guid ExperienceLevelId { get; set; }
        public Guid UserTypeId { get; set; }
        public List<SupervisorDto> Supervisors { get; set; }

        // Other project manager-specific properties can be added here
    }

}
