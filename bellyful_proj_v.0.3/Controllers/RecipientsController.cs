using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bellyful_proj_v._0._3.Models;

namespace bellyful_proj_v._0._3.Controllers
{
    public class RecipientsController : Controller
    {
        private readonly bellyful_v03Context _context;

        public RecipientsController(bellyful_v03Context context)
        {
            _context = context;
        }

        // GET: Recipients
        public async Task<IActionResult> Index()
        {
            var bellyful_v03Context = _context.Recipient.Include(r => r.Branch).Include(r => r.DietaryRequirement).Include(r => r.ReferralReason).Include(r => r.Referrer);
            return View(await bellyful_v03Context.ToListAsync());
        }

        // GET: Recipients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _context.Recipient
                .Include(r => r.Branch)
                .Include(r => r.DietaryRequirement)
                .Include(r => r.ReferralReason)
                .Include(r => r.Referrer)
                .FirstOrDefaultAsync(m => m.RecipientId == id);
            if (recipient == null)
            {
                return NotFound();
            }

            return View(recipient);
        }

        // GET: Recipients/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name");
            ViewData["DietaryRequirementId"] = new SelectList(_context.DietaryRequirement, "DietaryRequirementId", "DietaryName");
            ViewData["ReferralReasonId"] = new SelectList(_context.ReferralReason, "ReferralReasonId", "Content");
            ViewData["ReferrerId"] = new SelectList(_context.Referrer, "ReferrerId", "FirstName");
            return View();
        }

        // POST: Recipients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipientId,FirstName,LastName,AddressNumStreet,TownCity,Postcode,PhoneNumber,Email,DogOnProperty,BranchId,ReferralReasonId,OtherReferralInfo,AdultsNum,Under5ChildrenNum,_510ChildrenNum,_1117ChildrenNum,DietaryRequirementId,OtherAllergyInfo,AdditionalInfo,ReferrerId,CreatedDate")] Recipient recipient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipient);
                //var name = new SqlParameter("@RName", recipient.FirstName);
                //_context.Database.ExecuteSqlCommand("sp_Add_Recipient @RName", name);
                await _context.SaveChangesAsync();  //使用 Procedure 不用save
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name", recipient.BranchId);
            ViewData["DietaryRequirementId"] = new SelectList(_context.DietaryRequirement, "DietaryRequirementId", "DietaryName", recipient.DietaryRequirementId);
            ViewData["ReferralReasonId"] = new SelectList(_context.ReferralReason, "ReferralReasonId", "Content", recipient.ReferralReasonId);
            ViewData["ReferrerId"] = new SelectList(_context.Referrer, "ReferrerId", "FirstName", recipient.ReferrerId);
            return View(recipient);
        }

        // GET: Recipients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _context.Recipient.FindAsync(id);
            if (recipient == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name", recipient.BranchId);
            ViewData["DietaryRequirementId"] = new SelectList(_context.DietaryRequirement, "DietaryRequirementId", "DietaryName", recipient.DietaryRequirementId);
            ViewData["ReferralReasonId"] = new SelectList(_context.ReferralReason, "ReferralReasonId", "Content", recipient.ReferralReasonId);
            ViewData["ReferrerId"] = new SelectList(_context.Referrer, "ReferrerId", "FirstName", recipient.ReferrerId);
            return View(recipient);
        }

        // POST: Recipients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipientId,FirstName,LastName,AddressNumStreet,TownCity,Postcode,PhoneNumber,Email,DogOnProperty,BranchId,ReferralReasonId,OtherReferralInfo,AdultsNum,Under5ChildrenNum,_510ChildrenNum,_1117ChildrenNum,DietaryRequirementId,OtherAllergyInfo,AdditionalInfo,ReferrerId,CreatedDate")] Recipient recipient)
        {
            if (id != recipient.RecipientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipientExists(recipient.RecipientId))
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
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name", recipient.BranchId);
            ViewData["DietaryRequirementId"] = new SelectList(_context.DietaryRequirement, "DietaryRequirementId", "DietaryName", recipient.DietaryRequirementId);
            ViewData["ReferralReasonId"] = new SelectList(_context.ReferralReason, "ReferralReasonId", "Content", recipient.ReferralReasonId);
            ViewData["ReferrerId"] = new SelectList(_context.Referrer, "ReferrerId", "FirstName", recipient.ReferrerId);
            return View(recipient);
        }

        // GET: Recipients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _context.Recipient
                .Include(r => r.Branch)
                .Include(r => r.DietaryRequirement)
                .Include(r => r.ReferralReason)
                .Include(r => r.Referrer)
                .FirstOrDefaultAsync(m => m.RecipientId == id);
            if (recipient == null)
            {
                return NotFound();
            }

            return View(recipient);
        }

        // POST: Recipients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipient = await _context.Recipient.FindAsync(id);
            _context.Recipient.Remove(recipient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipientExists(int id)
        {
            return _context.Recipient.Any(e => e.RecipientId == id);
        }
    }
}
