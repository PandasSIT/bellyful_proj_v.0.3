using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bellyful_proj_v._0._3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bellyful_proj_v._0._3.ViewComponents
{
    public class MealInstockViewComponent : ViewComponent
    {
        private readonly bellyful_v03Context _context;

        public MealInstockViewComponent(bellyful_v03Context context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bellyful_v03Context = _context.MealInstock.Include(m => m.MealType);
          

            return View("ListMealInstock", await bellyful_v03Context.ToListAsync());
        }
    }
}
