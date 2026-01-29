using DocumentFormat.OpenXml.Office2010.Excel;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Exchange.WebServices.Data;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Transactions;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Size = Rotativa.AspNetCore.Options.Size;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator", AuthenticationSchemes = "AdminAuth")]

    public class SiparislerController : Controller
    {
        SiparislerServis _siparisServis = null;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Sipariş Detayı";
        private readonly string entityAltBaslik = "Sipariş Detayı";

        private readonly IHttpContextAccessor _httpContextAccessor;
        [Obsolete]
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public SiparislerController(AppDbContext _context, SiparislerServis _siparisServis, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this._context = _context;
            this._siparisServis = _siparisServis;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _siparisServis.Listele();

            return View(model);
        }

        public async Task<IActionResult> SiparisDetay(int Id)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            PopulateDropdown();

            var model = await _siparisServis.SiparisDetay(Id);

            return View(model);
        }

        public async Task<IActionResult> SiparisDuzenle(int Id)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;


            if (Id > 0)
            {
                SiparisViewModel model = new SiparisViewModel()
                {
                    Siparis = _context.Siparisler.Find(Id),
                };

                PopulateDropdown();

                return View(model);

            }
            PopulateDropdown();

            SiparisViewModel Model = new SiparisViewModel();

            return View(Model);
        }

        [HttpPost]
        public async Task<IActionResult> SiparisDuzenle(SiparisViewModel Model)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _siparisServis.SiparisDuzenle(Model);

            PopulateDropdown();

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction(model.Action, controllerValue, new { Id = model.SayfaId });
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction("Index", controllerValue, new { Id = model.SayfaId });
            }

        }



        public IActionResult _SiparisDurumuGuncelle(int Id = 0)
        {

            PopulateDropdown();

            return PartialView();

        }
        

        public IActionResult SiparisDurumuGuncelle(SiparisGecmisViewModel Model, IFormFile[] Files)
        {

            var model = _siparisServis.SiparisDurumuGuncelle(Model, Files);

            ResultViewModel Sonuc = new ResultViewModel();

            if (model.Result.Basarilimi == true)
            {

                Sonuc.Basarilimi = model.Result.Basarilimi;
                Sonuc.Display = "block";
                Sonuc.MesajDurumu = "success";
                Sonuc.Mesaj = model.Result.Mesaj;
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
        //[HttpPost]
        //public async Task<IActionResult> KargoKoduEkle([FromBody] SiparisGecmisViewModel kargomodel)
        //{
        //    ResultViewModel sonuc = new ResultViewModel();

        //    try
        //    {

        //        var model = _siparisServis.KargoKoduGuncelle(kargomodel.KargoSiparisId, kargomodel.KargoKodu);
        //        sonuc.Basarilimi = true;
        //        sonuc.MesajDurumu = model.Result.Mesaj;
        //        sonuc.Mesaj = model.Result.Mesaj;
        //        return Json(sonuc);


        //    }
        //    catch (Exception hata)
        //    {
        //        sonuc.Basarilimi = false;
        //        sonuc.MesajDurumu = "danger";
        //        sonuc.Mesaj = "Hata Oluştu." + hata.Message;
        //        return Json(sonuc);
        //    }
        //}
        public IActionResult ProformaFatura()
        {
            return new ViewAsPdf("~/Areas/Admin/Views/Siparisler/ProformaFatura.cshtml", viewData: ViewData);
        }
        

        public async Task<IActionResult> ProformaFaturaMailGonder(IFormCollection pdfFormu, int Id, int UyeId)
        {
            ResultViewModel sonuc = new ResultViewModel();


            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var siteAyari = _context.SiteAyarlariTranslate.ToList().Where(p => p.SiteAyarId == 1).FirstOrDefault();


                    var viewAsPdf = new ViewAsPdf("~/Areas/Admin/Views/Siparisler/ProformaFatura.cshtml", viewData: ViewData)
                    {
                        FileName = "ProformaFatura-" + Id + ".pdf",
                        PageSize = Size.A4,
                        PageMargins = { Left = 1, Right = 1 }
                    };

                    var pdfBytes = await viewAsPdf.BuildFile(ControllerContext);


                    string body;
                    body = "Sipariş Bilgileri";

                    List<string> gonderilecekMailler = new List<string>();
                    gonderilecekMailler.Add(siteAyari?.SiteAyarlari.GonderilecekMail ?? "");

                    List<System.Net.Mail.Attachment> dosya = new List<System.Net.Mail.Attachment>();

                    using (var ms = new MemoryStream())
                    {
                        var fileBytes = ms.ToArray();
                        System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(new MemoryStream(pdfBytes), "ProformaFatura-" + Id + ".pdf");
                        dosya.Add(att);
                    }


                    MailHelper.HostMailGonder(
                    siteAyari?.SiteAyarlari.EmailAdresi ?? "",
                    siteAyari?.SiteAyarlari.EmailSifre ?? "",
                    siteAyari?.SiteAyarlari.EmailHost ?? "",
                    siteAyari.SiteAyarlari.EmailSSL,
                    siteAyari.SiteAyarlari.EmailPort,
                    konu: siteAyari.SiteAyarlari.MailKonu,
                    mailBaslik: siteAyari.SiteAyarlari.MailBaslik,
                    body,
                    dosya,
                    gonderilecekMailler);


                    sonuc.Basarilimi = true;
                    sonuc.MesajDurumu = "success";
                    sonuc.Mesaj = "Mail Başarıyla Gönderildi";


                    transaction.Complete();

                    return Json(sonuc);

                }

            }
            catch (Exception hata)
            {
                sonuc.Basarilimi = false;
                sonuc.MesajDurumu = "danger";
                sonuc.Mesaj = "Hata Oluştu." + hata.Message;
                return Json(sonuc);
            }

        }



        public async Task<IActionResult> Delete(SiparisViewModel Model)
        {

            var model = await _siparisServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _siparisServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }



        private void PopulateDropdown()
        {
            ViewData["SiparisDurumlari"] = _context.SiparisDurumlari.ToList().Where(x=> x.Id != (int)SiparisDurumTipleri.EksikSiparis).AsQueryable().Select(p => new SelectListItem() { Text = p.SiparisDurumlariTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").SiparisDurumu, Value = p.Id.ToString() });
        }
    }
}
