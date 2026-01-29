using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class KasaServis : IKasaServis
    {
        private readonly AppDbContext _context;
        AdresServis _adresServis = null;
        private static IHttpContextAccessor _httpContextAccessor;
        private readonly SepetServis _sepetServis;
        private readonly HelperServis _helperServis;
        private readonly UyelerServis _uyeServis;



        private readonly string entity = "Kasa";

        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;


        [Obsolete]
        public KasaServis(AppDbContext _context, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, SepetServis shoppingCartService, HelperServis helperServis, UyelerServis uyeServis)
        {
            this._context = _context;
            _adresServis = new AdresServis(_context);
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _sepetServis = shoppingCartService;
            _helperServis = helperServis;
            _uyeServis = uyeServis;

        }



        public async Task<List<Siparisler>> PageList()
        {
            return (await _context.Siparisler.ToListAsync());
        }

        [Obsolete]
        public async Task<ResultViewModel> SiparisEkle(SiparisViewModel Model, IList<IFormFile> Files, int? uyeId)
        {

            var result = new ResultViewModel();
            var uye = _context.Users.Where(p => p.Id == Convert.ToInt32(uyeId)).FirstOrDefault();

            var teslimatAdresi = _context.Adresler.Where(x => x.Id == Model.TeslimatAdresId).FirstOrDefault();
            var faturaAdresi = _context.Adresler.Where(x => x.Id == Model.FaturaAdresId).FirstOrDefault();
            var odemeMetodu = _context.OdemeMetodlariTranslate.SingleOrDefault(x => x.OdemeMetodId == Model.SiparisOdemeMetodId && x.Diller.DilKodlari.DilKodu == "tr-TR");
            var kargoMetodu = _context.KargoMetodlariTranslate.SingleOrDefault(x => x.KargoMetodId == Model.SiparisKargoMetodId && x.Diller.DilKodlari.DilKodu == "tr-TR");

            var sartliKargoFiyati = _helperServis.GetAktifKargo((decimal)_sepetServis.SepetGenelToplam(FiyatTipleri.BayiFiyat));

            decimal kargoFiyati = 0;
            if (sartliKargoFiyati.KargoMetodId == (int)EticaretWebCoreEntity.Enums.KargoMetodlari.SartliOdeme)
            {
                kargoFiyati = sartliKargoFiyati.KargoMetodlari.Fiyat;
            }

            var dil = _helperServis.GetCurrentCulture();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    #region Sipariş Ekleme
                    var ipAdresi = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;


                    var siteAyari = _context.SiteAyarlari.Where(p => p.Id == 1).FirstOrDefault();
                    var kur = _context.Kur.Where(p => p.ParaBirimId == siteAyari.ParaBirimId).FirstOrDefault();
                    var adresVarmi = _context.Adresler.Where(p => p.UyeId == Convert.ToInt32(uyeId)).ToList().Count();

                    //int adresid = 0;
                    //if (adresVarmi == 0)
                    //{
                    //    var adres = await _adresServis.EkleGuncelle(Model.AdresEkle, uyeId);
                    //    adresid = Convert.ToInt32(adres.SayfaId);
                    //}

                    if (Model.KasaSiparis.UyeOlmakIstiyorum == true)
                    {
                        BayiOlViewModel uyeOlModel = new BayiOlViewModel();

                        var uyeIlce = _context.Ilceler.Where(x => x.Id == Model.KasaSiparis.TeslimatIlceId).FirstOrDefault();

                        uyeOlModel.Ad = Model.KasaSiparis.TeslimatAd;
                        uyeOlModel.Soyad = Model.KasaSiparis.TeslimatSoyad;
                        uyeOlModel.FirmaAdi = Model.KasaSiparis?.TeslimatFirmaAdi;
                        uyeOlModel.UserName = Replace.UrlSeo(Model.KasaSiparis.TeslimatAd.Replace(" ", "") + Model.KasaSiparis.TeslimatAd.Replace(" ", ""));
                        uyeOlModel.Email = Model.KasaSiparis.TeslimatEmail.Trim();
                        uyeOlModel.Password = Model.KasaSiparis.Sifre;
                        uyeOlModel.PhoneNumber = Model.KasaSiparis.TeslimatTelefon;
                        uyeOlModel.IlceId = uyeIlce.Id;
                        uyeOlModel.Adres = Model.KasaSiparis.TeslimatAdres;
                        uyeOlModel.Uyetipi = UyeTipleri.Bireysel;
                        uyeOlModel.EmailConfirmed = false;
                        uyeOlModel.UyeDurumu = UyeDurumlari.OnayBekliyor;
                        var userVarmi = _context.Users.Where(x => x.UserName == Model.KasaSiparis.TeslimatAd).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (userVarmi != null)
                        {
                            var sonUye = _context.Users.Max(x => x.Id);
                            uye.UserName = uye.UserName + sonUye;
                        }
                        var uyeEkle = await _uyeServis.UyeOl(uyeOlModel, null);

                        if (uyeEkle.Basarilimi == false)
                        {
                            result.Basarilimi = false;
                            result.MesajDurumu = uyeEkle.MesajDurumu;
                            result.Mesaj = uyeEkle.Mesaj;
                            return result;
                        }

                        var uyeteslimailce = _context.Ilceler.Where(x => x.Id == Model.KasaSiparis.TeslimatIlceId).FirstOrDefault();

                        AdresViewModel teslimatAdresModel = new AdresViewModel();
                        teslimatAdresModel.Ad = Model.KasaSiparis.TeslimatAd;
                        teslimatAdresModel.Soyad = Model.KasaSiparis.TeslimatSoyad;
                        teslimatAdresModel.Telefon = Model.KasaSiparis.TeslimatTelefon;
                        teslimatAdresModel.IlceId = uyeteslimailce.Id;
                        teslimatAdresModel.UyeId = (int)uyeEkle.SayfaId;
                        await _adresServis.EkleGuncelle(teslimatAdresModel, Convert.ToInt32(uyeEkle.SayfaId));


                        if (Model.KasaSiparis.FaturaveTeslimatAdresiAynimi == true)
                        {
                            AdresViewModel faturaAdresModel = new AdresViewModel();
                            faturaAdresModel.Ad = Model.KasaSiparis.FaturaAd;
                            faturaAdresModel.Soyad = Model.KasaSiparis.FaturaSoyad;
                            faturaAdresModel.Telefon = Model.KasaSiparis.FaturaTelefon;
                            faturaAdresModel.VergiDairesi = Model.KasaSiparis.VergiDairesi;
                            faturaAdresModel.VergiNumarasi = Model.KasaSiparis.VergiNumarasi;
                            faturaAdresModel.IlceId = uyeteslimailce.Id;
                            faturaAdresModel.UyeId = (int)uyeEkle.SayfaId;
                            await _adresServis.EkleGuncelle(faturaAdresModel, Convert.ToInt32(uyeEkle.SayfaId));
                        }

                    }

                    var siparisDurumId = _context.OdemeMetodlari.Where(p => p.Id == Model.SiparisOdemeMetodId).FirstOrDefault().SiparisDurumId;



                    var teslimatAd = uyeId == 0 ? Model.KasaSiparis.TeslimatAd : teslimatAdresi.Ad;
                    var teslimatSoyad = uyeId == 0 ? Model.KasaSiparis.TeslimatSoyad : teslimatAdresi.Soyad;
                    var teslimatEmail = uyeId == 0 ? Model.KasaSiparis.TeslimatEmail : teslimatAdresi.Uyeler.Email;
                    var teslimatTelefon = uyeId == 0 ? Model.KasaSiparis.TeslimatTelefon : uye.PhoneNumber;
                    var teslimatAdres = uyeId == 0 ? Model.KasaSiparis.TeslimatAdres : teslimatAdresi.Adres;
                    var teslimatFirmaAdi = uyeId == 0 ? Model.KasaSiparis.TeslimatFirmaAdi : teslimatAdresi.FirmaAdi;


                    var teslimatUlkeAdi = (uyeId == 0 ? _context.Ulkeler.Where(x => x.Id == Model.KasaSiparis.TeslimatUlkeId).FirstOrDefault().UlkeAdi : teslimatAdresi.Ilceler.Iller.Ulkeler.UlkeAdi);
                    var teslimatilAdi = (uyeId == 0 ? _context.Iller.Where(x => x.Id == Model.KasaSiparis.TeslimatIlId).FirstOrDefault().IlAdi : teslimatAdresi.Ilceler.Iller.IlAdi);
                    var teslimailceAdi = (uyeId == 0 ? _context.Ilceler.Where(x => x.Id == Model.KasaSiparis.TeslimatIlceId).FirstOrDefault().IlceAdi : teslimatAdresi.Ilceler.IlceAdi);



                    var faturaAd = uyeId == 0 ? (Model.KasaSiparis.FaturaveTeslimatAdresiAynimi == true ? Model.KasaSiparis.FaturaAd : Model.KasaSiparis.TeslimatAd) : faturaAdresi.Ad;
                    var faturaSoyad = uyeId == 0 ? (Model.KasaSiparis.FaturaveTeslimatAdresiAynimi == true ? Model.KasaSiparis.FaturaSoyad : Model.KasaSiparis.TeslimatSoyad) : faturaAdresi.Soyad;
                    var faturaEmail = uyeId == 0 ? (Model.KasaSiparis.FaturaveTeslimatAdresiAynimi == true ? Model.KasaSiparis.FaturaEmail : Model.KasaSiparis.TeslimatEmail) : uye.Email;
                    var faturaTelefon = uyeId == 0 ? (Model.KasaSiparis.FaturaveTeslimatAdresiAynimi == true ? Model.KasaSiparis.FaturaTelefon : Model.KasaSiparis.TeslimatTelefon) : uye.PhoneNumber;
                    var faturaAdres = uyeId == 0 ? (Model.KasaSiparis.FaturaveTeslimatAdresiAynimi == true ? Model.KasaSiparis.FaturaAdres : Model.KasaSiparis.TeslimatAdres) : faturaAdresi.Adres;
                    var faturaFirmaAdi = uyeId == 0 ? (Model.KasaSiparis.FaturaveTeslimatAdresiAynimi == true ? Model.KasaSiparis.FaturaFirmaAdi : "") : faturaAdresi.FirmaAdi;
                    var faturavergiDairesi = uyeId == 0 ? (Model.KasaSiparis.FaturaveTeslimatAdresiAynimi == true ? Model.KasaSiparis.VergiDairesi : "") : faturaAdresi.VergiDairesi;
                    long faturaVergiNo = uyeId == 0 ? (Model.KasaSiparis.FaturaveTeslimatAdresiAynimi == true ? Model.KasaSiparis.VergiNumarasi : 0) : Convert.ToInt64(faturaAdresi.VergiNumarasi);

                    var faturaUlkeAdi = teslimatUlkeAdi;
                    var faturailAdi = teslimatilAdi;
                    var faturailceAdi = teslimailceAdi;
                    if (Model.KasaSiparis.FaturaveTeslimatAdresiAynimi == true)
                    {
                        faturaUlkeAdi = (uyeId == 0 ? _context.Ulkeler.Where(x => x.Id == Model.KasaSiparis.FaturaUlkeId).FirstOrDefault().UlkeAdi : faturaAdresi.Ilceler.Iller.Ulkeler.UlkeAdi);
                        faturailAdi = (uyeId == 0 ? _context.Iller.Where(x => x.Id == Model.KasaSiparis.FaturaIlId).FirstOrDefault().IlAdi : faturaAdresi.Ilceler.Iller.IlAdi); ;
                        faturailceAdi = (uyeId == 0 ? _context.Ilceler.Where(x => x.Id == Model.KasaSiparis.FaturaIlceId).FirstOrDefault().IlceAdi : faturaAdresi.Ilceler.IlceAdi);
                    }

                    //if (uyeId == 0)
                    //{
                    //    if (Model.KasaSiparis.FaturaveTeslimatAdresiAynimi == true)
                    //    {
                    //        faturaAd = Model.KasaSiparis.FaturaAd;
                    //        faturaSoyad = Model.KasaSiparis.FaturaSoyad;
                    //        faturaEmail = Model.KasaSiparis.FaturaEmail;
                    //        faturaTelefon = Model.KasaSiparis.FaturaTelefon;
                    //        faturaAdres = Model.KasaSiparis.FaturaAdres;
                    //        faturavergiDairesi = Model.KasaSiparis.VergiDairesi;
                    //        faturaVergiNo = Model.KasaSiparis.VergiNumarasi;

                    //        faturaUlkeAdi = _context.Ulkeler.Where(x => x.Id == Model.KasaSiparis.FaturaUlkeId).FirstOrDefault().UlkeAdi;
                    //        faturailAdi = _context.Iller.Where(x => x.Id == Model.KasaSiparis.FaturaIlId).FirstOrDefault().IlAdi;
                    //        faturailceAdi = _context.Ilceler.Where(x => x.Id == Model.KasaSiparis.FaturaIlceId).FirstOrDefault().IlceAdi;
                    //    }


                    //    teslimatUlkeAdi = _context.Ulkeler.Where(x => x.Id == Model.KasaSiparis.TeslimatUlkeId).FirstOrDefault().UlkeAdi;
                    //    teslimatilAdi = _context.Iller.Where(x => x.Id == Model.KasaSiparis.TeslimatIlId).FirstOrDefault().IlAdi;
                    //    teslimailceAdi = _context.Ilceler.Where(x => x.Id == Model.KasaSiparis.TeslimatIlceId).FirstOrDefault().IlceAdi;
                    //}

                    //else
                    //{
                    //    teslimatUlkeAdi = teslimatAdresi.Ilceler.Iller.Ulkeler.UlkeAdi;
                    //    teslimatilAdi = teslimatAdresi.Ilceler.Iller.IlAdi;
                    //    teslimailceAdi = teslimatAdresi.Ilceler.IlceAdi;

                    //    if (Model.KasaSiparis.FaturaveTeslimatAdresiAynimi == true)
                    //    {
                    //        faturaAd = faturaAdresi.Ad;
                    //        faturaSoyad = faturaAdresi.Soyad;
                    //        faturaEmail = Model.Email;
                    //        faturaTelefon = faturaAdresi.Telefon;
                    //        faturaAdres = faturaAdresi.FaturaAdres;
                    //        faturavergiDairesi = faturaAdresi.VergiDairesi;
                    //        faturaVergiNo = Convert.ToInt64(faturaAdresi.VergiNumarasi);

                    //        faturaUlkeAdi = faturaAdresi.Ilceler.Iller.Ulkeler.UlkeAdi;
                    //        faturailAdi = faturaAdresi.Ilceler.Iller.IlAdi;
                    //        faturailceAdi = faturaAdresi.Ilceler.IlceAdi;
                    //    }
                    //}

                    //var testlimatAdresi = uyeId == 0 ? Model.KasaSiparis.TeslimatAdres : teslimatAdresi.Adres;
                    //var testlimatTelefon = uyeId == 0 ? Model.KasaSiparis.TeslimatTelefon : teslimatAdresi.Telefon;

                    FiyatTipleri fiyatTipi = FiyatTipleri.ListeFiyat;
                    if (uye.IskontoOrani > 0)
                    {
                        fiyatTipi = FiyatTipleri.BayiFiyat;
                    }

                    var email = uyeId == 0 ? Model.KasaSiparis.TeslimatEmail : teslimatAdresi.Uyeler.Email;
                    var telefon = uyeId == 0 ? Model.KasaSiparis.TeslimatTelefon : teslimatAdresi.Uyeler.PhoneNumber;

                    var genelToplam = (decimal)await _sepetServis.YeniSepetGenelToplamAsync(fiyatTipi, dovizDurum: false, formatted: false);

                    var siparisEkle = new Siparisler()
                    {
                        UyeId = uyeId == 0 ? null : uyeId,
                        TeslimatFirmaAdi = teslimatFirmaAdi,
                        TeslimatAd = teslimatAd,
                        TeslimatSoyad = teslimatSoyad,
                        TeslimatUlke = teslimatUlkeAdi,
                        TeslimatIl = teslimatilAdi,
                        TeslimatIlce = teslimailceAdi,
                        TeslimatAdres = teslimatAdres,
                        SiparisNotu = Model.SiparisNotu,
                        KargoUcreti = kargoFiyati,
                        FaturaFirmaAdi = faturaFirmaAdi,
                        FaturaAd = faturaAd,
                        FaturaSoyad = faturaSoyad,
                        FaturaUlke = faturaUlkeAdi,
                        FaturaIl = faturailAdi,
                        FaturaIlce = faturailceAdi,
                        FaturaAdres = faturaAdres,
                        KargoMetodu = kargoMetodu.KargoAdi,
                        OdemeMetodu = odemeMetodu.OdemeAdi,
                        Telefon = telefon,
                        VergiDairesi = faturavergiDairesi,
                        VergiNumarasi = faturaVergiNo,
                        Email = email,
                        ParaBirimId = siteAyari.ParaBirimId,
                        Kur = kur.TLKur,
                        ToplamFiyat = genelToplam,
                        SiparisTarihi = DateTime.Now,
                        Ip = ipAdresi.ToString(),
                        SiparisDurumu = (int)SiparisDurumTipleri.EksikSiparis
                    };
                    _context.Entry(siparisEkle).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    #endregion



                    #region Sipariş Ürünleri Ekleme
                    var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];

                    var sepetListesi = _context.Sepet.Where(p => p.CookieId == sepetIdCookie).ToList();
                    foreach (var item in sepetListesi)
                    {

                        //var fiyatStr = _helperServis.FormatCurrency(
                        //    item.Urunler.SizeOzelFiyat > 0 ? item.Urunler.SizeOzelFiyat : item.Urunler.ListeFiyat,
                        //    FiyatTipleri.ListeFiyat,
                        //    ParaBirimi.USD.ToString(),
                        //    true
                        //);
                        //decimal fiyat = decimal.Parse(fiyatStr, CultureInfo.InvariantCulture);

                        //decimal kdv = Convert.ToDecimal(
                        //    ((fiyat) * item.Adet)
                        //    * Convert.ToDecimal(item.Urunler.Kdv?.KdvOrani) / 100
                        //);
                        //decimal uruntoplamFiyat =
                        //    ((fiyat) * item.Adet)
                        //    + kdv;



                        var urunFiyat = await _helperServis.GetPriceAsync(item.Urunler.ListeFiyat, fiyatTipi, ParaBirimi.USD, dovizDurum: false, item.UrunId);

                        var kdvToplam = (decimal)await _helperServis.KdvToplamByUrunAsync((int)item.UrunId, fiyatTipi, dovizDurum: false, formatted: false);
                        var urunAraToplam = await _helperServis.GetLineSubtotalAsync(
                           fiyat: item.Urunler.ListeFiyat,
                           fiyatTipi: fiyatTipi,
                           paraBirimi: (ParaBirimi)siteAyari.ParaBirimId,
                           dovizDurum: false,
                           adet: item.Adet,
                           urunId: item.UrunId,
                           roundPerUnit: false
                           );
                        var urungenelToplam = urunAraToplam.Net + kdvToplam;
                        var sepet = new SiparisUrunleri()
                        {
                            SiparisId = siparisEkle.Id,
                            UrunId = item.UrunId,
                            UrunAdi = item.Urunler.UrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == "tr-TR").UrunAdi,
                            UrunKodu = item.Urunler.UrunKodu,
                            Adet = item.Adet,
                            Fiyat = urunFiyat.Net,
                            Kdv = kdvToplam,
                            Toplam = urungenelToplam,

                        };
                        _context.Entry(sepet).State = EntityState.Added;
                        await _context.SaveChangesAsync();

                        if (!string.IsNullOrEmpty(item.UrunSecenek))
                        {
                            Newtonsoft.Json.Linq.JObject urunSecenekleri = Newtonsoft.Json.Linq.JObject.Parse(item.UrunSecenek);
                            var sepeteEklenenSecenekUrunler = urunSecenekleri.Properties().SelectMany(p => p.Value).Select(v => v.ToString()).ToList();

                            if (sepeteEklenenSecenekUrunler != null && sepeteEklenenSecenekUrunler.Any())
                            {
                                var secenekler = _context.UrunSecenekDegerleri
                                                        .Where(u => sepeteEklenenSecenekUrunler.Contains(u.Id.ToString()))
                                                        .ToList();

                                foreach (var urunSecenek in secenekler)
                                {
                                    var urunsecenek = urunSecenek.UrunSecenekleri.UrunSecenekleriTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == dil);
                                    var urunsecenekDeger = urunSecenek?.UrunSecenekDegerleriTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == dil);
                                    if (urunsecenek != null && urunsecenekDeger != null)
                                    {
                                        var sepetUrunSecenek = new SiparisUrunSecenekleri()
                                        {
                                            SiparisId = siparisEkle.Id,
                                            UrunId = item.UrunId,
                                            UrunSecenekId = urunSecenek.UrunSecenekId,
                                            UrunSecenekDegerId = urunSecenek.Id,
                                            SecenekTipi = urunSecenek.UrunSecenekleri.SecenekTipi,
                                            SecenekAdi = urunsecenek.SecenekAdi,
                                            SecenekDegeri = urunsecenekDeger.DegerAdi,
                                        };
                                        _context.Entry(sepetUrunSecenek).State = EntityState.Added;
                                    }
                                }

                                await _context.SaveChangesAsync();
                            }
                        }

                        await _context.SaveChangesAsync();
                    }


                    #endregion


                    #region Kupon İndirimi
                    var kupon = _sepetServis.KuponKaydet(siparisEkle.Id).Result;
                    if (kupon.Basarilimi == false)
                    {
                        result.MesajDurumu = "danger";
                        result.Mesaj = "Kupon Uygulanırken Hata Oluştu.";
                        return result;
                    }


                    #endregion

                    #region Sipariş Geçmişi
                    var siparisGecmisi = new SiparisGecmisleri()
                    {
                        SiparisId = siparisEkle.Id,
                        SiparisDurumId = siparisDurumId,
                        Aciklama = odemeMetodu.Aciklama,
                        EklenmeTarihi = DateTime.Now,
                    };
                    _context.Entry(siparisGecmisi).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    #endregion



                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} ekleme islemi basariyla tamamlanmistir.";
                    result.SayfaId = siparisEkle.Id;

                    transaction.Complete();
                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu." + hata.Message;

            }

            return result;


        }

        public async Task<ResultViewModel> SiparisMailGonder(IList<IFormFile> Files, int faturaAdresiId, int odemeMetoduId, int kargoMetoduId, string teslimatilAdi, string teslimailceAdi, string testlimatAdresi, string testlimatTelefon, string uyeemail, int siparisId)
        {
            #region Mail Gönderimi

            var result = new ResultViewModel();

            try
            {
                var ipAdresi = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;

                var siteAyari = _context.SiteAyarlari.FirstOrDefault();


                var odemeMetodu = _context.OdemeMetodlariTranslate.SingleOrDefault(x => x.OdemeMetodId == odemeMetoduId && x.Diller.DilKodlari.DilKodu == _helperServis.GetCurrentCulture());
                var kargoMetodu = _context.KargoMetodlariTranslate.SingleOrDefault(x => x.KargoMetodId == kargoMetoduId && x.Diller.DilKodlari.DilKodu == _helperServis.GetCurrentCulture());
                var siparis = _context.Siparisler.Where(x => x.Id == siparisId).FirstOrDefault();

                var siparisDurumu = _helperServis.GetSiparisDurumu(siparisId).Result;


                var faturaAdresi = _context.Adresler.Where(x => x.Id == faturaAdresiId).FirstOrDefault();

                // Mail listesi: Key = e-posta, Value = rol
                Dictionary<string, string> gonderilecekMailler = new Dictionary<string, string>();

                // Kullanıcı maili (rol: Bayi)
                if (!string.IsNullOrEmpty(uyeemail) && !gonderilecekMailler.ContainsKey(uyeemail))
                {
                    gonderilecekMailler.Add(uyeemail, RolTipleri.Bayi.ToString());
                }

                // Site ayarından gelen mail(ler) (rol: Administrator)
                if (!string.IsNullOrEmpty(siteAyari?.GonderilecekMail))
                {
                    var adminMailler = siteAyari.GonderilecekMail
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim());

                    foreach (var mail in adminMailler)
                    {
                        if (!string.IsNullOrEmpty(mail) && !gonderilecekMailler.ContainsKey(mail))
                        {
                            gonderilecekMailler.Add(mail, RolTipleri.Administrator.ToString());
                        }
                    }
                }



                // Dosya ekleri
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


                var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];

                var sepetListesi = _context.Sepet.Where(p => p.CookieId == sepetIdCookie).ToList();


                foreach (var emailitem in gonderilecekMailler)
                {
                    var uye = _context.Users.Where(x => x.Email == emailitem.Key).FirstOrDefault();
                    //var emailAdres = "";
                    //if (emailitem.Value == RolTipleri.Uye.ToString())
                    //{
                    //    emailAdres = _context.Users.Where(x => x.Email == emailitem.Key).FirstOrDefault().Email;
                    //}
                    //else
                    //{
                    //    emailAdres = siteAyari.GonderilecekMail;
                    //}

                    var sb = new StringBuilder();

                    foreach (var item in sepetListesi)
                    {
                        var kurCeviri = _helperServis.GetKurTLCeviri(item.Urunler.ListeFiyat, FiyatTipleri.BayiFiyat, ParaBirimi.USD).Result;
                        var urunAraToplam = (decimal)_helperServis.GetUrunAraFiyat((int)item.UrunId, item.Adet);

                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td valign=\"top\" style=\"padding: 0 15px;\">");
                        sb.AppendLine("<h5 style=\"margin-top: 15px;\">" + item.Urunler.UrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == "tr-TR").UrunAdi + "</h5>");
                        sb.AppendLine("</td>");
                        sb.AppendLine("<td valign=\"top\" style=\"padding:015px;\">");
                        sb.AppendLine("<h5 style=\"margin-top: 15px;\">" + item.Urunler.UrunKodu + "</h5>");
                        sb.AppendLine("</td>");
                        sb.AppendLine("<td valign=\"top\" style=\"padding: 0 15px;\">");
                        sb.AppendLine("<h5 style=\"font-size: 14px; color:#444;margin-top:15px;\"><b>" + item.Adet + "</b></h5>");
                        sb.AppendLine("</td>");
                        sb.AppendLine("<td valign=\"top\" style=\"padding: 0 15px;\">");
                        sb.AppendLine("<h5 style=\"font-size: 14px; color:#444;margin-top:15px;\"><b>" + _helperServis.FormatCurrency(urunAraToplam, FiyatTipleri.ListeFiyat, ParaBirimi.USD.ToString()) + "</b></h5>");
                        sb.AppendLine("</td>");
                        sb.AppendLine("<td valign=\"top\" style=\"padding: 0 15px;\">");
                        sb.AppendLine("<h5 style=\"font-size: 14px; color:#444;margin-top:15px;\"><b>" + _helperServis.FormatCurrency(kurCeviri, FiyatTipleri.ListeFiyat, ParaBirimi.TRY.ToString()) + "</b></h5>");
                        sb.AppendLine("</td>");
                        sb.AppendLine("</tr>");

                    }

                    var items = sb.ToString();

                    string body;
                    string hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                    string templatePath;
                    if (emailitem.Key != siteAyari.GonderilecekMail) // Üye için
                    {
                        templatePath = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "MailTemplates", "Uye_Siparis.html");
                    }
                    else // Admin için
                    {
                        templatePath = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "MailTemplates", "Admin_Siparis.html");
                    }

                    using (StreamReader reader = new StreamReader(templatePath))
                    {
                        body = reader.ReadToEnd();
                    }


                    if (emailitem.Value.ToString() == RolTipleri.Bayi.ToString())
                    {
                        var sipariBildirimAciklama = "";
                        if (odemeMetoduId == (int)OdemeMetodTiplieri.BankaHavalesi)
                        {
                            sipariBildirimAciklama = "Ödemeniz onaylandıktan sonra siparişiniz işleme alınacaktır.";
                        }
                        else if (odemeMetoduId == (int)OdemeMetodTiplieri.Paytr)
                        {
                            sipariBildirimAciklama = "Siparişiniz en kısa sürede kargoya verilecektir.";
                        }

                        body = body.Replace("{Uye_Adi}", $"Merhaba {siparis.TeslimatAd} {siparis.TeslimatSoyad}");
                        body = body.Replace("{Siparis_Aciklama}", $"{siteAyari.FirmaAdi} ürünlerine göstermiş olduğunuz ilgiden dolayı teşekkür ederiz. {sipariBildirimAciklama}");
                    }
                    else if (emailitem.Value.ToString() == RolTipleri.Administrator.ToString())
                    {
                        body = body.Replace("{Siparis_Baslik}", "Yeni bir sipariş aldınız.");
                        body = body.Replace("{Firma_Adi}", siteAyari.FirmaAdi);
                        body = body.Replace("{Siparis_Durumu}", $"Sipariş Durumu: {siparisDurumu}");
                    }



                    body = body.Replace("{Siparis_Listesi}", items);
                    body = body.Replace("{Siparis_No}", siparis.Id.ToString());
                    body = body.Replace("{Siparis_Tarihi}", siparis.SiparisTarihi.ToString());
                    body = body.Replace("{Siparis_Aciklama}", siparis.SiparisTarihi.ToString());
                    body = body.Replace("{Odeme_Metodu}", odemeMetodu.OdemeAdi.ToString());
                    body = body.Replace("{Kargo_Metodu}", kargoMetodu.KargoAdi);
                    body = body.Replace("{Email}", uyeemail);
                    body = body.Replace("{Telefon}", testlimatTelefon);
                    body = body.Replace("{Ip_Adresi}", ipAdresi.ToString());
                    body = body.Replace("{Firma_Adi}", siteAyari.FirmaAdi.ToString());
                    body = body.Replace("{Logo}", hostUrl + siteAyari.MailLogo);
                    body = body.Replace("{Host_Url}", hostUrl);

                    //var adres = _context.Adresler.Where(p => p.Id == adresid).FirstOrDefault();
                    var teslimatBilgileri = $"{siparis.TeslimatAd} {siparis.TeslimatSoyad} <br/> " +
                        $"{siparis.TeslimatAdres} {siparis.TeslimatIlce} {siparis.TeslimatIl} <br/> " +
                        $"Telefon: {siparis.Telefon} <br/>" +
                        $"Email: {siparis.Email}";

                    var faturaBilgileri = $"{siparis.FaturaAd} {siparis.FaturaSoyad} <br/>" +
                    $"{(siparis?.FaturaFirmaAdi == null ? "" : siparis.FaturaFirmaAdi + "<br/>")}" +
                    $"{siparis.FaturaAdres} {siparis.FaturaIlce} {siparis.FaturaIl}<br/>" +
                    $"Telefon : {siparis.Telefon} <br/>" +
                    $"Email : {siparis.Email} <br/>" +
                    $"{(siparis?.VergiDairesi == "" ? "" : "Vergi Dairesi : " + siparis.VergiDairesi + "<br/>")}" +
                    $"{(siparis?.VergiNumarasi == 0 ? "" : "Vergi No : " + siparis.VergiNumarasi)}";

                    body = body.Replace("{Kargo_Fiyati}", kargoMetodu.KargoMetodlari.Fiyat.ToString("C2"));

                    body = body.Replace("{Teslimat_Adresi}", teslimatBilgileri);

                    if (!string.IsNullOrEmpty(siparis.FaturaAd))
                    {
                        body = body.Replace("{Fatura_Adresi}", faturaBilgileri);
                    }
                    else
                    {
                        body = body.Replace("{Fatura_Adresi}", teslimatBilgileri);
                    }


                    body = body.Replace("{Ara_Toplam}", (string)_sepetServis.SepetAraToplam(FiyatTipleri.BayiFiyat, true));

                    var geneltoplam = (string)_sepetServis.SepetGenelToplam(FiyatTipleri.BayiFiyat, true);

                    body = body.Replace("{Genel_Toplam}", geneltoplam);


                    string odemeBilgi = string.Empty;

                    if (odemeMetoduId == (int)OdemeMetodTiplieri.BankaHavalesi)
                    {
                        odemeBilgi += "Banka Havalesi/EFT Talimatları <br/><br/>";

                        string odemeaciklama = odemeMetodu.Aciklama;
                        var odemeList = !string.IsNullOrWhiteSpace(odemeaciklama)
                            ? odemeaciklama.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            : Array.Empty<string>(); // Daha temiz bir yol ile boş liste oluşturuldu

                        foreach (var item in odemeList)
                        {
                            odemeBilgi += item + "<br/>"; // Her öğeden sonra satır sonu eklendi
                        }

                        odemeBilgi += "<br/>Siparişi verdikten sonraki 5 iş günü içinde havale/eft yapmadığınız takdirde siparişiniz iptal olur.";
                    }


                    if (odemeMetoduId == (int)OdemeMetodTiplieri.Paytr)
                    {
                        var paytrIframeTransaction = _context.PaytrIframeTransaction.FirstOrDefault(x => x.SiparisId == siparisId);

                        if (paytrIframeTransaction != null)
                        {
                            odemeBilgi += "Ödeme onaylandı.<br/><br/>";
                            odemeBilgi += "PAYTR SİSTEM NOTU <br/>";
                            odemeBilgi += "Ödeme Tutarı : " + paytrIframeTransaction.OdenenTutar.ToString("C2") + "<br/>";
                            odemeBilgi += "Paytr Sipariş No : " + paytrIframeTransaction.MerchantOid;
                        }
                    }

                    body = body.Replace("{Odeme_Bilgi}", odemeBilgi);

                    List<string> emailList = new List<string> { emailitem.Key };

                    MailHelper.HostMailGonder(
                     siteAyari?.EmailAdresi ?? "",
                     siteAyari?.EmailSifre ?? "",
                     siteAyari?.EmailHost ?? "",
                     siteAyari.EmailSSL,
                     siteAyari.EmailPort,
                     konu: "Sipariş No : " + siparisId,
                     mailBaslik: $"{siteAyari.FirmaAdi} Sipariş Bildirimi",
                     body,
                     dosya,
                     emailList
                     );
                }
                result.Basarilimi = true;
                result.MesajDurumu = "success";
                result.Mesaj = "Başarılı";

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu." + hata.Message;
            }
            #endregion

            return await Task.FromResult(result);

        }
    }
}
