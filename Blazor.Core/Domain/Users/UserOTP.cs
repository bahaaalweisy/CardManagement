using CardManagement.Core.Domain.Common;

namespace CardManagement.Core.Domain.Users
{
    public class UserOTP : BaseEntity, ISoftDeleteEntity
    {
        public string OneTimePassword { get; set; } = null!;
        public Guid UserId { get; set; }
        public DateTime ValidTillUtc { get; set; }
        public bool IsDeleted { get; set; }

        //navigations
        public virtual  User User { get; set; }
    }
}
