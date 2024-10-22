using CardManagement.Core.Domain.Common;
namespace CardManagement.Core.Domain.Users
{
    public class UserRoleAccessibility : BaseEntity, ISoftDeleteEntity
    {
        public Guid UserRoleId { get; set; }
        public Guid AccessibilityId { get; set; }
        public Guid UserID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOnUtc { get; set; } = DateTime.UtcNow;

        //navigation
        public virtual  UserRole UserRole { get; set; }
        public virtual  Accessibility Accessibility { get; set; }
    }
}
