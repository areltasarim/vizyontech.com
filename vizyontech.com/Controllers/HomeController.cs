using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using Microsoft.Exchange.WebServices.Data;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Wordprocessing;
using EticaretWebCoreViewModel;
using DocumentFormat.OpenXml.Math;
using EticaretWebCoreService;
using Pchp.Library;
using System.Text;
using vizyontech.com.Models;
using SixLabors.ImageSharp.Formats.Png;
using Color = SixLabors.ImageSharp.Color;
using EticaretWebCoreService.InstagramService;
using Azure.Core;
using Microsoft.Extensions.Options;
using EticaretWebCoreService.CariOdeme;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Web.Helpers;

namespace vizyontech.com.Controllers
{
    [Authorize(Roles = "Bayi", AuthenticationSchemes = "BayiAuth")]

    public class HomeController : Controller
    {
        private readonly IHtmlLocalizer<HomeController> _localizer;

        private readonly ILogger<HomeController> _logger;

        private readonly AppDbContext _context;
        private readonly InstagramService _instagramServis;
        private readonly HelperServis _helperServis;


        private UnitOfWork _uow = null;
        MarkalarServis _markaServis = null;

        public HomeController(ILogger<HomeController> logger,
            IHtmlLocalizer<HomeController> localizer, AppDbContext _context, MarkalarServis markaServis, InstagramService instagramServis, HelperServis helperServis)
        {
            _logger = logger;
            _localizer = localizer;

            this._context = _context;
            _uow = new UnitOfWork();
            _markaServis = markaServis;
            _instagramServis = instagramServis;
            _helperServis = helperServis;
        }

        [AllowAnonymous]

        public async Task<IActionResult> Index()
        {
            HttpContext.Session.Set("test",Encoding.UTF8.GetBytes("Deneme"));
            List<SayfaToSayfalar> SayfaToSayfalar = await _context.SayfaToSayfalar.Where(x => x.Id > 0).ToListAsync();
            ViewBag.SayfaToSayfaListesi = SayfaToSayfalar;

            List<Sayfalar> Sayfalar = await _context.Sayfalar.Where(x => x.Id != 1 && x.SayfaTipi == SayfaTipleri.Blog).OrderBy(x => x.Sira).ToListAsync();
            ViewBag.Sayfalar = Sayfalar;

            //var accessToken = "EAAWhG5aMw0EBOzzLimWfQpHrTKRZACkFzjKg9u48sTmRLUWrHVnEr8xo6IcY9Jqb4Az7yOeEhkpJxoRbxAodk2cScrnfJPUqwcfAizaQFZC3KQ72wyn4uFCyi4WkBFwsiZClNCNpeGWIXymZBDLOnqh9PiL4uCJnuPyAVFzVuD5tS18SQnQ9sur2mBU0DqBSVcrekRkhpeSlm1VvOkwJWZBiQMCfjuGPZAzjlrGsjx6AZDZD";
            // var userId = "542254036198679";
            //await _instagramServis.GetInstagramMediaAsync(accessToken, userId);
            
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Index(IFormCollection iletisimFormu, int KategoriId, IList<IFormFile> Files)
        {
            ResultViewModel sonuc = new ResultViewModel();


            // Uygun verileri alın ve ara tabloyu oluşturun
            List<UrunToKategori> urunToKategori = _context.UrunToKategori.ToList();

            // Filtreleme işlemini gerçekleştirin
            List<Urunler> filtrelenmisUrunler = urunToKategori
                .Where(x => x.KategoriId == KategoriId)
                .Select(x => x.Urunler)
                .ToList();

            // ViewData'ya filtrelenmiş urunler listesini atayın
            ViewData["Urunler"] = filtrelenmisUrunler;

            //#region Form
            //if (iletisimFormu.TryGetValue("AdSoyad", out var veri))
            //{
            //    try
            //    {
            //        using (System.Transactions.TransactionScope Transaction = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, TimeSpan.FromMinutes(30)))
            //        {

            //            var siteAyari = _context.SiteAyarlariTranslate.ToList().Where(p => p.SiteAyarId == 1).FirstOrDefault();

            //            var adSoyad = iletisimFormu["AdSoyad"];
            //            var telefon = iletisimFormu["Telefon"];
            //            var konu = iletisimFormu["Konu"];
            //            var mesaj = iletisimFormu["Mesaj"];

            //            string body;
            //            body = _localizer["strAdSoyad"].Value + adSoyad + "<br /><br />";
            //            body += _localizer["strTelefon"].Value + telefon + "<br /><br />";
            //            body += _localizer["strKonu"].Value + konu + "<br /><br />";
            //            body += _localizer["strMesaj"].Value + mesaj + "<br /><br />";

            //            List<string> gonderilecekMailler = new List<string>();
            //            gonderilecekMailler.Add(siteAyari?.SiteAyarlari.GonderilecekMail ?? "");

            //            List<System.Net.Mail.Attachment> dosya = new List<System.Net.Mail.Attachment>();
            //            foreach (var file in Files)
            //            {
            //                if (file.Length > 0)
            //                {
            //                    using (var ms = new MemoryStream())
            //                    {
            //                        file.CopyTo(ms);
            //                        var fileBytes = ms.ToArray();
            //                        System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(new MemoryStream(fileBytes), file.FileName);
            //                        dosya.Add(att);
            //                    }
            //                }
            //            }

            //            MailHelper.HostMailGonder(
            //            siteAyari?.SiteAyarlari.EmailAdresi ?? "",
            //            siteAyari?.SiteAyarlari.EmailSifre ?? "",
            //            siteAyari?.SiteAyarlari.EmailHost ?? "",
            //            siteAyari.SiteAyarlari.EmailSSL,
            //            siteAyari.SiteAyarlari.EmailPort,
            //            konu: siteAyari.SiteAyarlari.MailKonu,
            //            mailBaslik: siteAyari.SiteAyarlari.MailBaslik,
            //            body,
            //            dosya,
            //            gonderilecekMailler);


            //            jModel.IsSuccess = true;
            //            jmodel.Mesaj = siteAyari?.SiteAyarlari.MailGonderildiMesaji ?? "Başarıyla Gönderildi";
            //            jModel.Display = "block";

            //            jModel.Status = "alert alert-success";

            //            ViewBag.Alert = "block";

            //            Transaction.Complete();

            //            return Json(jModel);



            //        }
            //    }
            //    catch (Exception Hata)
            //    {
            //        jModel.IsSuccess = false;
            //        jmodel.Mesaj = _localizer["strstrFormMesajHata"].Value + Hata.Message;
            //        jModel.Display = "block";
            //        jModel.Status = "alert alert-danger";
            //        return Json(jModel);
            //    }
            //}

            //#endregion

            return View();
        }

  

        [AllowAnonymous]

        public PartialViewResult PopularUrunler(int KategoriId, int page = 1, int pageSize = 16)
        {
            IQueryable<Urunler> query = _context.Urunler.Where(x => x.Vitrin == SayfaDurumlari.Aktif);

            if (KategoriId != 0)
            {
                var urunToKategori = _context.UrunToKategori
                    .Where(x => x.KategoriId == KategoriId)
                    .Select(x => x.UrunId)
                    .ToList();

                query = query.Where(x => urunToKategori.Contains(x.Id));
            }

            int skipCount = (page - 1) * pageSize;

            List<Urunler> urunler = query
                .OrderBy(p => p.Sira)
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            return PartialView("_PopularUrunler", urunler);
        }
        [AllowAnonymous]

        public PartialViewResult IndirimliUrunler(int KategoriId, int page = 1, int pageSize = 16)
        {
            IQueryable<Urunler> query = _context.Urunler.Where(x => x.Vitrin == SayfaDurumlari.Aktif);

            if (KategoriId != 0)
            {
                var urunToKategori = _context.UrunToKategori
                    .Where(x => x.KategoriId == KategoriId)
                    .Select(x => x.UrunId)
                    .ToList();

                query = query.Where(x => urunToKategori.Contains(x.Id));
            }

            int skipCount = (page - 1) * pageSize;

            List<Urunler> urunler = query
                .OrderBy(p => p.Sira)
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            return PartialView("_IndirimliUrunler", urunler);
        }

        [AllowAnonymous]

        public IActionResult SlaytUrunler(int SlaytId)
        {
            var model = _context.UrunToSlayt.Where(x => x.SlaytId == SlaytId).ToList();

            return View(model);
        }

        [AllowAnonymous]

        [HttpGet]
        public IActionResult ProcessImage(
        [FromQuery] string imagePath,
        [FromQuery] int width = 0,
        [FromQuery] int height = 0,
        [FromQuery] string mode = "crop",
        [FromQuery] string background = "ffffff"
    )
        {
            if (!Path.IsPathFullyQualified(imagePath))
            {
                string basePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot");
                imagePath = Path.Combine(basePath, imagePath.TrimStart('/'));
            }

            if (!System.IO.File.Exists(imagePath))
            {
                return BadRequest($"Image not found: {imagePath}");
            }

            var backgroundColor = Color.ParseHex(background);

            using (var image = Image.Load(imagePath))
            {
                var resizeOptions = new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = mode.ToLower() == "pad" ? ResizeMode.Pad : ResizeMode.Crop,
                    PadColor = backgroundColor
                };

                image.Mutate(x => x.Resize(resizeOptions));

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, new PngEncoder());
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms.ToArray(), "image/png");
                }
            }
        }


        [AllowAnonymous]

        public IActionResult CultureManamagent(string culture)
        {

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30)
                });


            //string key = "DilCookie";
            //string value = culture;
            //CookieOptions dilCookie = new CookieOptions();
            //dilCookie.Expires = DateTime.Now.AddYears(30);
            //Response.Cookies.Append(key, value, dilCookie);

            //return LocalRedirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }
        private void PopulateDropdown()
        {

        }
        [AllowAnonymous]

        [Route("/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
