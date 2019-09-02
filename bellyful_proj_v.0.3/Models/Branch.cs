using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class Branch
    {
        public Branch()
        {
            Recipient = new HashSet<Recipient>();
            Volunteer = new HashSet<Volunteer>();
        }

        public int BranchId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressNumStreet { get; set; }
        public string TownCity { get; set; }

        public virtual ICollection<Recipient> Recipient { get; set; }
        public virtual ICollection<Volunteer> Volunteer { get; set; }
    }
}
