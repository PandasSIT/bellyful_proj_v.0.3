using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bellyful_proj_v._0._3.Models
{
    public partial class Batch
    {
        public int BatchId { get; set; }
        [Display(Name = "Meal Amount")]
        public int AddAmount { get; set; }
       
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ProductionDate { get; set; }
        [Display(Name = "Meal Type")]
        public int MealTypeId { get; set; }

        public virtual MealType MealType { get; set; }
    }
}
