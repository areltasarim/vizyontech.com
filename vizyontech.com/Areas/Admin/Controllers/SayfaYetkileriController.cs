using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using EticaretWebCoreViewModel;
using vizyontech.com.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]
    public class SayfaYetkileri : Controller
    {
        private readonly AppDbContext _context;


        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager = null;

        private readonly string entityBaslik = "Sayfa Yetkileri";
        private readonly string entityAltBaslik = "Yetki Ekle";

        public SayfaYetkileri(AppDbContext _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;
        }

        public async Task<ActionResult> RolYetkileri(string roleId)
        {

            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = new SayfaYetkiViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            var allProductsPermissions = new List<RoleClaimsViewModel>();
            var allSayfalarPermissions = new List<RoleClaimsViewModel>();

            allProductsPermissions.GetRolPermissions(typeof(Permissions.Uyeler), roleId);
            allSayfalarPermissions.GetRolPermissions(typeof(Permissions.Uyeler.Uyeler_Roller), roleId);
            allSayfalarPermissions.GetRolPermissions(typeof(Permissions.Uyeler.Uyeler_Uyeler), roleId);

            allProductsPermissions.GetRolPermissions(typeof(Permissions.Sayfalar), roleId);
            allSayfalarPermissions.GetRolPermissions(typeof(Permissions.Sayfalar.Sayfalar_Roller), roleId);
            allSayfalarPermissions.GetRolPermissions(typeof(Permissions.Sayfalar.Sayfalar_Sayfalar), roleId);

            allPermissions.AddRange(allProductsPermissions);
            allPermissions.AddRange(allSayfalarPermissions);
            var role = await _roleManager.FindByIdAsync(roleId);
            model.RoleId = roleId;
            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();

            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
            return View(model);
        }

        public async Task<IActionResult> RolYetkiGuncelle(SayfaYetkiViewModel model, string submit)
        {

            var role = await _roleManager.FindByIdAsync(model.RoleId);
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddRolPermissionClaim(role, claim.Value);
            }

            try
            {

                #region Sayfa Butonlari
                string action = "";
                string controller = "";
                string PageId = "";
                if (submit == "Kaydet")
                {
                    action = "Roller";
                    controller = "Account";
                }
                if (submit == "KaydetGuncelle")
                {
                    action = "RolYetkileri";
                    controller = "SayfaYetkileri";
                    PageId = model.RoleId;
                }
                #endregion

                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Yetki Başarıyla Verildi" });

                return RedirectToAction(action, controller, new { roleId = PageId });
            }
            catch
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "danger", Text = "Hata Oluştu" });

                return RedirectToAction("RolYetkileri", "SayfaYetkileri", new { roleId = model.RoleId });
            }

        }


        public async Task<ActionResult> UyeYetkileri(string uyeId)
        {

            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = new SayfaYetkiViewModel();
            var allPermissions = new List<UserClaimsViewModel>();
            var allProductsPermissions = new List<UserClaimsViewModel>();
            var allSayfalarPermissions = new List<UserClaimsViewModel>();

            allProductsPermissions.GetUserPermissions(typeof(Permissions.Uyeler), uyeId);
            allSayfalarPermissions.GetUserPermissions(typeof(Permissions.Uyeler.Uyeler_Roller), uyeId);
            allSayfalarPermissions.GetUserPermissions(typeof(Permissions.Uyeler.Uyeler_Uyeler), uyeId);

            allProductsPermissions.GetUserPermissions(typeof(Permissions.Sayfalar), uyeId);
            allSayfalarPermissions.GetUserPermissions(typeof(Permissions.Sayfalar.Sayfalar_Roller), uyeId);
            allSayfalarPermissions.GetUserPermissions(typeof(Permissions.Sayfalar.Sayfalar_Sayfalar), uyeId);

            allPermissions.AddRange(allProductsPermissions);
            allPermissions.AddRange(allSayfalarPermissions);
            var uye = await _userManager.FindByIdAsync(uyeId);
            model.UserId = uyeId;
            var claims = await _userManager.GetClaimsAsync(uye);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var userClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(userClaimValues).ToList();

            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.UserClaims = allPermissions;
            return View(model);
        }

        public async Task<IActionResult> UyeYetkiGuncelle(SayfaYetkiViewModel model, string submit)
        {

            var user = await _userManager.FindByIdAsync(model.UserId);
            var claims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in claims)
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }
            var selectedClaims = model.UserClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _userManager.AddUserPermissionClaim(user, claim.Value);
            }

            try
            {

                #region Sayfa Butonlari
                string action = "";
                string controller = "";
                string PageId = "";
                if (submit == "Kaydet")
                {
                    action = "Roller";
                    controller = "Account";
                }
                if (submit == "KaydetGuncelle")
                {
                    action = "UyeYetkileri";
                    controller = "SayfaYetkileri";
                    PageId = model.UserId;
                }
                #endregion

                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Yetki Başarıyla Verildi" });

                return RedirectToAction(action, controller, new { uyeId = PageId });
            }
            catch
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "danger", Text = "Hata Oluştu" });

                return RedirectToAction("UyeYetkileri", "SayfaYetkileri", new { uyeId = model.UserId });
            }

        }
    }
}
