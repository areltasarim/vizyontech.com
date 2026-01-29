using EticaretWebCoreEntity;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Exchange.WebServices.Data;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EticaretWebCoreEntity.Enums;
using System.Globalization;
using OfficeOpenXml.Export.HtmlExport.StyleCollectors.StyleContracts;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using EticaretWebCoreHelper;
using EticaretWebCoreService.OpakOdeme;
using EticaretWebCoreViewModel.Opak;

namespace EticaretWebCoreService.CariOdeme
{

    public partial class CariOdemeServis : ICariOdemeServis
    {
        private readonly AppDbContext _context;
        private readonly HelperServis _helperServis;
        private readonly ZiraatPaySettings _ziraatpatsetting;
        private readonly OpakServis _opakServis;

        private readonly HttpClient _httpClient;
        private readonly SepetServis _sepetServis;
        private static IHttpContextAccessor _httpContextAccessor;

        public CariOdemeServis(AppDbContext _context, IHttpContextAccessor httpContextAccessor, HttpClient httpClient, HelperServis helperServis, IOptions<ZiraatPaySettings> options, OpakServis opakServis)
        {
            _httpClient = httpClient;
            _helperServis = helperServis;
            this._context = _context;
            _httpContextAccessor = httpContextAccessor;
            _ziraatpatsetting = options.Value;
            _opakServis = opakServis;
        }
        public async Task<List<EticaretWebCoreEntity.CariOdeme>> OdemeListesi()
        {
            var model =  _context.CariOdeme.Where(x => x.OdemeDurumu == ZiraatPayOdemeDurumu.Basarili).ToList();
            return model;
        }

        public async Task<ResultViewModel> OdemeAsync(decimal OdenenTutar, string id)
        {
            var result = new ResultViewModel();
            string idDecrypt = EncryptionHelper.Decrypt(id);
            int cariId = int.Parse(idDecrypt);

            var aktif = _ziraatpatsetting.Active;
            var uyeBilgi = _context.Users.Where(x => x.Id == cariId).FirstOrDefault();
            var random = new Random();

            var ipAdresi = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            string hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            string merchant = aktif.Merchant;
            string merchantuser = aktif.MerchantUser;
            string merchantpassword = aktif.MerchantPassword;
            string apiurl = aktif.ApiUrl;

            string siparisId = $"CARIODEME-{DateTime.Now:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";

            var formData = new Dictionary<string, string>
            {
                { "ACTION", "SESSIONTOKEN" },
                { "MERCHANT", merchant },
               { "MERCHANTUSER", merchantuser },
                { "MERCHANTPASSWORD", merchantpassword },
                { "AMOUNT", OdenenTutar.ToString() },
                { "CURRENCY", "TRY" },
                { "MERCHANTPAYMENTID", $"{siparisId}" },
                { "RETURNURL",hostUrl+ "/CariOdeme/CariOdemeZiraatPayCallback" },
                { "CUSTOMER", $"{uyeBilgi.Id}" },
                { "CUSTOMERNAME", $"{uyeBilgi.Ad}" },
                { "CUSTOMEREMAIL", uyeBilgi.Email },
                { "CUSTOMERPHONE", uyeBilgi.Gsm },
                { "SESSIONTYPE", "PAYMENTSESSION" },
                { "BILLTOADDRESSLINE", uyeBilgi.Adres },
                { "BILLTOCITY", uyeBilgi.Ilceler.Iller.IlAdi },
                { "BILLTOPOSTALCODE", "34000" },
                { "BILLTOCOUNTRY", uyeBilgi.Ilceler.Iller.Ulkeler.UlkeAdi },
                { "BILLTOPHONE", uyeBilgi.Gsm},
                { "SHIPTOADDRESSLINE", uyeBilgi.Adres },
                { "SHIPTOCITY", uyeBilgi.Ilceler.Iller.IlAdi },
                { "SHIPTOPOSTALCODE", "34000" },
                { "SHIPTOCOUNTRY", uyeBilgi.Ilceler.Iller.Ulkeler.UlkeAdi },
                { "SHIPTOPHONE", uyeBilgi.Gsm },
                { "HASORDERITEMS", "NO" },
                { "CUSTOMERIP", ipAdresi }
            };

            var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync(apiurl, content);
            var sonuc = await response.Content.ReadAsStringAsync();
            var CariOdemeresult = JsonSerializer.Deserialize<ZiraatSessionTokenResponseViewModel>(sonuc);


            //ziraatpaydeki hata giderildikten sonra siliencek bu kodlar
            //var odemeEkle = new EticaretWebCoreEntity.CariOdeme()
            //{
            //    OdemeTarihi = DateTime.Now,
            //    OdenenTutar = OdenenTutar,
            //    UyeId = cariId,
            //    ZiraatPaySiparisId = $"{siparisId}",
            //};
            //_context.Entry(odemeEkle).State = EntityState.Added;
            //_context.SaveChanges();

            //string mpId = siparisId.ToString().Trim();
            //var siparis = _context.CariOdeme
            //    .FirstOrDefault(x => x.ZiraatPaySiparisId == mpId);
            //siparis.OdemeDurumu = ZiraatPayOdemeDurumu.Basarili;
            //_context.Entry(siparis).State = EntityState.Modified;
            //_context.SaveChanges();


            //var bankaHaraket = new CariBankaHaraketViewModel()
            //{
            //    UyeId = siparis.UyeId,
            //    OdenenTutar = siparis.OdenenTutar,
            //};
            //await _opakServis.TblBankaHaraketKayitAsync(bankaHaraket);
            //var uye = _context.Users.FirstOrDefault(x => x.Id == (int)siparis.UyeId);
            //var mevcutLimit = uye.CariLimit; // null ise 0 olarak kabul et
            //var odenecekTutar = siparis.OdenenTutar; // null kontrolü
            //uye.CariLimit = mevcutLimit - odenecekTutar;
            //_context.Entry(uye).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            //ziraatpaydeki hata giderildikten sonra siliencek bu kodlar

            if (CariOdemeresult.ResponseCode == "00")
            {


                var odemeEkle = new EticaretWebCoreEntity.CariOdeme()
                {
                    OdemeTarihi = DateTime.Now,
                    OdenenTutar = OdenenTutar,
                    UyeId = cariId,
                    ZiraatPaySiparisId = $"{siparisId}",
                };
                _context.Entry(odemeEkle).State = EntityState.Added;
                _context.SaveChanges();

                result.Basarilimi = true;
                result.MesajDurumu = "success";
                result.Sonuc = CariOdemeresult.SessionToken;

            }
            return result;
        }




    }
}
