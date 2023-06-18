using ASP_NET_Core_MVC_Template.Helpers.Constants;
using ASP_NET_Core_MVC_Template.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ASP_NET_Core_MVC_Template.Helpers.Constants.Enumerations;

namespace ASP_NET_Core_MVC_Template.Data.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager)
        {
            var basicUser = new ApplicationUser
            {
                UserName = "basic",
                LastName = "Basic",
                FirstName = "User",
                Email = "basicuser@domain.com",
                EmailConfirmed = true,
                IsActive = true
            };

            var user = await userManager.FindByNameAsync(basicUser.UserName)
                ?? await userManager.FindByEmailAsync(basicUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(basicUser, "P@ssword123");
                await userManager.AddToRoleAsync(basicUser, Roles.Basic.ToString());
            }
        }

        public static async Task SeedSuperAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var saUser = new ApplicationUser
            {
                UserName = "obed",
                LastName = "Degbo",
                FirstName = "Komi Obed",
                Email = "degbo80@gmail.com",
                EmailConfirmed = true,
                IsActive = true
            };

            var user = await userManager.FindByNameAsync(saUser.UserName)
                ?? await userManager.FindByEmailAsync(saUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(saUser, "P@ssword123");
                await userManager.AddToRoleAsync(saUser, Roles.SuperAdmin.ToString());
            }

            await roleManager.SeedClaimsForSuperAdminUser();
        }

        private static async Task SeedClaimsForSuperAdminUser(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
            await roleManager.AddPermissionClaims(adminRole, "Products");
        }

        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsList(module);

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}
