using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class VolunteerStatus
    {
        public VolunteerStatus()
        {
            Volunteer = new HashSet<Volunteer>();
        }

        public int StatusId { get; set; }
        public string Content { get; set; }

        public virtual ICollection<Volunteer> Volunteer { get; set; }
    }
}
