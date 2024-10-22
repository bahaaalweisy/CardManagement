using Microsoft.AspNetCore.Identity;

using CardManagement.Core.Domain.Common;

namespace CardManagement.Core.Domain.Users;

public partial class UserRole : IdentityRole<Guid>, ISoftDeleteEntity
{
    public bool IsDeleted { get; set; }
}


