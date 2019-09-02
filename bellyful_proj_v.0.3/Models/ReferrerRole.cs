using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class ReferrerRole
    {
        public ReferrerRole()
        {
            Referrer = new HashSet<Referrer>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<Referrer> Referrer { get; set; }
    }
}
