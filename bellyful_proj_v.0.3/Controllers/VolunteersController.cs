using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bellyful_proj_v._0._3.Models;
using Microsoft.AspNetCore.Authorization;

namespace bellyful_proj_v._0._3.Controllers
{
    public class VolunteersController : Controller
    {
        private readonly bellyful_v03Context _context;

        public VolunteersController(bellyful_v03Context context)
        {
            _context = context;
        }

        // GET: Volunteers
        public async Task<IActionResult> Index()
        {
            var bellyful_v03Context = _context.Volunteer.Include(v => v.Branch).Include(v => v.Role).Include(v => v.Status);
            return View(await bellyful_v03Context.ToListAsync());
        }

        // GET: Volunteers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteer
                .Include(v => v.Branch)
                .Include(v => v.Role)
                .Include(v => v.Status)
                .FirstOrDefaultAsync(m => m.VolunteerId == id);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // GET: Volunteers/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name");
            ViewData["RoleId"] = new SelectList(_context.VolunteerRole, "RoleId", "RoleName");
            ViewData["StatusId"] = new SelectList(_context.VolunteerStatus, "StatusId", "Content");
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VolunteerId,FirstName,LastName,Dob,Email,PreferredPhone,AlternativePhone,Address,TownCity,PostCode,StatusId,BranchId,RoleId")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name", volunteer.BranchId);
            ViewData["RoleId"] = new SelectList(_context.VolunteerRole, "RoleId", "RoleName", volunteer.RoleId);
            ViewData["StatusId"] = new SelectList(_context.VolunteerStatus, "StatusId", "Content", volunteer.StatusId);
            return View(volunteer);
        }

        // GET: Volunteers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteer.FindAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name", volunteer.BranchId);
            ViewData["RoleId"] = new SelectList(_context.VolunteerRole, "RoleId", "RoleName", volunteer.RoleId);
            ViewData["StatusId"] = new SelectList(_context.VolunteerStatus, "StatusId", "Content", volunteer.StatusId);
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerId,FirstName,LastName,Dob,Email,PreferredPhone,AlternativePhone,Address,TownCity,PostCode,StatusId,BranchId,RoleId")] Volunteer volunteer)
        {
            if (id != volunteer.VolunteerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerExists(volunteer.VolunteerId))
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
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name", volunteer.BranchId);
            ViewData["RoleId"] = new SelectList(_context.VolunteerRole, "RoleId", "RoleName", volunteer.RoleId);
            ViewData["StatusId"] = new SelectList(_context.VolunteerStatus, "StatusId", "Content", volunteer.StatusId);
            return View(volunteer);
        }





        //=============================VolunteerEdit

        // GET: Volunteers/Edit/5  //抵御通过URL 编辑其他志愿者,验证当前App Account 的Email 和志愿者的Email 是否一致，从而判定是否是本人，实现仅编辑本人数据
        [Authorize(Roles = "L4_DeliverMan, L5_GeneralStaff,Guest")]  //level 4 ,5 and register User access only
        public async Task<IActionResult> VolunteerEdit(int? volunteerId , string userEmail)
        {
            if (volunteerId == null)
            {
                return NotFound();
            }
            var volunteer = await _context.Volunteer.FindAsync(volunteerId);
            if (volunteer !=null)
            {
                if (volunteer.Email != userEmail)
                {
                    return NotFound();// 无法编辑别的志愿者
                }
                else//打包
                {
                    ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name", volunteer.BranchId);
                    //ViewData["RoleId"] = new SelectList(_context.VolunteerRole, "RoleId", "RoleName", volunteer.RoleId);
                    //ViewData["StatusId"] = new SelectList(_context.VolunteerStatus, "StatusId", "Content", volunteer.StatusId);
                    //model 转成 viewmodel  再传view

                  //  volunteer.
                    return View(volunteer);
                }
            }
            //
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VolunteerEdit(int id, [Bind("VolunteerId,FirstName,LastName,Dob,Email,PreferredPhone,AlternativePhone,Address,TownCity,PostCode,StatusId,BranchId,RoleId")] Volunteer volunteer)
        {
            if (id != volunteer.VolunteerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerExists(volunteer.VolunteerId))
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
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name", volunteer.BranchId);
           // ViewData["RoleId"] = new SelectList(_context.VolunteerRole, "RoleId", "RoleName", volunteer.RoleId);
         //   ViewData["StatusId"] = new SelectList(_context.VolunteerStatus, "StatusId", "Content", volunteer.StatusId);
            return View(volunteer);
        }



        //=============================VolunteerEdit

            
        // GET: Volunteers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteer
                .Include(v => v.Branch)
                .Include(v => v.Role)
                .Include(v => v.Status)
                .FirstOrDefaultAsync(m => m.VolunteerId == id);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteer = await _context.Volunteer.FindAsync(id);
            _context.Volunteer.Remove(volunteer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerExists(int id)
        {
            return _context.Volunteer.Any(e => e.VolunteerId == id);
        }
    }
}
