using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Vml.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text.Json;
using Azure;
using Org.BouncyCastle.Utilities.Net;

namespace EticaretWebCoreService
{

    public partial class PaytrServis : IPaytrServis
    {
        private readonly AppDbContext _context;
        private UnitOfWork _uow = null;
        AdresServis _adresServis = null;
        private static IHttpContextAccessor _httpContextAccessor;
        private readonly SepetServis _sepetServis;
        private readonly HelperServis _helperServis;
        private readonly UyelerServis _uyeServis;



        private readonly string entity = "Paytr";

        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;


        [Obsolete]
        public PaytrServis(AppDbContext _context, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, SepetServis shoppingCartService, HelperServis helperServis, UyelerServis uyeServis, UnitOfWork uow)
        {
            this._context = _context;
            _uow = uow;
            _adresServis = new AdresServis(_context);
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _sepetServis = shoppingCartService;
            _helperServis = helperServis;
            _uyeServis = uyeServis;
        }

        public async Task<ResultViewModel> UpdatePage(PaytrViewModel Model, string submit)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    #region Sayfa Güncelleme
                    var sayfaGuncelle = _uow.Repository<Paytr>().GetById(Model.Id).Result;
                    sayfaGuncelle.MagazaNo = Model.MagazaNo;
                    sayfaGuncelle.MagazaParola = Model.MagazaParola;
                    sayfaGuncelle.MagazaAnahtar = Model.MagazaAnahtar;
                    sayfaGuncelle.TestModu = Model.TestModu;
                    sayfaGuncelle.TaksitSayisi = Model.TaksitSayisi;
                    sayfaGuncelle.MaksimumTaksitSayisi = Model.MaksimumTaksitSayisi;
                    sayfaGuncelle.DilId = Model.DilId;
                    sayfaGuncelle.ParaBirimId = Model.ParaBirimId;
                    sayfaGuncelle.BasariliSiparisDurumId = Model.BasariliSiparisDurumId;
                    sayfaGuncelle.HataliSiparisDurumId = Model.HataliSiparisDurumId;

                    _uow.Repository<Paytr>().Update(sayfaGuncelle);
                    await _uow.CompleteAsync();
                    #endregion


                    #region Sayfa Butonlari
                    if (submit == "Kaydet")
                    {
                        result.Action = "Index";
                    }
                    if (submit == "KaydetGuncelle")
                    {
                        result.Action = "AddOrUpdate";
                        result.SayfaId = sayfaGuncelle.Id;
                    }

                    #endregion
                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} güncelleme işlemi başarıyla tamamlanmıştır.";


                    transaction.Complete();

                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";
            }

            return result;


        }




        public async Task<ResultViewModel> PaytrIframeTransactionAdd(int SiparisId, string MerchantOid)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var siparis = _context.Siparisler.Find(SiparisId);
                    #region Sayfa Güncelleme
                    var paytrEkle = new PaytrIframeTransaction()
                    {
                        EklemeTarihi = DateTime.Now,
                        GuncellemeTarihi = DateTime.Now,
                        SiparisId = SiparisId,
                        MerchantOid = MerchantOid,
                        Status = "",
                        StatusMessage = "",
                        ToplamFiyat = siparis.ToplamFiyat,
                        IadeEdilenTutar = 0,

                    };
                    _context.Entry(paytrEkle).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    #endregion

                   
                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} güncelleme işlemi başarıyla tamamlanmıştır.";


                    transaction.Complete();

                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";
            }

            return result;


        }

        [Obsolete]
        public async Task<ResultViewModel> PaytrOdeme(SiparisViewModel siparisModel, int siparisId)
        {

            var result = new ResultViewModel
            {
                PaytrModel = new PaytrViewModel() 
            };


            var uyeBilgi = _helperServis.GetUye().Result;

            var uyeid = uyeBilgi != null ? uyeBilgi.Id : 0;

            var model = _context.Paytr.Where(x => x.Id == 1).FirstOrDefault();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var teslimatAdresi = _context.Adresler.Where(x => x.Id == siparisModel.TeslimatAdresId).FirstOrDefault();


                    string aktifDil = _helperServis.GetCurrentCulture().ToString();

                    string hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

                    string urunListe = "";


                    string merchant_id = model.MagazaNo;
                    string merchant_key = model.MagazaParola;
                    string merchant_salt = model.MagazaAnahtar;
                    //
                    // Müşterinizin sitenizde kayıtlı veya form vasıtasıyla aldığınız eposta adresi
                    string emailstr = uyeid == 0 ? siparisModel.KasaSiparis.TeslimatEmail : uyeBilgi.Email;
                    //
                    // Tahsil edilecek tutar. 9.99 için 9.99 * 100 = 999 gönderilmelidir.
                    int payment_amountstr = (int)Math.Floor((decimal)_sepetServis.SepetGenelToplam(FiyatTipleri.BayiFiyat) * 100);
                    var toplamfiyat = payment_amountstr.ToString();

                    //
                    // Sipariş numarası: Her işlemde benzersiz olmalıdır!! Bu bilgi bildirim sayfanıza yapılacak bildirimde geri gönderilir.
                    string merchant_oid = Guid.NewGuid().ToString("N").Substring(0, 10) + "PAYTR"+ siparisId;
                    //
                    // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız ad ve soyad bilgisi
                    string user_namestr = uyeid == 0 ? siparisModel.KasaSiparis.TeslimatAd + " " + siparisModel.KasaSiparis.TeslimatSoyad : teslimatAdresi.Ad + " " + teslimatAdresi.Soyad;


                    //
                    // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız adres bilgisi
                    string user_addressstr = uyeid == 0 ? siparisModel.KasaSiparis.TeslimatAdres : teslimatAdresi.Adres;
                    //
                    // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız telefon bilgisi
                    string user_phonestr = uyeid == 0 ? siparisModel.KasaSiparis.TeslimatTelefon : teslimatAdresi.Telefon;
                    //
                    // Başarılı ödeme sonrası müşterinizin yönlendirileceği sayfa
                    // !!! Bu sayfa siparişi onaylayacağınız sayfa değildir! Yalnızca müşterinizi bilgilendireceğiniz sayfadır!
                    // !!! Siparişi onaylayacağız sayfa "Bildirim URL" sayfasıdır (Bakınız: 2.ADIM Klasörü).
                    string merchant_ok_url = hostUrl + "/Sepet/OdemeSonuc";
                    //
                    // Ödeme sürecinde beklenmedik bir hata oluşması durumunda müşterinizin yönlendirileceği sayfa
                    // !!! Bu sayfa siparişi iptal edeceğiniz sayfa değildir! Yalnızca müşterinizi bilgilendireceğiniz sayfadır!
                    // !!! Siparişi iptal edeceğiniz sayfa "Bildirim URL" sayfasıdır (Bakınız: 2.ADIM Klasörü).
                    string merchant_fail_url = hostUrl + "/Sepet/Kasa";

                    // Test Modunda Dış ip yazıyoruz canlı modda gerçek ipyi alıyoruz (Yazdığım ip herhangi bir sallamasyon ip localde test için yazdım)
                    var ipAdresi =  _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    //var ipAdresi = model.TestModu == "1" ? "111.222.55.666" : _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                    var crt = _sepetServis.GetCart().Result.Count();
                    object[][] user_basket = new object[crt][];
                    int i = 0;
                    foreach (var item in _sepetServis.GetCart().Result)
                    {

                        var urun = item.Urunler.UrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == _helperServis.GetAktifDil().Result.DilKodlari.DilKodu);

                        urunListe = urun.UrunAdi;

                        user_basket[i] = new object[] { urunListe, item.Urunler.ListeFiyat, item.Adet };
                        i++;
                    }


                    // İşlem zaman aşımı süresi - dakika cinsinden
                    string timeout_limit = "30";
                    //
                    // Hata mesajlarının ekrana basılması için entegrasyon ve test sürecinde 1 olarak bırakın. Daha sonra 0 yapabilirsiniz.
                    string debug_on = "1";
                    //
                    // Mağaza canlı modda iken test işlem yapmak için 1 olarak gönderilebilir.
                    string test_mode = model.TestModu;
                    //
                    // Taksit yapılmasını istemiyorsanız, sadece tek çekim sunacaksanız 1 yapın
                    string no_installment = model.TaksitSayisi == TaksitSayilari.TekCekim ? "1" : "0";
                    //
                    // Sayfada görüntülenecek taksit adedini sınırlamak istiyorsanız uygun şekilde değiştirin.
                    // Sıfır (0) gönderilmesi durumunda yürürlükteki en fazla izin verilen taksit geçerli olur.
                    string max_installment = model.MaksimumTaksitSayisi.ToString();
                    //
                    // Para birimi olarak TL, EUR, USD gönderilebilir. USD ve EUR kullanmak için kurumsal@paytr.com 
                    // üzerinden bilgi almanız gerekmektedir. Boş gönderilirse TL geçerli olur.
                    string currency = "TL";
                    //
                    // Türkçe için tr veya İngilizce için en gönderilebilir. Boş gönderilirse tr geçerli olur.
                    string lang = "tr";


                    // Gönderilecek veriler oluşturuluyor
                    NameValueCollection data = new NameValueCollection();
                    data["merchant_id"] = merchant_id;
                    data["user_ip"] = ipAdresi;
                    data["merchant_oid"] = merchant_oid;
                    data["email"] = emailstr;
                    data["payment_amount"] = toplamfiyat;
                    //
                    // Sepet içerği oluşturma fonksiyonu, değiştirilmeden kullanılabilir.
                    string user_basket_json = System.Text.Json.JsonSerializer.Serialize(user_basket);
                    string user_basketstr = Convert.ToBase64String(Encoding.UTF8.GetBytes(user_basket_json));
                    data["user_basket"] = user_basketstr;
                    //
                    // Token oluşturma fonksiyonu, değiştirilmeden kullanılmalıdır.
                    string Birlestir = string.Concat(merchant_id, ipAdresi, merchant_oid, emailstr, toplamfiyat, user_basketstr, no_installment, max_installment, currency, test_mode, merchant_salt);
                    HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchant_key));
                    byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Birlestir));
                    data["paytr_token"] = Convert.ToBase64String(b);
                    //
                    data["debug_on"] = debug_on;
                    data["test_mode"] = test_mode;
                    data["no_installment"] = no_installment;
                    data["max_installment"] = max_installment;
                    data["user_name"] = user_namestr;
                    data["user_address"] = user_addressstr;
                    data["user_phone"] = user_phonestr;
                    data["merchant_ok_url"] = merchant_ok_url;
                    data["merchant_fail_url"] = merchant_fail_url;
                    data["timeout_limit"] = timeout_limit;
                    data["currency"] = currency;
                    data["lang"] = lang;


                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        byte[] paytrresult = client.UploadValues("https://www.paytr.com/odeme/api/get-token", "POST", data);
                        string ResultAuthTicket = Encoding.UTF8.GetString(paytrresult);
                        dynamic json = JValue.Parse(ResultAuthTicket);

                        if (json.status == "success")
                        {
                            result.PaytrModel.IFrameSrc = "https://www.paytr.com/odeme/guvenli/" + json.token;
                            result.PaytrModel.Visible = true;
                            result.PaytrModel.merchant_oid = merchant_oid;
                            result.Basarilimi = true;
                        }
                        else
                        {

                            result.Basarilimi = false;
                            result.MesajDurumu = "danger";
                            result.Mesaj = "Hata Oluştu : " + json.reason;
                        }


                        transaction.Complete();
                    }

                }
            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Genel Bir Hata Oluştu." + hata.Message;

            }

            return result;


        }
    

    }
}
