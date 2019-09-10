using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using bellyful_proj_v._0._3.Models;
using bellyful_proj_v._0._3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace bellyful_proj_v._0._3.Controllers
{
    [Authorize(Roles = "L1_Admin, L2_Manager")]
    public class AppUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly bellyful_v03Context _context;

        public AppUserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            bellyful_v03Context context
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        async Task<IEnumerable<AppUserIndexViewMode>> GetAppUserIndexViewModel()
        {
            var appusers = new List<AppUserIndexViewMode>();

            foreach (var user in _userManager.Users)
            {
                var vm = new AppUserIndexViewMode
                {
                    Id = user.Id,
                    Email = user.Email,
                    Password = user.PasswordHash
                };
                if (user.VolunteerId != null)
                {
                    var v = _context.Volunteer.SingleOrDefault(i => i.VolunteerId == user.VolunteerId);
                    if (v != null)
                    {
                        if (v.IsAssignedUserAccount != null)
                        {
                            if (v.IsAssignedUserAccount.Value)
                            {
                                vm.VIdName = v.VolunteerId.ToString() + ". " + v.FirstName;
                            }
                        }
                    }
                }
                if (user.AppRoleId != null)
                {
                    vm.Role = _roleManager.FindByIdAsync(user.AppRoleId.ToString()).Result.Name;
                }
                appusers.Add(vm);
            }
            return appusers;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetAppUserIndexViewModel());
        }


        public async Task<IActionResult> EditUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (user.VolunteerId != null)
                {
                    ViewData["Volunteers"] = new SelectList(await _context.GetVolunteerForSelection(user.VolunteerId), "VId", "IdFullName");
                    ViewData["AppUserRoles"] = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
                }
                else
                {
                    ViewData["Volunteers"] = new SelectList(await _context.GetVolunteerForSelection(-1), "VId", "IdFullName");
                    ViewData["AppUserRoles"] = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
                }


                return View(new AppUserCreateViewModel
                {
                    Email = user.Email,
                    VolunteerId = user.VolunteerId,
                    AppUserRoleId = user.AppRoleId
                });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(AppUserCreateViewModel appUserCreateViewModel)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(appUserCreateViewModel.Email);
                if (appUserCreateViewModel.VolunteerId != null)
                {
                    var currentVolunteer = await _context.Volunteer.FindAsync(appUserCreateViewModel.VolunteerId);
                    if (currentVolunteer != null)
                    {
                        if (user.VolunteerId != null)
                        {
                            var previousVolunteer = await _context.Volunteer.FindAsync(user.VolunteerId);
                            if (previousVolunteer != null && previousVolunteer.IsAssignedUserAccount == true)
                            {
                                previousVolunteer.IsAssignedUserAccount = false;
                            }
                        }
                        currentVolunteer.IsAssignedUserAccount = true;
                        await _context.SaveChangesAsync();
                    }
                }

                if (user != null)
                {
                    user.AppRoleId = appUserCreateViewModel.AppUserRoleId;
                    user.VolunteerId = appUserCreateViewModel.VolunteerId;

                    try
                    {
                        await _userManager.UpdateAsync(user);
                        await _context.SaveChangesAsync();

                        var role = await _roleManager.FindByIdAsync(appUserCreateViewModel.AppUserRoleId.ToString());
                        if (role != null)
                        {
                            var resultAddToRole = await _userManager.AddToRoleAsync(user, role.Name);//Add AppUser to role table
                            if (resultAddToRole.Succeeded)
                            {
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }

                    return RedirectToAction("index");
                }
            }
            ViewData["Volunteers"] = new SelectList(await _context.GetVolunteerForSelection(-1), "VId", "IdFullName");
            ViewData["AppUserRoles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", appUserCreateViewModel.AppUserRoleId);
            return View(appUserCreateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (user.VolunteerId != null)
                {
                    var Volunteer = await _context.Volunteer.FindAsync(user.VolunteerId);
                    if (Volunteer != null)
                    {
                        Volunteer.IsAssignedUserAccount = false;
                        await _context.SaveChangesAsync();
                    }
                }


                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Something wrong with Deleting User！");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User dose not exist！");
            }

            return View("Index", await GetAppUserIndexViewModel());
        }
        
        public async Task<IActionResult> CreateAppUser()
        {
            ViewData["Volunteers"] = new SelectList(await _context.GetVolunteerForSelection(-1), "VId", "IdFullName");
            ViewData["AppUserRoles"] = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppUser(AppUserCreateViewModel appUserCreateViewModel)
        {
            if (ModelState.IsValid)//If not valid, return that model
            {
                var user = new ApplicationUser();

                user.VolunteerId = appUserCreateViewModel.VolunteerId;
                user.AppRoleId = appUserCreateViewModel.AppUserRoleId;
                user.UserName = appUserCreateViewModel.Email;
                user.Email = appUserCreateViewModel.Email;
                var resultPassword = await _userManager.CreateAsync(user, appUserCreateViewModel.Password);
                if (resultPassword.Succeeded)
                {
                    if (appUserCreateViewModel.AppUserRoleId != null)
                    {
                        var role = await _roleManager.FindByIdAsync(appUserCreateViewModel.AppUserRoleId.ToString());
                        var resultAddToRole = await _userManager.AddToRoleAsync(user, role.Name);//Add AppUser to role table
                        if (resultAddToRole.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }

                        foreach (var error in resultAddToRole.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return await ReturnToCreateGet();

                    }
                    return RedirectToAction("Index");
                }

                foreach (var error in resultPassword.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }


                // otherwise load all errors 


                return await ReturnToCreateGet();
            }

            return await ReturnToCreateGet();

            async Task<IActionResult> ReturnToCreateGet()
            {

                ViewData["Volunteers"] = new SelectList(await _context.GetVolunteerForSelection(-1), "VId", "IdFullName");
                ViewData["AppUserRoles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name");
                return View(appUserCreateViewModel);
            }
        }
    }
}