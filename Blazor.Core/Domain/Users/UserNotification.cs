using CardManagement.Core.Domain.Common;

namespace CardManagement.Core.Domain.Users
{
    public class UserNotification : BaseEntity, ISoftDeleteEntity
    {
        public string Message { set; get; } = null!;
        public Guid UserId { set; get; }
        public bool IsDeleted { get; set; }

        //navigation
        public virtual  User User { set; get; }

    }
}
