using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class ReferralReason
    {
        public ReferralReason()
        {
            Recipient = new HashSet<Recipient>();
        }

        public int ReferralReasonId { get; set; }
        public string Content { get; set; }

        public virtual ICollection<Recipient> Recipient { get; set; }
    }
}
