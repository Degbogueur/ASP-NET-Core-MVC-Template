using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ASP_NET_Core_MVC_Template.Helpers.Constants.Enumerations;

namespace ASP_NET_Core_MVC_Template.Helpers.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsList(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }

        public static List<string> GenerateAllPermissions()
        {
            var allPermissions = new List<string>();

            var modules = Enum.GetValues(typeof(Modules));

            foreach (var module in modules)
            {
                allPermissions.AddRange(GeneratePermissionsList(module.ToString()));
            }

            return allPermissions;
        }
    }
}
