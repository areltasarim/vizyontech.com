using Castle.Components.DictionaryAdapter.Xml;
using DocumentFormat.OpenXml.Wordprocessing;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreService.Sepet;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

namespace EticaretWebCoreService
{
    public partial class SepetServis : ISepetServis
    {
        private UnitOfWork _uow = null;
        private readonly AppDbContext _context;
        private readonly HelperServis _helperServis;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public SepetServis(AppDbContext _context, IHttpContextAccessor _httpContextAccessor, HelperServis _helperServis)
        {
            _uow = new UnitOfWork();
            this._context = _context;
            this._httpContextAccessor = _httpContextAccessor;
            this._helperServis = _helperServis;
        }


        public async Task<ResultViewModel> AddToCart(ShoppingCartItem Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {



                    var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
                    CookieOptions cart = new();

                    string key = "SepetCookie";
                    string value = Guid.NewGuid().ToString();
                    if (sepetIdCookie == null)
                    {
                        cart.Expires = DateTime.Now.AddDays(7);
                        _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, cart);

                        sepetIdCookie = value;
                    }

                    Model.SepetCookie = sepetIdCookie;

                    var sepetKontrol = _context.Sepet.SingleOrDefault(p => p.CookieId == Model.SepetCookie && p.UrunId == Model.UrunId);

                    var urun = _context.UrunlerTranslate.Where(p => p.UrunId == Model.UrunId).FirstOrDefault();



                    string urunSecenekDegerler = Model.UrunSecenekleri;
                    Dictionary<object, object> urunsecenekIds = null;
                    string urunSecenekleriJson = null;

                    if (urunSecenekDegerler != null)
                    {
                        Dictionary<object, object> urunsecenekListesi = ConvertToDictionary(urunSecenekDegerler);
                        urunsecenekIds = GetUrunSecenekIds(urunsecenekListesi, _context);
                        urunSecenekleriJson = ConvertToJSON(urunsecenekIds);
                    }

                    if (sepetKontrol == null)
                    {
                        sepetKontrol = new EticaretWebCoreEntity.Sepet()
                        {
                            UyeId = Convert.ToInt32(Model.UyeId) == 0 ? null : Convert.ToInt32(Model.UyeId),
                            UrunId = Model.UrunId,
                            CookieId = Model.SepetCookie,
                            Adet = Model.Adet,
                            UrunSecenek = urunSecenekleriJson,
                            EklenmeTarihi = DateTime.Now,
                        };
                        _context.Sepet.Add(sepetKontrol);

                        if (sepetKontrol.Adet > urun.Urunler.Stok)
                        {
                            result.Basarilimi = false;
                            result.Mesaj = "Stokta Yeterli Ürün Yok";
                            result.Display = "";
                            result.MesajDurumu = "error";
                            return result;
                        }

                        await _context.SaveChangesAsync();

                    }
                    else
                    {

                        if (sepetKontrol.Adet >= urun.Urunler.Stok)
                        {
                            result.Basarilimi = false;
                            result.Mesaj = "Stokta Yeterli Ürün Yok";
                            result.MesajDurumu = "error";
                            result.Display = "";
                            return result;
                        }

                        if (Model.SepetAdetGuncellemeDurum == SepetAdetGuncellemeDurumlari.Arttir)
                        {
                            sepetKontrol.Adet += Model.Adet;
                        }
                        else
                        {
                            if (sepetKontrol.Adet == 0)
                            {
                                result.Basarilimi = false;
                                result.Mesaj = "Sepete Eklenen Ürün Sayısı Sıfır Olamaz";
                                result.Display = "";
                                result.MesajDurumu = "error";
                                return result;
                            }

                            sepetKontrol.Adet--;

                        }
                        await _context.SaveChangesAsync();
                    }


                    result.Basarilimi = true;
                    result.Mesaj = "Başarıyla Sepete Eklendi";
                    result.MesajDurumu = "success";
                    result.Display = "";

                    transaction.Complete();

                }
            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.Mesaj = "Hata Oluştu";
                result.MesajDurumu = "error";
                result.Display = hata.Message;

                return result;
            }

            return result;
        }

        public static Dictionary<object, object> ConvertToDictionary(string urunSecenekDegerler)
        {
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            string[] degerler = urunSecenekDegerler.Split(',');

            foreach (string deger in degerler)
            {
                int degerId;
                if (int.TryParse(deger, out degerId))
                {
                    dictionary[degerId] = degerId;
                }
            }

            return dictionary;
        }



        public static Dictionary<object, object> GetUrunSecenekIds(Dictionary<object, object> urunsecenekListesi, AppDbContext _context)
        {
            Dictionary<object, object> urunsecenekIds = new Dictionary<object, object>();
            var urunsecenek = _context.UrunSecenekDegerleri
                .Where(x => urunsecenekListesi.Values.Contains(x.Id))
                .ToList();

            foreach (var item in urunsecenek)
            {
                urunsecenekIds[item.UrunSecenekId] = item.Id;
            }

            return urunsecenekIds;
        }

        public static string ConvertToJSON(Dictionary<object, object> urunsecenekIds)
        {
            Dictionary<object, List<string>> yeniObj = new Dictionary<object, List<string>>();

            foreach (var item in urunsecenekIds)
            {
                yeniObj[item.Key] = new List<string> { $"{item.Value}" };
            }

            return JsonConvert.SerializeObject(yeniObj);
        }

        public async Task<ResultViewModel> RemoveFromCart(int UrunId)
        {
            var result = new ResultViewModel();

            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await System.Threading.Tasks.Task.Run(async () =>
                    {

                        var model = _context.Sepet.SingleOrDefault(p => p.UrunId == UrunId && p.CookieId == sepetIdCookie);
                        _context.Entry(model).State = EntityState.Deleted;
                        await _context.SaveChangesAsync();

                    });

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Başarıyla Silindi";
                    result.Display = "";

                    transaction.Complete();
                }
            }

            catch (Exception)
            {
                result.Basarilimi = false;

                throw;
            }

            return result;
        }

        public async System.Threading.Tasks.Task ClearCart()
        {
            var result = new ResultViewModel();

            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await System.Threading.Tasks.Task.Run(async () =>
                    {
                        var model = _context.Sepet.Where(p => p.CookieId == sepetIdCookie).ToList();

                        foreach (var item in model)
                        {
                            _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                        }
                        await _context.SaveChangesAsync();

                        _httpContextAccessor.HttpContext.Response.Cookies.Delete("SepetCookie");

                    });

                    transaction.Complete();
                }
            }
            catch
            {
                result.Basarilimi = false;

            }
        }

        public async Task<List<EticaretWebCoreEntity.Sepet>> GetCart()
        {
            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];

            var model = await _context.Sepet.Where(p => p.CookieId == sepetIdCookie).ToListAsync();

            return (model);
        }

        public decimal SepetUrunTutar(int urunId)
        {
            var bayi = _helperServis.GetUye();

            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
            var sepet = _context.Sepet.Where(p => p.CookieId == sepetIdCookie).FirstOrDefault();

            decimal araToplam = _context.Sepet
                .Where(x => x.CookieId == sepetIdCookie && x.UrunId == urunId)
                .Select(x => (x.Urunler.ListeFiyat) * x.Adet)
                .ToList()
                .Sum();
            return araToplam;
        }


        public object SepetAraToplam(FiyatTipleri fiyatTipi, bool formatted = false)
        {
            var bayi = _helperServis.GetUye().Result;

            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
            if (string.IsNullOrEmpty(sepetIdCookie))
                return formatted
                    ? _helperServis.FormatCurrency(0m, FiyatTipleri.ListeFiyat, ParaBirimi.TRY.ToString(), false)
                    : 0m;

            decimal araToplam;

            if (fiyatTipi == FiyatTipleri.BayiFiyat)
            {
                decimal iskontoOrani = (bayi?.IskontoOrani ?? 0m) / 100m;
                decimal faktor = 1m - iskontoOrani;

                araToplam = _context.Sepet
                    .Where(c => c.CookieId == sepetIdCookie)
                    .Select(p => (decimal?)(
                        (p.Urunler.SizeOzelFiyat > 0
                            ? p.Urunler.SizeOzelFiyat
                            : p.Urunler.ListeFiyat * faktor) * p.Adet))
                    .Sum() ?? 0m;
            }
            else if (fiyatTipi == FiyatTipleri.ListeFiyat)
            {
                araToplam = _context.Sepet
                    .Where(c => c.CookieId == sepetIdCookie)
                    .Select(p => (decimal?)(p.Urunler.ListeFiyat * p.Adet))
                    .Sum() ?? 0m;
            }
            else 
            {
               
                araToplam = _context.Sepet
                    .Where(c => c.CookieId == sepetIdCookie)
                    .Select(p => (decimal?)(
                        (p.Urunler.SizeOzelFiyat > 0 ? p.Urunler.SizeOzelFiyat : p.Urunler.ListeFiyat) * p.Adet))
                    .Sum() ?? 0m;
            }

            if (!formatted)
                return araToplam; // düz decimal (TL)

            // Formatlı çıktı (ekranda TL sembolü gösterilecek)
            var siteAyari = _helperServis.GetSiteAyari().Result;

            if (siteAyari.SiteAyarlari.ParaBirimId == (int)ParaBirimi.USD)
            {
                // Her durumda TL gösterecektik: dövizi TL'ye çevir, TRY ile formatla
                var tlTutar = _helperServis.GetKurTLCeviri(araToplam, FiyatTipleri.ListeFiyat, ParaBirimi.USD).Result;
                return _helperServis.FormatCurrency(tlTutar, FiyatTipleri.ListeFiyat, ParaBirimi.TRY.ToString(), false);
            }
            else if (siteAyari.SiteAyarlari.ParaBirimId == (int)ParaBirimi.EUR)
            {
                var tlTutar = _helperServis.GetKurTLCeviri(araToplam, FiyatTipleri.ListeFiyat, ParaBirimi.EUR).Result;
                return _helperServis.FormatCurrency(tlTutar, FiyatTipleri.ListeFiyat, ParaBirimi.TRY.ToString(), false);
            }
            else
            {
                // Zaten TL
                return _helperServis.FormatCurrency(araToplam, FiyatTipleri.ListeFiyat, ParaBirimi.TRY.ToString(), false);
            }
        }

        public object SepetKdvToplam(FiyatTipleri fiyatTipi, bool formatted = false)
        {
            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
            if (string.IsNullOrEmpty(sepetIdCookie))
                return formatted
                    ? _helperServis.FormatCurrency(0m, FiyatTipleri.ListeFiyat, ParaBirimi.TRY.ToString(), false)
                    : 0m;

            var bayi = _helperServis.GetUye().Result;

            decimal kdvToplam;

            if (fiyatTipi == FiyatTipleri.BayiFiyat)
            {
                decimal iskontoOrani = (bayi?.IskontoOrani ?? 0m) / 100m;
                decimal faktor = 1m - iskontoOrani;

                Console.WriteLine($"[DEBUG] Iskonto Oranı: {iskontoOrani}, Faktor: {faktor}");

                kdvToplam = _context.Sepet
                    .Where(c => c.CookieId == sepetIdCookie)
                    .Select(p => new
                    {
                        Adet = p.Adet,
                        ListeFiyat = p.Urunler.ListeFiyat,
                        SizeOzelFiyat = p.Urunler.SizeOzelFiyat,
                        KdvOrani = (decimal)(p.Urunler.Kdv.KdvOrani) / 100m
                    })
                    .AsEnumerable() // Console yazdırmak için memory’e çekiyoruz
                    .Select(p =>
                    {
                        var birimFiyat = (p.SizeOzelFiyat > 0m ? p.SizeOzelFiyat : p.ListeFiyat * faktor);
                        var kdvTutar = (birimFiyat * p.Adet) * p.KdvOrani;

                        Console.WriteLine($"[DEBUG] Adet: {p.Adet}, ListeFiyat: {p.ListeFiyat}, SizeOzelFiyat: {p.SizeOzelFiyat}, KdvOrani: {p.KdvOrani}, BirimFiyat: {birimFiyat}, KdvTutar: {kdvTutar}");

                        return kdvTutar;
                    })
                    .Sum();
            }
            else // FiyatTipleri.ListeFiyat
            {
                kdvToplam = _context.Sepet
                    .Where(c => c.CookieId == sepetIdCookie)
                    .Select(p => new
                    {
                        Adet = p.Adet,
                        ListeFiyat = p.Urunler.ListeFiyat,
                        KdvOrani = (decimal)(p.Urunler.Kdv.KdvOrani) / 100m
                    })
                    .AsEnumerable()
                    .Select(p =>
                    {
                        var kdvTutar = (p.ListeFiyat * p.Adet) * p.KdvOrani;

                        Console.WriteLine($"[DEBUG] Adet: {p.Adet}, ListeFiyat: {p.ListeFiyat}, KdvOrani: {p.KdvOrani}, KdvTutar: {kdvTutar}");

                        return kdvTutar;
                    })
                    .Sum();
            }

            Console.WriteLine($"[DEBUG] KDV Toplam (TL): {kdvToplam}");

            if (!formatted)
                return kdvToplam;

            var siteAyari = _helperServis.GetSiteAyari().Result;

            if (siteAyari.SiteAyarlari.ParaBirimId == (int)ParaBirimi.USD)
            {
                var tlTutar = _helperServis.GetKurTLCeviri(kdvToplam, FiyatTipleri.ListeFiyat, ParaBirimi.USD).Result;
                Console.WriteLine($"[DEBUG] KDV Toplam USD -> TL: {tlTutar}");
                return _helperServis.FormatCurrency(tlTutar, FiyatTipleri.ListeFiyat, ParaBirimi.TRY.ToString(), false);
            }
            else if (siteAyari.SiteAyarlari.ParaBirimId == (int)ParaBirimi.EUR)
            {
                var tlTutar = _helperServis.GetKurTLCeviri(kdvToplam, FiyatTipleri.ListeFiyat, ParaBirimi.EUR).Result;
                Console.WriteLine($"[DEBUG] KDV Toplam EUR -> TL: {tlTutar}");
                return _helperServis.FormatCurrency(tlTutar, FiyatTipleri.ListeFiyat, ParaBirimi.TRY.ToString(), false);
            }
            else
            {
                return _helperServis.FormatCurrency(kdvToplam, FiyatTipleri.ListeFiyat, ParaBirimi.TRY.ToString(), false);
            }
        }


        public object SepetGenelToplam(FiyatTipleri fiyatTipi, bool formatted = false)
        {
            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
            if (string.IsNullOrEmpty(sepetIdCookie))
                return formatted
                    ? _helperServis.FormatCurrency(0m, FiyatTipleri.ListeFiyat, ParaBirimi.TRY.ToString(), false)
                    : 0m;

            var sepet = _context.Sepet.FirstOrDefault(p => p.CookieId == sepetIdCookie);
            var bayi = _helperServis.GetUye().Result;
            var siteAyari = _helperServis.GetSiteAyari().Result;

            // 1) Ara Toplam Hesapla
            decimal araToplam;
            if (fiyatTipi == FiyatTipleri.BayiFiyat)
            {
                decimal iskontoOrani = (bayi?.IskontoOrani ?? 0m) / 100m;
                decimal faktor = 1m - iskontoOrani;

                araToplam = _context.Sepet
                    .Where(c => c.CookieId == sepetIdCookie)
                    .Select(p => (decimal?)(
                        ((p.Urunler.SizeOzelFiyat > 0m)
                            ? p.Urunler.SizeOzelFiyat
                            : p.Urunler.ListeFiyat * faktor) * p.Adet))
                    .Sum() ?? 0m;
            }
            else
            {
                araToplam = _context.Sepet
                    .Where(c => c.CookieId == sepetIdCookie)
                    .Select(p => (decimal?)(p.Urunler.ListeFiyat * p.Adet))
                    .Sum() ?? 0m;
            }

            // 2) Kupon indirimi uygula
            decimal kuponIndirimi = 0m;
            if (sepet != null && !string.IsNullOrEmpty(sepet.KuponKodu))
                kuponIndirimi = Convert.ToDecimal(KuponHesapla().Result.kuponIndirim);

            decimal kuponIndirimliTutar = araToplam - kuponIndirimi;

            // 3) KDV toplamını hesapla (TL)
            decimal kdvToplam = _context.Sepet
                .Where(c => c.CookieId == sepetIdCookie)
                .Select(p => (decimal?)(
                    ((p.Urunler.SizeOzelFiyat > 0m
                        ? p.Urunler.SizeOzelFiyat
                        : p.Urunler.ListeFiyat) * p.Adet)
                    *
                    ((decimal)(p.Urunler.Kdv.KdvOrani) / 100m)
                ))
                .Sum() ?? 0m;

            // 4) Para birimi USD/EUR ise ara toplam ve KDV'yi TL'ye çevir
            if (siteAyari.SiteAyarlari.ParaBirimId == (int)ParaBirimi.USD)
            {
                kuponIndirimliTutar = _helperServis.GetKurTLCeviri(kuponIndirimliTutar, FiyatTipleri.ListeFiyat, ParaBirimi.USD).Result;
                kdvToplam = _helperServis.GetKurTLCeviri(kdvToplam, FiyatTipleri.ListeFiyat, ParaBirimi.USD).Result;
            }
            else if (siteAyari.SiteAyarlari.ParaBirimId == (int)ParaBirimi.EUR)
            {
                kuponIndirimliTutar = _helperServis.GetKurTLCeviri(kuponIndirimliTutar, FiyatTipleri.ListeFiyat, ParaBirimi.EUR).Result;
                kdvToplam = _helperServis.GetKurTLCeviri(kdvToplam, FiyatTipleri.ListeFiyat, ParaBirimi.EUR).Result;
            }

            // 5) Kargo (TL)
            decimal kargo = _helperServis.GetAktifKargo(kuponIndirimliTutar).KargoMetodlari.Fiyat;

            // 6) Genel toplam = (Ara Toplam - Kupon) + KDV + Kargo
            decimal genelToplam = kuponIndirimliTutar + kdvToplam + kargo;

            if (!formatted)
                return genelToplam;

            return _helperServis.FormatCurrency(genelToplam, FiyatTipleri.ListeFiyat, ParaBirimi.TRY.ToString(), false);
        }


        public int SepetUrunSayisi()
        {
            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
            var sepeturunsayisi = _uow.Repository<EticaretWebCoreEntity.Sepet>().GetAll().Result.Where(p => p.CookieId == sepetIdCookie).ToList().Sum(p => p.Adet);

            return sepeturunsayisi;
        }

        public async Task<EticaretWebCoreEntity.Sepet> GetSepetBilgi()
        {
            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];

            var model = await _context.Sepet.Where(p => p.CookieId == sepetIdCookie).FirstOrDefaultAsync();

            return (model);
        }
        public async Task<ResultViewModel> KuponVarmi(string kuponKodu)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var uye = _helperServis.GetUye().Result;
                    int? uyeid = null;
                    if (uye != null)
                    {
                        uyeid = uye.Id;
                    }
                    var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];

                    #region Sayfa Güncelleme
                    var kupon = _context.Kuponlar.Where(x => x.Kod.Trim() == kuponKodu.Trim()).FirstOrDefault();
                    if (kupon != null)
                    {

                        var sepet = _context.Sepet.Where(p => p.CookieId == sepetIdCookie).FirstOrDefault();

                        if (kupon.ToplamTutar != null && Convert.ToDecimal(SepetAraToplam(FiyatTipleri.BayiFiyat)) < kupon.ToplamTutar)
                        {
                            result.Basarilimi = false;
                            result.MesajDurumu = "error";
                            result.Mesaj = "Kupon";
                            result.Display = "Kuponu kullanmak için sepet toplamınızın kuponun toplamından büyük olması gerekmektedir.";
                            return result;
                        }



                        var suankiTarih = DateTime.Now;
                     
                        if (kupon.BaslangicTarihi > suankiTarih)
                        {
                            result.Basarilimi = false;
                            result.MesajDurumu = "error";
                            result.Mesaj = $"Kupon Kodu";
                            result.Display = $"Bu kupon henüz geçerli değil.";
                            return result;
                        }
                        else if (kupon.BitisTarihi < suankiTarih)
                        {
                            result.Basarilimi = false;
                            result.MesajDurumu = "error";
                            result.Mesaj = $"Kupon Kodu";
                            result.Display = $"Bu kuponun geçerlilik süresi dolmuş.";
                            return result;
                        }
                        else
                        {

                            sepet.KuponKodu = kupon.Kod;
                            _context.Entry(sepet).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            result.Basarilimi = true;
                            result.MesajDurumu = "success";
                            result.Mesaj = $"Kupon Kodu.";
                            result.Display = $"Başarıyla Uygulandı.";
                        }
                        
                    }
                    else
                    {

                        result.Basarilimi = false;
                        result.MesajDurumu = "error";
                        result.Mesaj = $"Kupon Kodu";
                        result.Display = $"Kupon Kodu Bulunamamıştır.";
                        return result;
                    }
                    #endregion


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

        public async Task<ResultViewModel> KuponKaydet(int siparisId)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var uye = _helperServis.GetUye().Result;
                    int? uyeid = null;
                    if (uye != null)
                    {
                        uyeid = uye.Id;
                    }

                    #region Sayfa Güncelleme
                    var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
                    var sepet = _context.Sepet.Where(p => p.CookieId == sepetIdCookie).FirstOrDefault();

                    if (!string.IsNullOrEmpty(sepet.KuponKodu))
                    {
                        var kupon = _context.Kuponlar.Where(x => x.Kod.Trim() == sepet.KuponKodu.Trim()).FirstOrDefault();
                        if (kupon != null)
                        {
                            var kuponHesapla = KuponHesapla().Result;

                            var sayfaEkle = new KuponToSiparis()
                            {
                                KuponId = kupon.Id,
                                SiparisId = siparisId,
                                UyeId = uyeid,
                                IndirimTutari = Convert.ToDecimal(kuponHesapla.kuponIndirim)
                            };
                            _context.Entry(sayfaEkle).State = EntityState.Added;
                            await _context.SaveChangesAsync();

            
                        }

                    }
                    #endregion

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"Kupon Başarıyla Kaydedildi.";
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

        public async Task<(decimal kuponIndirim, string kuponIndirimTL)> KuponHesapla()
        {
            try
            {
                var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
                var sepet = _context.Sepet.FirstOrDefault(p => p.CookieId == sepetIdCookie);

                if (sepet == null || string.IsNullOrEmpty(sepet.KuponKodu))
                    return (0, "₺0,00");

                var kupon = _context.Kuponlar.FirstOrDefault(x => x.Kod.Trim() == sepet.KuponKodu.Trim());
                if (kupon == null)
                    return (0, "₺0,00");

                decimal kuponindirimTutari = kupon.Indirim;

                if (kupon.OranTipi == KuponOranTipi.Yuzde)
                {
                    var araToplam = Convert.ToDecimal(SepetAraToplam(FiyatTipleri.BayiFiyat));
                    kuponindirimTutari = araToplam * (kupon.Indirim / 100);
                }

                string kurCeviri = (await _helperServis.GetKurTLCeviri(kuponindirimTutari, FiyatTipleri.BayiFiyat, ParaBirimi.USD)).ToString("C2");

                return (kuponindirimTutari, kurCeviri);
            }
            catch
            {
                return (0, "₺0,00");
            }
        }


        //yeni

        public async Task<object> YeniSepetKdvToplamAsync(
           FiyatTipleri fiyatTipi,
           bool dovizDurum = false,
           bool formatted = false,
           bool paraBirimiGosterimi = true)
        {
            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
            if (string.IsNullOrEmpty(sepetIdCookie))
                return formatted
                    ? await _helperServis.FormatFiyatAsync(0m, fiyatTipi, ParaBirimi.TRY, false, paraBirimiGosterimi)
                    : 0m;

            var bayi = await _helperServis.GetUye();
            decimal iskontoOrani = (bayi?.IskontoOrani ?? 0m) / 100m;
            decimal faktor = 1m - iskontoOrani;

            bool isBayi = fiyatTipi == FiyatTipleri.BayiFiyat;
            bool isSizeOzel = fiyatTipi == FiyatTipleri.SizeOzelFiyat; // göndermeyeceksin ama kalsın
                                                                       // bool isListe = fiyatTipi == FiyatTipleri.ListeFiyat;    // gerek yok

            // KDV Toplam (TL)
            decimal kdvToplamRaw = await _context.Sepet
                .Where(c => c.CookieId == sepetIdCookie)
                .Select(p => (decimal?)(

                    // ► Birim fiyat seçimi
                    (
                        isBayi
                            ? (
                                // Bayi: şartlar sağlanırsa SizeOzel, değilse iskontolu Liste
                                ((p.Urunler.SizeOzelFiyat) > 0m) &&
                                (p.Urunler.OzelFiyatStokSarti > 0) &&
                                (p.Adet >= p.Urunler.OzelFiyatStokSarti)
                                    ? (p.Urunler.SizeOzelFiyat)
                                    : ((p.Urunler.ListeFiyat) * faktor)
                              )
                            : (
                                // Diğer tipler: isSizeOzel ise varsa SizeOzel, yoksa Liste; aksi halde Liste
                                isSizeOzel
                                    ? (((p.Urunler.SizeOzelFiyat) > 0m)
                                        ? (p.Urunler.SizeOzelFiyat)
                                        : (p.Urunler.ListeFiyat))
                                    : (p.Urunler.ListeFiyat)
                              )
                    )
                    * (decimal)p.Adet
                    *
                    // ► KDV oranı
                    (((p.Urunler.Kdv != null ? (decimal?)p.Urunler.Kdv.KdvOrani : (decimal?)0m) ?? 0m) / 100m)

                ))
                .SumAsync() ?? 0m;

            var siteAyari = await _helperServis.GetSiteAyari();
            var sitePB = (ParaBirimi)siteAyari.SiteAyarlari.ParaBirimId;

            // KDV tutarına sadece kur çevirisi uygulansın
            decimal kdvToplam = await _helperServis.ComputeFiyatAsync(
                kdvToplamRaw, FiyatTipleri.ListeFiyat, sitePB, dovizDurum);

            if (!formatted)
                return kdvToplam;

            var gosterimPB = dovizDurum ? ParaBirimi.TRY : sitePB;
            return paraBirimiGosterimi
                ? kdvToplam.ToString("C2", new CultureInfo(ParaBirimiToCulture(gosterimPB)))
                : kdvToplam.ToString("N2", CultureInfo.InvariantCulture);
        }


        public async Task<object> YeniSepetAraToplamAsync(
            FiyatTipleri fiyatTipi,
            bool dovizDurum = false,
            bool formatted = false,
            bool paraBirimiGosterimi = true)
        {
            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
            if (string.IsNullOrEmpty(sepetIdCookie))
                return formatted
                    ? await _helperServis.FormatFiyatAsync(0m, fiyatTipi, ParaBirimi.TRY, false, paraBirimiGosterimi)
                    : 0m;

            var bayi = await _helperServis.GetUye();
            decimal iskontoOrani = (bayi?.IskontoOrani ?? 0m) / 100m;
            decimal faktor = 1m - iskontoOrani;

            bool isBayi = fiyatTipi == FiyatTipleri.BayiFiyat;

            // EF'e çevrilebilir tek ifade:
            decimal araToplamRaw = await _context.Sepet
                .Where(c => c.CookieId == sepetIdCookie)
                .Select(p => (decimal?)(
                    (
                        isBayi
                            ? (
                                // SizeOzelFiyat koşulu:
                                ((p.Urunler.SizeOzelFiyat) > 0m)               // özel fiyat var
                                && (p.Urunler.OzelFiyatStokSarti > 0)                 // stok şartı tanımlı
                                && (p.Adet >= p.Urunler.OzelFiyatStokSarti)           // adet şartı sağlandı
                                    ? (p.Urunler.SizeOzelFiyat)                  // → size özel fiyat
                                    : ((p.Urunler.ListeFiyat) * faktor)          // → bayi iskontolu liste
                              )
                            : (p.Urunler.ListeFiyat)                             // liste fiyatı
                    ) * (decimal)p.Adet
                ))
                .SumAsync() ?? 0m;

            var siteAyari = await _helperServis.GetSiteAyari();
            var sitePB = (ParaBirimi)siteAyari.SiteAyarlari.ParaBirimId;

            decimal araToplam = await _helperServis.ComputeFiyatAsync(
                araToplamRaw, FiyatTipleri.ListeFiyat, sitePB, dovizDurum);

            if (!formatted) return araToplam;

            var gosterimPB = dovizDurum ? ParaBirimi.TRY : sitePB;
            return paraBirimiGosterimi
                ? araToplam.ToString("C2", new CultureInfo(ParaBirimiToCulture(gosterimPB)))
                : araToplam.ToString("N2", CultureInfo.InvariantCulture);
        }



        public async Task<object> YeniSepetKargoAsync(
            FiyatTipleri fiyatTipi,
            bool formatted = false,
            bool paraBirimiGosterimi = true)
        {
            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
            if (string.IsNullOrEmpty(sepetIdCookie))
                return formatted
                    ? await _helperServis.FormatFiyatAsync(0m, fiyatTipi, ParaBirimi.TRY, false, paraBirimiGosterimi)
                    : 0m;

            // 1) Ara toplamı TL olarak al (kargo eşiği TL bazlıdır)
            decimal araToplamTl = (decimal)await YeniSepetAraToplamAsync(
                fiyatTipi: fiyatTipi,
                dovizDurum: true,    // <<< DÜZELTME: her zaman TL
                formatted: false,    // <<< DÜZELTME: ham decimal
                paraBirimiGosterimi: false
            );

            // 2) Kargo fiyatını servis ile al (kuponsuz)
            string kargoStr = _helperServis.GetKargoFiyat(araToplamTl);

            if (!formatted)
            {
                if (kargoStr.Equals("Ücretsiz", StringComparison.OrdinalIgnoreCase))
                    return 0m;

                return decimal.Parse(
                    kargoStr,
                    NumberStyles.Currency,
                    CultureInfo.GetCultureInfo("tr-TR"));
            }

            if (kargoStr.Equals("Ücretsiz", StringComparison.OrdinalIgnoreCase))
                return "Ücretsiz";

            if (!paraBirimiGosterimi)
            {
                decimal kargoDecimal = decimal.Parse(
                    kargoStr,
                    NumberStyles.Currency,
                    CultureInfo.GetCultureInfo("tr-TR"));
                return kargoDecimal.ToString("N2", CultureInfo.InvariantCulture);
            }

            return kargoStr; // Zaten C2 formatında
        }




        public async Task<object> YeniSepetGenelToplamAsync(
            FiyatTipleri fiyatTipi,
            bool dovizDurum = false,
            bool formatted = false,
            bool paraBirimiGosterimi = true)
        {
            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
            if (string.IsNullOrEmpty(sepetIdCookie))
                return formatted
                    ? await _helperServis.FormatFiyatAsync(0m, fiyatTipi, ParaBirimi.TRY, false, paraBirimiGosterimi)
                    : 0m;

            // Site para birimi (USD/EUR/TRY)
            var siteAyari = await _helperServis.GetSiteAyari();
            var sitePB = (ParaBirimi)siteAyari.SiteAyarlari.ParaBirimId;

            // 1) Ara toplam ve KDV — kullanıcı ne istiyorsa o modda (TL istiyorsa TL, döviz istiyorsa döviz)
            decimal araToplam = (decimal)await YeniSepetAraToplamAsync(
                fiyatTipi: fiyatTipi,
                dovizDurum: dovizDurum,
                formatted: false,
                paraBirimiGosterimi: false);

            decimal kdvToplam = (decimal)await YeniSepetKdvToplamAsync(
                fiyatTipi: fiyatTipi,
                dovizDurum: dovizDurum,
                formatted: false,
                paraBirimiGosterimi: false);

            // 2) Kargo için EŞİK hesabı TL’de yapılır → TL bazlı ara toplamı ayrıca al
            decimal araToplamTL_Esik = (decimal)await YeniSepetAraToplamAsync(
                fiyatTipi: fiyatTipi,
                dovizDurum: true,   // TL'ye çevirerek al
                formatted: false,
                paraBirimiGosterimi: false);

            // 3) Kargo fiyatı (string: "Ücretsiz" veya "₺x,xx")
            string kargoStr = _helperServis.GetKargoFiyat(araToplamTL_Esik);

            // 4) Kargo tutarını decimal'e çevir
            decimal kargoTL = 0m;
            if (!kargoStr.Equals("Ücretsiz", StringComparison.OrdinalIgnoreCase))
                kargoTL = decimal.Parse(kargoStr, NumberStyles.Currency, CultureInfo.GetCultureInfo("tr-TR"));

            // 5) Kargonun toplama eklenmesi
            decimal genelToplam;

            if (dovizDurum)
            {
                // Çıktı TL olacak → araToplam & kdv TL'de, kargo TL direkt eklenir
                // NOT: araToplam/kdv yukarıda dovizDurum=true alınsaydı TL olurdu;
                // biz kullanıcının seçimine göre already aldık.
                // Eğer dovizDurum=true ise şu an araToplam/kdv TL'de.
                genelToplam = araToplam + kdvToplam + kargoTL;
            }
            else
            {
                // Çıktı USD/EUR olacak → kargoyu TL→USD/EUR çevir
                if (sitePB == ParaBirimi.TRY)
                {
                    // TRY isteniyorsa (nadiren) kargoyu çevirme
                    genelToplam = araToplam + kdvToplam + kargoTL;
                }
                else
                {
                    // TL→PB (USD/EUR): TCMB BanknoteSelling = TL per 1 PB
                    var kur = await _helperServis.GetKur((int)sitePB);
                    if (kur <= 0m) throw new InvalidOperationException("Kur değeri geçersiz.");
                    decimal kargoInPB = kargoTL / kur; // örn TL→USD

                    genelToplam = araToplam + kdvToplam + kargoInPB;
                }
            }

            if (!formatted)
                return Math.Round(genelToplam, 2, MidpointRounding.AwayFromZero);

            // 6) Formatlı çıktı
            var gosterimPB = dovizDurum ? ParaBirimi.TRY : sitePB;
            return paraBirimiGosterimi
                ? genelToplam.ToString("C2", new CultureInfo(ParaBirimiToCulture(gosterimPB)))
                : genelToplam.ToString("N2", CultureInfo.InvariantCulture);
        }



        public static string ParaBirimiToCulture(ParaBirimi pb)
        {
            return pb switch
            {
                ParaBirimi.TRY => "tr-TR",
                ParaBirimi.USD => "en-US",
                ParaBirimi.EUR => "de-DE",
                _ => CultureInfo.InvariantCulture.Name
            };
        }
    }
}
