using vizyontech.com.Controllers;
using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EticaretWebCoreEntity.Enums;
using System.Security.Claims;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using System.Web;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Linq.Expressions;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Office2010.Excel;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using EticaretWebCoreService.OpakOdeme;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly OpakServis _opakServis;
        private readonly IWebHostEnvironment _env;

        private readonly IHttpContextAccessor _httpContextAccessor;


        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager = null;

        [Obsolete]
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        [Obsolete]
        public AccountController(AppDbContext _context, IHttpContextAccessor _httpContextAccessor, IHostingEnvironment _hostingEnvironment, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager, OpakDbContext opakDbContext, OpakServis opakServis, IWebHostEnvironment env)
        {
            this._context = _context;
            this._httpContextAccessor = _httpContextAccessor;
            this._hostingEnvironment = _hostingEnvironment;

            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;
            _opakServis = opakServis;
            _env = env;
        }

        public IActionResult Uyeler()
        {
            ViewData["Baslik"] = "Üyeler";
            ViewData["AltBaslik"] = "Üye Ekle";

            var uyeRol = _context.UserRoles.Where(x => x.RoleId != 1).Select(x => x.UserId).ToList();

            var model = _userManager.Users.ToList();

            if (!User.IsInRole("Administrator"))
            {
                model = _userManager.Users.Where(x => uyeRol.Contains(x.Id)).ToList();
            }

            return View(model);
        }


        [HttpPost]
        public JsonResult UyeleriListele(IFormCollection form)
        {
            var dataTableFilter = form.ToDataTableFilter();
            var data = _context.Users
                .Include(u => u.Ilceler)
                    .ThenInclude(ilce => ilce.Iller)
                .ToList();


            string hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";



            // Arama değeri boş değilse, verilerde arama yapılır
            try
            {
                if (!string.IsNullOrEmpty(dataTableFilter.searchValue))
                {
                    string searchValue = dataTableFilter.searchValue.ToLower();

                    // LINQ to Objects: burası artık bellekte çalışıyor
                    data = data.Where(x =>
                        x.Id.ToString().Contains(searchValue)
                        || x.Tarih.ToShortDateString().Contains(searchValue)
                        || x.UyeKayitTipi.GetDisplayName().ToLower().Contains(searchValue)
                        || (x.Ad?.ToLower().Contains(searchValue) ?? false)
                        || (x.CariKodu?.ToLower().Contains(searchValue) ?? false)
                        || "cariodeme".Contains(searchValue)
                        || (x.Email?.ToLower().Contains(searchValue) ?? false)
                        || (x.Gsm?.ToLower().Contains(searchValue) ?? false)
                        || (x.Ilceler?.Iller?.IlAdi?.ToLower().Contains(searchValue) ?? false)
                        || "roller".Contains(searchValue)
                        || x.UyeDurumu.ToString().ToLower().Contains(searchValue)
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Arama sırasında hata oluştu: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("İç hata: " + ex.InnerException.Message);
            }



            // Toplam veri sayısını alın
            dataTableFilter.totalRecord = data.Count();

            // Sıralamayı uygula (sortBy ve sortDir değerlerini kullanarak sıralama yapın)
            Console.WriteLine($"SortColumn: {dataTableFilter.sortColumn}, SortDirection: {dataTableFilter.sortColumnDirection}");

            if (!string.IsNullOrEmpty(dataTableFilter.sortColumn) && !string.IsNullOrEmpty(dataTableFilter.sortColumnDirection))
            {
                try
                {
                    data = ApplySorting(data.AsQueryable(), dataTableFilter.sortColumn, dataTableFilter.sortColumnDirection).ToList();

                    // Sıralama sonrası kontrol
                    Console.WriteLine("Sorted Data:");
                    foreach (var item in data)
                    {
                        Console.WriteLine($"ID: {item.Id}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sıralama hatası oluştu: " + ex.Message);
                }
            }


            // Filtrelenmiş veri sayısını alın
            dataTableFilter.filterRecord = data.Count();

            var filteredData = data
                .Skip(dataTableFilter.skip)
                .Take(dataTableFilter.pageSize)
                .ToList();


            var empList = filteredData
     .Select(x => new
     {
         Id = x.Id,
         Tarih = x.Tarih.ToString("dd.MM.yyyy HH:mm"),
         KayitTipi = $"<span class='badge badge-soft-{(x.UyeKayitTipi == UyeKayitTipi.Web ? "info" : "warning text-dark")}'>{x.UyeKayitTipi.GetDisplayName()}</span>",
         AdSoyad = x.Ad,
         CariKodu = x.CariKodu,
         CariOdeme = $"<a target='_blank' href='{hostUrl}/cari-odeme/{EncryptionHelper.Encrypt(x.Id.ToString())}' class='text-primary'>Ödeme Linki</a>",
         Email = x.Email,
         Gsm = x.Gsm,
         IlAdi = x.Ilceler?.Iller?.IlAdi ?? "",
         Roller = string.Join(" ",
             (from ur in _context.UserRoles
              join r in _context.Roles on ur.RoleId equals r.Id
              where ur.UserId == x.Id
              select $"<span class='badge badge-soft-info'>{r.Name}</span>").ToList()
         ),
         UyeDurumu = $"<span class='badge badge-soft-{(x.UyeDurumu == UyeDurumlari.Onaylandi ? "success" : "danger")}'>{x.UyeDurumu.GetDisplayName()}</span>",
         Buttons = $"<a href='/Admin/Account/UyeEkleGuncelle/{x.Id}' class='mr-3 text-primary'><i class='mdi mdi-pencil font-size-18'></i></a>" +
                   $"<a href='/Admin/Account/UyeSil/{x.Id}' class='text-danger remove'><i class='mdi mdi-trash-can font-size-18'></i></a>"
     })
     .ToList();

            var returnObj = new
            {
                draw = dataTableFilter.draw,
                recordsTotal = dataTableFilter.totalRecord,
                recordsFiltered = dataTableFilter.filterRecord,
                data = empList
            };

            return Json(returnObj, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Döngü hatalarını önler
            });
        }

        private IQueryable<AppUser> ApplySorting(IQueryable<AppUser> data, string sortBy, string sortDir)
        {
            switch (sortBy.ToLower())
            {
                case "id":
                    return ApplyOrder(data, x => x.Id, sortDir);
                case "ad":
                    return ApplyOrder(data, x => x.Ad, sortDir);
                case "soyad":
                    return ApplyOrder(data, x => x.Soyad, sortDir);
                case "email":
                    return ApplyOrder(data, x => x.Email, sortDir);
                case "durum":
                    return ApplyOrder(data, x => x.UyeDurumu, sortDir);
                // Diğer sıralama kriterleri için case'ler eklenebilir
                default:
                    // Varsayılan olarak, belirtilen sütuna göre ID'ye göre sıralama yapılacak
                    return ApplyOrder(data, x => x.Id, sortDir);
            }
        }

        private IQueryable<T> ApplyOrder<T, TKey>(IQueryable<T> data, Expression<Func<T, TKey>> keySelector, string sortDir)
        {
            return sortDir.ToLower() == "asc" ? data.OrderBy(keySelector) : data.OrderByDescending(keySelector);
        }


        [AllowAnonymous]
        public IActionResult UyeOl()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UyeOl(UyeOlViewModel Model)
        {
            if (ModelState.IsValid)
            {
                AppUser uye = new();
                uye.Ad = Model.Ad;
                uye.Soyad = Model.Soyad;
                uye.UserName = Replace.UrlSeo(Model.Ad.Replace(" ", "") + Model.Soyad.Replace(" ", ""));
                uye.Email = Model.Email.Trim();
                uye.PhoneNumber = Model.PhoneNumber;
                uye.EmailConfirmed = true;

                IdentityResult result = await _userManager.CreateAsync(uye, Model.Password);
                if (result.Succeeded)
                {
                    if (uye.EmailConfirmed == false)
                    {
                        string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(uye);
                        string link = Url.Action("EmailDogrulama", "Account", new
                        {
                            userId = uye.Id,
                            token = confirmationToken,
                        }, protocol: HttpContext.Request.Scheme);
                        EmailDogrulamaHelper.MailGonder(link, uye.Email);
                    }

                    await _userManager.AddToRoleAsync(uye, "Administrator");

                    ViewData["KayitBasarili"] = "success";
                    return Redirect("GirisYap");
                }
                else
                {
                    AddModelError(result);
                }
            }


            return View(Model);
        }



        [AllowAnonymous]
        public IActionResult GirisYap(string ReturnUrl)
        {


            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GirisYap(GirisYapViewModel Model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(Model.Email);
                if (user != null)
                {

                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabınız bir süreliğine kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz");

                        return View(Model);
                    };

                    if (_userManager.IsEmailConfirmedAsync(user).Result == false)
                    {
                        ModelState.AddModelError("", "Email Adresiniz Onaylanmamıştır. Lütfen Epostanizi Kontrol Ediniz");

                        return View(Model);

                    }

                    await _signInManager.SignOutAsync();

                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, Model.Password, Model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);


                        await _userManager.ResetAccessFailedCountAsync(user);

                        if (roles.Contains("Administrator") || roles.Contains("Yonetici"))
                        {
                            var userClaim = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name,user.UserName),
                                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                            };

                            foreach (var role in roles)
                            {
                                userClaim.Add(new Claim(ClaimTypes.Role, role));
                            }

                            var personIdentity = new ClaimsIdentity(userClaim, "identitycardAdministrator");
                            var principle = new ClaimsPrincipal(new[] { personIdentity });
                            await HttpContext.SignInAsync("AdminAuth", principle);
                            if (TempData["ReturnUrl"] != null)
                            {
                                return Redirect(TempData["AdminReturnUrl"].ToString());
                            }
                            return RedirectToAction("Index", "Home", new { Area = "Admin" });
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user);

                        int fail = await _userManager.GetAccessFailedCountAsync(user);

                        if (fail == 3)
                        {
                            await _userManager.SetLockoutEndDateAsync(user, new System.DateTimeOffset(DateTime.Now.AddMinutes(20)));
                            ModelState.AddModelError("", "Hesabınız 3 başarısız girişten dolayı 20 dakika süreyle kitlenmiştir.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Geçersiz Kullanıcı Adı veya Şifre");

                        }
                    }


                }

                else
                {
                    //ModelState.AddModelError(nameof(LoginViewModel.Email), "Gecersiz Kullanici Adi veya sifresi");
                    ModelState.AddModelError("", "Bu email adresine kayıtlı kullanıcı bulunamamıştır.");
                }
            }


            return View(Model);
        }

        public async Task<IActionResult> UyeEkleGuncelle(int Id)
        {
            ViewData["Baslik"] = "Üyeler";
            ViewData["AltBaslik"] = "Üye Ekle";

            IQueryable<AppRole> roles = _roleManager.Roles;
            List<RoleAssignViewModel> roleAssignViewModel = new();
            if (Id == 0)
            {
                ViewData["Ulkeler"] = _context.Ulkeler.Select(x => new SelectListItem() { Text = x.UlkeAdi.ToString(), Value = x.Id.ToString() }).ToList();
                ViewData["Iller"] = _context.Iller.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.IlAdi, Value = p.Id.ToString() });

                UyeOlViewModel uye = new UyeOlViewModel();

                foreach (var role in roles)
                {
                    RoleAssignViewModel r = new();
                    r.RoleId = role.Id.ToString();
                    r.RoleName = role.Name;

                    roleAssignViewModel.Add(r);

                }
                ViewData["Plasiyer"] = _context.Plasiyer.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.AdSoyad, Value = p.Id.ToString() });

                ViewData["Roller"] = roleAssignViewModel;

                uye.Roller = roleAssignViewModel;

                return View(uye);
            }

            else
            {

                AppUser user = await _userManager.FindByIdAsync(Id.ToString());


                UyeOlViewModel uye = user.Adapt<UyeOlViewModel>();

                foreach (var role in roles)
                {
                    RoleAssignViewModel r = new RoleAssignViewModel
                    {
                        RoleId = role.Id.ToString(),
                        RoleName = role.Name,
                        Exist = _userManager.IsInRoleAsync(user, role.Name).Result
                    };

                    roleAssignViewModel.Add(r);
                }
                uye.Roller = roleAssignViewModel;


                var ilce = _context.Ilceler.Find(uye.IlceId);
                ViewData["Ulkeler"] = _context.Ulkeler.Select(x => new SelectListItem() { Text = x.UlkeAdi.ToString(), Value = x.Id.ToString() }).ToList();
                ViewData["Iller"] = _context.Iller.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.IlAdi, Value = p.Id.ToString() });
                ViewData["Plasiyer"] = _context.Plasiyer.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.AdSoyad, Value = p.Id.ToString() });
                if (ilce != null)
                {
                    ViewData["Ilceler"] = _context.Ilceler.Where(x => x.IlId == ilce.IlId).ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.IlceAdi, Value = p.Id.ToString() });
                    uye.UlkeId = ilce.Iller.UlkeId;
                    uye.IlId = ilce.IlId;
                    uye.IlceId = ilce.Id;
                }
                else
                {
                    ViewData["Ilceler"] = _context.Ilceler.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.IlceAdi, Value = p.Id.ToString() });
                }

                return View(uye);

            }
        }

        [HttpPost]
        public async Task<IActionResult> UyeEkleGuncelle(UyeOlViewModel Model, IFormFile[] Files, string[] Roles, int Id, string submit)
        {
            ViewData["Baslik"] = "Üyeler";
            ViewData["AltBaslik"] = "Üye Ekle";

            var resultmodel = new ResultViewModel();

            if (Id == 0)
            {

                AppUser uye = new();
                uye.Ad = Model.Ad;
                uye.Tarih = DateTime.Now;
                uye.PlasiyerId = Model?.PlasiyerId;
                uye.FirmaAdi = Model.FirmaAdi;
                uye.UserName = Replace.UrlSeo(Model.Ad.Replace(" ", "")) + "_" + Guid.NewGuid().ToString("N").Substring(0, 6);
                uye.Email = Model.Email.Trim();
                uye.PhoneNumber = Model.PhoneNumber;
                uye.Adres = Model.Adres;
                uye.IlceId = Model?.IlceId;
                uye.IskontoOrani = Model.IskontoOrani;
                uye.CariLimit = Model.CariLimit;
                uye.EmailConfirmed = Model.UyeDurumu == UyeDurumlari.Onaylandi ? true : false;
                uye.UyeDurumu = Model.UyeDurumu;
                uye.UyeKayitTipi = UyeKayitTipi.Web;

                #region Kapak Resmi
                if (Model.UyeResim != null)
                {

                    List<string> ContentTypeListesi = new()
                        {
                            "image/jpeg",
                            "image/png",
                            "image/gif",
                            "image/WebP"

                        };

                    string imageName = ImageHelper.ImageReplaceName(Model.UyeResim, Model.Ad);

                    string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Uyeler/" + imageName;

                    if (ContentTypeListesi.Contains(Model.UyeResim.ContentType))
                    {
                        //Resmi Belirli Boyutta Kaydetmek için (ImageHelper dan boyutlandırma ayarlanıyor)
                        //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UyeResim.OpenReadStream()));

                        using (var stream = new FileStream(Mappath, FileMode.Create))
                        {
                            Model.UyeResim.CopyTo(stream);
                        }

                        uye.ProfilResmi = Mappath.Remove(0, 7);

                    }

                    else
                    {
                        resultmodel.Basarilimi = false;
                        resultmodel.MesajDurumu = "danger";
                        resultmodel.Mesaj = "Jpeg, Png, Gif veya WebP formatında resim yükleyiniz.";

                        return (IActionResult)resultmodel;
                    }

                    if (Model.UyeResim.Length > 5242880)
                    {
                        resultmodel.Basarilimi = false;
                        resultmodel.MesajDurumu = "danger";
                        resultmodel.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";

                        return (IActionResult)resultmodel;
                    }
                }

                else
                {
                    uye.ProfilResmi = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Uyeler/profil.png";
                }
                #endregion


                IdentityResult result = await _userManager.CreateAsync(uye, Model.Password);

                List<IdentityError> hataListesi = result.Errors.ToList();

                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));


                var userRoles = _userManager.GetRolesAsync(uye).Result;

                if (result.Succeeded)
                {
                    if (uye.EmailConfirmed == false)
                    {
                        string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(uye);
                        string link = Url.Action("EmailDogrulama", "Account", new
                        {
                            userId = uye.Id,
                            token = confirmationToken,
                        }, protocol: HttpContext.Request.Scheme);

                        //EmailDogrulamaHelper.MailGonder(link, uye.Email);

                    }




                    AppUser user = _userManager.FindByIdAsync(uye.Id.ToString()).Result;

                    foreach (var role in Model.Roller)
                    {
                        if (role.Exist)
                        {
                            await _userManager.AddToRoleAsync(user, role.RoleName);
                        }
                    }


                    TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Üye Başarıyla Eklendi." });

                    #region Sayfa Butonlari
                    if (submit == "Kaydet")
                    {

                        return RedirectToAction("Uyeler", "Account");
                    }
                    if (submit == "KaydetGuncelle")
                    {


                        return RedirectToAction("UyeEkleGuncelle", "Account", new { Id = user.Id });
                    }
                    #endregion

                }
                else
                {

                    var hataMesaji = string.Join(", ", hataListesi.Select(e => e.Description));

                    TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "error", Text = hataMesaji });
                }


            }

            else
            {



                //ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));



                AppUser userName = await _userManager.FindByIdAsync(Id.ToString());

                var uye = await _userManager.Users
                    .FirstOrDefaultAsync(x => x.UserName == userName.UserName);

                uye.PlasiyerId = Model.PlasiyerId;
                uye.Ad = Model.Ad;
                uye.FirmaAdi = Model.FirmaAdi;
                uye.VergiDairesi = Model.VergiDairesi;
                uye.VergiNumarasi = Model.VergiNumarasi;
                uye.Gsm = Model.Gsm;
                uye.Adres = Model.Adres;
                uye.IlceId = Model.IlceId;
                uye.IskontoOrani = Model.IskontoOrani;
                uye.CariLimit = Model.CariLimit;
                uye.UserName = Replace.UrlSeo(Model.Ad.Replace(" ", "") + "_" + Guid.NewGuid().ToString("N").Substring(0, 6));
                uye.Email = Model.Email.Trim();
                uye.EmailConfirmed = Model.UyeDurumu == UyeDurumlari.Onaylandi ? true : false;
                uye.UyeDurumu = Model.UyeDurumu;

                #region Kapak Resim
                if (Model.UyeResim != null)
                {
                    List<string> ContentTypeListesi = new()
                        {
                            "image/jpeg",
                            "image/png",
                            "image/gif",
                            "image/WebP"

                        };

                    string imageName = ImageHelper.ImageReplaceName(Model.UyeResim, Model.Ad);

                    string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Uyeler/" + imageName;


                    //string Mappath2 = ImageHelper.ImageMappath2() + "Diller/" + imageName;

                    if (ContentTypeListesi.Contains(Model.UyeResim.ContentType))
                    {
                        //Resmi Belirli Boyutta Kaydetmek için (ImageHelper dan boyutlandırma ayarlanıyor)
                        //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UyeResim.OpenReadStream()));

                        using (var stream = new FileStream(Mappath, FileMode.Create))
                        {
                            Model.UyeResim.CopyTo(stream);
                        }

                        uye.ProfilResmi = Mappath.Remove(0, 7);

                    }

                    else
                    {
                        resultmodel.Basarilimi = false;
                        resultmodel.MesajDurumu = "danger";
                        resultmodel.Mesaj = "Jpeg, Png, Gif veya WebP formatında resim yükleyiniz.";

                        return (IActionResult)resultmodel;
                    }

                    if (Model.UyeResim.Length > 5242880)
                    {
                        resultmodel.Basarilimi = false;
                        resultmodel.MesajDurumu = "danger";
                        resultmodel.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";

                        return (IActionResult)resultmodel;
                    }

                }

                else
                {
                    uye.ProfilResmi = new AppDbContext().Users.Find(uye.Id).ProfilResmi;
                }
                #endregion

                IdentityResult result = await _userManager.UpdateAsync(uye);

                List<IdentityError> hataListesi = result.Errors.ToList();

                var userRoles = _userManager.GetRolesAsync(uye).Result;
                foreach (var role in Model.Roller)
                {
                    if (role.Exist && !userRoles.Contains(role.RoleName))
                    {
                        _userManager.AddToRoleAsync(uye, role.RoleName).Wait();
                    }
                    else if (!role.Exist && userRoles.Contains(role.RoleName))
                    {
                        _userManager.RemoveFromRoleAsync(uye, role.RoleName).Wait();
                    }
                }
                if (Model.Password != null)
                {
                    var removePasswordResult = await _userManager.RemovePasswordAsync(uye);

                    result = await _userManager.AddPasswordAsync(uye, Model.Password);

                    if (result.Succeeded)
                    {
                        string token = await _userManager.GeneratePasswordResetTokenAsync(uye);
                        await _userManager.ResetPasswordAsync(uye, token, Model.Password);
                        await _userManager.UpdateSecurityStampAsync(uye);
                    }
                    else
                    {
                        hataListesi = result.Errors.ToList();
                    }
                }

                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(uye);

                    if (Model.EmailConfirmed == false && Model.UyeDurumu == UyeDurumlari.Onaylandi)
                    {

                        var siteAyari = _context.SiteAyarlari.FirstOrDefault();

                        List<string> gonderilecekMailler = new List<string>();
                        gonderilecekMailler.Add(uye.Email);

                        List<System.Net.Mail.Attachment> dosya = new List<System.Net.Mail.Attachment>();
                        foreach (var file in Files)
                        {
                            if (file.Length > 0)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    var fileBytes = ms.ToArray();
                                    System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(new MemoryStream(fileBytes), file.FileName);
                                    dosya.Add(att);
                                }
                            }
                        }

                        string body;
                        string hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                        using (StreamReader reader = new StreamReader(_hostingEnvironment.WebRootPath + @$"/Content/MailTemplates/UyeKayitOnaylandi.html"))
                        {

                            body = reader.ReadToEnd();
                        }



                        #region Üye Onaylanınca Opak Muhasebe Programına Üyeyi Kaydeder
                        if (!_env.IsDevelopment())
                        {
                            await _opakServis.TblCariSbKayitEtAsync(Id);
                        }
                        #endregion



                        //MailHelper.HostMailGonder(
                        //             siteAyari?.EmailAdresi ?? "",
                        //             siteAyari?.EmailSifre ?? "",
                        //             siteAyari?.EmailHost ?? "",
                        //             siteAyari.EmailSSL,
                        //             siteAyari.EmailPort,
                        //             konu: "Üye Kaydınız Onaylandı",
                        //             mailBaslik: "Üye Kaydınız Onaylandı",
                        //             body,
                        //             dosya,
                        //             gonderilecekMailler.ToList()
                        //             );
                    }

                    //await _signInManager.SignOutAsync();
                    //await _signInManager.SignInAsync(user, true);



                    TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Üye Bilgileri Başarıyla Güncellendi." });


                    #region Sayfa Butonlari
                    if (submit == "Kaydet")
                    {
                        return RedirectToAction("Uyeler", "Account");

                    }
                    if (submit == "KaydetGuncelle")
                    {
                        return RedirectToAction("UyeEkleGuncelle", "Account", new { Id = uye.Id });
                    }
                    #endregion
                }

                else
                {

                    var hataMesaji = string.Join(", ", hataListesi.Select(e => e.Description));

                    TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "error", Text = hataMesaji });
                }


            }
            IQueryable<AppRole> roles = _roleManager.Roles;

            List<RoleAssignViewModel> roleAssignViewModel = new();

            foreach (var role in roles)
            {
                RoleAssignViewModel r = new();
                r.RoleId = role.Id.ToString();
                r.RoleName = role.Name;

                roleAssignViewModel.Add(r);

            }

            ViewData["Roller"] = roleAssignViewModel;



            return View(Model);
        }


        public async Task<IActionResult> UyeSifreGuncelle(int Id)
        {

            ViewData["Baslik"] = "Üyeler";
            ViewData["AltBaslik"] = "Sifre Guncelle";

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            AppUser user = await _userManager.FindByIdAsync(Id.ToString());

            UyeSifreGuncelleViewModel userPasswordResetViewModel = new();
            userPasswordResetViewModel.UserId = user.Id.ToString();



            return View(userPasswordResetViewModel);

        }
        [HttpPost]
        public async Task<IActionResult> UyeSifreGuncelle(UyeSifreGuncelleViewModel Model)
        {

            AppUser user = await _userManager.FindByIdAsync(Model.UserId);

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _userManager.ResetPasswordAsync(user, token, Model.YeniSifre);

            await _userManager.UpdateSecurityStampAsync(user);

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Şifre Başarıyla Güncellenmiştir." });

            return RedirectToAction("Uyeler", "Account");
        }

        [AllowAnonymous]
        public IActionResult SifremiUnuttum()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SifremiUnuttum(SifremiUnuttumViewModel Model)
        {
            AppUser user = _userManager.FindByEmailAsync(Model.Email).Result;

            if (user != null)
            {
                string passwordResetToken = _userManager.GeneratePasswordResetTokenAsync(user).Result;

                string passwordResetLink = Url.Action("SifreSifirlamaYeniSifreOlustur", "Account", new
                {
                    userId = user.Id,
                    token = passwordResetToken

                }, HttpContext.Request.Scheme);

                SifreSifirmalaEmailHelper.PasswordResetSendEmail(passwordResetLink, user.Email);

                ViewBag.Status = "success";
                ViewBag.BilgiGizle = "none";

            }
            else
            {
                ModelState.AddModelError("", "Sistemde Kayıtlı Bir Email Bulunamadı.");

            }
            return View(Model);
        }

        [AllowAnonymous]
        public IActionResult SifreSifirlamaYeniSifreOlustur(string userId, string token)
        {
            ViewData["userId"] = userId;
            ViewData["token"] = token;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SifreSifirlamaYeniSifreOlustur([Bind("YeniSifre")] SifremiUnuttumViewModel Model)
        {

            string token = ViewData["token"].ToString();
            string userId = ViewData["userId"].ToString();

            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IdentityResult result = await _userManager.ResetPasswordAsync(user, token, Model.YeniSifre);

                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);

                    ViewBag.status = "success";
                }

                else
                {
                    AddModelError(result);

                }


            }

            else
            {
                ModelState.AddModelError("", "Bir Hata Meydana Gelmiştir. Lütfen Daha Sonra Tekrar Deneyiniz. ");

            }

            return View(Model);
        }


        public IActionResult UyeSil(string Id)
        {
            AppUser user = _userManager.FindByIdAsync(Id).Result;
            if (user != null)
            {
                IdentityResult result = _userManager.DeleteAsync(user).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Uyeler", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Hata Olustu");
                }

            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı Bulunamadı");
            }

            return RedirectToAction("Uyeler", "Account");

        }

        public IActionResult Roller()
        {
            ViewData["Baslik"] = "Roller";
            ViewData["AltBaslik"] = "Rol Ekle";

            return View(_roleManager.Roles.ToList());
        }

        public IActionResult RolEkle(string Id)
        {
            ViewData["Baslik"] = "Roller";
            ViewData["AltBaslik"] = "Rol Ekle";

            if (User.Identity.IsAuthenticated & User.IsInRole("Administrator"))
            {
                AppRole role = _roleManager.FindByIdAsync(Id).Result;

                if (role != null)
                {
                    return View(role.Adapt<RoleViewModel>());

                }

                return View();
            }
            else
            {
                if (_context.Roles.ToList().Count > 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }

        }

        [HttpPost]
        public IActionResult RolEkle(RoleViewModel Model, string Id)
        {


            if (Id == null)
            {
                AppRole role = new();
                role.Name = Model.Name;

                IdentityResult result = _roleManager.CreateAsync(role).Result;
                if (result.Succeeded)
                {
                    TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Başarıyla Eklendi" });

                    return RedirectToAction("Roller");
                }
                else
                {
                    TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "danger", Text = "Bu İsimde Bir Rol Bulunmaktadır." });

                    return View(Model);
                    //AddModelError(result);
                }

                //return View(Model);
            }

            else
            {
                AppRole role = _roleManager.FindByIdAsync(Model.Id).Result;

                if (role != null)
                {
                    role.Name = Model.Name;
                    IdentityResult result = _roleManager.UpdateAsync(role).Result;
                    if (result.Succeeded)
                    {
                        TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Başarıyla Güncellendi" });

                        return RedirectToAction("Roller");
                    }
                    else
                    {

                        AddModelError(result);
                    }

                }

                else
                {
                    ModelState.AddModelError("", "Güncelleme İşlemi Başarısız Oldu");
                }


                return View(Model);
            }
        }


        public IActionResult RolAta(string Id)
        {
            ViewData["Baslik"] = "Roller";
            ViewData["AltBaslik"] = "Rol Ata";

            TempData["userId"] = Id;

            AppUser user = _userManager.FindByIdAsync(Id).Result;

            ViewBag.userName = user.UserName;

            IQueryable<AppRole> roles = _roleManager.Roles;

            List<string> userroles = _userManager.GetRolesAsync(user).Result as List<string>;


            List<RoleAssignViewModel> roleAssignViewModel = new();

            foreach (var role in roles)
            {
                RoleAssignViewModel r = new();
                r.RoleId = role.Id.ToString();
                r.RoleName = role.Name;
                if (userroles.Contains(role.Name))
                {

                    r.Exist = true;
                }
                else
                {
                    r.Exist = false;
                }
                roleAssignViewModel.Add(r);

            }


            return View(roleAssignViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> RolAta(List<RoleAssignViewModel> Model, string submit)
        {
            AppUser user = _userManager.FindByIdAsync(TempData["userId"].ToString()).Result;

            foreach (var item in Model)
            {
                if (item.Exist)
                {
                    await _userManager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.RoleName);
                }

            }
            #region Sayfa Butonlari
            if (submit == "Kaydet")
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Rol Basariyla Atandi." });

                return RedirectToAction("Uyeler", "Account");
            }
            if (submit == "KaydetGuncelle")
            {

                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Rol Basariyla Atandi." });

                return RedirectToAction("RolAta", "Account", new { Id = user.Id });
            }
            #endregion

            return View(Model);

        }
        public IActionResult RolSil(string Id)
        {
            AppRole role = _roleManager.FindByIdAsync(Id).Result;
            if (role != null)
            {
                IdentityResult result = _roleManager.DeleteAsync(role).Result;

            }

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Başarıyla Silindi" });

            return RedirectToAction("Roller");
        }

        public async Task<IActionResult> CikisYap()
        {
            await _signInManager.SignOutAsync();
            Response.Cookies.Delete(".AspNetCore.AdminAuth");
            return RedirectToAction("Index", "Home");
        }
        public void AddModelError(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
