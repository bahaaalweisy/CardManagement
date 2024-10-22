using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Core.Models.Cards
{
    public class TransferBalanceModel
    {
        public Guid FromCardId { get; set; }
        public Guid ToCardId { get; set; }
        public decimal Amount { get; set; }
    }
}
