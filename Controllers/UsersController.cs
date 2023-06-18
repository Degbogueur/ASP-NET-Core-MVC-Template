using ASP_NET_Core_MVC_Template.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ASP_NET_Core_MVC_Template.Helpers.Constants.Enumerations;

namespace ASP_NET_Core_MVC_Template.Controllers
{
    public class UsersController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var currentUser = await GetCurrentUserAsync();

            var users = _userManager.Users
                .Where(a => a.Id != currentUser.Id && a.UserName != "obed")
                .OrderBy(u => u.UserName)
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    Roles = _userManager.GetRolesAsync(user).Result
                }).ToList();

            return View(users);
        }

        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            var roles = _roleManager.Roles.Where(r => r.Name != Roles.SuperAdmin.ToString()).ToList();

            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserFullName = user.FullName,
                Roles = roles.Select(role => new CheckBoxViewModel
                {
                    DisplayValue = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.DisplayValue));

            return RedirectToAction(nameof(Index));
        }
    }
}
