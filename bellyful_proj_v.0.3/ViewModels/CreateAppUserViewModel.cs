using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bellyful_proj_v._0._3.ViewModels
{
    public class CreateAppUserViewModel
    {
       
        [Required, DataType(DataType.EmailAddress),EmailAddress,Display(Name ="Email(Login Use)")]
        public string Email { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "VID(if has)")]
        public int? VolunteerId { get; set; }

        [Display(Name = "AppUserRole")]
        public int? AppUserRoleId { get; set; }

    }
}