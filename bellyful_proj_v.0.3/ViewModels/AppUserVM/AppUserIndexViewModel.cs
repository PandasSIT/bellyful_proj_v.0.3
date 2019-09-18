using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using bellyful_proj_v._0._3.Models;
using Microsoft.AspNetCore.Identity;

namespace bellyful_proj_v._0._3.ViewModels
{
    public class AppUserIndexViewMode
    {
        public string Id { get; set; }

        [Display(Name = "Email(Use to Login11)")]
        public string Email { get; set; }

        [Display(Name = "App User Role")]
        public string Role { get; set; }
        
        [Display(Name = "Volunteer Name")]
        public string VIdName { get; set; }
        
        public string Password { get; set; }
    }
}
