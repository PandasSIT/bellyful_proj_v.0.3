using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class VolunteerRole
    {
        public VolunteerRole()
        {
            Volunteer = new HashSet<Volunteer>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<Volunteer> Volunteer { get; set; }
    }
}
