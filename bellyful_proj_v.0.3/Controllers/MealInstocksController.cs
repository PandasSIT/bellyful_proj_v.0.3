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
    public class MealInstocksController : Controller
    {
        private readonly bellyful_v03Context _context;

        public MealInstocksController(bellyful_v03Context context)
        {
            _context = context;
        }

        // GET: MealInstocks
        public async Task<IActionResult> Index()
        {
            var bellyful_v03Context = _context.MealInstock.Include(m => m.MealType);
            return View(await bellyful_v03Context.ToListAsync());
        }

        // GET: MealInstocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealInstock = await _context.MealInstock
                .Include(m => m.MealType)
                .FirstOrDefaultAsync(m => m.MealTypeId == id);
            if (mealInstock == null)
            {
                return NotFound();
            }

            return View(mealInstock);
        }

        // GET: MealInstocks/Create
        public IActionResult Create()
        {
            ViewData["MealTypeId"] = new SelectList(_context.MealType, "MealTypeId", "MealTypeName");
            return View();
        }

        // POST: MealInstocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MealTypeId,InstockAmount")] MealInstock mealInstock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mealInstock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MealTypeId"] = new SelectList(_context.MealType, "MealTypeId", "MealTypeName", mealInstock.MealTypeId);
            return View(mealInstock);
        }

        // GET: MealInstocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealInstock = await _context.MealInstock.FindAsync(id);
            if (mealInstock == null)
            {
                return NotFound();
            }
            ViewData["MealTypeId"] = new SelectList(_context.MealType, "MealTypeId", "MealTypeName", mealInstock.MealTypeId);
            return View(mealInstock);
        }

        // POST: MealInstocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MealTypeId,InstockAmount")] MealInstock mealInstock)
        {
            if (id != mealInstock.MealTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mealInstock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealInstockExists(mealInstock.MealTypeId))
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
            ViewData["MealTypeId"] = new SelectList(_context.MealType, "MealTypeId", "MealTypeName", mealInstock.MealTypeId);
            return View(mealInstock);
        }

        // GET: MealInstocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealInstock = await _context.MealInstock
                .Include(m => m.MealType)
                .FirstOrDefaultAsync(m => m.MealTypeId == id);
            if (mealInstock == null)
            {
                return NotFound();
            }

            return View(mealInstock);
        }

        // POST: MealInstocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mealInstock = await _context.MealInstock.FindAsync(id);
            _context.MealInstock.Remove(mealInstock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MealInstockExists(int id)
        {
            return _context.MealInstock.Any(e => e.MealTypeId == id);
        }
    }
}
