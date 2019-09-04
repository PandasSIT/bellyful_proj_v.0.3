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
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }


        [ValidateAntiForgeryToken,HttpPost]
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

            return View("Index", await _userManager.Users.ToListAsync());
        }

        public IActionResult CreateAppUser()
        {
            var vsList = _context.Volunteer.Select(
                volunteer => 
                    new VolunteerForSelection {
                        VId = volunteer.VolunteerId,
                        IdFullName = volunteer.VolunteerId.ToString()+". " +volunteer.FirstName + "   " + volunteer.LastName }
                ).OrderBy(c=>c.VId);

            var rList = _roleManager.Roles.ToList();

            ViewData["Volunteers"] = new SelectList(vsList, "VId", "IdFullName");
            ViewData["AppUserRoles"] = new SelectList(rList, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAppUser(CreateAppUserViewModel createAppUserViewModel)
        {
            if (!ModelState.IsValid)//If not valid, return that model
            {
                return View(createAppUserViewModel);
            }
            //otherwise create new IdentityUser
            var user = new ApplicationUser
            {
                VolunteerId = createAppUserViewModel.VolunteerId,
                UserName = createAppUserViewModel.Email,
                Email = createAppUserViewModel.Email
            };
            var result = await _userManager.CreateAsync(user, createAppUserViewModel.Password);

            if (result.Succeeded)
            {//if succeeded, return to AppUser index page with all users
                return RedirectToAction("Index");
            }
            // otherwise load all errors 
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(createAppUserViewModel);

        }
    }
}