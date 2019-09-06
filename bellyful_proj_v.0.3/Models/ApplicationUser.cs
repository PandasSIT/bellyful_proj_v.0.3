using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace bellyful_proj_v._0._3.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? VolunteerId { get; set; }
        public int? AppRoleId { get; set; }

    }
}
