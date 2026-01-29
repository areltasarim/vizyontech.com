using DocumentFormat.OpenXml.Office2010.Excel;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreService.CariOdeme;
using EticaretWebCoreService.OpakOdeme;
using EticaretWebCoreService.ZiraatPay;
using EticaretWebCoreViewModel;
using EticaretWebCoreViewModel.Opak;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;

namespace vizyontech.com.Controllers
{


    public class CariOdemeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UnitOfWork _uow;
        private readonly ZiraatPayServis _ziraatPayServis;
        private readonly ZiraatPaySettings _ziraatPaySetting;
        private readonly HelperServis _helperServis;
        private readonly CariOdemeServis _cariOdemeServis;
        private readonly OpakServis _opakServis;

        public CariOdemeController(AppDbContext _context, HelperServis _helperServis, ZiraatPayServis ziraatPayServis, IOptions<ZiraatPaySettings> options, AppDbContext context, CariOdemeServis cariOdemeServis, UnitOfWork uow, OpakServis opakServis)
        {
            this._helperServis = _helperServis;
            _ziraatPayServis = ziraatPayServis;
            _ziraatPaySetting = options.Value;
            this._context = context;
            _cariOdemeServis = cariOdemeServis;
            _uow = uow;
            _opakServis = opakServis;
        }

        [HttpGet("cari-odeme/{id}")]
        public async Task<IActionResult> CariOdeme(string id)
        {
            string idDecrypt = EncryptionHelper.Decrypt(id);
            int cariId = int.Parse(idDecrypt);

            var uye = await _context.Users
                .Where(p => p.Id == cariId)
                .FirstOrDefaultAsync();

            if (uye == null)
            {
                return NotFound();
            }

            // ViewBag ile şifrelenmiş ID'yi View'a gönderiyoruz
            ViewBag.EncryptedId = id;

            return View(uye); // View içerisine üye de dönebilir
        }


        [HttpPost("cari-odeme/{id}")]
        public async Task<IActionResult> CariOdeme(string id, IFormCollection form)
        {
            var odenenTutar = form["OdenenTutar"];
            var cardOwner = form["cardOwner"];
            var pan = form["pan"];
            var expiryMonth = form["expiryMonth"];
            var expiryYear = form["expiryYear"];
            var cvv = form["cvv"];

            var ziraatPaySonuc = await _cariOdemeServis.OdemeAsync(decimal.Parse(odenenTutar), id);

            if (string.IsNullOrEmpty(ziraatPaySonuc?.Sonuc))
            {
                TempData["BilgiMesaji"] = "ZiraatPay bağlantısı başarısız.";
                return Redirect("cari-odeme/" + id);
            }

            // ZiraatPay ödeme yönlendirmesi için HTML form
            string postUrl = $"{_ziraatPaySetting.Active.ApiUrl}/post/sale3d/{ziraatPaySonuc.Sonuc}";
            string htmlForm = $@"
                <html><body onload='document.forms[0].submit()'>
                <form method='post' action='{postUrl}'>
                    <input type='hidden' name='cardOwner' value='{cardOwner}' />
                    <input type='hidden' name='pan' value='{pan}' />
                    <input type='hidden' name='expiryMonth' value='{expiryMonth}' />
                    <input type='hidden' name='expiryYear' value='{expiryYear}' />
                    <input type='hidden' name='cvv' value='{cvv}' />
                </form></body></html>";

            return Content(htmlForm, "text/html");
        }



        [HttpPost]
        public async Task<IActionResult> CariOdemeZiraatPayCallback([FromForm] IFormCollection formCollection)
        {

            string cariId = Request.Query["cariId"];

            var sessionToken = Request.Form["sessionToken"];
            var responseCode = Request.Form["responseCode"];
            var responseMsg = Request.Form["responseMsg"];
            var merchantPaymentId = Request.Form["merchantPaymentId"];


            string mpId = merchantPaymentId.ToString().Trim();
            var siparis = _context.CariOdeme
                .FirstOrDefault(x => x.ZiraatPaySiparisId == mpId);

            if (responseCode == "00")
            {
              
                siparis.OdemeDurumu = ZiraatPayOdemeDurumu.Basarili;
                _context.Entry(siparis).State = EntityState.Modified;
                _context.SaveChanges();


                var bankaHaraket = new CariBankaHaraketViewModel()
                {
                    UyeId = siparis.UyeId,
                    OdenenTutar = siparis.OdenenTutar,
                };
                await _opakServis.TblBankaHaraketKayitAsync(bankaHaraket);


                if (siparis != null)
                {
                    TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Ödemeniz başarıyla tamamlandı!" });
                    return RedirectToAction("CariOdemeSonuc", "CariOdeme");

                }
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "danger", Text = responseMsg });
                return RedirectToAction("CariOdeme", "CariOdeme", new { Id = EncryptionHelper.Encrypt(siparis.Uye.Id.ToString()) });
            }
            return BadRequest("Geçersiz işlem");
        }


        [HttpGet("cari-odeme-sonuc")]
        public async Task<IActionResult> CariOdemeSonuc()
        {
            return View();
        }

    }
}
