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
using Microsoft.EntityFrameworkCore;

namespace bellyful_proj_v._0._3.Controllers
{
    [Authorize]
    public class AppUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AppUserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(AddAppUserViewModel addAppUserViewModel)
        {
            if (!ModelState.IsValid)//If not valid, return that model
            {
                return View(addAppUserViewModel);
            }
            //otherwise create new IdentityUser
            var user = new ApplicationUser
            {
                UserName = addAppUserViewModel.UserName,
                Email = addAppUserViewModel.Email
            };
            var result = await _userManager.CreateAsync(user, addAppUserViewModel.Password);

            if (result.Succeeded)
            {//if succeeded, return to AppUser index page with all users
                return View("Index", await _userManager.Users.ToListAsync());
            }
            // otherwise load all errors 
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(addAppUserViewModel);

        }
    }
}