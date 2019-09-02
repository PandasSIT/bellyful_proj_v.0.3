using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class Batch
    {
        public int BatchId { get; set; }
        public int AddAmount { get; set; }
        public DateTime ProductionDate { get; set; }
        public int MealTypeId { get; set; }

        public virtual MealType MealType { get; set; }
    }
}
