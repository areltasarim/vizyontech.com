using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vizyontech.com.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<AppUser> _userManager { get; set; }
        protected SignInManager<AppUser> _signInManager { get; set; }
        protected RoleManager<AppRole> _roleManager { get; set; }
        protected AppUser CurrentUser => _userManager.FindByNameAsync(User.Identity.Name).Result;
        protected BaseController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager = null)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }

        public void AddModelError(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }
    }
}
