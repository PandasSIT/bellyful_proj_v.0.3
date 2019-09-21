using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bellyful_proj_v._0._3.Models;
using bellyful_proj_v._0._3.ViewModels.BatchVM;

namespace bellyful_proj_v._0._3.Controllers
{
    public class BatchesController : Controller
    {
        private readonly bellyful_v03Context _context;

        public BatchesController(bellyful_v03Context context)
        {
            _context = context;
        }

        // GET: Batches
        public async Task<IActionResult> Index()
        {
            var bellyful_v03Context = _context.Batch.Include(b => b.MealType);
            var list = await bellyful_v03Context.ToListAsync();
            list.Reverse();    //倒序显示
            return View(list);
        }

        // GET: Batches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batch
                .Include(b => b.MealType)
                .FirstOrDefaultAsync(m => m.BatchId == id);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // GET: Batches/Create
        public IActionResult Create()
        {
            ViewData["MealTypeId"] = new SelectList(_context.MealType, "MealTypeId", "MealTypeName");
            return View();
        }

        // POST: Batches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( BatchCreateViewModel batchCVM)
        {
            if (ModelState.IsValid)
            {

                _context.Add(new Batch
                {
                   // BatchId = batchCVM.BatchId,
                    MealTypeId = batchCVM.MealTypeId,
                    AddAmount = batchCVM.AddAmount
                });
                await _context.SaveChangesAsync();
                ViewData["MealTypeId"] = new SelectList(_context.MealType, "MealTypeId", "MealTypeName");
                batchCVM.StatusMessage = string.Format("Batch Added! Amount: {0}", batchCVM.AddAmount); 
                return View(batchCVM);
            }
            ViewData["MealTypeId"] = new SelectList(_context.MealType, "MealTypeId", "MealTypeName", batchCVM.MealTypeId);
            return View(batchCVM);
        }

        // GET: Batches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batch.FindAsync(id);
            if (batch == null)
            {
                return NotFound();
            }
            ViewData["MealTypeId"] = new SelectList(_context.MealType, "MealTypeId", "MealTypeName", batch.MealTypeId);
            return View(batch);
        }

        // POST: Batches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BatchId,AddAmount,ProductionDate,MealTypeId")] Batch batch)
        {
            if (id != batch.BatchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(batch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatchExists(batch.BatchId))
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
            ViewData["MealTypeId"] = new SelectList(_context.MealType, "MealTypeId", "MealTypeName", batch.MealTypeId);
            return View(batch);
        }

        // GET: Batches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batch
                .Include(b => b.MealType)
                .FirstOrDefaultAsync(m => m.BatchId == id);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // POST: Batches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var batch = await _context.Batch.FindAsync(id);
            _context.Batch.Remove(batch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BatchExists(int id)
        {
            return _context.Batch.Any(e => e.BatchId == id);
        }
    }
}
