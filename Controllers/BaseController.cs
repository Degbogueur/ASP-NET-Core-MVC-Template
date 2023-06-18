using ASP_NET_Core_MVC_Template.Data;
using ASP_NET_Core_MVC_Template.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_NET_Core_MVC_Template.Controllers
{
    public class BaseController : Controller
    {
        private ApplicationDbContext Context;
        private UserManager<ApplicationUser> UserManager;
        private SignInManager<ApplicationUser> SignInManager;
        private RoleManager<IdentityRole> RoleManager;

        protected ApplicationDbContext _context => Context ??= HttpContext.RequestServices.GetService<ApplicationDbContext>();
        protected UserManager<ApplicationUser> _userManager => UserManager ??= HttpContext.RequestServices.GetService<UserManager<ApplicationUser>>();
        protected SignInManager<ApplicationUser> _signInManager => SignInManager ??= HttpContext.RequestServices.GetService<SignInManager<ApplicationUser>>();
        protected RoleManager<IdentityRole> _roleManager => RoleManager ??= HttpContext.RequestServices.GetService<RoleManager<IdentityRole>>();

        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return user;
        }
    }
}
