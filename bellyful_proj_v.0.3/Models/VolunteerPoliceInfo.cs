using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class VolunteerPoliceInfo
    {
        public int VolunteerId { get; set; }
        public DateTime PoliceVetDate { get; set; }
        public bool PoliceVetVerified { get; set; }

        public virtual Volunteer Volunteer { get; set; }
    }
}
