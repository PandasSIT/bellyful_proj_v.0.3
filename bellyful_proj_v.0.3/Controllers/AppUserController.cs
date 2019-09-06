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
    [Authorize]
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
                    //Role = _roleManager.FindByIdAsync(item.AppRoleId.ToString()).Result.Name,
                    //
                };
                if (user.VolunteerId != null)
                {
                    var v = _context.Volunteer.SingleOrDefault(i => i.VolunteerId == user.VolunteerId);
                    if (v != null)
                    {
                        vm.VIdName = v.VolunteerId.ToString() + ". " + v.FirstName;
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
                ViewData["Volunteers"] = new SelectList(GetVolunteerForSelection(), "VId", "IdFullName");
                ViewData["AppUserRoles"] = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
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
                
            }
        }











        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
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


        public List<VolunteerForSelection> GetVolunteerForSelection()
        {
            return _context.Volunteer.Select(volunteer => new VolunteerForSelection
            {
                VId = volunteer.VolunteerId,
                IdFullName = volunteer.VolunteerId +
                              ". " + volunteer.FirstName + "   " + volunteer.LastName
            }).OrderBy(c => c.VId).ToList();
        }



        public IActionResult CreateAppUser()
        {
            ViewData["Volunteers"] = new SelectList(GetVolunteerForSelection(), "VId", "IdFullName");
            ViewData["AppUserRoles"] = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppUser(AppUserCreateViewModel appUserCreateViewModel)
        {
            async Task<IActionResult> ReturnToCreateGet()
            {
                var vsList = await _context.Volunteer.Select(volunteer => new VolunteerForSelection
                {
                    VId = volunteer.VolunteerId,
                    IdFullName = volunteer.VolunteerId +
                                 ". " + volunteer.FirstName + "   " + volunteer.LastName
                }).OrderBy(c => c.VId).ToListAsync();
                ViewData["Volunteers"] = new SelectList(vsList, "VId", "IdFullName");
                ViewData["AppUserRoles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", appUserCreateViewModel.AppUserRoleId);
                return View(appUserCreateViewModel);
            }

            if (ModelState.IsValid)//If not valid, return that model
            {
                var user = new ApplicationUser
                {
                    VolunteerId = appUserCreateViewModel.VolunteerId,
                    AppRoleId = appUserCreateViewModel.AppUserRoleId,
                    UserName = appUserCreateViewModel.Email,
                    Email = appUserCreateViewModel.Email
                };
                var role = await _roleManager.FindByIdAsync(appUserCreateViewModel.AppUserRoleId.ToString());
                var resultPassword = await _userManager.CreateAsync(user, appUserCreateViewModel.Password);
                var resultAddToRole = await _userManager.AddToRoleAsync(user, role.Name);//Add AppUser to role table
                if (resultPassword.Succeeded && resultAddToRole.Succeeded)
                {//if succeeded, return to AppUser index page with all users


                    return RedirectToAction("Index");
                }
                // otherwise load all errors 
                foreach (var error in resultPassword.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                foreach (var error in resultAddToRole.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return await ReturnToCreateGet();
            }

            return await ReturnToCreateGet();
        }
    }
}