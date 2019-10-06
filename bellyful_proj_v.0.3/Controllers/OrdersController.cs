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
using bellyful_proj_v._0._3.ViewModels.OrderVm;
using Microsoft.AspNetCore.Identity;

namespace bellyful_proj_v._0._3.Controllers
{
    public class OrdersController : Controller
    {
        private readonly bellyful_v03Context _context;
        private readonly IMyEmailSender _googleEmailSender;

        private readonly UserManager<ApplicationUser> _userManager;
        string[] userEmails;
        public OrdersController(bellyful_v03Context context,
            UserManager<ApplicationUser> userManager,
            IMyEmailSender googleEmailSender)
        {
            _userManager = userManager;
            _context = context;
            _googleEmailSender = googleEmailSender;
            userEmails = _userManager.Users.Where(u => u.AppRoleId == 4).Select(u => u.Email).ToArray();
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            //  var bellyful_v03Context = _context.Order.Include(o => o.Recipient).Include(o => o.Status).Include(o => o.Volunteer);
            //  return View(await bellyful_v03Context.ToListAsync());

            var ordersVMs = new List<OrderIndexViewModel>();

            foreach (var o in _context.Order)
            {
                var vm = new OrderIndexViewModel
                {
                    OrderId = o.OrderId
                };

                if (o.CreatedDatetime != null) vm.PlacedTime = o.CreatedDatetime;
                if (o.AssignDatetime != null) vm.AssignedTime = o.AssignDatetime;
                if (o.PickupDatetime != null) vm.PickedUpTime = o.PickupDatetime;
                if (o.DeliveredDatetime != null) vm.DeliveredTime = o.DeliveredDatetime;
                if (o.StatusId != null) vm.StatusId = o.StatusId.Value;
                if (o.VolunteerId != null)
                {
                    var ss = await _context.GetVolunteerForIndex(o.VolunteerId.Value, 1);//code 0 判定查值，1 一般查值
                    vm.VIdName = ss;
                }
                vm.RIdName = await _context.GetRecipientForIndex(o.RecipientId);
                //if (o.StatusId != null)
                //{
                //    vm.Status = _context.OrderStatus.FindAsync(o.StatusId.Value).Result.Content;
                //}

                ordersVMs.Add(vm);

            }

            ordersVMs.Sort();


            return View(ordersVMs);



        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Recipient)
                .Include(o => o.Status)
                .Include(o => o.Volunteer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            ViewData["RecipientId"] = new SelectList(await _context.GetRecipientsForSelection(), "RId", "IdFullName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel orderCVM)
        {
            if (ModelState.IsValid)
            {

                //_context.Add(order);

                //_context.Database.ExecuteSqlCommand("sp_Add_Recipient @RName", name);

                try
                {
                    //await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlCommandAsync("INSERT INTO [order] ([recipient_id]) VALUES ( @OrderId  ); ", new SqlParameter("@OrderId", orderCVM.RecipientId));
                }
                catch (Exception)
                {
                    // ModelState.AddModelError("", "Out of Instock!!");
                    ViewData["RecipientId"] = new SelectList(await _context.GetRecipientsForSelection(), "RId", "IdFullName", orderCVM.RecipientId);
                    // ViewData["StatusId"] = new SelectList(_context.OrderStatus, "StatusId", "Content", orderCVM.StatusId);
                    //   ViewData["VolunteerId"] = new SelectList(await _context.GetVolunteersForSelection(null), "VId", "IdFullName", order.VolunteerId);
                    orderCVM.StatusMessage = "Oops, Out of Instock!! ";

                    return View(orderCVM);
                }

                //return RedirectToAction(nameof(Index));
                ViewData["RecipientId"] = new SelectList(await _context.GetRecipientsForSelection(), "RId", "IdFullName");
                orderCVM.StatusMessage = string.Format("Order for Recipient: {0} is Created!", _context.Recipient.FindAsync(orderCVM.RecipientId).Result.FirstName);
                return View(orderCVM);
            }
            ViewData["RecipientId"] = new SelectList(await _context.GetRecipientsForSelection(), "RId", "IdFullName", orderCVM.RecipientId);
            //  ViewData["StatusId"] = new SelectList(_context.OrderStatus, "StatusId", "Content", order.StatusId);
            //    ViewData["VolunteerId"] = new SelectList(await _context.GetVolunteersForSelection(null), "VId", "IdFullName", order.VolunteerId);
            orderCVM.StatusMessage = "Oops, Something went wrong ! ";
            return View(orderCVM);
        }

        public IActionResult Push(int orderId)
        {
            //list.Where(a => !string.IsNullOrEmpty(a.user_type)).Select(a => a.id).ToArray();
            //_context.Volunteer.Find(orderId.ToString);

            // var volunteer =  _context.Volunteer.Where(v => v.StatusId == 1 && v.RoleId == 4).FirstOrDefaultAsync();
            _googleEmailSender.SendEmailAsync(orderId.ToString(), userEmails);

            try
            {
                _context.Database.ExecuteSqlCommand("sp_PushOrderWithP @OrderId ", new SqlParameter("@OrderId", orderId));
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("Index");
        }//Cancel
        public IActionResult PushAll()
        {
            try
            {
                _context.Database.ExecuteSqlCommand("sp_PushAllOrders");
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index");
        }//Cancel

        public IActionResult Cancel(int orderId)
        {
            try
            {
                _context.Database.ExecuteSqlCommand("sp_CancelAnOrder @OrderId ", new SqlParameter("@OrderId", orderId));
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index");
        }//

        public IActionResult ResetAllOrderBatchInstock()
        {

            try
            {
                _context.Database.ExecuteSqlCommand("sp_ResetInstock_Order_Batch");
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index");
        }


        //// GET: Orders/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Order
        //        .Include(o => o.Recipient)
        //        .Include(o => o.Status)
        //        .Include(o => o.Volunteer)
        //        .FirstOrDefaultAsync(m => m.OrderId == id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}

        //// POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var order = await _context.Order.FindAsync(id);
        //    _context.Order.Remove(order);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //// GET: Orders/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Order.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["RecipientId"] = new SelectList(await _context.GetRecipientsForSelection(), "RId", "IdFullName", order.RecipientId);
        //    ViewData["StatusId"] = new SelectList(_context.OrderStatus, "StatusId", "Content", order.StatusId);
        //    ViewData["VolunteerId"] = new SelectList(await _context.GetVolunteersForSelection(null), "VId", "IdFullName", order.VolunteerId);
        //    return View(order);
        //}

        //// POST: Orders/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("OrderId,StatusId,VolunteerId,RecipientId,CreatedDatetime,AssignDatetime,PickupDatetime,DeliveredDatetime")] Order order)
        //{
        //    if (id != order.OrderId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(order);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderExists(order.OrderId))
        //            {
        //                return NotFound();
        //            }
        //            throw;
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["RecipientId"] = new SelectList(await _context.GetRecipientsForSelection(), "RId", "IdFullName", order.RecipientId);
        //    ViewData["StatusId"] = new SelectList(_context.OrderStatus, "StatusId", "Content", order.StatusId);
        //    ViewData["VolunteerId"] = new SelectList(await _context.GetVolunteersForSelection(null), "VId", "IdFullName", order.VolunteerId);
        //    return View(order);
        //}

        //private bool OrderExists(int id)
        //{
        //    return _context.Order.Any(e => e.OrderId == id);
        //}
    }
}

