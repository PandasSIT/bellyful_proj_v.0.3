using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace bellyful_proj_v._0._3.ViewModels.BatchVM
{
    public class BatchCreateViewModel
    {
        public int BatchId { get; set; }
        [Display(Name = "Meal Amount")]
        public int AddAmount { get; set; }
        [Display(Name = "Meal Type")]
        public int MealTypeId { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
    }
}
