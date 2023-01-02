using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace BatVpn.Domain.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegisterDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public DateTime BirthDate { get; set; }
        public bool HasSellerProfile { get; set; }
        public bool HasUserProfile { get; set; }
        public string FullName { get; set; }

        public bool IsActive { get; set; }


    }
}
