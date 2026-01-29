using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class AdresServis : IAdresServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Adres";
        public AdresServis(AppDbContext _context)
        {
            this._context = _context;

        }

        public async Task<List<Siparisler>> Listele()
        {
            return (await _context.Siparisler.ToListAsync());
        }

        public async Task<ResultViewModel> EkleGuncelle(AdresViewModel Model, int uyeId)
        {

            var result = new ResultViewModel();

            List<string> ResimDosyaTipleri = new()
                    {
                        "image/jpeg",
                        "image/png",
                        "image/gif",
                        "image/webp",
                        "image/svg+xml",
                        "application/pdf",
                        "application/msword"
                    };

            var uye = _context.Users.Where(x => x.Id == uyeId).FirstOrDefault();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Model.Id == 0)
                    {

                        var ad = Model.AdresKayitTipi == AdresKayitTipleri.UyeOlSayfa ? uye.Ad : Model.AdresEkle.Ad;
                        var soyad = Model.AdresKayitTipi == AdresKayitTipleri.UyeOlSayfa ? uye.Soyad : Model.AdresEkle.Soyad;
                        var adresAdi = Model.AdresKayitTipi == AdresKayitTipleri.UyeOlSayfa ? "" : Model.AdresEkle.AdresAdi;
                        var adres = Model.AdresKayitTipi == AdresKayitTipleri.UyeOlSayfa ? uye.Adres : Model.AdresEkle.Adres;
                        var faturaadres = Model.AdresKayitTipi == AdresKayitTipleri.UyeOlSayfa ? "" : Model.AdresEkle.FaturaAdres;
                        int? ilceId = Model.AdresKayitTipi == AdresKayitTipleri.UyeOlSayfa ? uye.IlceId : Model.AdresEkle.IlceId;
                        var telefon = Model.AdresKayitTipi == AdresKayitTipleri.UyeOlSayfa ? uye.PhoneNumber : Model.AdresEkle.Telefon;
                        var gsm = Model.AdresKayitTipi == AdresKayitTipleri.UyeOlSayfa ? uye.Gsm : Model.AdresEkle.Gsm;
                        var firmaAdi = Model.AdresKayitTipi == AdresKayitTipleri.UyeOlSayfa ? uye.FirmaAdi : Model.AdresEkle.FirmaAdi;
                        FaturaTurleri faturaTuru = Model.AdresKayitTipi == AdresKayitTipleri.UyeOlSayfa ? Model.FaturaTuru : Model.AdresEkle.VergiNumarasi != null ? FaturaTurleri.Kurumsal : FaturaTurleri.Bireysel;
                        var vergiDairesi = Model.AdresKayitTipi == AdresKayitTipleri.UyeOlSayfa ? uye.VergiDairesi : Model.AdresEkle.VergiDairesi;
                        var vergiNo = Model.AdresKayitTipi == AdresKayitTipleri.UyeOlSayfa ? Convert.ToInt64(uye.VergiNumarasi) : Convert.ToInt64(Model.AdresEkle.VergiNumarasi == null ? null : Model.AdresEkle.VergiNumarasi);

                        #region Sayfa Ekleme
                        var adresEkle = new Adresler()
                        {
                            Ad = ad,
                            Soyad = soyad,
                            UyeId = Convert.ToInt32(uyeId),
                            AdresAdi = adresAdi,
                            IlceId = Convert.ToInt32(ilceId),
                            Adres = adres,
                            Telefon = telefon,
                            Gsm = gsm,
                            PostaKodu = "",
                            FirmaAdi = firmaAdi,
                            FaturaTuru = faturaTuru,
                            FaturaAdres = faturaadres,
                            VergiDairesi = vergiDairesi,
                            VergiNumarasi = Convert.ToInt64(vergiNo),
                        };

                        _context.Entry(adresEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();



                        #endregion

                        result.Basarilimi = true;
                        result.MesajDurumu = "alert-success";
                        result.Mesaj = $"{entity} başarıyla eklendi.";
                        result.SayfaId = adresEkle.Id;

                    }
                    else
                    {
                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.Adresler.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.Ad = Model.AdresEkle.Ad;
                        sayfaGuncelle.Soyad = Model.AdresEkle.Soyad;
                        sayfaGuncelle.AdresAdi = Model.AdresEkle.AdresAdi;
                        sayfaGuncelle.IlceId = Model.AdresEkle.IlceId;
                        sayfaGuncelle.Adres = Model.AdresEkle.Adres;
                        sayfaGuncelle.Telefon = Model.AdresEkle.Telefon;
                        sayfaGuncelle.Gsm = Model.AdresEkle.Gsm;
                        sayfaGuncelle.PostaKodu = Model.AdresEkle.PostaKodu;
                        sayfaGuncelle.FirmaAdi = Model.AdresEkle.FirmaAdi;
                        sayfaGuncelle.FaturaTuru = Model.AdresEkle.FaturaTuru;
                        sayfaGuncelle.VergiDairesi = Model.AdresEkle.VergiDairesi;
                        sayfaGuncelle.VergiNumarasi = Model.AdresEkle.VergiNumarasi;
                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        _context.SaveChanges();
                        #endregion

                        result.Basarilimi = true;
                        result.MesajDurumu = "alert-success";
                        result.Mesaj = $"{entity} başarıyla güncellendi.";

                    }


                    transaction.Complete();
                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "alert-danger";
                result.Mesaj = "Hata Oluştu." + hata.Message;
                result.SayfaId = Model.Id;
            }

            return result;


        }



    }
}
