using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bellyful_proj_v._0._3.ViewModels.OrderVm
{
    public class OrderIndexPageVM
    {
        public List<OrderIndexViewModel> OrderVms { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
    }
}
