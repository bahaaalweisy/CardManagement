using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Core.Models.Users
{
    public class AttachmentModel
    {
        public string? Name { get; set; }
        public byte[]? File { get; set; }
    }
}
