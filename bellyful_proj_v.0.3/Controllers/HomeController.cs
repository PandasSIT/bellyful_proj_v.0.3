using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bellyful_proj_v._0._3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using bellyful_proj_v._0._3.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace bellyful_proj_v._0._3.Controllers
{

    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly bellyful_v03Context _context;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            bellyful_v03Context context
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index(string userEmail)
        {

            var user = await _userManager.FindByEmailAsync(userEmail);
            var dashboardViewModel = new DashBoardViewModel();

            if (user != null)
            {
                if (user.AppRoleId == 4)
                {
                  





                    return View(dashboardViewModel);
                }
                else if (user.AppRoleId == 1 || user.AppRoleId == 2 || user.AppRoleId == 3) {


                    //batch number
                    // var bellyful_v03Context = _context.Batch;
                    var list = await _context.Batch.ToListAsync();
                     dashboardViewModel.TotalMeal = list.Sum(x => x.AddAmount).ToString();
                    return View(dashboardViewModel);
                }
            }
           
            return View(dashboardViewModel);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Contact()
        {
            return View();
        }


    }
}
