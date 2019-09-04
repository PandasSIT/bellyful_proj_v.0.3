using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bellyful_proj_v._0._3.Models;

namespace bellyful_proj_v._0._3.ViewModels
{
    public class AppUserIndexViewModel
    {
        public IEnumerable<Volunteer> Volunteers { get; set; }
        public IEnumerable<AppRole> AppRoles { get; set; }
    }
}
