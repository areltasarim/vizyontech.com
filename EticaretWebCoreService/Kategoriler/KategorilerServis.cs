using AutoMapper;
using EticaretWebCoreCaching.Abstraction;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class KategorilerServis : IKategorilerServis
    {
        private readonly AppDbContext _context;
        private UnitOfWork _uow = null;
        private readonly IMapper _mapper;
        private readonly SeoServis _seoServis;
        private readonly MenulerServis _menuServis;
        private readonly ICacheService _cacheService;

        private readonly string entity = "Kategori";

        public KategorilerServis(UnitOfWork uow, AppDbContext _context, IMapper mapper, SeoServis _seoServis, MenulerServis _menuServis, ICacheService cacheService)
        {
            _uow = uow;
            this._context = _context;
            _mapper = mapper;
            this._seoServis = _seoServis;
            this._menuServis = _menuServis;
            _cacheService = cacheService;
        }
        public async Task<List<Kategoriler>> PageList()
        {
            return (await _context.Kategoriler.ToListAsync());
        }


        public async Task<ResultViewModel> UpdatePage(KategoriViewModel Model, string submit)
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

                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        int parentKategoriId = 0;
                        if (_context.Kategoriler.ToList().Count > 0)
                        {
                            parentKategoriId = Model.Kategori.ParentKategoriId;
                        }
                        else
                        {
                            parentKategoriId = 1;
                        }

                        var sayfaEkle = new Kategoriler()
                        {
                            ParentKategoriId = parentKategoriId,
                            Sira = Model.Kategori.Sira,
                            Durum = Model.Kategori.Durum,
                            Vitrin = Model.Kategori.Vitrin,
                            Ikon = Model.Kategori.Ikon,
                            KategorilerTranslate = new List<KategorilerTranslate>(),
                        };

                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        #region Kapak Resmi
                        if (Model.SayfaResmi != null)
                        {
                            var model = DosyaHelper.DosyaYukle(Model.SayfaResmi, "Kategoriler", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);
                            if (model.Result.Basarilimi == true)
                            {
                                Model.Resim = model.Result.Sonuc;
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
                            Model.Resim = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                        }
                        #endregion

                        #region Breadcrumb Resim
                        if (Model.BreadcrumbImage != null)
                        {
                            var model = DosyaHelper.DosyaYukle(Model.BreadcrumbImage, "Kategoriler", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);
                            if (model.Result.Basarilimi == true)
                            {
                                Model.Resim = model.Result.Sonuc;
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
                            Model.BreadcrumbResim = ImageHelper.DosyaYok(DosyaYoluTipleri.Breadcumb);
                        }
                        #endregion

                        // Resim ve BreadcrumbResim'i sayfaEkle nesnesine ata
                        sayfaEkle.Resim = Model.Resim;
                        sayfaEkle.BreadcrumbResim = Model.BreadcrumbResim;

                        foreach (var item in Model.Kategori.KategorilerTranslate)
                        {
                            var sayfaEkleTranslate = new KategorilerTranslate()
                            {
                                KategoriAdi = item.KategoriAdi,
                                BreadcrumbAdi = item.BreadcrumbAdi,
                                BreadcrumbAciklama = item.BreadcrumbAciklama,
                                Aciklama = item.Aciklama,
                                KisaAciklama = item.KisaAciklama,
                                UstAciklama = item.UstAciklama,
                                SolAciklama = item.SolAciklama,
                                AltAciklama = item.AltAciklama,
                                MetaBaslik = item.MetaBaslik,
                                MetaAnahtar = item.MetaAnahtar,
                                MetaAciklama = item.MetaAciklama,
                                DilId = item.DilId
                            };
                            sayfaEkle.KategorilerTranslate.Add(sayfaEkleTranslate);
                        }


                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();

                        #endregion

                        #region Seo Url
                        foreach (var kategori in Model.Kategori.KategorilerTranslate)
                        {
                            var seoUrl = await _seoServis.SeoLinkOlustur(sayfaAdi: kategori.KategoriAdi, sayfaId: sayfaEkle.Id, entityName: SeoUrlTipleri.Kategori, seoTipi: SeoTipleri.Kategori, dilId: kategori.DilId);

                            if (seoUrl.Basarilimi == false)
                            {
                                return seoUrl;
                            }

                            var menuEkle = await MenuHelper.MenuKaydet(Model.MenuYerleri, sayfaAdi: kategori.KategoriAdi, sayfaId: sayfaEkle.Id, parentSayfaId: 1, seoTipi: SeoUrlTipleri.Kategori, MenuTipleri.Kategoriler, dilId: kategori.DilId);

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

                        _cacheService.RemoveByPattern($"KategoriTranslate");
                        _cacheService.RemoveByPattern($"KategoriUrunlerSinirsiz");
                        _cacheService.RemoveByPattern($"KategorilerListesiDTO");


                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";

                    }

                    else
                    {

                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.Kategoriler.Where(p => p.Id == Model.Id).FirstOrDefault();
                        pageId = sayfaGuncelle.Id;

                        sayfaGuncelle.ParentKategoriId = Model.Kategori.ParentKategoriId;
                        sayfaGuncelle.Sira = Model.Kategori.Sira;
                        sayfaGuncelle.Durum = Model.Kategori.Durum;
                        sayfaGuncelle.Vitrin = Model.Kategori.Vitrin;
                        sayfaGuncelle.Ikon = Model.Kategori.Ikon;

                        #region Kapak Resim
                        if (Model.SayfaResmi != null)
                        {
                            var model = DosyaHelper.DosyaYukle(Model.SayfaResmi, "Kategoriler", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);
                            if (model.Result.Basarilimi == true)
                            {
                                sayfaGuncelle.Resim = model.Result.Sonuc;
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
                            sayfaGuncelle.Resim = new AppDbContext().Kategoriler.Find(Model.Id).Resim;
                        }
                        #endregion

                        #region Breadcrumb Resim
                        if (Model.BreadcrumbImage != null)
                        {
                            var model = DosyaHelper.DosyaYukle(Model.BreadcrumbImage, "Kategoriler", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);
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
                            sayfaGuncelle.BreadcrumbResim = new AppDbContext().Kategoriler.Find(sayfaGuncelle.Id).BreadcrumbResim;
                        }
                        #endregion


                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        if (!UstKategoriAltKategoriAtanamaz(Model.Id, Model.Kategori.ParentKategoriId))
                        {
                            // Hata durumunu ele al
                            result.Basarilimi = false;
                            result.MesajDurumu = "danger";
                            result.Mesaj = "Kategorinin kendisi ana kategori olarak seçilemez!";
                            result.SayfaId = sayfaGuncelle.Id;
                            return result;
                        }


                        foreach (var item in Model.Kategori.KategorilerTranslate)
                        {
                            var sayfaGuncelleTranslate = _context.KategorilerTranslate.Find(item.Id);

                            sayfaGuncelleTranslate.KategoriAdi = item.KategoriAdi;
                            sayfaGuncelleTranslate.BreadcrumbAdi = item.BreadcrumbAdi;
                            sayfaGuncelleTranslate.BreadcrumbAciklama = item.BreadcrumbAciklama;
                            sayfaGuncelleTranslate.Aciklama = item.Aciklama;
                            sayfaGuncelleTranslate.KisaAciklama = item.KisaAciklama;
                            sayfaGuncelleTranslate.UstAciklama = item.UstAciklama;
                            sayfaGuncelleTranslate.UstAciklama = item.UstAciklama;
                            sayfaGuncelleTranslate.SolAciklama = item.SolAciklama;
                            sayfaGuncelleTranslate.AltAciklama = item.AltAciklama;
                            sayfaGuncelleTranslate.MetaBaslik = item.MetaBaslik;
                            sayfaGuncelleTranslate.MetaAnahtar = item.MetaAnahtar;
                            sayfaGuncelleTranslate.MetaAciklama = item.MetaAciklama;
                            sayfaGuncelleTranslate.DilId = item.DilId;

                            _uow.Repository<KategorilerTranslate>().Update(sayfaGuncelleTranslate);
                            await _uow.CompleteAsync();
                        }

                        #endregion

                        #region Seo Url
                        foreach (var kategori in Model.Kategori.KategorilerTranslate)
                        {
                            var seoUrl = await _seoServis.SeoLinkOlustur(sayfaAdi: kategori.KategoriAdi, sayfaId: sayfaGuncelle.Id, entityName: SeoUrlTipleri.Kategori, seoTipi: SeoTipleri.Kategori, dilId: kategori.DilId);
                            if (seoUrl.Basarilimi == false)
                            {
                                return seoUrl;
                            }


                            var menuGuncelle = await MenuHelper.MenuKaydet(Model.MenuYerleri, sayfaAdi: kategori.KategoriAdi, sayfaId: sayfaGuncelle.Id, parentSayfaId: 1, seoTipi: SeoUrlTipleri.Kategori, MenuTipleri.Kategoriler, dilId: kategori.DilId);

                            if (menuGuncelle.Basarilimi == false)
                            {
                                return menuGuncelle;
                            }
                            await _context.SaveChangesAsync();



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
                            result.SayfaId = sayfaGuncelle.Id;
                        }
                        #endregion

                        _cacheService.RemoveByPattern($"KategoriTranslate");
                        _cacheService.RemoveByPattern($"KategoriUrunlerSinirsiz");
                        _cacheService.RemoveByPattern($"KategorilerListesiDTO");

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
                result.Mesaj = "Hata Oluştu : " + hata.Message;
                result.SayfaId = pageId;

            }

            return result;

        }


        public async Task<ResultViewModel> BannerEkleGuncelle(KategoriBannerViewModel Model, string submit)
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
                    db.KategoriBanner.Where(p => p.KategoriId == Model.KategoriId).ToList().ForEach(p => db.KategoriBanner.Remove(p));
                    db.SaveChanges();
                    foreach (var item in Model.KategoriBannerListe.Where(x => x.SilinmeDurum == false))
                    {
                        if (item.SayfaResim != null)
                        {
                            var model = DosyaHelper.DosyaYukle(item.SayfaResim, "KategoriBanner", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);
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

                        var kategoriBanner = new KategoriBanner()
                        {
                            KategoriId = Model.KategoriId,
                            Url = item.Url,
                            Resim = item.Resim,
                            Sira = item.Sira,
                            DilId = item.DilId
                        };
                        _context.Entry(kategoriBanner).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                    }
                    pageId = Model.KategoriId;
                    #endregion

                    #region Sayfa Butonlari
                    if (submit == "Kaydet")
                    {
                        result.Action = "Index";
                        result.SayfaId = pageId;
                    }
                    if (submit == "KaydetGuncelle")
                    {
                        result.Action = "KategoriBanner";
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


        public bool UstKategoriAltKategoriAtanamaz(int parentSayfaId, int yeniKategoriId)
        {
            // Kategori, kendi alt kategorisine atanamaz
            if (parentSayfaId == yeniKategoriId)
            {
                return false;
            }

            // Mevcut üst kategori kontrolü
            var mevcutKategori = _context.Kategoriler.FirstOrDefault(s => s.Id == parentSayfaId);
            if (mevcutKategori == null)
            {
                // Geçerli bir üst kategori bulunamadı, işlem geçersiz
                return false;
            }

            // Mevcut üst kategori, düzenlenen kategorinin gerçekten üst kategorisi mi kontrol edilir
            if (mevcutKategori.Id != null && mevcutKategori.Id == yeniKategoriId)
            {
                // Düzenlenen kategori, mevcut üst kategorinin alt kategorisi ise işlem geçersiz
                return false;
            }

            // Alt kategorileri kontrol et
            var altKategoriler = _context.Kategoriler.Where(s => s.ParentKategoriId == parentSayfaId).ToList();
            foreach (var altkategori in altKategoriler)
            {
                if (altkategori.Id == yeniKategoriId || !UstKategoriAltKategoriAtanamaz(altkategori.Id, yeniKategoriId))
                {
                    // Düzenlenen kategori, alt kategorisi olarak atanamaz
                    return false;
                }
            }

            // Geçerli bir üst kategori
            return true;
        }
        public async Task<ResultViewModel> DeletePage(KategoriViewModel Model)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var urunToKategoriList = await _context.UrunToKategori
                        .Where(x => x.KategoriId == Model.Id)
                        .ToListAsync();
                    _context.UrunToKategori.RemoveRange(urunToKategoriList);
                    await _context.SaveChangesAsync();

                    async Task SilAltKategoriler(int id)
                    {
                        var kategori = _context.Kategoriler.Include(k => k.AltKategoriler).FirstOrDefault(k => k.Id == id);

                        if (kategori != null)
                        {
                            foreach (var altKategori in kategori.AltKategoriler.ToList())
                            {
                                await SilAltKategoriler(altKategori.Id);

                                var seoSil = await _seoServis.SeoSil(sayfaId: altKategori.Id, entityName: SeoUrlTipleri.Kategori);
                                if (seoSil.Basarilimi == false)
                                {
                                    throw new Exception($"SEO silinemedi. Kategori Id: {altKategori.Id}");
                                }

                                // Alt kategoriyi işaretle
                                _context.Entry(altKategori).State = EntityState.Deleted;
                            }
                        }
                    }
                    var model = _context.Kategoriler.Include(k => k.AltKategoriler).FirstOrDefault(k => k.Id == Model.Id);

                    if (model != null)
                    {
                        await SilAltKategoriler(model.Id);

                        var seoSil = await _seoServis.SeoSil(sayfaId: model.Id, entityName: SeoUrlTipleri.Kategori);
                        if (seoSil.Basarilimi == false)
                        {
                            return seoSil;
                        }

                        // Ana kategoriyi işaretle
                        _context.Entry(model).State = EntityState.Deleted;

                        // Değişiklikleri kaydet
                        await _context.SaveChangesAsync();
                    }

                    var modelresim = _context.Kategoriler.ToList().Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").ToList().Find(p => p.Id == Model.Id);
                    FileInfo file = new(@"wwwroot" + modelresim?.Resim);
                    if (file.Exists)
                    {
                        file.Delete();
                    }


                    var menuSil = await MenuHelper.MenuSil(sayfaId: Model.Id, seoTipi: SeoUrlTipleri.Kategori);
                    if (menuSil.Basarilimi == false)
                    {
                        return menuSil;
                    }
                    _context.SaveChanges();


                    _cacheService.RemoveByPattern($"KategoriTranslate");
                    _cacheService.RemoveByPattern($"KategoriUrunlerSinirsiz");
                    _cacheService.RemoveByPattern($"KategorilerListesiDTO");


                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} Başarıyla Silindi.";

                    transaction.Complete();
                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu : " + hata.Message;

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
                            var model = _context.Kategoriler.Find(item);

                            foreach (var item2 in model.AltKategoriler)
                            {
                                _context.Entry(item2).State = EntityState.Deleted;

                            }
                            var modelresim = _context.Kategoriler.ToList().Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").ToList().Find(p => p.Id == item);

                            FileInfo file = new(@"wwwroot" + modelresim?.Resim);
                            if (file.Exists)
                            {
                                file.Delete();
                            }


                            var seoUrl = _context.SeoUrl.FirstOrDefault(p => p.EntityId == item & p.EntityName == SeoUrlTipleri.Kategori)?.Url;
                            _context.SeoUrl.Where(p => p.EntityId == model.Id & p.Url == seoUrl).ToList().ForEach(p => _context.SeoUrl.Remove(p));

                            _context.Entry(model).State = EntityState.Deleted;

                            _context.Menuler.Where(p => p.EntityId == item & p.SeoUrlTipi == SeoUrlTipleri.Kategori).ToList().ForEach(p => _context.Menuler.Remove(p));
                        }
                        await _context.SaveChangesAsync();


                        _cacheService.RemoveByPattern($"KategoriTranslate");
                        _cacheService.RemoveByPattern($"KategoriUrunlerSinirsiz");
                        _cacheService.RemoveByPattern($"KategorilerListesiDTO");

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

    }
}
