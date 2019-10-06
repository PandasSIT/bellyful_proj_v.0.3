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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
namespace bellyful_proj_v._0._3.Controllers
{
    [Authorize(Roles = "L4_DeliverMan")]
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


       

        public async Task<IActionResult> PushedOrdersIndex()
        {
            var ordersVMs = new List<OrderIndexViewModel>();

            foreach (var o in _context.Order)
            {
                if (o.StatusId == 4)//
                {
                    var vm = new OrderIndexViewModel { OrderId = o.OrderId };

                    var r = await _context.Recipient.FindAsync(o.RecipientId);

                    if (r != null)
                    {
                       vm.TheRecipientAddress=  r.AddressNumStreet;
                       if (r.DogOnProperty != null) vm.TheRecipientDogOnProperty = r.DogOnProperty.Value;
                       vm.RIdName = r.FirstName + "" + r.LastName;
                    }
                    if (o.CreatedDatetime != null) vm.PlacedTime = o.CreatedDatetime;
                    if (o.StatusId != null) vm.StatusId = o.StatusId.Value;
                    //if (o.AssignDatetime != null) vm.AssignedTime = o.AssignDatetime.Value.ToString("d/M/yy HH:mm");
                    //if (o.PickupDatetime != null) vm.PickedUpTime = o.PickupDatetime.Value.ToString("d/M/yy HH:mm");
                    //if (o.DeliveredDatetime != null) vm.DeliveredTime = o.DeliveredDatetime.Value.ToString("d/M/yy HH:mm");
                    // if (o.StatusId != null)vm.Status = _context.OrderStatus.FindAsync(o.StatusId.Value).Result.Content;

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

                        var r = await _context.Recipient.FindAsync(o.RecipientId);

                        if (r != null)
                        {
                            vm.TheRecipientAddress = r.AddressNumStreet;
                            if (r.DogOnProperty != null) vm.TheRecipientDogOnProperty = r.DogOnProperty.Value;
                            vm.RIdName = r.FirstName + "" + r.LastName;
                        }

                        if (o.CreatedDatetime != null)vm.PlacedTime = o.CreatedDatetime;
                        if (o.AssignDatetime != null)vm.AssignedTime = o.AssignDatetime;
                        if (o.PickupDatetime != null) vm.PickedUpTime = o.PickupDatetime;
                        if (o.DeliveredDatetime != null) vm.DeliveredTime = o.DeliveredDatetime;
                        if (o.StatusId != null) vm.StatusId = o.StatusId.Value;
                        if (o.VolunteerId != null)
                        {
                            var ss = await _context.GetVolunteerForIndex(o.VolunteerId.Value,1);
                            vm.VIdName = ss;
                        }
                        
                        //if (o.StatusId != null)
                        //{
                        //    vm.Status = _context.OrderStatus.FindAsync(o.StatusId.Value).Result.Content;
                        //}

                        ordersVMs.Add(vm);
                    }
                }

            }
            ordersVMs.Sort();
            return View(ordersVMs);

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

        public async Task<IActionResult> PickupMeal(int orderId, string name)
        {
            var appuser = await _userManager.FindByEmailAsync(name);
            if (appuser != null)
            {
                try
                {
                    _context.Database.ExecuteSqlCommand("sp_PickupMeal @Vid, @OrderId ",
                        new SqlParameter("@Vid", appuser.VolunteerId),
                        new SqlParameter("@OrderId", orderId));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return RedirectToAction("MyOrdersIndex", new { name = name });
        }

        public async Task<IActionResult> PickUpAllMeals(string name)
        {
            var appuser = await _userManager.FindByEmailAsync(name);
            if (appuser != null)
            {
                try
                {
                    _context.Database.ExecuteSqlCommand("sp_PickupAllMealForAVolunteer @Vid ",
                        new SqlParameter("@Vid", appuser.VolunteerId));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return RedirectToAction("MyOrdersIndex",new {name = name});
        }

        public async Task<IActionResult> Finish(int orderId, string name)
        {//
            var appuser = await _userManager.FindByEmailAsync(name);
            if (appuser != null)
            {
                try
                {
                    _context.Database.ExecuteSqlCommand("sp_FinishedOrder @Vid, @OrderId ",
                        new SqlParameter("@Vid", appuser.VolunteerId),
                        new SqlParameter("@OrderId", orderId));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return RedirectToAction("MyOrdersIndex", new { name = name });
        }
    }
}