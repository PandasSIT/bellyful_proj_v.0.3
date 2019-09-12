using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bellyful_proj_v._0._3.Models;
using bellyful_proj_v._0._3.ViewModels;
using Microsoft.AspNetCore.Identity;
namespace bellyful_proj_v._0._3.Controllers
{
    public class OrdersForVolunteerController : Controller
    {


        private readonly bellyful_v03Context _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersForVolunteerController(bellyful_v03Context context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> PushedOrdersIndex()
        {
            var ordersVMs = new List<OrderIndexViewModel>();

            foreach (var o in _context.Order)
            {
                if (o.StatusId == 2)
                {
                    var vm = new OrderIndexViewModel { OrderId = o.OrderId };

                    if (o.CreatedDatetime != null)
                        vm.PlacedTime = o.CreatedDatetime.Value.ToString("d/M/yy HH:mm");
                    if (o.AssignDatetime != null)
                        vm.AssignedTime = o.AssignDatetime.Value.ToString("d/M/yy HH:mm");
                    if (o.PickupDatetime != null)
                        vm.PickedUpTime = o.PickupDatetime.Value.ToString("d/M/yy HH:mm");
                    if (o.DeliveredDatetime != null)
                        vm.DeliveredTime = o.DeliveredDatetime.Value.ToString("d/M/yy HH:mm");
                    if (o.VolunteerId != null)
                    {
                        var ss = await _context.GetVolunteerForIndex(o.VolunteerId.Value);
                        vm.VIdName = ss;
                    }

                    vm.RIdName = await _context.GetRecipientForIndex(o.RecipientId);
                    if (o.StatusId != null)
                    {
                        vm.Status = _context.OrderStatus.FindAsync(o.StatusId.Value).Result.Content;
                    }
                    ordersVMs.Add(vm);
                }
            }
            return View(ordersVMs);
        }


        public async Task<IActionResult> MyOrdersIndex(string name)
        {
            if (name == null)
            {
                 return NotFound();
            }

            var appuser = await _userManager.FindByEmailAsync(name);
            var ordersVMs = new List<OrderIndexViewModel>();
            if (appuser != null)
            {
                foreach (var o in _context.Order)
                {
                    if (o.VolunteerId == appuser.VolunteerId)
                    {
                        var vm = new OrderIndexViewModel
                        {
                            OrderId = o.OrderId
                        };

                        if (o.CreatedDatetime != null)
                            vm.PlacedTime = o.CreatedDatetime.Value.ToString("d/M/yy HH:mm");
                        if (o.AssignDatetime != null)
                            vm.AssignedTime = o.AssignDatetime.Value.ToString("d/M/yy HH:mm");
                        if (o.PickupDatetime != null)
                            vm.PickedUpTime = o.PickupDatetime.Value.ToString("d/M/yy HH:mm");
                        if (o.DeliveredDatetime != null)
                            vm.DeliveredTime = o.DeliveredDatetime.Value.ToString("d/M/yy HH:mm");

                        if (o.VolunteerId != null)
                        {
                            var ss = await _context.GetVolunteerForIndex(o.VolunteerId.Value);
                            vm.VIdName = ss;
                        }

                        vm.RIdName = await _context.GetRecipientForIndex(o.RecipientId);
                        if (o.StatusId != null)
                        {
                            vm.Status = _context.OrderStatus.FindAsync(o.StatusId.Value).Result.Content;
                        }
                        ordersVMs.Add(vm);
                    }
                }
            }
            return View(ordersVMs);

        }

        public IActionResult Push()
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> TakeOrder(int orderId,string name)
        {


            //if (name == null)
            //{// ModelState.AddModelError("", "Can't find this user!!");
            //    return NotFound();
            //}

            //var appuser = await _userManager.FindByEmailAsync(name);


            var appuser = await _userManager.FindByEmailAsync(name);
            if (appuser != null)
            {
                try
                {
                    _context.Database.ExecuteSqlCommand("sp_TakeOrder @Vid, @OrderId ",
                        new SqlParameter("@Vid", appuser.VolunteerId),
                        new SqlParameter("@OrderId", orderId));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
          

            return RedirectToAction("PushedOrdersIndex");
        }
    }
}