using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Core.Models.Users
{
    public class GetUserLicenceModel
    {
        public Guid Id { get; set; }
        public Guid? LicenceId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LicenceName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
