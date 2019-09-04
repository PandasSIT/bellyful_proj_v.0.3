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
    public class VolunteerPoliceInfoesController : Controller
    {
        private readonly bellyful_v03Context _context;

        public VolunteerPoliceInfoesController(bellyful_v03Context context)
        {
            _context = context;
        }

        // GET: VolunteerPoliceInfoes
        public async Task<IActionResult> Index()
        {
            var bellyful_v03Context = _context.VolunteerPoliceInfo.Include(v => v.Volunteer);
            return View(await bellyful_v03Context.ToListAsync());
        }

        // GET: VolunteerPoliceInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerPoliceInfo = await _context.VolunteerPoliceInfo
                .Include(v => v.Volunteer)
                .FirstOrDefaultAsync(m => m.VolunteerId == id);
            if (volunteerPoliceInfo == null)
            {
                return NotFound();
            }

            return View(volunteerPoliceInfo);
        }

        // GET: VolunteerPoliceInfoes/Create
        public IActionResult Create()
        {
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address");
            return View();
        }

        // POST: VolunteerPoliceInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VolunteerId,PoliceVetDate,PoliceVetVerified")] VolunteerPoliceInfo volunteerPoliceInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteerPoliceInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address", volunteerPoliceInfo.VolunteerId);
            return View(volunteerPoliceInfo);
        }

        // GET: VolunteerPoliceInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerPoliceInfo = await _context.VolunteerPoliceInfo.FindAsync(id);
            if (volunteerPoliceInfo == null)
            {
                return NotFound();
            }
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address", volunteerPoliceInfo.VolunteerId);
            return View(volunteerPoliceInfo);
        }

        // POST: VolunteerPoliceInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerId,PoliceVetDate,PoliceVetVerified")] VolunteerPoliceInfo volunteerPoliceInfo)
        {
            if (id != volunteerPoliceInfo.VolunteerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerPoliceInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerPoliceInfoExists(volunteerPoliceInfo.VolunteerId))
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
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address", volunteerPoliceInfo.VolunteerId);
            return View(volunteerPoliceInfo);
        }

        // GET: VolunteerPoliceInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerPoliceInfo = await _context.VolunteerPoliceInfo
                .Include(v => v.Volunteer)
                .FirstOrDefaultAsync(m => m.VolunteerId == id);
            if (volunteerPoliceInfo == null)
            {
                return NotFound();
            }

            return View(volunteerPoliceInfo);
        }

        // POST: VolunteerPoliceInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteerPoliceInfo = await _context.VolunteerPoliceInfo.FindAsync(id);
            _context.VolunteerPoliceInfo.Remove(volunteerPoliceInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerPoliceInfoExists(int id)
        {
            return _context.VolunteerPoliceInfo.Any(e => e.VolunteerId == id);
        }
    }
}
