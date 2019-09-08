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
    public class DietaryRequirementsController : Controller
    {
        private readonly bellyful_v03Context _context;

        public DietaryRequirementsController(bellyful_v03Context context)
        {
            _context = context;
        }

        // GET: DietaryRequirements
        public async Task<IActionResult> Index()
        {
            return View(await _context.DietaryRequirement.ToListAsync());
        }

        // GET: DietaryRequirements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dietaryRequirement = await _context.DietaryRequirement
                .FirstOrDefaultAsync(m => m.DietaryRequirementId == id);
            if (dietaryRequirement == null)
            {
                return NotFound();
            }

            return View(dietaryRequirement);
        }

        // GET: DietaryRequirements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DietaryRequirements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DietaryRequirementId,DietaryName,MatchedSetMeal,Description")] DietaryRequirement dietaryRequirement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dietaryRequirement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dietaryRequirement);
        }

        // GET: DietaryRequirements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dietaryRequirement = await _context.DietaryRequirement.FindAsync(id);
            if (dietaryRequirement == null)
            {
                return NotFound();
            }
            return View(dietaryRequirement);
        }

        // POST: DietaryRequirements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DietaryRequirementId,DietaryName,MatchedSetMeal,Description")] DietaryRequirement dietaryRequirement)
        {
            if (id != dietaryRequirement.DietaryRequirementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dietaryRequirement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DietaryRequirementExists(dietaryRequirement.DietaryRequirementId))
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
            return View(dietaryRequirement);
        }

        // GET: DietaryRequirements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dietaryRequirement = await _context.DietaryRequirement
                .FirstOrDefaultAsync(m => m.DietaryRequirementId == id);
            if (dietaryRequirement == null)
            {
                return NotFound();
            }

            return View(dietaryRequirement);
        }

        // POST: DietaryRequirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dietaryRequirement = await _context.DietaryRequirement.FindAsync(id);
            _context.DietaryRequirement.Remove(dietaryRequirement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DietaryRequirementExists(int id)
        {
            return _context.DietaryRequirement.Any(e => e.DietaryRequirementId == id);
        }
    }
}
