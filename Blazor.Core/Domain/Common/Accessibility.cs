using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Core.Domain.Common
{
    public class Accessibility : BaseEntity, ISoftDeleteEntity
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
    public enum CardStatus
    { 
        Active, // 0
        Blocked  // 1
    }
}
