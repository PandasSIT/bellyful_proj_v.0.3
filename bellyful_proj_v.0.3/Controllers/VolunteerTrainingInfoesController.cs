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
    public class VolunteerTrainingInfoesController : Controller
    {
        private readonly bellyful_v03Context _context;

        public VolunteerTrainingInfoesController(bellyful_v03Context context)
        {
            _context = context;
        }

        // GET: VolunteerTrainingInfoes
        public async Task<IActionResult> Index()
        {
            var bellyful_v03Context = _context.VolunteerTrainingInfo.Include(v => v.Volunteer);
            return View(await bellyful_v03Context.ToListAsync());
        }

        // GET: VolunteerTrainingInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerTrainingInfo = await _context.VolunteerTrainingInfo
                .Include(v => v.Volunteer)
                .FirstOrDefaultAsync(m => m.VolunteerId == id);
            if (volunteerTrainingInfo == null)
            {
                return NotFound();
            }

            return View(volunteerTrainingInfo);
        }

        // GET: VolunteerTrainingInfoes/Create
        public IActionResult Create()
        {
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address");
            return View();
        }

        // POST: VolunteerTrainingInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VolunteerId,DeliveryTraining,HSTraining,FirstAidRaining,OtherTrainingSkill")] VolunteerTrainingInfo volunteerTrainingInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteerTrainingInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address", volunteerTrainingInfo.VolunteerId);
            return View(volunteerTrainingInfo);
        }

        // GET: VolunteerTrainingInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerTrainingInfo = await _context.VolunteerTrainingInfo.FindAsync(id);
            if (volunteerTrainingInfo == null)
            {
                return NotFound();
            }
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address", volunteerTrainingInfo.VolunteerId);
            return View(volunteerTrainingInfo);
        }

        // POST: VolunteerTrainingInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerId,DeliveryTraining,HSTraining,FirstAidRaining,OtherTrainingSkill")] VolunteerTrainingInfo volunteerTrainingInfo)
        {
            if (id != volunteerTrainingInfo.VolunteerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerTrainingInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerTrainingInfoExists(volunteerTrainingInfo.VolunteerId))
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
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address", volunteerTrainingInfo.VolunteerId);
            return View(volunteerTrainingInfo);
        }

        // GET: VolunteerTrainingInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerTrainingInfo = await _context.VolunteerTrainingInfo
                .Include(v => v.Volunteer)
                .FirstOrDefaultAsync(m => m.VolunteerId == id);
            if (volunteerTrainingInfo == null)
            {
                return NotFound();
            }

            return View(volunteerTrainingInfo);
        }

        // POST: VolunteerTrainingInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteerTrainingInfo = await _context.VolunteerTrainingInfo.FindAsync(id);
            _context.VolunteerTrainingInfo.Remove(volunteerTrainingInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerTrainingInfoExists(int id)
        {
            return _context.VolunteerTrainingInfo.Any(e => e.VolunteerId == id);
        }
    }
}
