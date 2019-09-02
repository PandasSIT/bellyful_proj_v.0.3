using System;
using System.Collections.Generic;

namespace bellyful_proj_v._0._3.Models
{
    public partial class MealType
    {
        public MealType()
        {
            Batch = new HashSet<Batch>();
        }

        public int MealTypeId { get; set; }
        public string MealTypeName { get; set; }
        public string ShelfLocation { get; set; }

        public virtual MealInstock MealInstock { get; set; }
        public virtual ICollection<Batch> Batch { get; set; }
    }
}
