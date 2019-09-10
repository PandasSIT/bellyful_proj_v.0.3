using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bellyful_proj_v._0._3.Models;

namespace bellyful_proj_v._0._3.Controllers
{
    public class OrdersController : Controller
    {
        private readonly bellyful_v03Context _context;

        public OrdersController(bellyful_v03Context context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var bellyful_v03Context = _context.Order.Include(o => o.Recipient).Include(o => o.Status).Include(o => o.Volunteer);
            return View(await bellyful_v03Context.ToListAsync());
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
            ViewData["RecipientId"] = new SelectList(await _context.GetRecipientForSelection(), "RId", "IdFullName");
            ViewData["StatusId"] = new SelectList(_context.OrderStatus, "StatusId", "Content");
            ViewData["VolunteerId"] = new SelectList(await _context.GetVolunteerForSelection(null), "VId", "IdFullName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,StatusId,VolunteerId,RecipientId,CreatedDatetime,AssignDatetime,PickupDatetime,DeliveredDatetime")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
             //   _context.Database.ExecuteSqlCommandAsync("s");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipientId"] = new SelectList(_context.Recipient, "RecipientId", "AddressNumStreet", order.RecipientId);
            ViewData["StatusId"] = new SelectList(_context.OrderStatus, "StatusId", "Content", order.StatusId);
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address", order.VolunteerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["RecipientId"] = new SelectList(_context.Recipient, "RecipientId", "AddressNumStreet", order.RecipientId);
            ViewData["StatusId"] = new SelectList(_context.OrderStatus, "StatusId", "Content", order.StatusId);
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address", order.VolunteerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,StatusId,VolunteerId,RecipientId,CreatedDatetime,AssignDatetime,PickupDatetime,DeliveredDatetime")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipientId"] = new SelectList(_context.Recipient, "RecipientId", "AddressNumStreet", order.RecipientId);
            ViewData["StatusId"] = new SelectList(_context.OrderStatus, "StatusId", "Content", order.StatusId);
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address", order.VolunteerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }

        public IActionResult MyOrderIndex()
        {
            throw new NotImplementedException();
        }
    }
}
