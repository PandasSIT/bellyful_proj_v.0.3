using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class DietaryRequirement
    {
        public DietaryRequirement()
        {
            Recipient = new HashSet<Recipient>();
        }

        public int DietaryRequirementId { get; set; }
        public string DietaryName { get; set; }
        public string MatchedSetMeal { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Recipient> Recipient { get; set; }
    }
}
