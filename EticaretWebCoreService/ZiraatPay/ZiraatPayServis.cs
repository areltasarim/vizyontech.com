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
using EticaretWebCoreService.OpakOdeme;
using EticaretWebCoreViewModel.Opak;

namespace EticaretWebCoreService.ZiraatPay
{

    public partial class ZiraatPayServis : IZiraatPayServis
    {
        private readonly AppDbContext _context;
        private readonly HelperServis _helperServis;
        private readonly ZiraatPaySettings _ziraatpatsetting;

        private readonly HttpClient _httpClient;
        private readonly SepetServis _sepetServis;
        private static IHttpContextAccessor _httpContextAccessor;
        private readonly OpakServis _opakServis;

        public ZiraatPayServis(AppDbContext _context, IHttpContextAccessor httpContextAccessor, HttpClient httpClient, SepetServis sepetServis, HelperServis helperServis, IOptions<ZiraatPaySettings> options, OpakServis opakServis)
        {
            _httpClient = httpClient;
            _sepetServis = sepetServis;
            _helperServis = helperServis;
            this._context = _context;
            _httpContextAccessor = httpContextAccessor;
            _ziraatpatsetting = options.Value;
            _opakServis = opakServis;
        }

        public async Task<ResultViewModel> OdemeAsync(SiparisViewModel siparisModel, int siparisId)
        {
            var result = new ResultViewModel();

            var aktif = _ziraatpatsetting.Active;


            var teslimatAdresi = _context.Adresler.Where(x => x.Id == siparisModel.TeslimatAdresId).FirstOrDefault();
            var il = _context.Iller.Where(x => x.Id == siparisModel.TeslimatAdresId).FirstOrDefault();
            var uyeBilgi = _helperServis.GetUye().Result;

            var random = new Random();
            long customerUye = 0;
            var uyeid = uyeBilgi != null ? uyeBilgi.Id : 0;

            customerUye = uyeid == 0
                ? long.Parse($"{DateTime.UtcNow:yyyyMMddHHmmssfff}{random.Next(100, 999)}")
                : uyeid;

            var ipAdresi = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            string hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var sepetGenelToplamKurCeviri = _helperServis
                .GetKurTLCeviri((decimal)_sepetServis.SepetGenelToplam(FiyatTipleri.BayiFiyat), FiyatTipleri.BayiFiyat, ParaBirimi.USD).Result;

            decimal toplamOrderItemsTutar = 0;

            var orderItems = new List<Dictionary<string, object>>();
            foreach (var item in await _sepetServis.GetCart())
            {
                var urun = _helperServis.GetUrun((int)item.UrunId).Result;

                var urunfiyatkurCeviri = _helperServis.GetKurTLCeviri(item.Urunler.ListeFiyat, FiyatTipleri.BayiFiyat, ParaBirimi.USD).Result;

                decimal itemTotal = urunfiyatkurCeviri * item.Adet;
                toplamOrderItemsTutar += itemTotal;

                Console.WriteLine($"Ürün: {urun.UrunAdi}, Adet: {item.Adet}, BirimFiyat: {urunfiyatkurCeviri}, Toplam: {itemTotal}");
                orderItems.Add(new Dictionary<string, object>
                    {

                        { "productCode", urun.Urunler.UrunKodu ?? ""},
                        { "name", urun.UrunAdi },
                        { "description", urun.UrunAdi },
                        { "quantity", item.Adet },
{ "amount", (urunfiyatkurCeviri * item.Adet).ToString("F2").Replace(",",".") }
                    });
            }
            string orderItemsJson = JsonConvert.SerializeObject(orderItems);

            Console.WriteLine($"Sepet Genel Toplam (Kur çevrilmiş): {sepetGenelToplamKurCeviri}");
            Console.WriteLine($"Toplam Order Items Tutarı: {toplamOrderItemsTutar}");
            Console.WriteLine($"Fark: {Math.Abs(toplamOrderItemsTutar - sepetGenelToplamKurCeviri)}");

            string merchant = aktif.Merchant;
            string merchantuser = aktif.MerchantUser;
            string merchantpassword = aktif.MerchantPassword;
            string apiurl = aktif.ApiUrl;


            var toplamStr = toplamOrderItemsTutar.ToString("F2", CultureInfo.InvariantCulture);
            Console.WriteLine("AMOUNT gönderilen değer: " + toplamStr);

            var merchantPaymentId = $"ZIRAATPAY-{siparisId}_{DateTime.Now.Ticks}";
            var formData = new Dictionary<string, string>
            {
                { "ACTION", "SESSIONTOKEN" },
                { "MERCHANT", merchant },
               { "MERCHANTUSER", merchantuser },
                { "MERCHANTPASSWORD", merchantpassword },
                { "AMOUNT", toplamStr },
                { "CURRENCY", "TRY" },
                { "MERCHANTPAYMENTID",  merchantPaymentId },
                { "RETURNURL",hostUrl+ "/Sepet/ZiraatPayCallback" },
                { "CUSTOMER", $"{customerUye}" },
                { "CUSTOMERNAME", uyeid == 0 ? siparisModel.KasaSiparis.TeslimatAd + " " + siparisModel.KasaSiparis.TeslimatSoyad : teslimatAdresi.Ad + " " + teslimatAdresi.Soyad },
                { "CUSTOMEREMAIL", uyeid == 0 ? siparisModel.KasaSiparis.TeslimatEmail : uyeBilgi.Email },
                { "CUSTOMERPHONE", uyeid == 0 ? siparisModel.KasaSiparis.TeslimatTelefon : teslimatAdresi.Telefon },
                { "SESSIONTYPE", "PAYMENTSESSION" },
                { "BILLTOADDRESSLINE", siparisModel.FaturaAdres },
                { "BILLTOCITY", siparisModel.FaturaIl },
                { "BILLTOPOSTALCODE", "34000" },
                { "BILLTOCOUNTRY", siparisModel.FaturaUlke },
                { "BILLTOPHONE", uyeid == 0 ? siparisModel.KasaSiparis.TeslimatTelefon : teslimatAdresi.Telefon},
                { "SHIPTOADDRESSLINE", uyeid == 0 ? siparisModel.KasaSiparis.TeslimatAdres : teslimatAdresi.Adres },
                { "SHIPTOCITY", uyeid == 0 ? il.IlAdi : teslimatAdresi.Ilceler.Iller.IlAdi },
                { "SHIPTOPOSTALCODE", "34000" },
                { "SHIPTOCOUNTRY", siparisModel.TeslimatUlke },
                { "SHIPTOPHONE", siparisModel.Telefon },
                { "ORDERITEMS", orderItemsJson },
                { "HASORDERITEMS", "YES" },
                { "CUSTOMERIP", ipAdresi }
            };

            //ziraatpaydeki hata giderildikten sonra siliencek bu kodlar
            //var siparsiNoGuncelle = _context.Siparisler.Find(siparisId);
            //siparsiNoGuncelle.SiparisNo = merchantPaymentId;
            //_context.Entry(siparsiNoGuncelle).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //_context.SaveChanges();

            //var cariHaraket = new CariHaraketKayitViewModel()
            //{
            //    UyeId = (int)siparsiNoGuncelle.UyeId,
            //    SiparisId = Convert.ToInt32(siparsiNoGuncelle.Id),
            //    Aciklama = "B2B ZiraatPay Kredi Kartı Tahsilat",
            //};
            //var cariKayitEkle = await _opakServis.TblCariHaraketKayitAsync(cariHaraket);
            //ziraatpaydeki hata giderildikten sonra siliencek bu kodlar
            var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync(apiurl, content);
            var sonuc = await response.Content.ReadAsStringAsync();
            var ziraatpayresult = JsonSerializer.Deserialize<ZiraatSessionTokenResponseViewModel>(sonuc);
            if (ziraatpayresult.ResponseCode == "00")
            {

                var siparsiNoGuncelle = _context.Siparisler.Find(siparisId);
                siparsiNoGuncelle.SiparisNo = merchantPaymentId;
                _context.Entry(siparsiNoGuncelle).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                result.Basarilimi = true;
                result.MesajDurumu = "success";
                result.Sonuc = ziraatpayresult.SessionToken;

            }
            return result;
        }


    }
}
