using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bellyful_proj_v._0._3.Models;
using bellyful_proj_v._0._3.ViewModels.VolunteerVM;
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
        [Authorize(Roles = "L1_Admin, L2_Manager")]
        public async Task<IActionResult> Index()
        {
            var bellyful_v03Context = _context.Volunteer.Include(v => v.Branch).Include(v => v.Role).Include(v => v.Status);
            return View(await bellyful_v03Context.ToListAsync());
        }
        [Authorize(Roles = "L1_Admin, L2_Manager")]
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
        public async Task<IActionResult> Create([Bind("VolunteerId,FirstName,LastName,Dob,Email,PreferredPhone,AlternativePhone,Address,TownCity,PostCode,StatusId,BranchId,RoleId,IsAssignedUserAccount")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                volunteer.IsAssignedUserAccount = false;
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
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerId,FirstName,LastName,Dob,Email,PreferredPhone,AlternativePhone,Address,TownCity,PostCode,StatusId,BranchId,RoleId,IsAssignedUserAccount")] Volunteer volunteer)
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
        public async Task<IActionResult> VolunteerEdit(int? volunteerId, string userEmail)
        {
            if (volunteerId == null)
            {
                return NotFound();
            }
            var volunteer = await _context.Volunteer.FindAsync(volunteerId);
            var volunteerEC = await _context.VolunteerEmergencyContact.FindAsync(volunteerId);
            if (volunteer != null)
            {
                if (volunteer.Email != userEmail)
                {
                    ModelState.AddModelError(string.Empty, "Volunteer Info Email 与 AppUser Name 不一致");
                    //return RedirectToRoute(new {area = "Identity",controller="" pages = "/Account/Manage/Index"});
                    return Redirect("/Identity/Account/Manage");
                    // return NotFound();// 无法编辑别的志愿者
                }
                else//打包
                {
                    ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name", volunteer.BranchId);
                    ViewData["RoleId"] = new SelectList(_context.VolunteerRole, "RoleId", "RoleName", volunteer.RoleId);
                    ViewData["StatusId"] = new SelectList(_context.VolunteerStatus, "StatusId", "Content", volunteer.StatusId);

                    var vEvm = new VolunteerEditViewModel
                    {
                        VolunteerId = volunteer.VolunteerId,
                        FirstName = volunteer.FirstName,
                        LastName = volunteer.LastName,
                        Dob = volunteer.Dob,
                        Email = volunteer.Email,
                        PreferredPhone = volunteer.PreferredPhone,
                        AlternativePhone = volunteer.AlternativePhone,
                        Address = volunteer.Address,
                        TownCity = volunteer.TownCity,
                        PostCode = volunteer.PostCode,
                        StatusId = volunteer.StatusId,
                        BranchId = volunteer.BranchId,
                        RoleId = volunteer.RoleId,
                        IsAssignedUserAccount = volunteer.IsAssignedUserAccount
                    };
                    if (volunteerEC != null)
                    {
                        vEvm.EFirstName = volunteerEC.FirstName;
                        vEvm.ELastName = volunteerEC.LastName;
                        vEvm.EPhoneNumber = volunteerEC.PhoneNumber;
                        vEvm.ERelationship = volunteerEC.Relationship;
                        
                    }
                    //  volunteer.
                   
                    return View(vEvm);
                }
            }
            //
            return RedirectToAction("Index");

        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VolunteerEdit(int volunteerId, VolunteerEditViewModel vVM)
        {
            if (volunteerId != vVM.VolunteerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //vVM 转为  volunteer 和 EVolunteerContact
                    //如果 EVolunteerContact 查找不存在，创建
                    // _context.Update(vVM);
                    var vol = await _context.Volunteer.FindAsync(vVM.VolunteerId);
                    if (vol != null)
                    {
                        vol.VolunteerId = vVM.VolunteerId;
                        vol.FirstName = vVM.FirstName;
                        vol.LastName = vVM.LastName;
                        vol.Dob = vVM.Dob;
                        vol.Email = vVM.Email;
                        vol.PreferredPhone = vVM.PreferredPhone;
                        vol.AlternativePhone = vVM.AlternativePhone;
                        vol.Address = vVM.Address;
                        vol.TownCity = vVM.TownCity;
                        vol.PostCode = vVM.PostCode;
                        vol.StatusId = vVM.StatusId;
                        vol.BranchId = vVM.BranchId;
                        vol.RoleId = vVM.RoleId;
                        vol.IsAssignedUserAccount = vVM.IsAssignedUserAccount;
                    }
                    var evol = await _context.VolunteerEmergencyContact.FindAsync(vVM.VolunteerId);
                    if (evol != null)
                    {
                        evol.FirstName = vVM.EFirstName;
                        evol.LastName = vVM.ELastName;
                        evol.PhoneNumber = vVM.EPhoneNumber;
                        evol.Relationship = vVM.ERelationship;
                    }
                    else
                    {
                        _context.Add(new VolunteerEmergencyContact
                        {
                            VolunteerId = vVM.VolunteerId,
                            FirstName = vVM.EFirstName,
                            LastName = vVM.ELastName,
                            PhoneNumber = vVM.EPhoneNumber,
                            Relationship = vVM.ERelationship
                        });
                    }
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerExists(vVM.VolunteerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                vVM.StatusMessage = "Your Volunteer Info has been saved !";
                ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name", vVM.BranchId);
                ViewData["RoleId"] = new SelectList(_context.VolunteerRole, "RoleId", "RoleName", vVM.RoleId);
                ViewData["StatusId"] = new SelectList(_context.VolunteerStatus, "StatusId", "Content", vVM.StatusId);
                return View(vVM);
            }
           
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "Name", vVM.BranchId);
             ViewData["RoleId"] = new SelectList(_context.VolunteerRole, "RoleId", "RoleName", vVM.RoleId);
               ViewData["StatusId"] = new SelectList(_context.VolunteerStatus, "StatusId", "Content", vVM.StatusId);
            vVM.StatusMessage = "Oops, Something went wrong ! ";
            return View(vVM);

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
