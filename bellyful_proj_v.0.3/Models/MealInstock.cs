using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class MealInstock
    {
        public int MealTypeId { get; set; }
        public int InstockAmount { get; set; }

        public virtual MealType MealType { get; set; }
    }
}
