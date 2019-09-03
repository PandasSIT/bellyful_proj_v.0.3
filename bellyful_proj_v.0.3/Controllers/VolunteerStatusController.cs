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
    public class VolunteerStatusController : Controller
    {
        private readonly bellyful_v03Context _context;

        public VolunteerStatusController(bellyful_v03Context context)
        {
            _context = context;
        }

        // GET: VolunteerStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.VolunteerStatus.ToListAsync());
        }

        // GET: VolunteerStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerStatus = await _context.VolunteerStatus
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (volunteerStatus == null)
            {
                return NotFound();
            }

            return View(volunteerStatus);
        }

        // GET: VolunteerStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VolunteerStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusId,Content")] VolunteerStatus volunteerStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteerStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(volunteerStatus);
        }

        // GET: VolunteerStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerStatus = await _context.VolunteerStatus.FindAsync(id);
            if (volunteerStatus == null)
            {
                return NotFound();
            }
            return View(volunteerStatus);
        }

        // POST: VolunteerStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatusId,Content")] VolunteerStatus volunteerStatus)
        {
            if (id != volunteerStatus.StatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerStatusExists(volunteerStatus.StatusId))
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
            return View(volunteerStatus);
        }

        // GET: VolunteerStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerStatus = await _context.VolunteerStatus
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (volunteerStatus == null)
            {
                return NotFound();
            }

            return View(volunteerStatus);
        }

        // POST: VolunteerStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteerStatus = await _context.VolunteerStatus.FindAsync(id);
            _context.VolunteerStatus.Remove(volunteerStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerStatusExists(int id)
        {
            return _context.VolunteerStatus.Any(e => e.StatusId == id);
        }
    }
}
