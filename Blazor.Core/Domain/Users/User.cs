using Microsoft.AspNetCore.Identity;

using CardManagement.Core.Domain.Common;
using CardManagement.Core.Domain.Activities;

namespace CardManagement.Core.Domain.Users;
public class User : IdentityUser<Guid>, ISoftDeleteEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedOnUtc { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    public string? CurrentLocation { get; set; }
    public string? PreferdLangugae { get; set; }
    public bool? AllowNotification { get; set; }
   


    // Navigation property: One user can have many cards
    public ICollection<Card> Cards { get; set; }
    public virtual ICollection<UserLoginInfo> UserDeviceLoginInfos { get; set; }

    public User()
    {
        Cards = new List<Card>();
        UserDeviceLoginInfos = new List<UserLoginInfo>();
    }

}