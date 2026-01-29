using DocumentFormat.OpenXml.Wordprocessing;
using EticaretWebCoreCaching.Abstraction;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class UrunlerServis : IUrunlerServis
    {
        private readonly AppDbContext _context;
        private UnitOfWork _uow = null;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SeoServis _seoServis;
        private readonly ICacheService _cacheService;

        private readonly string entity = "Ürün";

        public UrunlerServis(UnitOfWork uow, AppDbContext _context, IHttpContextAccessor _httpContextAccessor, SeoServis _seoServis, ICacheService cacheService)
        {
            _uow = uow;
            this._context = _context;
            this._httpContextAccessor = _httpContextAccessor;
            this._seoServis = _seoServis;
            _cacheService = cacheService;
        }

        public async Task<List<Urunler>> PageList()
        {
            return (await _context.Urunler.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(UrunViewModel Model, string submit)
        {

            var result = new ResultViewModel();
            int pageId = 0;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    List<string> ResimDosyaTipleri = new()
                    {
                        "image/jpeg",
                        "image/png",
                        "image/gif",
                        "image/webp",
                        "image/svg+xml"
                    };

                    List<string> UrunDosyaTipleri = new()
                    {
                        "application/pdf",
                        "application/msword"
                    };

                    var db = new AppDbContext();

                    if (Model.Id == 0)
                    {


                        #region Sayfa Ekleme
                        var sayfaEkle = new Urunler()
                        {
                            UrunKodu = Model.Urun.UrunKodu,
                            MarkaId = Model.Urun.MarkaId,
                            DataSheetId = Model.Urun.DataSheetId,
                            StokTipi = Model.Urun.StokTipi,
                            Stok = Model.Urun.Stok,
                            ListeFiyat = Model.Urun.ListeFiyat,
                            Sira = Model.Urun.Sira,
                            Durum = Model.Urun.Durum,
                            Vitrin = Model.Urun.Vitrin,
                            UrunlerTranslate = new List<UrunlerTranslate>(),
                            UrunToKategori = new List<UrunToKategori>(),
                        };

                        #region Breadcrumb Resim
                        if (Model.BreadcrumbImage != null)
                        {

                            var model = DosyaHelper.DosyaYukle(Model.BreadcrumbImage, "Urunler", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim).Result;

                            if (model.Basarilimi == true)
                            {
                                sayfaEkle.BreadcrumbResim = model.Sonuc;
                            }
                            else
                            {
                                result.Basarilimi = model.Basarilimi;
                                result.MesajDurumu = model.MesajDurumu;
                                result.Mesaj = model.Mesaj;

                                return result;
                            }
                        }

                        else
                        {
                            sayfaEkle.BreadcrumbResim = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                        }
                        #endregion


                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        _context.SaveChanges();

                   


                        foreach (var item in Model.Urun.UrunlerTranslate)
                        {
                            var sayfaEkleTranslate = new UrunlerTranslate()
                            {
                                UrunAdi = item.UrunAdi,
                                KisaAciklama = item.KisaAciklama,
                                Ozellik = item.Ozellik,
                                Aciklama = item.Aciklama,
                                Aciklama2 = item.Aciklama2,
                                MetaBaslik = item.MetaBaslik,
                                MetaAnahtar = item.MetaAnahtar,
                                MetaAciklama = item.MetaAciklama,
                                Video = item.Video,
                                DilId = item.DilId
                            };


                            #region Kapak Resmi
                            if (item.SayfaResmi != null)
                            {
                                var model = DosyaHelper.DosyaYukle(item.SayfaResmi, "Urunler", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim).Result;
                                if (model.Basarilimi == true)
                                {
                                    sayfaEkleTranslate.Resim = model.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = model.Basarilimi;
                                    result.MesajDurumu = model.MesajDurumu;
                                    result.Mesaj = model.Mesaj;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaEkleTranslate.Resim = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                            }
                            #endregion

                            #region Dosya
                            if (item.SayfaDosya != null)
                            {

                                var model = DosyaHelper.DosyaYukle(item.SayfaDosya, "Dosyalar", UrunDosyaTipleri, 5242880, DosyaYoluTipleri.Dosya).Result;

                                if (model.Basarilimi == true)
                                {
                                    sayfaEkleTranslate.Dosya = model.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = model.Basarilimi;
                                    result.MesajDurumu = model.MesajDurumu;
                                    result.Mesaj = model.Mesaj;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaEkleTranslate.Dosya = ImageHelper.DosyaYok(DosyaYoluTipleri.Dosya);
                            }
                            #endregion

                            #region Dosya 2
                            if (item.SayfaDosya2 != null)
                            {

                                var model = DosyaHelper.DosyaYukle(item.SayfaDosya2, "Dosyalar", UrunDosyaTipleri, 5242880, DosyaYoluTipleri.Dosya).Result;

                                if (model.Basarilimi == true)
                                {
                                    sayfaEkleTranslate.Dosya2 = model.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = model.Basarilimi;
                                    result.MesajDurumu = model.MesajDurumu;
                                    result.Mesaj = model.Mesaj;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaEkleTranslate.Dosya2 = ImageHelper.DosyaYok(DosyaYoluTipleri.Dosya);
                            }
                            #endregion

                            #region Youtube Resmi
                            if (item.YoutubeSayfaResim != null)
                            {
                                var model = DosyaHelper.DosyaYukle(item.YoutubeSayfaResim, "Urunler", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim).Result;
                                if (model.Basarilimi == true)
                                {
                                    sayfaEkleTranslate.YoutubeResim = model.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = model.Basarilimi;
                                    result.MesajDurumu = model.MesajDurumu;
                                    result.Mesaj = model.Mesaj;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaEkleTranslate.YoutubeResim = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                            }
                            #endregion
                            sayfaEkle.UrunlerTranslate.Add(sayfaEkleTranslate);
                        }

                        _context.SaveChanges();

                        #endregion

                        #region Çoklu Kategori
                        if (Model.SeciliKategoriler != null)
                        {
                            Model.SeciliKategoriler.ToList().ForEach(p =>
                            {
                                var uruntoCategory = new UrunToKategori { KategoriId = p };
                                sayfaEkle.UrunToKategori.Add(uruntoCategory);
                                _context.Entry(uruntoCategory).State = EntityState.Added;

                            });
                            _context.SaveChanges();
                        }
                        #endregion

                        #region Ürün Seçenekleri
                        if (Model.UrunSecenekDegerId != null)
                        {
                            foreach (var urunSecenek in Model.UrunSecenekDegerId)
                            {
                                var urunsecenekdeger = _context.UrunSecenekDegerleriTranslate.Where(x => x.Id == urunSecenek).FirstOrDefault();
                                var urunToUrunSecenek = new UrunToUrunSecenek()
                                {
                                    UrunId = sayfaEkle.Id,
                                    UrunSecenekId = urunsecenekdeger.UrunSecenekDegerleri.UrunSecenekId,
                                    UrunToUrunSecenekToUrunDeger = new List<UrunToUrunSecenekToUrunDeger>(),
                                };
                                _context.Entry(urunToUrunSecenek).State = EntityState.Added;

                            }
                            _context.SaveChanges();


                            foreach (var urunSecenek in Model.UrunSecenekDegerId)
                            {

                                var urunsecenekDeger = _context.UrunSecenekDegerleriTranslate.Where(x => x.Id == urunSecenek).FirstOrDefault();

                                var urunToUrunsecenek = _context.UrunToUrunSecenek.Where(x => x.UrunSecenekId == urunsecenekDeger.UrunSecenekDegerleri.UrunSecenekId).FirstOrDefault();

                                var urunToUrunSecenekToUrunDeger = new UrunToUrunSecenekToUrunDeger()
                                {
                                    UrunToUrunSecenekId = urunToUrunsecenek.Id,
                                    UrunId = sayfaEkle.Id,
                                    UrunSecenekId = urunsecenekDeger.UrunSecenekDegerleri.UrunSecenekId,
                                    UrunSecenekDegerId = urunsecenekDeger.UrunSecenekDegerId,
                                    Adet = 1,
                                    Fiyat = 0.0m,
                                };
                                _context.Entry(urunToUrunSecenekToUrunDeger).State = EntityState.Added;
                            }
                            _context.SaveChanges();

                        }

                        #endregion


                        #region Benzer Ürünler
                        if (Model.SeciliBenzerUrunAutocomplete != null)
                        {
                            foreach (var item in Model.SeciliBenzerUrunAutocomplete)
                            {
                                var uruntoBenzerUrunEkle = new UrunToBenzerUrun()
                                {
                                    UrunId = sayfaEkle.Id,
                                    BenzerUrunId = item
                                };
                                _context.Entry(uruntoBenzerUrunEkle).State = EntityState.Added;
                                _context.SaveChanges();
                            }
                        }
                        #endregion

                        #region Tamamlayıcı Ürünler
                        if (Model.SeciliTamamlayiciUrunAutocomplete != null)
                        {
                            foreach (var item in Model.SeciliTamamlayiciUrunAutocomplete)
                            {
                                var uruntoTamamlayiciUrunEkle = new UrunToTamamlayiciUrun()
                                {
                                    UrunId = sayfaEkle.Id,
                                    TamamlayiciUrunId = item
                                };
                                _context.Entry(uruntoTamamlayiciUrunEkle).State = EntityState.Added;
                                _context.SaveChanges();
                            }
                        }
                        #endregion

                        #region Sayfa Butonlari
                        if (submit == "Kaydet")
                        {
                            result.Action = "Index";
                        }
                        if (submit == "KaydetGuncelle")
                        {
                            result.Action = "AddOrUpdate";
                            result.SayfaId = sayfaEkle.Id;
                        }
                        #endregion

                        #region Seo Url
                        foreach (var urun in Model.Urun.UrunlerTranslate)
                        {
                            var seoUrl = await _seoServis.SeoLinkOlustur(sayfaAdi: urun.UrunAdi, sayfaId: sayfaEkle.Id, entityName: SeoUrlTipleri.Urun, seoTipi: SeoTipleri.Urun, dilId: urun.DilId);

                            if (seoUrl.Basarilimi == false)
                            {
                                return seoUrl;
                            }

                            var menuEkle = await MenuHelper.MenuKaydet(Model.MenuYerleri, sayfaAdi: urun.UrunAdi, sayfaId: sayfaEkle.Id, parentSayfaId: 1, seoTipi: SeoUrlTipleri.Kategori, MenuTipleri.DinamikSayfalar, dilId: urun.DilId);

                        }
                        #endregion


                         _cacheService.RemoveByPattern($"KategoriDetay");
                         _cacheService.RemoveByPattern($"OneCikanUrunResimleri");


                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";

                        pageId = sayfaEkle.Id;
                    }

                    else
                    {
                        #region Sayfa Guncelleme
                        var sayfaGuncelle = _context.Urunler.Where(p => p.Id == Model.Id).FirstOrDefault();
                        pageId = sayfaGuncelle.Id;

                        sayfaGuncelle.UrunKodu = Model.Urun.UrunKodu;
                        sayfaGuncelle.DataSheetId = Model.Urun.DataSheetId;
                        sayfaGuncelle.MarkaId = Model.Urun.MarkaId;
                        sayfaGuncelle.StokTipi = Model.Urun.StokTipi;
                        sayfaGuncelle.Stok = Model.Urun.Stok;
                        sayfaGuncelle.ListeFiyat = Model.Urun.ListeFiyat;
                        sayfaGuncelle.Sira = Model.Urun.Sira;
                        sayfaGuncelle.Durum = Model.Urun.Durum;
                        sayfaGuncelle.Vitrin = Model.Urun.Vitrin;


                        #endregion
                     

                        #region Breadcrumb Resim
                        if (Model.BreadcrumbImage != null)
                        {

                            var model = DosyaHelper.DosyaYukle(Model.BreadcrumbImage, "Urunler", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);

                            if (model.Result.Basarilimi == true)
                            {
                                sayfaGuncelle.BreadcrumbResim = model.Result.Sonuc;
                            }
                            else
                            {
                                result.Basarilimi = result.Basarilimi;
                                result.MesajDurumu = result.MesajDurumu;
                                result.Mesaj = result.Mesaj;

                                return result;
                            }
                        }

                        else
                        {
                            sayfaGuncelle.BreadcrumbResim = new AppDbContext().Urunler.Find(Model.Id).BreadcrumbResim;
                        }
                        #endregion


                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        _context.SaveChanges();


                        foreach (var item in Model.Urun.UrunlerTranslate)
                        {
                            var sayfaGuncelleTranslate = _context.UrunlerTranslate.Find(item.Id);


                            sayfaGuncelleTranslate.UrunAdi = item.UrunAdi;
                            sayfaGuncelleTranslate.KisaAciklama = item.KisaAciklama;
                            sayfaGuncelleTranslate.Ozellik = item.Ozellik;
                            sayfaGuncelleTranslate.Aciklama = item.Aciklama;
                            sayfaGuncelleTranslate.Aciklama2 = item.Aciklama2;
                            sayfaGuncelleTranslate.MetaBaslik = item.MetaBaslik;
                            sayfaGuncelleTranslate.MetaAnahtar = item.MetaAnahtar;
                            sayfaGuncelleTranslate.MetaAciklama = item.MetaAciklama;
                            sayfaGuncelleTranslate.Video = item.Video;
                            sayfaGuncelleTranslate.DilId = item.DilId;
                            


                            #region Kapak Resmi
                            if (item.SayfaResmi != null)
                            {
                                var model = DosyaHelper.DosyaYukle(item.SayfaResmi, "Urunler", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);
                                if (model.Result.Basarilimi == true)
                                {
                                    sayfaGuncelleTranslate.Resim = model.Result.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = result.Basarilimi;
                                    result.MesajDurumu = result.MesajDurumu;
                                    result.Mesaj = result.Mesaj;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaGuncelleTranslate.Resim = new AppDbContext().Urunler.Find(Model.Id).UrunlerTranslate.SingleOrDefault(p => p.DilId == item.DilId)?.Resim;
                            }
                            #endregion

                            #region Dosya
                            if (item.SayfaDosya != null)
                            {

                                var model = DosyaHelper.DosyaYukle(item.SayfaDosya, "Dosyalar", UrunDosyaTipleri, 5242880, DosyaYoluTipleri.Dosya).Result;

                                if (model.Basarilimi == true)
                                {
                                    sayfaGuncelleTranslate.Dosya = model.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = model.Basarilimi;
                                    result.MesajDurumu = model.MesajDurumu;
                                    result.Mesaj = model.Mesaj;
                                    result.SayfaId = Model.Id; 

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaGuncelleTranslate.Dosya = new AppDbContext().Urunler.Find(Model.Id).UrunlerTranslate.SingleOrDefault(p => p.DilId == item.DilId)?.Dosya;
                            }
                            #endregion

                            #region Dosya 2
                            if (item.SayfaDosya2 != null)
                            {

                                var model = DosyaHelper.DosyaYukle(item.SayfaDosya2, "Dosyalar", UrunDosyaTipleri, 5242880, DosyaYoluTipleri.Dosya).Result;

                                if (model.Basarilimi == true)
                                {
                                    sayfaGuncelleTranslate.Dosya2 = model.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = model.Basarilimi;
                                    result.MesajDurumu = model.MesajDurumu;
                                    result.Mesaj = model.Mesaj;
                                    result.SayfaId = Model.Id;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaGuncelleTranslate.Dosya2 = new AppDbContext().Urunler.Find(Model.Id).UrunlerTranslate.SingleOrDefault(p => p.DilId == item.DilId)?.Dosya2;
                            }
                            #endregion

                            #region Youtube Resmi
                            if (item.YoutubeSayfaResim != null)
                            {
                                var model = DosyaHelper.DosyaYukle(item.YoutubeSayfaResim, "Urunler", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim).Result;
                                if (model.Basarilimi == true)
                                {
                                    sayfaGuncelleTranslate.YoutubeResim = model.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = model.Basarilimi;
                                    result.MesajDurumu = model.MesajDurumu;
                                    result.Mesaj = model.Mesaj;
                                    result.SayfaId = Model.Id;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaGuncelleTranslate.YoutubeResim = new AppDbContext().Urunler.Find(Model.Id).UrunlerTranslate.SingleOrDefault(p => p.DilId == item.DilId)?.YoutubeResim;
                            }
                            #endregion

                            _uow.Repository<UrunlerTranslate>().Update(sayfaGuncelleTranslate);
                            await _uow.CompleteAsync();
                        }






                        #region Çoklu Kategori
                        var ekliKategoriler = _context.UrunToKategori
    .Where(p => p.UrunId == Model.Id)
    .ToList();

                        if (Model.SeciliKategoriler != null)
                        {
                            // Yeni kategoriler ekle
                            var eklenecekKategoriler = Model.SeciliKategoriler.Except(ekliKategoriler.Select(x => x.KategoriId)).ToList();
                            eklenecekKategoriler.ForEach(kategoriId =>
                            {
                                var yeniKategori = new UrunToKategori { UrunId = sayfaGuncelle.Id, KategoriId = kategoriId };
                                _context.UrunToKategori.Add(yeniKategori);
                            });

                            // Silinecek kategoriler
                            var silinecekKategoriler = ekliKategoriler.Where(x => !Model.SeciliKategoriler.Contains(x.KategoriId)).ToList();
                            silinecekKategoriler.ForEach(silinecek =>
                            {
                                _context.UrunToKategori.Remove(silinecek);
                            });
                        }
                        else
                        {
                            // Hiçbir kategori seçili değilse, tüm ilişkileri kaldır
                            _context.UrunToKategori.RemoveRange(ekliKategoriler);
                        }

                        #endregion




                        #region Ürün Seçenekleri
                        if (Model.UrunSecenekDegerId != null)
                        {
                            // Önce mevcut verileri sil
                            db.UrunToUrunSecenekToUrunDeger.RemoveRange(db.UrunToUrunSecenekToUrunDeger.Where(p => p.UrunId == sayfaGuncelle.Id));
                            db.UrunToUrunSecenek.RemoveRange(db.UrunToUrunSecenek.Where(p => p.UrunId == sayfaGuncelle.Id));
                            await db.SaveChangesAsync();

                            // Şimdi yeni verileri ekle
                            foreach (var urunSecenek in Model.UrunSecenekDegerId)
                            {
                                var urunsecenekdeger = _context.UrunSecenekDegerleriTranslate.FirstOrDefault(x => x.Id == urunSecenek);
                                if (urunsecenekdeger != null)
                                {
                                    var urunToUrunSecenek = new UrunToUrunSecenek()
                                    {
                                        UrunId = sayfaGuncelle.Id,
                                        UrunSecenekId = urunsecenekdeger.UrunSecenekDegerleri.UrunSecenekId,
                                    };
                                    _context.UrunToUrunSecenek.Add(urunToUrunSecenek);
                                    await _context.SaveChangesAsync();


                                    var urunToUrunSecenekToUrunDeger = new UrunToUrunSecenekToUrunDeger()
                                    {
                                        UrunId = sayfaGuncelle.Id,
                                        UrunSecenekId = urunsecenekdeger.UrunSecenekDegerleri.UrunSecenekId,
                                        UrunSecenekDegerId = urunsecenekdeger.UrunSecenekDegerId,
                                        UrunToUrunSecenekId = urunToUrunSecenek.Id,
                                        Adet = 1,
                                        Fiyat = 0.0m,
                                    };
                                    _context.UrunToUrunSecenekToUrunDeger.Add(urunToUrunSecenekToUrunDeger);
                                }
                            }

                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            db.UrunToUrunSecenekToUrunDeger.RemoveRange(db.UrunToUrunSecenekToUrunDeger.Where(p => p.UrunId == sayfaGuncelle.Id));
                            db.UrunToUrunSecenek.RemoveRange(db.UrunToUrunSecenek.Where(p => p.UrunId == sayfaGuncelle.Id));
                            await db.SaveChangesAsync();
                        }
                        #endregion


                        #region Benzer Ürünler
                        if (Model.SeciliBenzerUrunAutocomplete != null)
                        {
                            db.UrunToBenzerUrun.Where(p => p.UrunId == sayfaGuncelle.Id).ToList().ForEach(p => db.UrunToBenzerUrun.Remove(p));
                            db.SaveChanges();

                            foreach (var item in Model.SeciliBenzerUrunAutocomplete)
                            {
                                var uruntoBenzerUrunEkle = new UrunToBenzerUrun()
                                {
                                    UrunId = sayfaGuncelle.Id,
                                    BenzerUrunId = item
                                };
                                _context.Entry(uruntoBenzerUrunEkle).State = EntityState.Added;
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            db.UrunToBenzerUrun.Where(p => p.UrunId == sayfaGuncelle.Id).ToList().ForEach(p => db.UrunToBenzerUrun.Remove(p));
                            db.SaveChanges();
                        }
                        #endregion

                        #region Tamamlayıcı Ürünler
                        if (Model.SeciliTamamlayiciUrunAutocomplete != null)
                        {
                            db.UrunToTamamlayiciUrun.Where(p => p.UrunId == sayfaGuncelle.Id).ToList().ForEach(p => db.UrunToTamamlayiciUrun.Remove(p));
                            db.SaveChanges();

                            foreach (var item in Model.SeciliTamamlayiciUrunAutocomplete)
                            {
                                var uruntoTamamlayiciUrunEkle = new UrunToTamamlayiciUrun()
                                {
                                    UrunId = sayfaGuncelle.Id,
                                    TamamlayiciUrunId = item
                                };
                                _context.Entry(uruntoTamamlayiciUrunEkle).State = EntityState.Added;
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            db.UrunToTamamlayiciUrun.Where(p => p.UrunId == sayfaGuncelle.Id).ToList().ForEach(p => db.UrunToTamamlayiciUrun.Remove(p));
                            db.SaveChanges();
                        }
                        #endregion

                        #region Seo Url
                        foreach (var urun in Model.Urun.UrunlerTranslate)
                        {
                            var seoUrl = await _seoServis.SeoLinkOlustur(sayfaAdi: urun.UrunAdi, sayfaId: sayfaGuncelle.Id, entityName: SeoUrlTipleri.Urun, seoTipi: SeoTipleri.Urun, dilId: urun.DilId);
                            if (seoUrl.Basarilimi == false)
                            {
                                return seoUrl;
                            }

                            var menuGuncelle = await MenuHelper.MenuKaydet(Model.MenuYerleri, sayfaAdi: urun.UrunAdi, sayfaId: sayfaGuncelle.Id, parentSayfaId: 1, seoTipi: SeoUrlTipleri.Urun, MenuTipleri.Urunler, dilId: urun.DilId);

                            if (menuGuncelle.Basarilimi == false)
                            {
                                return menuGuncelle;
                            }

                        }
                        #endregion
                        await _context.SaveChangesAsync();

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

                        _cacheService.RemoveByPattern($"KategoriDetay");
                        _cacheService.RemoveByPattern($"OneCikanUrunResimleri");

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} güncelleme işlemi başarıyla tamamlanmıştır.";


                    }

                    transaction.Complete();
                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu." + hata.Message;
                result.SayfaId = pageId;
            }

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public async Task<ResultViewModel> UrunOzellik(UrunToOzellikViewModel Model, string submit)
        {

            var result = new ResultViewModel();
            int pageId = 0;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    List<string> ResimDosyaTipleri = new()
                    {
                        "image/jpeg",
                        "image/png",
                        "image/gif",
                        "image/webp",
                        "image/svg+xml"
                    };

                    #region Sayfa Ekleme

                    AppDbContext db = new AppDbContext();
                    db.UrunToOzellik.Where(p => p.UrunId == Model.UrunId && p.UrunOzellikleri.UrunOzellikGrupId == Model.OzellikGrupId).ToList().ForEach(p => db.UrunToOzellik.Remove(p));
                    db.SaveChanges();
                    foreach (var item in Model.UrunToOzellikListesi.Where(x=> x.SilinmeDurum == false))
                    {
                        if (item.OzellikResim != null)
                        {
                            var model = DosyaHelper.DosyaYukle(item.OzellikResim, "UrunOzellikleri", ResimDosyaTipleri, 5242880,DosyaYoluTipleri.Resim);
                            if (model.Result.Basarilimi == true)
                            {
                                item.Resim = model.Result.Sonuc;
                            }
                            else
                            {
                                result.Basarilimi = result.Basarilimi;
                                result.MesajDurumu = result.MesajDurumu;
                                result.Mesaj = result.Mesaj;
                                return result;
                            }
                        }
                   
                        var urunOzellik = new UrunToOzellik()
                        {
                            UrunOzellikId = item.UrunOzellikId,
                            UrunId = item.UrunId,
                            Aciklama = item.Aciklama,
                            Resim = item.Resim,
                            DilId = item.DilId
                        };
                        _context.Entry(urunOzellik).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                    }

                    #endregion

                    #region Sayfa Butonlari
                    if (submit == "Kaydet")
                    {
                        result.Action = "UrunOzellikleri";
                        result.SayfaId = pageId;
                    }
                    if (submit == "KaydetGuncelle")
                    {
                        result.Action = "UrunOzellikleri";
                        result.SayfaId = pageId;
                    }

                    #endregion

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";



                    transaction.Complete();
                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu : " + hata.Message;
                result.SayfaId = pageId;

            }

            return result;

        }
        public async Task<ResultViewModel> BegeniEkle(int urunId)
        {

            ResultViewModel result = new ResultViewModel();


            try
            {
                if (urunId != 0)
                {
                    var uyeid = Convert.ToInt32(this._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                    var begenildimi = _context.Begeniler.Where(x => x.UrunId == urunId && x.UyeId == uyeid).FirstOrDefault();
                    if (begenildimi == null)
                    {
                        var begeni = new Begeniler()
                        {
                            UrunId = urunId,
                            UyeId = uyeid
                        };
                        _context.Entry(begeni).State = EntityState.Added;
                        await _context.SaveChangesAsync();

                        result.Basarilimi = true;
                        result.Mesaj = "Başarıyla Beğendiniz";
                        result.MesajDurumu = "success";
                        result.SayfaId = urunId;

                    }
                    else
                    {
                        result.Basarilimi = true;
                        result.Mesaj = "Bu Ürünü Zaten Beğendiniz";
                        result.MesajDurumu = "success";
                        result.SayfaId = urunId;

                    }



                }
                else
                {

                    result.Basarilimi = false;
                    result.Mesaj = "Hata Oluştu";
                    result.MesajDurumu = "danger";
                }
            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.Mesaj = "Genel Bir Hata Oluştu : " + hata.Message;
                result.MesajDurumu = "danger";
            }



            return result;
        }
        public async Task<ResultViewModel> DeletePage(UrunViewModel Model)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.Urunler.Find(Model.Id);
                    _context.Entry(model).State = EntityState.Deleted;


                    var diller = _context.Diller.ToList();
                    for (int i = 0; i < diller.Count; i++)
                    {
                        FileInfo file = new(@"wwwroot" + model.UrunlerTranslate.Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == diller[i].DilKodlari.DilKodu)?.Resim);
                        if (file.Exists)
                        {
                            file.Delete();
                        }

                        var seoUrl = _context.SeoUrl.FirstOrDefault(p => p.EntityId == Model.Id & p.EntityName == SeoUrlTipleri.Urun)?.Url;
                        _context.SeoUrl.Where(p => p.EntityId == model.Id & p.Url == seoUrl).ToList().ForEach(p => _context.SeoUrl.Remove(p));
                        await _context.SaveChangesAsync();
                    }
                    await _context.SaveChangesAsync();

                    var menuSil = await MenuHelper.MenuSil(sayfaId: Model.Id, seoTipi: SeoUrlTipleri.Urun);
                    if (menuSil.Basarilimi == false)
                    {
                        return menuSil;
                    }
                    _context.SaveChanges();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} Başarıyla Silindi.";

                    _cacheService.RemoveByPattern($"KategoriTranslate");
                    _cacheService.RemoveByPattern($"OneCikanUrunResimleri");

                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }

            return result;
        }

        public async Task<ResultViewModel> DeleteAllPage(int[] Deletes)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Deletes != null)
                    {
                        foreach (var item in Deletes)
                        {
                            var model = _context.Urunler.Find(item);

                            var diller = _context.Diller.ToList();
                            for (int i = 0; i < diller.Count; i++)
                            {
                                FileInfo file = new(@"wwwroot" + model.UrunlerTranslate.Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == diller[i].DilKodlari.DilKodu)?.Resim);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }


                                List<UrunResimleri> cokluresim = _context.UrunResimleri.ToList();

                                foreach (var item2 in cokluresim.Where(p => p.UrunId == item))
                                {
                                    FileInfo files = new(@"wwwroot" + item2.Resim);
                                    if (files.Exists)
                                    {
                                        files.Delete();
                                    }
                                }

                                var seoUrl = _context.SeoUrl.FirstOrDefault(p => p.EntityId == item & p.EntityName == SeoUrlTipleri.Urun)?.Url;
                                _context.SeoUrl.Where(p => p.EntityId == model.Id & p.Url == seoUrl).ToList().ForEach(p => _context.SeoUrl.Remove(p));

                                _context.Entry(model).State = EntityState.Deleted;

                                _context.Menuler.Where(p => p.EntityId == item & p.SeoUrlTipi == SeoUrlTipleri.Urun).ToList().ForEach(p => _context.Menuler.Remove(p));

                            }
                        }
                        await _context.SaveChangesAsync();

                        _cacheService.RemoveByPattern($"KategoriTranslate");
                        _cacheService.RemoveByPattern($"OneCikanUrunResimleri");

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";
                    }


                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }


            return result;
        }

        //DROPZONE RESIM SIRALAMA VE SILME
        public async Task<ResultViewModel> ImageSortOrder(string sira)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    string sira_ = sira.Replace("sira[]=", "");
                    string[] p = sira_.Split('&');
                    int ss = 0;
                    foreach (string item in p)
                    {
                        var model = _context.UrunResimleri.Find(Convert.ToInt32(item));
                        model.Sira = ss;
                        _context.Entry(model).State = EntityState.Modified;
                        ss++;
                    }

                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Resimler Sıralandı.";

                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }

            return result;
        }

        public async Task<ResultViewModel> ImageDelete(int id)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.UrunResimleri.ToList().Find(p => p.Id == id);

                    _context.Entry(model).State = EntityState.Deleted;

                    FileInfo file = new(@"wwwroot" + model.Resim);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Resim silindi.";

                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }

            return result;
        }
        //DROPZONE RESIM SIRALAMA VE SILME

        //DROPZONE DOSYA SIRALAMA VE SILME
        public async Task<ResultViewModel> FilesSortOrder(string sira)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    string sira_ = sira.Replace("sira[]=", "");
                    string[] p = sira_.Split('&');
                    int ss = 0;
                    foreach (string item in p)
                    {
                        var model = _context.Dosyalar.Find(Convert.ToInt32(item));
                        model.Sira = ss;
                        _context.Entry(model).State = EntityState.Modified;
                        ss++;
                    }
                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Dosyalar Sıralandı.";

                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }




            return result;
        }

        public async Task<ResultViewModel> FilesDelete(int id)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var model = _context.Dosyalar.ToList().Find(p => p.Id == id);

                    _context.Entry(model).State = EntityState.Deleted;
                    //FileInfo file = new(@"wwwroot" + model.Dosya);
                    //if (file.Exists)
                    //{
                    //    file.Delete();
                    //}

                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Dosya silindi.";
                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }



            return result;
        }
        //DROPZONE DOSYA SIRALAMA VE SILME

        public async Task<ResultViewModel> AlisverisListemeEkle(string uyeid, int UrunId)
        {

            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var urunVarmi = _context.AlisverisListem.Where(p => p.UrunId == UrunId && p.UyeId == Convert.ToInt32(uyeid)).FirstOrDefault();

                    if (urunVarmi == null)
                    {
                        #region Sayfa Ekleme
                        var sayfaEkle = new AlisverisListem()
                        {
                            UyeId = Convert.ToInt32(uyeid),
                            UrunId = UrunId,
                        };

                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        #endregion
                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = "Alışveriş Listesine Eklendi";
                    }
                    else
                    {
                        result.Basarilimi = false;
                        result.MesajDurumu = "danger";
                        result.Mesaj = "Daha Önceden Eklenmiş";
                        return result;

                    }

                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }

            return result;

        }
        public async Task<ResultViewModel> AlisverisListeUrunSil(string uyeid, int UrunId)
        {

            var result = new ResultViewModel();

            var model = _context.AlisverisListem.Where(p => p.UrunId == UrunId && p.UyeId == Convert.ToInt32(uyeid)).FirstOrDefault();

            _context.Entry(model).State = EntityState.Deleted;

            try
            {

                await _context.SaveChangesAsync();

                result.Basarilimi = true;
                result.MesajDurumu = "success";
                result.Mesaj = $"{entity} Başarıyla Silindi.";

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";
            }


            return result;
        }
        public async Task<ResultViewModel> YorumYap(YorumViewModel Model, string uyeid)
        {

            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    #region Sayfa Ekleme
                    var sayfaEkle = new Yorumlar()
                    {
                        UyeId = Convert.ToInt32(uyeid),
                        UrunId = Model.UrunId,
                        Yorum = Model.Yorum,
                        YorumTarihi = DateTime.Now,
                        Yildiz = Model.Yildiz,
                    };

                    _context.Entry(sayfaEkle).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    #endregion

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";

                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }

            return result;

        }
    }
}
