using DocumentFormat.OpenXml.Presentation;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Exchange.WebServices.Data;
using System.Security.Claims;
using System.Web;

namespace vizyontech.com.Controllers
{

    [Authorize(Roles = "Bayi", AuthenticationSchemes = "BayiAuth")]
    public class AccountController : Controller
    {
        private UnitOfWork _uow = null;
        private readonly HelperServis _helperServis;

        private readonly AdresServis _adresServis;
        private readonly UyelerServis _uyeServis;
        private readonly SiparislerServis _siparisServis;

        private readonly AlisverisListemServis _alisverisListemServis;

        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager = null;

        [Obsolete]
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public AccountController(AppDbContext _context, HelperServis _helperServis, AdresServis _adresServis, UyelerServis _uyeServis, SiparislerServis _siparisServis, AlisverisListemServis _alisverisListemServis, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager)
        {
            this._context = _context;
            this._siparisServis = _siparisServis;
            this._alisverisListemServis = _alisverisListemServis;
            _hostingEnvironment = hostingEnvironment;

            _httpContextAccessor = httpContextAccessor;
            this._adresServis = _adresServis;
            this._uyeServis = _uyeServis;

            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;



            _uow = new UnitOfWork();
            this._helperServis = _helperServis;
        }

        [AllowAnonymous]
        [Route("uyeol")]
        public IActionResult UyeOl()
        {
            PopulateDropdown();

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("uyeol")]
        public async Task<IActionResult> UyeOl(BayiOlViewModel Model, IList<IFormFile> Files)
        {
            var model = await _uyeServis.UyeOl(Model, Files);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return Redirect(model.SayfaUrl);

            }
            else
            {
                PopulateDropdown();

                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                return View();
            }
        }

        [Route("hesabim")]
        public IActionResult Hesabim()
        {
            
            var uye = _helperServis.GetUye().Result;

            ViewData["Siparisler"] = _uow.Repository<Siparisler>().GetAll().Result.Where(x => x.UyeId == uye.Id).ToList();
            ViewData["Adresler"] = _uow.Repository<Adresler>().GetAll().Result.Where(x => x.UyeId == uye.Id).ToList();

            return View();
        }
        [Route("hesabimiguncelle")]
        public async Task<IActionResult> HesabimiGuncelle()
        {

            var uyeBilgi = _helperServis.GetUye().Result;

            ViewData["Baslik"] = "Üyeler";
            ViewData["AltBaslik"] = "Üye Düzenle";

            //Rolleri Listele
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
            //Rolleri Listele

            AppUser uye = await _userManager.FindByIdAsync(uyeBilgi.Id.ToString());

            var kullanici = _userManager.FindByNameAsync(uye.UserName).Result;

            UyeOlViewModel uyeModel = kullanici.Adapt<UyeOlViewModel>();

            int ilceId = 0;

            var ilce = _context.Ilceler.Find(uyeModel?.IlceId);
            if (ilce != null)
            {
                ilceId = ilce.IlId;
            }
            ViewData["Ulkeler"] = _context.Ulkeler.Select(x => new SelectListItem() { Text = x.UlkeAdi.ToString(), Value = x.Id.ToString() }).ToList();
            ViewData["Iller"] = _context.Iller.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.IlAdi, Value = p.Id.ToString() });
            ViewData["Ilceler"] = _context.Ilceler.Where(x => x.IlId == ilceId).ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.IlceAdi, Value = p.Id.ToString() });


            if (ilce != null)
            {
                uyeModel.UlkeId = ilce.Iller?.UlkeId;
                uyeModel.IlId = ilce.IlId;
                uyeModel.IlceId = ilce.Id;
            }

            //ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            return View(uyeModel);


        }

        [HttpPost]
        [Route("hesabimiguncelle")]
        public async Task<IActionResult> HesabimiGuncelle(BayiOlViewModel Model, int Id, IList<IFormFile> Files)
        {
            var uyeBilgi = _helperServis.GetUye().Result;
            var model = await _uyeServis.HesabimiGuncelle(Model, uyeBilgi.Id, Files);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();


            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction("HesabimiGuncelle", "Account");
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction("HesabimiGuncelle", "Account");
            }
        }


        [AllowAnonymous]
        [Route("emaildogrulama")]
        public async Task<IActionResult> EmailDogrulama(string userId, string token)
        {

            var model = await _uyeServis.EmailDogrulama(userId, HttpUtility.UrlEncode(token));

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return Redirect(model.SayfaUrl);

            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                return View();
            }

        }
        [AllowAnonymous]
        [Route("girisyap")]
        public IActionResult GirisYap(string ReturnUrl)
        {
            var uyeLoginmi = _helperServis.GetUyeLoginMi().Result;

            if (uyeLoginmi.User != null)
            {
                return RedirectToAction("Hesabim", "Account");
            }


            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("girisyap")]
        public async Task<IActionResult> GirisYap(GirisYapViewModel Model)
        {
            var model = await _uyeServis.GirisYap(Model, _userManager, _signInManager, _roleManager);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });



                return Redirect(model.SayfaUrl);

            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                return View();
            }

        }

        [AllowAnonymous]
        [Route("sifreunuttum")]
        public IActionResult SifremiUnuttum()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("sifreunuttum")]

        public async Task<IActionResult> SifremiUnuttum(SifremiUnuttumViewModel Model, IList<IFormFile> Files)
        {
            var model = await _uyeServis.SifremiUnuttum(Model, Files);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                return View();

            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return View(Model);

            }

        }
        [AllowAnonymous]
        [Route("sifresifirlamayenisifreOlustur")]
        public IActionResult SifreSifirlamaYeniSifreOlustur(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("sifresifirlamayenisifreOlustur")]
        public async Task<IActionResult> SifreSifirlamaYeniSifreOlustur(SifremiUnuttumViewModel Model)
        {
            string token = TempData["token"].ToString();
            string userId = TempData["userId"].ToString();

            var model = await _uyeServis.SifreSifirlamaYeniSifreOlustur(Model, userId, HttpUtility.UrlEncode(token));

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                ViewBag.Status = model.MesajDurumu;

                return Redirect(model.SayfaUrl);

            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return View(Model);

            }

        }
        [Route("alisverislistem")]
        public IActionResult AlisverisListem()
        {
            var model = _alisverisListemServis.PageList().Result;

            return View(model);
        }

        public async Task<IActionResult> AlisverisListeUrunSil(int UrunId)
        {

            var sonuc = new ResultViewModel();

            try
            {
              

               await _alisverisListemServis.GetAlisverisListeUrunSil(UrunId);

                sonuc.Basarilimi = true;
                sonuc.MesajDurumu = "success";
                sonuc.Mesaj = "Başarıyla Silindi";

                return Json(sonuc);

            }
            catch (Exception hata)
            {
                sonuc.Basarilimi = false;
                sonuc.MesajDurumu = "danger";
                sonuc.Mesaj = "Hata oluştu : " + hata.Message.ToString();
                return Json(sonuc);
            }

        }


        public IActionResult _AltKullaniciEkle(int Id = 0)
        {
            //    if (Id > 0)
            //    {
            //        var uyeDuzenleModel = _context.Users.Find(Id);
            //        return PartialView(uyeDuzenleModel);
            //    }

            //    UyeOlViewModel uyeEkleModel = new UyeOlViewModel();

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> AltKullaniciEkle(BayiOlViewModel Model, int UyeId)
        {
            var sonuc = new ResultViewModel();


            bool Basarilimi = true;
            string Mesaj = "";
            if (UyeId == 0)
            {
                Model.ParentId = _helperServis.GetUye().Result.Id;

                var model = await _uyeServis.UyeOl(Model, null);
                Basarilimi = model.Basarilimi;
                Mesaj = model.Mesaj;
            }
            else
            {

                var model = await _uyeServis.HesabimiGuncelle(Model, UyeId, null);
                Basarilimi = model.Basarilimi;
                Mesaj = model.Mesaj;
            }



            sonuc.Basarilimi = Basarilimi;
            sonuc.MesajDurumu = Basarilimi == true ? "alert-success" : "alert-danger";
            sonuc.Mesaj = Mesaj;


            return Json(sonuc);
        }

        public PartialViewResult _AltKullaniciListe()
        {
            var uyeId = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = _context.Users.Where(x => x.ParentId == uyeId).ToList();

            return PartialView("~/Views/Account/_AltKullaniciListe.cshtml", model);
        }
        [Route("adreslerim")]
        public IActionResult Adresler()
        {

            return View();
        }


        public IActionResult _AdresEkle(int Id = 0)
        {

            if (Id > 0)
            {
                AdresViewModel adresDuzenleModel = new AdresViewModel()
                {
                    AdresEkle = _context.Adresler.Find(Id),
                };

                var ilce = _context.Ilceler.Find(adresDuzenleModel.AdresEkle.IlceId);

                ViewData["Ulkeler"] = _context.Ulkeler.Select(x => new SelectListItem() { Text = x.UlkeAdi.ToString(), Value = x.Id.ToString() }).ToList();
                ViewData["Iller"] = _context.Iller.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.IlAdi, Value = p.Id.ToString() });
                ViewData["Ilceler"] = _context.Ilceler.Where(x => x.IlId == ilce.IlId).ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.IlceAdi, Value = p.Id.ToString() });


                adresDuzenleModel.Id = Id;
                adresDuzenleModel.UlkeId = ilce.Iller.UlkeId;
                adresDuzenleModel.IlId = ilce.IlId;
                adresDuzenleModel.IlceId = ilce.Id;


                return PartialView(adresDuzenleModel);

            }

            AdresViewModel adresEkleModel = new AdresViewModel();

            PopulateDropdown();

            return PartialView(adresEkleModel);

        }

        [HttpPost]
        public IActionResult AdresEkle(AdresViewModel Model)
        {
            var sonuc = new ResultViewModel();

            try
            {
                var userId = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var model = _adresServis.EkleGuncelle(Model, userId);
                sonuc.Basarilimi = model.Result.Basarilimi;
                sonuc.MesajDurumu = model.Result.MesajDurumu;
                sonuc.Mesaj = model.Result.Mesaj;

                return Json(sonuc);

            }
            catch (Exception hata)
            {
                sonuc.Basarilimi = false;
                sonuc.MesajDurumu = "danger";
                sonuc.Mesaj = "Hata oluştu : " + hata.Message.ToString();
                return Json(sonuc);
            }
        }

        public PartialViewResult _AdresSelectListe()
        {
            var uye = _helperServis.GetUye().Result;

            ViewData["Adresler"] = _uow.Repository<Adresler>().GetAll().Result.Where(x => x.UyeId == uye.Id).Select(x => new SelectListItem { Text = x.AdresAdi, Value = x.Id.ToString() });

            return PartialView("~/Views/Account/_AdresSelectListe.cshtml");
        }

        public PartialViewResult _AdresListesi()
        {
            var uye = _helperServis.GetUye().Result;

            var model = _uow.Repository<Adresler>().GetAll().Result.Where(x => x.UyeId == uye.Id).ToList();

            return PartialView("~/Views/Account/_AdresListesi.cshtml", model);
        }

        public async Task<IActionResult> AdresSil(int Id)
        {
            var model = await _context.Adresler.FindAsync(Id);

            var sonuc = new ResultViewModel();

            try
            {
                var userId = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

                _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                await _context.SaveChangesAsync();

                sonuc.Basarilimi = true;
                sonuc.MesajDurumu = "success";
                sonuc.Mesaj = "Başarıyla Silindi";

                return Json(sonuc);

            }
            catch (Exception hata)
            {
                sonuc.Basarilimi = false;
                sonuc.MesajDurumu = "danger";
                sonuc.Mesaj = "Hata oluştu : " + hata.Message.ToString();
                return Json(sonuc);
            }

        }


        [AllowAnonymous]
        public JsonResult IlleriGetir(int id)
        {
            var model = _context.Iller.Where(p => p.UlkeId == id).Select(x => new { Id = x.Id, IlAdi = x.IlAdi }).OrderBy(x=> x.IlAdi).ToList();

            return Json(model);
        }

        [AllowAnonymous]
        public JsonResult IlceleriGetir(int id)
        {
            var model = _context.Ilceler.Where(p => p.IlId == id).Select(x => new { Id = x.Id, IlceAdi = x.IlceAdi }).OrderBy(x => x.IlceAdi).ToList();

            return Json(model);
        }

        [Route("siparisler")]
        public IActionResult Siparisler()
        {
            var uye = _helperServis.GetUye().Result;

            var model = _context.Siparisler
                .Where(p => p.SiparisDurumu != (int)SiparisDurumTipleri.EksikSiparis &&
                            (p.UyeId == uye.Id || p.Email == uye.Email))
                .ToList();

            return View(model);
        }
        [Route("siparisdetay")]
        public IActionResult SiparisDetay(int Id)
        {
           
            var model = _context.Siparisler.Find(Id);

            return View(model);
        }


        public IActionResult _SiparisIptalEt(int Id = 0)
        {
            return PartialView();
        }
        public IActionResult SiparisIptalEt(int Id)
        {

            SiparisGecmisViewModel siparisGecmisi = new SiparisGecmisViewModel();
            siparisGecmisi.SiparisId = Id;
            siparisGecmisi.SiparisDurumId = (int)SiparisDurumTipleri.IptalEdildi;

            var model = _siparisServis.SiparisDurumuGuncelle(siparisGecmisi, null);

            ResultViewModel Sonuc = new ResultViewModel();

            if (model.Result.Basarilimi == true)
            {

                Sonuc.Basarilimi = model.Result.Basarilimi;
                Sonuc.Display = "block";
                Sonuc.MesajDurumu = "success";
                Sonuc.Mesaj = "Sipariş Başarıyla İptal Edildi";
                return Json(Sonuc);


            }
            else
            {
                Sonuc.Basarilimi = model.Result.Basarilimi;
                Sonuc.Display = "block";
                Sonuc.MesajDurumu = "danger";
                Sonuc.Mesaj = model.Result.Mesaj;
                return Json(Sonuc);
            }

        }

        public void PopulateDropdown()
        {
            ViewData["Ulkeler"] = _context.Ulkeler.Select(x => new SelectListItem() { Text = x.UlkeAdi.ToString(), Value = x.Id.ToString() }).ToList().OrderBy(x => x.Text);
            ViewData["Iller"] = _context.Iller.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.IlAdi, Value = p.Id.ToString() }).ToList().OrderBy(x => x.Text);
            ViewData["Ilceler"] = _context.Ilceler.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.IlceAdi, Value = p.Id.ToString() }).ToList().OrderBy(x => x.Text);
        }
        [Route("cikisyap")]
        public async Task<IActionResult> CikisYap()
        {
            await _signInManager.SignOutAsync();
            Response.Cookies.Delete(".AspNetCore.BayiAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}
