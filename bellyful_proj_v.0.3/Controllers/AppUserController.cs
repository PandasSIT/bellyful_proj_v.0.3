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

        //async Task<IEnumerable<AppUserIndexViewMode>> GetAppUserIndexViewModel()
        //{
        //    return appusers;
        //}

        public async Task<IActionResult> Index()
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
                    var ss = await _context.GetVolunteerForIndex(user.VolunteerId.Value);

                    vm.VIdName = ss;
                }
                if (user.AppRoleId != null)
                {
                    vm.Role = _roleManager.FindByIdAsync(user.AppRoleId.ToString()).Result.Name;
                }
                appusers.Add(vm);
            }
            return View( appusers);
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
                    ViewData["Volunteers"] = new SelectList(await _context.GetVolunteersForSelection(user.VolunteerId), "VId", "IdFullName");
                    ViewData["AppUserRoles"] = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
                }
                else
                {
                    ViewData["Volunteers"] = new SelectList(await _context.GetVolunteersForSelection(-1), "VId", "IdFullName");
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
            if (ModelState.IsValid) //如果验证通过
            {    //通过接收的AppUserVM的邮件找到AppUser
                var user = await _userManager.FindByEmailAsync(appUserCreateViewModel.Email);

                //如果找到User
                if (user != null)
                {
                    //============更新AppUser前  先判定志愿者的，并且取到，变更前和变更后的  VId=============
                    //如果传进来的AppUserVM的VId不为空：即用户设定了新的Volunteer
                    if (appUserCreateViewModel.VolunteerId != null)
                    {
                        //通过AppUserVM的VId，找到志愿者， 赋值给 currentVolunteer
                        var currentVolunteer = await _context.Volunteer.FindAsync(appUserCreateViewModel.VolunteerId);
                        //通过AppUserVM的VId，如果找到志愿者
                        if (currentVolunteer != null)
                        {   //看是否有前任：如果AppUser之前的Vid（该AppUser的前任Volunteer） 不为空，
                            if (user.VolunteerId != null)
                            {
                                //找到 前任，还原为 未分配。
                                var previousVolunteer = await _context.Volunteer.FindAsync(user.VolunteerId);
                                if (previousVolunteer != null && previousVolunteer.IsAssignedUserAccount == true)
                                {
                                    previousVolunteer.IsAssignedUserAccount = false;
                                }
                            }
                            //新任志愿者  标记为 已分配
                            currentVolunteer.IsAssignedUserAccount = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                    else//如果传进来的AppUser 的Vid 为空： 即用户不打算分配任何志愿者
                    {
                        //看是否有前任：如果AppUser之前的Vid（该AppUser的前任Volunteer） 不为空，
                        if (user.VolunteerId != null)
                        {
                            //找到 前任，还原为 未分配。
                            var previousVolunteer = await _context.Volunteer.FindAsync(user.VolunteerId);
                            if (previousVolunteer != null && previousVolunteer.IsAssignedUserAccount == true)
                            {
                                previousVolunteer.IsAssignedUserAccount = false;
                            }
                        }
                    }
                    //============更新AppUser前  先判定志愿者的选择状态，并且取到，变更前和变更后的  VId=============



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
            }//如果验证没通过 ， 直接返回
            ViewData["Volunteers"] = new SelectList(await _context.GetVolunteersForSelection(-1), "VId", "IdFullName",appUserCreateViewModel.VolunteerId);
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
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateAppUser()
        {
            ViewData["Volunteers"] = new SelectList(await _context.GetVolunteersForSelection(-1), "VId", "IdFullName");
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

                    //通过AppUserVM的VId，找到志愿者， 赋值给 currentVolunteer
                    var Volunteer = await _context.Volunteer.FindAsync(appUserCreateViewModel.VolunteerId);
                    //通过AppUserVM的VId，如果找到志愿者
                    if (Volunteer != null)
                    {
                        //新任志愿者  标记为 已分配
                        Volunteer.IsAssignedUserAccount = true;
                        await _context.SaveChangesAsync();
                    }


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

                ViewData["Volunteers"] = new SelectList(await _context.GetVolunteersForSelection(-1), "VId", "IdFullName");
                ViewData["AppUserRoles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name");
                return View(appUserCreateViewModel);
            }
        }
    }
}