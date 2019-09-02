using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class VolunteerEmergencyContact
    {
        public int VolunteerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Relationship { get; set; }

        public virtual Volunteer Volunteer { get; set; }
    }
}
