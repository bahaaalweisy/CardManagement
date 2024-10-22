using CardManagement.Core.Models.Users;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Core.Models.Cards
{
    public class CardApplicationModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }     
        public IFormFile IdPhoto { get; set; }
    }
}
