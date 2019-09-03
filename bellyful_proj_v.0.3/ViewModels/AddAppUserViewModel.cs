using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bellyful_proj_v._0._3.ViewModels
{
    public class AddAppUserViewModel
    {
        [Required, Display(Name = "User Full Name")]
        public string UserName { get; set; }

        [Required, DataType(DataType.EmailAddress),EmailAddress]
        public string Email { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }

        public int? VolunteerId { get; set; }
    }
}