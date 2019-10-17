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
      //  private readonly RoleManager<IdentityRole> _roleManager;
        private readonly bellyful_v03Context _context;

        public HomeController(
            UserManager<ApplicationUser> userManager,
      //      RoleManager<IdentityRole> roleManager,
            bellyful_v03Context context
            )
        {
            _userManager = userManager;
          //  _roleManager = roleManager;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index(string userEmail)
        {
            if (userEmail == null)
            {
                var m = new DashBoardViewModel();
                return View(m);
            }

            var user = await _userManager.FindByEmailAsync(userEmail);
            var dashboardViewModel = new DashBoardViewModel();

            if (user != null)
            {
                if (user.AppRoleId == 4)
                {
                    int num = 0;
                   

                    var orderlist = await _context.Order.ToListAsync();
                    var finishedOrder = orderlist.Where(o => o.StatusId == 2 && o.VolunteerId== user.VolunteerId);
                    dashboardViewModel.TotalOrder = finishedOrder.Count().ToString();
                    TimeSpan timeSpan = TimeSpan.Zero;

                    foreach (Order order in finishedOrder)
                    {
                        timeSpan += (order.DeliveredDatetime.Value - order.AssignDatetime.Value);
                    }
                    dashboardViewModel.TotalDeliveryHours = (timeSpan.TotalMinutes / 60).ToString("0.##");
                    //dashboardViewModel.TotalOrder = finishedOrder.Select(o=>o.AssignDatetime + o.DeliveredDatetime)

                    

                    for (Month i = Month.Jan; i <= Month.Dec; i++)//.ToString("0.##")
                    {
                        var monOrder = finishedOrder.Where(b => b.DeliveredDatetime.Value.Month == (int)i);
                        timeSpan = TimeSpan.Zero;
                        foreach (Order order in monOrder)
                        {
                            timeSpan += (order.DeliveredDatetime.Value - order.AssignDatetime.Value);
                        }
                              
                        if ((int)i <= DateTime.Now.Month)//小于等于当前月 给0
                        {
                            dashboardViewModel.DeliveryHrsReportMonthly[i] = double.Parse((timeSpan.TotalMinutes / 60).ToString("0.##")) ;
                        }
                        else
                        {  //超过当前月 给空值
                            if (num != 0)
                            {
                                dashboardViewModel.DeliveryHrsReportMonthly[i] = double.Parse((timeSpan.TotalMinutes / 60).ToString("0.##"));
                            }
                        }
                    }

                    for (Month i = Month.Jan; i <= Month.Dec; i++)
                    {
                        num = finishedOrder.Where(o => o.DeliveredDatetime.Value.Month == (int)i).Count();
                        if ((int)i <= DateTime.Now.Month)//小于等于当前月 给0
                        {
                            dashboardViewModel.DeliveriesReportMonthly[i] = num;
                        }
                        else
                        {
                            if (num != 0)
                            {
                                dashboardViewModel.DeliveriesReportMonthly[i] = num;
                            }
                        }
                    }
                    return View(dashboardViewModel);
                }
                else if (user.AppRoleId == 1 || user.AppRoleId == 2 || user.AppRoleId == 3)
                {

                    int num = 0;
                    //batch number
                    // var bellyful_v03Context = _context.Batch;
                    var batchlist = await _context.Batch.ToListAsync();
                    dashboardViewModel.TotalMeal = batchlist.Sum(x => x.AddAmount).ToString();
                    // total meals
                   
                    var orderlist = await _context.Order.ToListAsync();
                    var finishedOrder = orderlist.Where(o => o.StatusId == 2);
                    dashboardViewModel.TotalOrder = finishedOrder.Count().ToString();
                    TimeSpan timeSpan = TimeSpan.Zero;
                    
                    foreach (Order order in finishedOrder)
                    {
                        timeSpan += (order.DeliveredDatetime.Value - order.AssignDatetime.Value);
                    }
                    dashboardViewModel.TotalDeliveryHours= (timeSpan.TotalMinutes / 60).ToString("0.##");
                    //dashboardViewModel.TotalOrder = finishedOrder.Select(o=>o.AssignDatetime + o.DeliveredDatetime)

                    var recipientlist = await _context.Recipient.ToListAsync();
                    dashboardViewModel.TotalRecipient = recipientlist.Count().ToString();

                    for (Month i = Month.Jan; i <= Month.Dec; i++)
                    {
                        num = batchlist.Where(b => b.ProductionDate.Value.Month == (int)i).Sum(x => x.AddAmount);
                        if ((int)i <= DateTime.Now.Month)//小于等于当前月 给0
                        {
                            dashboardViewModel.MealsReportMonthly[i] = num;
                        }
                        else {  //超过当前月 给空值
                            if (num != 0)
                            {
                                dashboardViewModel.MealsReportMonthly[i] = num;
                            }
                        }
                    }

                    for (Month i = Month.Jan; i <= Month.Dec; i++)
                    {
                        num = finishedOrder.Where(o => o.DeliveredDatetime.Value.Month == (int)i).Count();
                        if ((int)i <= DateTime.Now.Month)//小于等于当前月 给0
                        {
                            dashboardViewModel.DeliveriesReportMonthly[i] = num;
                        }
                        else
                        {
                            if (num != 0)
                            {
                                dashboardViewModel.DeliveriesReportMonthly[i] = num;
                            }
                        }
                    }
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
