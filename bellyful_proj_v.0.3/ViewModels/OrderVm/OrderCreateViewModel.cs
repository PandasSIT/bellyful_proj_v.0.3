using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace bellyful_proj_v._0._3.ViewModels.OrderVm
{
    public class OrderCreateViewModel
    {
        public int OrderId { get; set; }

        [Required,Display(Name ="Recipient")]
        public int RecipientId { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
    }
}
