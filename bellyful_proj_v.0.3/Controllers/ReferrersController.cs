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
    public class ReferrersController : Controller
    {
        private readonly bellyful_v03Context _context;

        public ReferrersController(bellyful_v03Context context)
        {
            _context = context;
        }

        // GET: Referrers
        public async Task<IActionResult> Index()
        {
            var bellyful_v03Context = _context.Referrer.Include(r => r.Role);
            return View(await bellyful_v03Context.ToListAsync());
        }

        // GET: Referrers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referrer = await _context.Referrer
                .Include(r => r.Role)
                .FirstOrDefaultAsync(m => m.ReferrerId == id);
            if (referrer == null)
            {
                return NotFound();
            }

            return View(referrer);
        }

        // GET: Referrers/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.ReferrerRole, "RoleId", "RoleName");
            return View();
        }

        // POST: Referrers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReferrerId,FirstName,LastName,OrganisationName,PhoneNumber,Email,TownCity,RoleId")] Referrer referrer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(referrer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.ReferrerRole, "RoleId", "RoleName", referrer.RoleId);
            return View(referrer);
        }

        // GET: Referrers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referrer = await _context.Referrer.FindAsync(id);
            if (referrer == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.ReferrerRole, "RoleId", "RoleName", referrer.RoleId);
            return View(referrer);
        }

        // POST: Referrers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReferrerId,FirstName,LastName,OrganisationName,PhoneNumber,Email,TownCity,RoleId")] Referrer referrer)
        {
            if (id != referrer.ReferrerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(referrer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReferrerExists(referrer.ReferrerId))
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
            ViewData["RoleId"] = new SelectList(_context.ReferrerRole, "RoleId", "RoleName", referrer.RoleId);
            return View(referrer);
        }

        // GET: Referrers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referrer = await _context.Referrer
                .Include(r => r.Role)
                .FirstOrDefaultAsync(m => m.ReferrerId == id);
            if (referrer == null)
            {
                return NotFound();
            }

            return View(referrer);
        }

        // POST: Referrers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var referrer = await _context.Referrer.FindAsync(id);
            _context.Referrer.Remove(referrer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReferrerExists(int id)
        {
            return _context.Referrer.Any(e => e.ReferrerId == id);
        }
    }
}
