using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bellyful_proj_v._0._3.Models;
using Microsoft.AspNetCore.Identity;

namespace bellyful_proj_v._0._3.ViewModels
{
    public class AppUserIndexViewMode
    {
        //public IEnumerable<VolunteerForSelection> Volunteers { get; set; }
        //public IEnumerable<IdentityRole> AppRoles { get; set; }
        //public IEnumerable<ApplicationUser> AppUsers { get; set; }

        //public TYPE Type { get; set; }
        //public IEnumerable<PageModel> PageModels
        //{
        //    get
        //    {

        //    }; set;
        //}

        //public class PageModel
        //{

        //}

        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string VIdName { get; set; }
        public string Password { get; set; }
    }
}
