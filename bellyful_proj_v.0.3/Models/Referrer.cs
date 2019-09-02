using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class Referrer
    {
        public Referrer()
        {
            Recipient = new HashSet<Recipient>();
        }

        public int ReferrerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganisationName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TownCity { get; set; }
        public int RoleId { get; set; }

        public virtual ReferrerRole Role { get; set; }
        public virtual ICollection<Recipient> Recipient { get; set; }
    }
}
