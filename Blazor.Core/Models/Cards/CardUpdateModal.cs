using CardManagement.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Core.Models.Cards
{
    public class CardsUpdateModal
    {
        public Guid Id { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string CVV { get; set; }
        public decimal Balance { get; set; }
        public CardStatus Status { get; set; }
        public string PIN { get; set; }
        public string PhotoPath { get; set; }
        public Guid UserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
