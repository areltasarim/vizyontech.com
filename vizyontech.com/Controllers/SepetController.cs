using EticaretWebCoreEntity;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using EticaretWebCoreService.Sepet;
using EticaretWebCoreService;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using EticaretWebCoreHelper;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Iyzipay;
using Iyzipay.Request;
using Iyzipay.Model;
using EticaretWebCoreEntity.Enums;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.AspNetCore.Identity;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using vizyontech.com.Models;
using DocumentFormat.OpenXml.VariantTypes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OfficeOpenXml.Export.HtmlExport.StyleCollectors.StyleContracts;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using vizyontech.com.Models;
using EticaretWebCoreService.ZiraatPay;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.Options;
using EticaretWebCoreService.OpakOdeme;
using EticaretWebCoreViewModel.Opak;


namespace vizyontech.com.Controllers
{
    [Authorize(Roles = "Bayi", AuthenticationSchemes = "BayiAuth")]

    public class SepetController : Controller
    {
        private readonly AppDbContext _context;
        private readonly OpakDbContext _opakDbContext;

        private readonly UnitOfWork _uow;
        private readonly ILogger<SepetController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SepetServis _sepetServis;
        private readonly AlisverisListemServis _alisverisListemServis;
        private readonly UrunlerServis _urunServis;
        private readonly HelperServis _helperServis;
        private readonly KasaServis _kasaServis;
        private readonly AdresServis _adresServis;
        private readonly PaytrServis _paytrServis;
        private readonly ZiraatPayServis _ziraatPayServis;
        private readonly ZiraatPaySettings _ziraatPaySetting;
        private readonly OpakServis _opakServis;

        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager = null;
        private readonly IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public SepetController(AppDbContext _context, UnitOfWork _uow, UrunlerServis _urunServis, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, AlisverisListemServis _alisverisListemServis, AdresServis _adresServis, SepetServis _sepetServis, KasaServis _kasaServis, PaytrServis _paytrServis, HelperServis _helperServis, ILogger<SepetController> logger, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager, ZiraatPayServis ziraatPayServis, IOptions<ZiraatPaySettings> options, OpakServis opakServis, OpakDbContext opakDbContext)
        {
            this._context = _context;
            this._uow = _uow;
            this._sepetServis = _sepetServis;
            this._kasaServis = _kasaServis;
            this._adresServis = _adresServis;
            this._alisverisListemServis = _alisverisListemServis;
            this._urunServis = _urunServis;
            this._helperServis = _helperServis;
            _httpContextAccessor = httpContextAccessor;
            this._paytrServis = _paytrServis;

            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;

            //_kasaServis = new KasaServis(_context, hostingEnvironment, httpContextAccessor, _sepetServis, _helperServis);
            //_adresServis = new AdresServis(_context);
            _hostingEnvironment = hostingEnvironment;

            _logger = logger;
            _ziraatPayServis = ziraatPayServis;
            _ziraatPaySetting = options.Value;
            _opakServis = opakServis;
            _opakDbContext = opakDbContext;
        }

        [AllowAnonymous]
        public async Task<PartialViewResult> _HeaderSepet()
        {
            var model = await _sepetServis.GetCart();

            return PartialView("~/Views/Sepet/_HeaderSepet.cshtml", model);
        }

        [AllowAnonymous]

        public async Task<PartialViewResult> _Sepet()
        {
            var model = await _sepetServis.GetCart();

            return PartialView("~/Views/Sepet/_Sepet.cshtml", model);
        }

        [AllowAnonymous]
        public async Task<PartialViewResult> _SepetMobil()
        {
            var model = await _sepetServis.GetCart();

            return PartialView("~/Views/Sepet/_SepetMobil.cshtml", model);
        }


        [AllowAnonymous]

        public async Task<PartialViewResult> _SepetUrunSayisi()
        {
            var model = await _sepetServis.GetCart();

            return PartialView("~/Views/Sepet/_SepetUrunSayisi.cshtml", model);
        }

        [AllowAnonymous]

        public async Task<PartialViewResult> _SepetModal(int urunId)
        {

            var urun = await _helperServis.GetUrun(urunId);

            ShoppingCartItem sepetModel = new ShoppingCartItem();
            sepetModel.UrunId = urunId;
            sepetModel.Adet = 1;
            sepetModel.SepetAdetGuncellemeDurum = SepetAdetGuncellemeDurumlari.Arttir;

            await _sepetServis.AddToCart(sepetModel);

            return PartialView("~/Views/Sepet/_SepetModal.cshtml", urun);
        }

        [AllowAnonymous]

        public async Task<PartialViewResult> _SepetGenelToplam()
        {
            var sepetUrunSayisi = await _sepetServis.GetCart();

            return PartialView("~/Views/Sepet/_SepetGenelToplam.cshtml", sepetUrunSayisi.Count());
        }

        [AllowAnonymous]

        public async Task<PartialViewResult> _SepetGenelToplamTekSayfaOdeme()
        {
            var sepetUrunSayisi = await _sepetServis.GetCart();

            return PartialView("~/Views/Sepet/_SepetGenelToplamTekSayfaOdeme.cshtml", sepetUrunSayisi.Count());
        }

        [AllowAnonymous]

        public async Task<PartialViewResult> _AlisverisListeUrunSayisi()
        {
            var model = _alisverisListemServis.GetAlisverisListem();
            int urunSayisi = 0;
            if (model != null)
            {
                urunSayisi = model.UrunIdList.Count();
            }

            return PartialView("~/Views/Sepet/_AlisverisListeUrunSayisi.cshtml", urunSayisi);
        }


        [AllowAnonymous]

        public async Task<PartialViewResult> _AlisverisListemModal(int urunId)
        {

            await _alisverisListemServis.AlisverisListesineEkle(urunId);

            var urun = await _helperServis.GetUrun(urunId);

            return PartialView("~/Views/Sepet/_AlisverisListeModal.cshtml", urun);
        }

        [AllowAnonymous]

        public async Task<PartialViewResult> _BegeniModal(int urunId)
        {

            var result = _urunServis.BegeniEkle(urunId).Result;

            return PartialView("~/Views/Sepet/_BegeniModal.cshtml", result);
        }

        [AllowAnonymous]

        public async Task<IActionResult> AddToCart(ShoppingCartItem Model)
        {
            int uyeid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            Model.UyeId = uyeid;

            var model = await _sepetServis.AddToCart(Model);

            return Json(model);
        }

        [AllowAnonymous]

        public async Task<IActionResult> RemoveCart(int UrunId)
        {
            var model = await _sepetServis.RemoveFromCart(UrunId);

            return Json(model);
        }


        [AllowAnonymous]
        [Route("sepet")]
        public IActionResult Sepet()
        {

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> AlisverisListemeEkle(int UrunId)
        {

            var model = _alisverisListemServis.AlisverisListesineEkle(UrunId).Result;

            return Json(model);
        }

        [AllowAnonymous]
        [Route("kasa")]
        public IActionResult Kasa()
        {
            var uyeId = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (uyeId > 0)
            {
                ViewData["Adresler"] = _context.Adresler.Where(x => x.UyeId == uyeId).AsQueryable().Select(p => new SelectListItem() { Text = p.Adres, Value = p.Id.ToString() }).ToList();
            }


            PopulateDropdown();


            //////ViewBag.AdresEkle_UlkeId = new SelectList(_context.Ulkeler.OrderBy(p => p.UlkeAdi), "Id", "UlkeAdi");
            //ViewBag.IlId = new SelectList(_context.Iller.OrderBy(p => p.IlAdi), "Id", "IlAdi");
            //ViewBag.IlceId = new SelectList(_context.Ilceler.OrderBy(p => p.IlceAdi), "Id", "IlceAdi");

            return View();
        }

        [AllowAnonymous]

        [HttpPost]
        [Obsolete]
        [Route("kasa")]
        public async Task<IActionResult> Kasa(SiparisViewModel Model, IFormCollection form, IList<IFormFile> Files)
        {
            var uye = _helperServis.GetUye().Result;

            int uyeId = 0;

            if (uye != null)
            {
                AppUser user = await _userManager.FindByEmailAsync(uye.Email);

                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Bayi"))
                {
                    uyeId = uye.Id;
                }
            }


            var odemeMetodu = _context.OdemeMetodlari.Where(x => x.Id == Model.SiparisOdemeMetodId).FirstOrDefault();

            TempData["OdemeMetodu"] = odemeMetodu.Id;


            var siparisModel = await _kasaServis.SiparisEkle(Model, Files, uyeId);




            if (siparisModel.Basarilimi == true)
            {
                
                if (odemeMetodu.Id == (int)OdemeMetodTiplieri.Paytr)
                {
                    var paytrSonuc = await _paytrServis.PaytrOdeme(Model, (int)siparisModel.SayfaId);

                    if (paytrSonuc.Basarilimi == true)
                    {

                        await _paytrServis.PaytrIframeTransactionAdd(Convert.ToInt32(siparisModel.SayfaId), paytrSonuc.PaytrModel.merchant_oid);

                        TempData["IFrameSrc"] = paytrSonuc.PaytrModel.IFrameSrc;
                        TempData["Visible"] = paytrSonuc.PaytrModel.Visible;

                    }
                    else
                    {
                        TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = paytrSonuc.MesajDurumu, Text = paytrSonuc.Mesaj });

                        return RedirectToAction("Kasa", "Sepet");
                    }
                }
                else if (odemeMetodu.Id == (int)OdemeMetodTiplieri.ZiraatPay)
                {
                    var ziraatPaySonuc = await _ziraatPayServis.OdemeAsync(Model, (int)siparisModel.SayfaId);

                    if (ziraatPaySonuc.Basarilimi == true)
                    {
                        TempData["ApiUrl"] = $"{_ziraatPaySetting.Active.ApiUrl}/post/sale3d/{ziraatPaySonuc.Sonuc}";
                    }
                    else
                    {
                        TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = ziraatPaySonuc.MesajDurumu, Text = ziraatPaySonuc.Mesaj });
                        return RedirectToAction("Kasa", "Sepet");
                    }

                }

                TempData["SiparisId"] = siparisModel.SayfaId;

                return RedirectToAction("Odeme", "Sepet");
            }
            else
            {
                ViewData["Adresler"] = _context.Adresler.Where(x => x.UyeId == uyeId).AsQueryable().Select(p => new SelectListItem() { Text = p.Adres, Value = p.Id.ToString() }).ToList();

                PopulateDropdown();

                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = siparisModel.MesajDurumu, Text = siparisModel.Mesaj });

                return View();

            }

        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> KuponKullan(string KuponKodu)
        {

            ResultViewModel sonuc = new ResultViewModel();
            try
            {
                var model = await _sepetServis.KuponVarmi(KuponKodu);

                sonuc.Basarilimi = model.Basarilimi;
                sonuc.MesajDurumu = model.MesajDurumu;
                sonuc.Mesaj = model.Mesaj;
                sonuc.Display = model.Display;

                return Json(sonuc);

            }
            catch (Exception hata)
            {
                sonuc.Basarilimi = false;
                sonuc.MesajDurumu = "danger";
                sonuc.Mesaj = "Hata Oluştu";
                return Json(sonuc);
            }
        }

        [AllowAnonymous]
        [Route("odeme")]
        public IActionResult Odeme()
        {
            var tmpodememetodu = TempData["OdemeMetodu"];
            if (tmpodememetodu == null)
            {
                return RedirectToAction("Sepet", "Sepet");
            }
            OdemeMetodTiplieri odemeMetodu = (OdemeMetodTiplieri)TempData["OdemeMetodu"];


            return View(odemeMetodu);
        }

        [HttpPost]

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PaytrCallback([FromForm] IFormCollection formCollection)
        {
            var model = _context.Paytr.Where(x => x.Id == 1).FirstOrDefault();

            string merchant_oid = formCollection["merchant_oid"];
            string status = formCollection["status"];
            string total_amount = formCollection["total_amount"];
            string hash = formCollection["hash"];

            string statusmessage = formCollection["failed_reason_msg"];
            string statusmessagecode = formCollection["failed_reason_code"];

            decimal odenenTutar = decimal.Parse(total_amount) / 100;

            // Hash doğrulama işlemleri
            string Birlestir = string.Concat(merchant_oid, model.MagazaAnahtar, status, total_amount);
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(model.MagazaParola));
            byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Birlestir));
            string token = Convert.ToBase64String(b);


            // Hash kontrolü
            if (hash != token)
            {
                return BadRequest("PAYTR notification failed: bad hash");
            }

            var paytriframeTransaction = _uow.Repository<PaytrIframeTransaction>().GetAll().Result.Where(x => x.MerchantOid == merchant_oid).FirstOrDefault();
            paytriframeTransaction.Status = status;

            // Başarı durumu
            if (status == "success")
            {
                // Veritabanında siparişi güncelle
                // Burada gerekli güncellemeleri yapabilirsiniz

                paytriframeTransaction.StatusMessage = "completed";

                var siparis = _uow.Repository<SiparisGecmisleri>().GetAll().Result.Where(x => x.SiparisId == paytriframeTransaction.SiparisId).OrderBy(x => x.Id).LastOrDefaultAsync().Result;
                siparis.SiparisDurumId = (int)SiparisDurumTipleri.SiparisinizOnaylandi;
                _uow.Repository<SiparisGecmisleri>().Update(siparis);

                paytriframeTransaction.OdenenTutar = odenenTutar;
                _uow.Repository<PaytrIframeTransaction>().Update(paytriframeTransaction);
                await _uow.CompleteAsync();

                return Content("OK");

            }
            else
            {

                paytriframeTransaction.StatusMessage = $"{statusmessagecode} - {statusmessage}";
                _uow.Repository<PaytrIframeTransaction>().Update(paytriframeTransaction);
                await _uow.CompleteAsync();


                return Content("OK");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ZiraatPayCallback([FromForm] IFormCollection formCollection)
        {
            var sessionToken = Request.Form["sessionToken"];
            var responseCode = Request.Form["responseCode"];
            var responseMsg = Request.Form["responseMsg"];
            var merchantPaymentId = Request.Form["merchantPaymentId"];
            if (responseCode == "00")
            {


                var siparisno = merchantPaymentId.ToString();
                var siparis = await _uow.Repository<Siparisler>().GetByFilterAsync(x => x.SiparisNo == siparisno);

                if (siparis != null)
                {
                    var cariHaraket = new CariHaraketKayitViewModel()
                    {
                        UyeId = (int)siparis.UyeId,
                        SiparisId = Convert.ToInt32(siparis.Id),
                        Aciklama = "B2B ZiraatPay Kredi Kartı Tahsilat",
                    };
                    var cariKayitEkle = await _opakServis.TblCariHaraketKayitAsync(cariHaraket);

                    TempData["SiparisId"] = siparis.Id;
                    TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Ödemeniz başarıyla tamamlandı!" });
                    return RedirectToAction("OdemeSonuc", "Sepet");

                }
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "danger", Text = responseMsg });
                return RedirectToAction("Kasa", "Sepet");
            }
            return BadRequest("Geçersiz işlem");
        }

        [AllowAnonymous]
        [Route("odemesonuc")]
        public async Task<IActionResult> OdemeSonuc()
        {


            var siparisId = Convert.ToInt32(TempData["SiparisId"]);
            if (siparisId > 0)
            {

                var siparis = _context.Siparisler.Find(siparisId);

                var odemeMetodu = _context.OdemeMetodlariTranslate.Where(x => x.OdemeAdi == siparis.OdemeMetodu).FirstOrDefault();
                var kargoMetodu = _context.KargoMetodlariTranslate.Where(x => x.KargoAdi == siparis.KargoMetodu).FirstOrDefault();


                if ((OdemeMetodTiplieri)odemeMetodu.OdemeMetodId == OdemeMetodTiplieri.CariHesabimaYaz)
                {
                    var cariHaraket = new CariHaraketKayitViewModel()
                    {
                        UyeId = (int)siparis.UyeId,
                        SiparisId = Convert.ToInt32(siparis.Id),
                        Aciklama = "B2B Cari Hesapdan Ödeme",
                    };
                    var cariKayitEkle = _opakServis.TblCariHaraketKayitAsync(cariHaraket).Result;



                    if (cariKayitEkle.Basarilimi == false)
                    {
                        TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = cariKayitEkle.MesajDurumu, Text = cariKayitEkle.Mesaj });
                        return RedirectToAction("Kasa", "Sepet");
                    }
                }

                var siteAyari = _context.SiteAyarlari.FirstOrDefault();

                await _kasaServis.SiparisMailGonder(null, Convert.ToInt32(siparis.AdresId),
                        odemeMetodu.OdemeMetodId, kargoMetodu.KargoMetodId, siparis.TeslimatIl,
                        siparis.TeslimatIlce, siparis.TeslimatAdres, siparis.Telefon, siparis.Email, siparisId);



                var siparisDurumGuncelle = _uow.Repository<Siparisler>().GetAll().Result.Where(x => x.Id == siparisId).FirstOrDefault();
                siparisDurumGuncelle.SiparisDurumu = odemeMetodu.OdemeMetodlari.SiparisDurumId;
                _uow.Repository<Siparisler>().Update(siparisDurumGuncelle);
                await _uow.CompleteAsync();

                await _sepetServis.ClearCart();
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        private void PopulateDropdown()
        {
            ViewData["Ulkeler"] = _context.Ulkeler.AsQueryable().Select(p => new SelectListItem() { Text = p.UlkeAdi, Value = p.Id.ToString() }).ToList();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
