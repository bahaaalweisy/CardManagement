using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Core.Models.Common
{
    public class ReturnResult
    {
        public ReturnResult()
        {
            Errors = new List<string>();
        }
        public bool IsValid => !Errors.Any();
        public List<string> Errors { get; set; }
    }
}
