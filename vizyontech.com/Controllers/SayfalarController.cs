using DocumentFormat.OpenXml.Office2010.Excel;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using vizyontech.com.Models;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Transactions;
using X.PagedList;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Globalization;
using vizyontech.com.Areas.Admin.Controllers;

namespace vizyontech.com.Controllers
{
    [AllowAnonymous]

    public class SayfalarController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHtmlLocalizer<HomeController> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager = null;
        SayfalarServis _sayfaServis = null;
        SayfaFormuServis _sayfaFormuServis = null;
        private readonly HelperServis _helperServis;

        private readonly ICaptchaValidator _captchaValidator;


        public SayfalarController(AppDbContext _context, SayfalarServis _sayfaServis, HelperServis _helperServis, SayfaFormuServis _sayfaFormuServis, ICaptchaValidator _captchaValidator, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager)
        {
            this._context = _context;
            this._sayfaServis = _sayfaServis;
            this._helperServis = _helperServis;
            this._sayfaFormuServis = _sayfaFormuServis;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;
            this._captchaValidator = _captchaValidator;
        }

        //public IActionResult Index(string url, int sayfa = 1)
        //{

        //    var kategoriId = _context.SeoUrl.FirstOrDefault(p => p.Url == url)?.EntityId;
        //    var model = _context.Sayfalar.ToList().Where(p => p.ParentSayfaId == kategoriId || kategoriId == null).Where(p => p.ParentSayfaId != 1).OrderBy(p => p.Sira).ToPagedList(sayfa,18);

        //    return View(model);
        //}
        public IActionResult Index(string url, int sayfa = 1, int sayfaSayisi = 24)
        {
            var dil = _helperServis.GetCurrentCulture();
            var kategoriId = _context.SeoUrl.FirstOrDefault(p => p.Url == url)?.EntityId;
            var model = _context.SayfalarTranslate
             .ToList()
             .Where(p => p.Diller.DilKodlari.DilKodu == dil && p.Sayfalar.ParentSayfaId == kategoriId || kategoriId == null)
             .Where(p => p.Sayfalar.ParentSayfaId != 1)
             .OrderBy(p => p.Sayfalar.Sira)
             .ToPagedList(sayfa, sayfaSayisi);

            return View(model);
        }

        public IActionResult BlogDahaFazlaGoster(string url, int page = 1, int pageSize = 6)
        {
            var kategoriId = _context.SeoUrl.FirstOrDefault(p => p.Url == url)?.EntityId;
            var model = _context.Sayfalar.ToList().Where(p => p.ParentSayfaId == kategoriId || kategoriId == null)
                .Where(p => p.ParentSayfaId != 1).OrderBy(p => p.Sira).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return PartialView("_BlogPartial", model);
        }

        public IActionResult ReferanslarDahaFazlaGoster(string url, int page = 1, int pageSize = 6)
        {
            var kategoriId = _context.SeoUrl.FirstOrDefault(p => p.Url == url)?.EntityId;
            var model = _context.Sayfalar.ToList().Where(p => p.ParentSayfaId == kategoriId || kategoriId == null)
                .Where(p => p.ParentSayfaId != 1).OrderBy(p => p.Sira).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return PartialView("_ReferanslarPartial", model);
        }

        public IActionResult Detay(int Id)
        {
            var model = _context.Sayfalar.Find(Id);

            int aktifHit = model.Hit + 1;
            model.Hit = aktifHit;
            _context.Sayfalar.Update(model);
            _context.SaveChanges();

            return View(model);
        }

        public IActionResult Arama(string Keyword)
        {
            var dil = HttpContext.Features.Get<Microsoft.AspNetCore.Localization.IRequestCultureFeature>().RequestCulture.Culture.Name;

            var model = _context.Sayfalar.Where(s => s.ParentSayfaId != 1 && s.SayfalarTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == dil).SayfaAdi.ToLower().Contains(Keyword.ToLower().Trim())).ToList();

            if (model.ToList().Count == 0)
            {
                ViewData["AramaSonucu"] = "Sonuç Bulunamadı...";
            }

            return View(model);

        }

        #region Diğer Sayfalar
        public IActionResult Hakkimizda(int Id)
        {
            var model = _context.Sayfalar.Find(Id);
            return View(model);
        }

        public IActionResult Markalar(int Id)
        {
            var model = _context.Sayfalar.Find(Id);
            return View(model);
        }


        public IActionResult Yorumlar(string url, int sayfa = 1)
        {
            //var model = _context.Sayfalar.Where(p => p.ParentSayfaId != 1 && p.SayfaTipi == SayfaTipleri.Yorumlar && p.Durum == SayfaDurumlari.Aktif).ToList().OrderBy(p => p.Sira);

            ViewData["Yorumlar"] = _context.Yorumlar.Where(p => p.YorumDurumu == SayfaDurumlari.Aktif).ToList().OrderBy(p => p.YorumTarihi).ToPagedList(sayfa, 10);

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Yorumlar(YorumViewModel Model, IList<IFormFile> Files, string submit)
        {
            var model = await _sayfaServis.YorumEkle(Model, Files, submit);

            return Json(model);
        }

        public IActionResult SSS(string url)
        {
            var kategoriId = _context.SeoUrl.FirstOrDefault(p => p.Url == url)?.EntityId;
            var model = _context.Sayfalar.Where(p => p.ParentSayfaId != 1 && p.ParentSayfaId == kategoriId).ToList().OrderBy(p => p.Sira);

            return View(model);
        }

        public IActionResult Hizmetlerimiz(int Id)
        {
            var model = _context.Sayfalar.Find(Id);
            return View(model);
        }
        public IActionResult Referanslar()
        {
            var model = _context.Sayfalar.Where(p => p.ParentSayfaId != 1 && p.SayfaTipi == SayfaTipleri.Referanslar && p.Durum == SayfaDurumlari.Aktif).ToList().OrderBy(p => p.Sira);
            return View(model);
        }

        public IActionResult Ekibimiz()
        {
            var model = _context.Ekipler.Where(p => p.Durum == SayfaDurumlari.Aktif).ToList().OrderBy(p => p.Sira);

            return View(model);
        }

        public IActionResult VideoGaleri()
        {
            var model = _context.Sayfalar.ToList();

            return View(model);

        }

        public JsonResult TakvimListele()
        {
            using (AppDbContext db = new AppDbContext())
            {

                var dil = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

                var model = db.TakvimTranslate.Where(p => p.Diller.DilKodlari.DilKodu == dil).Select(v => new
                {
                    Id = v.Id,
                    Baslik = v.Baslik,
                    Aciklama = v.Aciklama,
                    BaslangicTarihi = v.Takvim.BaslangicTarihi,
                    BitisTarihi = v.Takvim.BitisTarihi,
                    Renk = v.Takvim.Renk,
                }).ToList();

                return Json(model);

            }
        }

        public IActionResult EKatalog(string url)
        {
            var entityId = _context.SeoUrl.FirstOrDefault(p => p.Url == url)?.EntityId;

            return View(_context.FotografGaleriResimleri.ToList().Where(p => p.FotografGaleriId == entityId & p.FotografGalerileri.GaleriTipi == GaleriTipleri.EKatalog));
        }


        public IActionResult BizeUlasin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BizeUlasin(IFormCollection Model, IList<IFormFile> Files)
        {
            ResultViewModel sonuc = new ResultViewModel();


            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    bool CapcheKontrol = await _captchaValidator.IsCaptchaPassedAsync(Model["Captcha"]);
                    if (CapcheKontrol)
                    {
                        var siteAyari = _context.SiteAyarlariTranslate.ToList().Where(p => p.SiteAyarId == 1).FirstOrDefault();

                        var firmaAdi = Model["FirmaAdi"];
                        var ad = Model["AdSoyad"];
                        var soyad = Model["Soyad"];
                        var email = Model["Email"];
                        var telefon = Model["Telefon"];
                        var konu = Model["Konu"];
                        var mesaj = Model["Mesaj"];

                        string body;
                        body = "Ad Soyad: " + ad + "<br /><br />";
                        body = "Firma Adı: " + firmaAdi + "<br /><br />";
                        body += "Email: " + email + "<br /><br />";
                        body += "Telefon: " + telefon + "<br /><br />";
                        body += "Konu: " + konu + "<br /><br />";
                        body += "Mesaj: " + mesaj + "<br /><br />";

                        // Mail listesi
                        List<string> gonderilecekMailler = new List<string>();

                        // Site ayarından gelen mailleri virgüle göre ayır ve listeye ekle
                        if (!string.IsNullOrEmpty(siteAyari?.SiteAyarlari.GonderilecekMail))
                        {
                            var mailler = siteAyari.SiteAyarlari.GonderilecekMail
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim());

                            gonderilecekMailler.AddRange(mailler);
                        }

                        // Aynı mail birden fazla gelirse kaldır
                        gonderilecekMailler = gonderilecekMailler.Distinct().ToList();

                        // Dosyaları ekle
                        List<System.Net.Mail.Attachment> dosya = new List<System.Net.Mail.Attachment>();

                        if (Files != null)
                        {
                            foreach (var file in Files)
                            {
                                if (file.Length > 0)
                                {
                                    using (var ms = new MemoryStream())
                                    {
                                        file.CopyTo(ms);
                                        var fileBytes = ms.ToArray();
                                        var att = new System.Net.Mail.Attachment(new MemoryStream(fileBytes), file.FileName);
                                        dosya.Add(att);
                                    }
                                }
                            }
                        }


                        MailHelper.HostMailGonder(
                        siteAyari?.SiteAyarlari.EmailAdresi ?? "",
                        siteAyari?.SiteAyarlari.EmailSifre ?? "",
                        siteAyari?.SiteAyarlari.EmailHost ?? "",
                        siteAyari.SiteAyarlari.EmailSSL,
                        siteAyari.SiteAyarlari.EmailPort,
                        konu: siteAyari.SiteAyarlari.MailKonu ?? "Bize Ulaşın Formu",
                        mailBaslik: siteAyari.SiteAyarlari.MailBaslik ?? "Bize Ulaşın Formu",
                        body,
                        dosya,
                        gonderilecekMailler);


                        sonuc.Basarilimi = true;
                        sonuc.Mesaj = siteAyari?.SiteAyarlari.MailGonderildiMesaji ?? "Başarıyla Gönderildi";
                        sonuc.Display = "block";

                        sonuc.MesajDurumu = "alert alert-success";

                        ViewBag.Alert = "block";

                        #region Veritabanı Kayıt
                        SayfaFormTipleri sayfaFormTipi = (SayfaFormTipleri)Enum.Parse(typeof(SayfaFormTipleri), Model["SayfaFormTipi"]);
                        var sayfaFormuModel = new SayfaFormuViewModel()
                        {
                            Ad = Model["AdSoyad"],
                            FirmaAdi = Model["FirmaAdi"],
                            Email = Model["Email"],
                            Telefon = Model["Telefon"],
                            Konu = Model["Konu"],
                            Mesaj = Model["Mesaj"],
                            SayfaFormTipi = sayfaFormTipi
                        };

                        var model = await _sayfaFormuServis.UpdatePage(sayfaFormuModel, "");
                        #endregion
                        transaction.Complete();
                    }
                    else
                    {
                        sonuc.Basarilimi = false;
                        sonuc.Mesaj = "Captcha Doğrulaması Başarısız Oldu";
                        sonuc.MesajDurumu = "alert alert-danger";
                        sonuc.Display = "block";
                    }
                    return Json(sonuc);

                }
            }
            catch (Exception Hata)
            {
                sonuc.Basarilimi = false;
                sonuc.Mesaj = _localizer["strFormMesajHata"].Value + Hata.Message;
                sonuc.Display = "block";
                sonuc.MesajDurumu = "alert alert-danger";
                return Json(sonuc);

                // Hata Oluşması Durumunda Uyarı Ver...
            }

        }


        [HttpPost]
        public async Task<IActionResult> SayfaFormuKaydet(IFormCollection Model, IList<IFormFile> Files)
        {
            ResultViewModel sonuc = new ResultViewModel();


            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    bool CapcheKontrol = await _captchaValidator.IsCaptchaPassedAsync(Model["Captcha"]);
                    if (CapcheKontrol)
                    {

                        var siteAyari = _context.SiteAyarlariTranslate.ToList().Where(p => p.SiteAyarId == 1).FirstOrDefault();

                        var ad = Model["AdSoyad"];
                        //var soyad = Model["Soyad"];
                        //var email = Model["Email"];
                        var telefon = Model["Telefon"];
                        //var konu = Model["Konu"];
                        var konaklamaTipi = Model["KonaklamaTipi"];
                        var girisTarihi = Model["GirisTarihi"];
                        var cikisTarihi = Model["CikisTarihi"];

                        var mesaj = Model["Mesaj"];
                        var entityId = Model["EntityId"];

                        string body;
                        body = "Ad Soyad: " + ad + "<br /><br />";
                        body += "Telefon: " + telefon + "<br /><br />";
                        body += "Konaklama Tipi: " + konaklamaTipi + "<br /><br />";
                        body += "Giriş Tarihi: " + girisTarihi + "<br /><br />";
                        body += "Çıkış Tarihi: " + cikisTarihi + "<br /><br />";
                        //body = "Soyad: " + soyad + "<br /><br />";
                        //body += "Email: " + email + "<br /><br />";
                        //body += "Telefon: " + telefon + "<br /><br />";
                        //body += "Konu: " + konu + "<br /><br />";
                        //body += "Mesaj: " + mesaj + "<br /><br />";

                        List<string> gonderilecekMailler = new List<string>();
                        gonderilecekMailler.Add(siteAyari?.SiteAyarlari.GonderilecekMail ?? "");

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

                        MailHelper.HostMailGonder(
                        siteAyari?.SiteAyarlari.EmailAdresi ?? "",
                        siteAyari?.SiteAyarlari.EmailSifre ?? "",
                        siteAyari?.SiteAyarlari.EmailHost ?? "",
                        siteAyari.SiteAyarlari.EmailSSL,
                        siteAyari.SiteAyarlari.EmailPort,
                        konu: "Rezervasyon Başvurusu Var",
                        mailBaslik: siteAyari?.SiteAyarlari?.FirmaAdi,
                        body,
                        dosya,
                        gonderilecekMailler);


                        sonuc.Basarilimi = true;
                        sonuc.Mesaj = siteAyari?.SiteAyarlari.MailGonderildiMesaji ?? "Başarıyla Gönderildi";
                        sonuc.Display = "block";
                        sonuc.MesajDurumu = "alert alert-success";

                        #region Veritabanı Kayıt
                        SayfaFormTipleri sayfaFormTipi = (SayfaFormTipleri)Enum.Parse(typeof(SayfaFormTipleri), Model["SayfaFormTipi"]);


                        string format = "dd.MM.yyyy";

                        DateTime girisTarihiModel = DateTime.ParseExact(Model["GirisTarihi"].ToString(), format, CultureInfo.InvariantCulture);
                        DateTime cikisTarihiModel = DateTime.ParseExact(Model["CikisTarihi"].ToString(), format, CultureInfo.InvariantCulture);


                        var sayfaFormuModel = new SayfaFormuViewModel()
                        {
                            Ad = Model["AdSoyad"],
                            Telefon = Model["Telefon"],
                            KonaklamaTipi = Model["KonaklamaTipi"],
                            GirisTarihi = girisTarihiModel,
                            CikisTarihi = cikisTarihiModel,
                            SayfaFormTipi = sayfaFormTipi,
                            EntityId = entityId == "" ? null : Convert.ToInt32(entityId)
                        };

                        var model = await _sayfaFormuServis.UpdatePage(sayfaFormuModel, "");
                        #endregion
                        transaction.Complete();
                        return Json(sonuc);
                    }
                    else
                    {
                        sonuc.Basarilimi = false;
                        sonuc.Mesaj = "Captcha Doğrulaması Başarısız Oldu";
                        sonuc.MesajDurumu = "alert alert-danger";
                        sonuc.Display = "block";
                        return Json(sonuc);
                    }

                }
            }
            catch (Exception Hata)
            {
                sonuc.Basarilimi = false;
                sonuc.Mesaj = _localizer["strFormMesajHata"].Value + Hata.Message;
                sonuc.Display = "block";
                sonuc.MesajDurumu = "alert alert-danger";
                return Json(sonuc);

                // Hata Oluşması Durumunda Uyarı Ver...
            }

        }




        private void PopulateDropdown()
        {
            var fiyatListesi = _context.Fiyatlar.ToList().Select(x => x.IlId);

            ViewData["Iller"] = _context.Iller.Where(p => fiyatListesi.Contains(p.Id)).AsQueryable().Select(p => new SelectListItem() { Text = p.IlAdi, Value = p.Plaka.ToString() }).ToList();

            ViewData["Tarih"] = _context.Fiyatlar.AsQueryable().Select(p => new SelectListItem() { Text = p.Tarih.ToShortDateString(), Value = p.Tarih.ToString() }).ToList().DistinctBy(p => p.Value);


        }
        #endregion
    }
}
