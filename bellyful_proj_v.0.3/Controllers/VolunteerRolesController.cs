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
    public class VolunteerRolesController : Controller
    {
        private readonly bellyful_v03Context _context;

        public VolunteerRolesController(bellyful_v03Context context)
        {
            _context = context;
        }

        // GET: VolunteerRoles
        public async Task<IActionResult> Index()
        {
            return View(await _context.VolunteerRole.ToListAsync());
        }

        // GET: VolunteerRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerRole = await _context.VolunteerRole
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (volunteerRole == null)
            {
                return NotFound();
            }

            return View(volunteerRole);
        }

        // GET: VolunteerRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VolunteerRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,RoleName")] VolunteerRole volunteerRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteerRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(volunteerRole);
        }

        // GET: VolunteerRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerRole = await _context.VolunteerRole.FindAsync(id);
            if (volunteerRole == null)
            {
                return NotFound();
            }
            return View(volunteerRole);
        }

        // POST: VolunteerRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleName")] VolunteerRole volunteerRole)
        {
            if (id != volunteerRole.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerRoleExists(volunteerRole.RoleId))
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
            return View(volunteerRole);
        }

        // GET: VolunteerRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerRole = await _context.VolunteerRole
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (volunteerRole == null)
            {
                return NotFound();
            }

            return View(volunteerRole);
        }

        // POST: VolunteerRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteerRole = await _context.VolunteerRole.FindAsync(id);
            _context.VolunteerRole.Remove(volunteerRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerRoleExists(int id)
        {
            return _context.VolunteerRole.Any(e => e.RoleId == id);
        }
    }
}
