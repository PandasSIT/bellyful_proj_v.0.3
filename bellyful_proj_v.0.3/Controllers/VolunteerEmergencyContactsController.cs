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
    public class VolunteerEmergencyContactsController : Controller
    {
        private readonly bellyful_v03Context _context;

        public VolunteerEmergencyContactsController(bellyful_v03Context context)
        {
            _context = context;
        }

        // GET: VolunteerEmergencyContacts
        public async Task<IActionResult> Index()
        {
            var bellyful_v03Context = _context.VolunteerEmergencyContact.Include(v => v.Volunteer);
            return View(await bellyful_v03Context.ToListAsync());
        }

        // GET: VolunteerEmergencyContacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerEmergencyContact = await _context.VolunteerEmergencyContact
                .Include(v => v.Volunteer)
                .FirstOrDefaultAsync(m => m.VolunteerId == id);
            if (volunteerEmergencyContact == null)
            {
                return NotFound();
            }

            return View(volunteerEmergencyContact);
        }

        // GET: VolunteerEmergencyContacts/Create
        public IActionResult Create()
        {
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address");
            return View();
        }

        // POST: VolunteerEmergencyContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VolunteerId,FirstName,LastName,PhoneNumber,Relationship")] VolunteerEmergencyContact volunteerEmergencyContact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteerEmergencyContact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address", volunteerEmergencyContact.VolunteerId);
            return View(volunteerEmergencyContact);
        }

        // GET: VolunteerEmergencyContacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerEmergencyContact = await _context.VolunteerEmergencyContact.FindAsync(id);
            if (volunteerEmergencyContact == null)
            {
                return NotFound();
            }
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address", volunteerEmergencyContact.VolunteerId);
            return View(volunteerEmergencyContact);
        }

        // POST: VolunteerEmergencyContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerId,FirstName,LastName,PhoneNumber,Relationship")] VolunteerEmergencyContact volunteerEmergencyContact)
        {
            if (id != volunteerEmergencyContact.VolunteerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerEmergencyContact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerEmergencyContactExists(volunteerEmergencyContact.VolunteerId))
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
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerId", "Address", volunteerEmergencyContact.VolunteerId);
            return View(volunteerEmergencyContact);
        }

        // GET: VolunteerEmergencyContacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerEmergencyContact = await _context.VolunteerEmergencyContact
                .Include(v => v.Volunteer)
                .FirstOrDefaultAsync(m => m.VolunteerId == id);
            if (volunteerEmergencyContact == null)
            {
                return NotFound();
            }

            return View(volunteerEmergencyContact);
        }

        // POST: VolunteerEmergencyContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteerEmergencyContact = await _context.VolunteerEmergencyContact.FindAsync(id);
            _context.VolunteerEmergencyContact.Remove(volunteerEmergencyContact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerEmergencyContactExists(int id)
        {
            return _context.VolunteerEmergencyContact.Any(e => e.VolunteerId == id);
        }
    }
}
