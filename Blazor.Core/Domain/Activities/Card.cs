using CardManagement.Core.Domain.Common;
using CardManagement.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Core.Domain.Activities
{
    public class Card : BaseEntity, ISoftDeleteEntity
    {    
        public string CardNumber { get; set; }  
        public DateTime ExpiryDate { get; set; }  
        public string CVV { get; set; }  
        public decimal Balance { get; set; }  
        public CardStatus Status { get; set; }  
        public string PIN { get; set; }  
        public string PhotoPath { get; set; }    
        public Guid UserId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User User { get; set; }
    }
}
