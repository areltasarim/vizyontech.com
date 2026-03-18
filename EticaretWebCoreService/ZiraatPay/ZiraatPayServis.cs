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
        private readonly LogsServis _logsServis;

        public ZiraatPayServis(AppDbContext _context, IHttpContextAccessor httpContextAccessor, HttpClient httpClient, SepetServis sepetServis, HelperServis helperServis, IOptions<ZiraatPaySettings> options, OpakServis opakServis, LogsServis logsServis)
        {
            _httpClient = httpClient;
            _sepetServis = sepetServis;
            _helperServis = helperServis;
            this._context = _context;
            _httpContextAccessor = httpContextAccessor;
            _ziraatpatsetting = options.Value;
            _opakServis = opakServis;
            _logsServis = logsServis;
        }

        public async Task<ResultViewModel> OdemeAsync(SiparisViewModel siparisModel, int siparisId)
        {
            _logsServis.Bilgi($"ZiraatPay OdemeAsync başlatıldı - SiparisId: {siparisId}", siparisId.ToString());
            
            var result = new ResultViewModel();

            try
            {
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

                var sepetGenelToplam = _sepetServis.YeniSepetGenelToplamAsync(FiyatTipleri.BayiFiyat, dovizDurum: true,
formatted: false,
paraBirimiGosterimi: false).Result;
                
                // Sepet toplamını logla (kur çevirisi öncesi)
                _logsServis.Bilgi($"ZiraatPay sepet toplam (çeviri öncesi) - SepetToplam: {sepetGenelToplam}", siparisId.ToString());
                
                //var sepetGenelToplamKurCeviri = _helperServis
                //    .GetKurTLCeviri(sepetGenelToplam, FiyatTipleri.BayiFiyat, ParaBirimi.USD).Result;

                _logsServis.Bilgi($"ZiraatPay ödeme bilgileri - UyeId: {uyeid}, CustomerUye: {customerUye}, SepetToplam(öncesi): {sepetGenelToplam}, SepetToplam(sonrası): {Convert.ToDecimal(sepetGenelToplam)}, IP: {ipAdresi}", siparisId.ToString());

                decimal toplamOrderItemsTutar = 0;
                var sepetItems = await _sepetServis.GetCart();
                var orderItems = new List<Dictionary<string, object>>();

                // Basit yaklaşım: Tek bir item olarak sepet toplamını gönder
                if (sepetItems.Count == 1)
                {
                    // Tek ürün varsa normal hesaplama
                    var item = sepetItems.First();
                    var urun = _helperServis.GetUrun((int)item.UrunId).Result;
                    
                    toplamOrderItemsTutar = Convert.ToDecimal(sepetGenelToplam);
                    
                    orderItems.Add(new Dictionary<string, object>
                    {
                        { "productCode", urun.Urunler.UrunKodu ?? ""},
                        { "name", urun.UrunAdi },
                        { "description", urun.UrunAdi },
                        { "quantity", item.Adet },
                        { "amount", sepetGenelToplam }
                    });
                }
                else
                {
                    // Birden fazla ürün varsa, hepsini tek item olarak gönder
                    toplamOrderItemsTutar = Convert.ToDecimal(sepetGenelToplam);
                    
                    orderItems.Add(new Dictionary<string, object>
                    {
                        { "productCode", "SEPET"},
                        { "name", "Sepet Toplamı" },
                        { "description", "Sepet Toplamı" },
                        { "quantity", 1 },
                        { "amount", sepetGenelToplam }
                    });
                }
                string orderItemsJson = JsonConvert.SerializeObject(orderItems);

                // Tutarları karşılaştır ve logla
                var fark = Math.Abs(toplamOrderItemsTutar - Convert.ToDecimal(sepetGenelToplam));
                _logsServis.Bilgi($"ZiraatPay tutar karşılaştırması - SepetGenelToplam: {Convert.ToDecimal(sepetGenelToplam)}, OrderItemsToplam: {toplamOrderItemsTutar}, Fark: {fark}", siparisId.ToString());
                _logsServis.Bilgi($"ZiraatPay OrderItems JSON: {orderItemsJson}", siparisId.ToString());

                Console.WriteLine($"Sepet Genel Toplam (Kur çevrilmiş): {Convert.ToDecimal(sepetGenelToplam)}");
                Console.WriteLine($"Toplam Order Items Tutarı: {toplamOrderItemsTutar}");
                Console.WriteLine($"Fark: {fark}");

                string merchant = aktif.Merchant;
                string merchantuser = aktif.MerchantUser;
                string merchantpassword = aktif.MerchantPassword;
                string apiurl = aktif.ApiUrl;

                // Sepet genel toplamını kullan (ZiraatPay tutarları eşleştirmek için)
                var toplamStr = Convert.ToDecimal(sepetGenelToplam).ToString("F2", CultureInfo.InvariantCulture);
                Console.WriteLine("AMOUNT gönderilen değer: " + toplamStr);

                var merchantPaymentId = $"ZIRAATPAY-{siparisId}_{DateTime.Now.Ticks}";
                
                _logsServis.Bilgi($"ZiraatPay API çağrısı hazırlanıyor - MerchantPaymentId: {merchantPaymentId}, Amount: {toplamStr}, ApiUrl: {apiurl}", siparisId.ToString());

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
                    { "HASORDERITEMS", "NO" },
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
                
                _logsServis.Bilgi($"ZiraatPay API yanıtı alındı - StatusCode: {response.StatusCode}, Response: {sonuc}", siparisId.ToString());
                
                var ziraatpayresult = JsonSerializer.Deserialize<ZiraatSessionTokenResponseViewModel>(sonuc);
                if (ziraatpayresult.ResponseCode == "00")
                {
                    _logsServis.Bilgi($"ZiraatPay SessionToken başarılı - SessionToken: {ziraatpayresult.SessionToken}", siparisId.ToString());

                    var siparsiNoGuncelle = _context.Siparisler.Find(siparisId);
                    siparsiNoGuncelle.SiparisNo = merchantPaymentId;
                    _context.Entry(siparsiNoGuncelle).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Sonuc = ziraatpayresult.SessionToken;
                }
                else
                {
                    _logsServis.Bilgi($"ZiraatPay SessionToken başarısız - ResponseCode: {ziraatpayresult.ResponseCode}, ResponseMsg: {ziraatpayresult.ResponseMsg}", siparisId.ToString());
                    
                    result.Basarilimi = false;
                    result.MesajDurumu = "danger";
                    result.Mesaj = ziraatpayresult.ResponseMsg ?? "ZiraatPay ödeme hatası";
                }
            }
            catch (Exception ex)
            {
                _logsServis.Bilgi($"ZiraatPay OdemeAsync Exception - {ex.Message}", siparisId.ToString());
                await _logsServis.Hata(ex);
                
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "ZiraatPay ödeme işleminde hata oluştu: " + ex.Message;
            }

            return result;
        }


    }
}
