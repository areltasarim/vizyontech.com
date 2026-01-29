using EticaretWebCoreEntity;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EticaretWebCoreHelper
{
    public static class ClaimsHelper
    {
        public static void GetRolPermissions(this List<RoleClaimsViewModel> allPermissions, Type policy, string roleId)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo fi in fields)
            {
                allPermissions.Add(new RoleClaimsViewModel { Value = fi.GetValue(null).ToString(), Type = "Permissions" });
            }
        }

        public static async Task AddRolPermissionClaim(this RoleManager<AppRole> roleManager, AppRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }



        public static void GetUserPermissions(this List<UserClaimsViewModel> allPermissions, Type policy, string userId)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo fi in fields)
            {
                allPermissions.Add(new UserClaimsViewModel { Value = fi.GetValue(null).ToString(), Type = "Permissions" });
            }
        }



        public static async Task AddUserPermissionClaim(this UserManager<AppUser> userManager, AppUser user, string permission)
        {
            var allClaims = await userManager.GetClaimsAsync(user);
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await userManager.AddClaimAsync(user, new Claim("Permission", permission));
            }
        }
    }
}
