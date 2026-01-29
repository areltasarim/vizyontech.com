using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class BannerServis : IBannerServis
    {
        private readonly AppDbContext _context;
        private readonly LogsServis _logServis;
        private UnitOfWork _uow = null;
        private readonly string entity = "Banner";
        public BannerServis(AppDbContext _context)
        {
            _uow = new UnitOfWork();
            this._context = _context;
        }


        public async Task<List<Banner>> PageList()
        {
            return (await _context.Banner.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(BannerViewModel Model, string submit)
        {

            var result = new ResultViewModel();
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
                        var sayfaEkle = new Banner()
                        {
                            ColumnDesktop = Model.Banner.ColumnDesktop,
                            ColumnMobil = Model.Banner.ColumnMobil,
                            Sira = Model.Banner.Sira,
                            Durum = Model.Banner.Durum,
                            BannerTranslate = Model.Banner.BannerTranslate.Select(item => new BannerTranslate
                            {
                                BannerAdi = item.BannerAdi,
                                DilId = item.DilId
                            }).ToList()
                        };

                        _context.Banner.Add(sayfaEkle);
                        await _context.SaveChangesAsync();
                        #endregion

                        #region Banner Resim Translate
                        var bannerResim = new BannerResim
                        {
                            BannerId = sayfaEkle.Id,
                        };
                        _context.BannerResim.Add(bannerResim);
                        await _context.SaveChangesAsync();

                        foreach (var dil in Model.BannerResimListesi)
                        {
                            foreach (var resim in dil.Value)
                            {
                                var menutpi = MenuHelper.GetMenuTipi(resim.UrlTipi, Convert.ToInt32(resim.EntityId));

                                string url = "";
                                if (resim.UrlTipi != MenuTipleri.Url && resim.UrlTipi != MenuTipleri.Dosyalar)
                                {
                                    url = _context.SeoUrl.Where(p => p.EntityId == resim.EntityId & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(menutpi.seoUrlTipi)).SingleOrDefault(p => p.Diller.DilKoduId == dil.Key).Url;
                                }
                                var bannerResimTranslate = new BannerResimTranslate
                                {
                                    BannerResimId = bannerResim.Id,
                                    DilId = dil.Key,
                                    BannerAdi = resim.BannerAdi,
                                    Url = url,
                                    UrlTipi = resim.UrlTipi,
                                    EntityId = resim.EntityId,
                                    SeoUrlTipi = (SeoUrlTipleri)menutpi.seoUrlTipi,
                                    Sira = resim.Sira,
                                };

                                #region Resim
                                if (resim.Resim != null)
                                {
                                    var model = DosyaHelper.DosyaYukle(resim.Resim, "Banner", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);

                                    if (model.Result.Basarilimi == true)
                                    {
                                        bannerResimTranslate.Resim = model.Result.Sonuc;
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
                                    bannerResimTranslate.Resim = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim);
                                }
                                #endregion


                                _context.BannerResimTranslate.Add(bannerResimTranslate);
                            }
                        }
                        await _context.SaveChangesAsync();
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

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";

                    }

                    else
                    {

                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.Banner.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.ColumnDesktop = Model.Banner.ColumnDesktop;
                        sayfaGuncelle.ColumnMobil = Model.Banner.ColumnMobil;
                        sayfaGuncelle.Sira = Model.Banner.Sira;
                        sayfaGuncelle.Durum = Model.Banner.Durum;

                        foreach (var item in Model.Banner.BannerTranslate)
                        {
                            var sayfaGuncelleTranslate = _context.BannerTranslate.Find(item.Id);
                            sayfaGuncelleTranslate.BannerAdi = item.BannerAdi;
                            _uow.Repository<BannerTranslate>().Update(sayfaGuncelleTranslate);
                            await _uow.CompleteAsync();
                        }


                        var bannerResim = _context.BannerResim.FirstOrDefault(p => p.BannerId == Model.Id);
                        if (bannerResim != null)
                        {

                            var bannerTranslateResimTranslateList = _context.BannerResimTranslate
                                .Where(x => x.BannerResimId == bannerResim.Id)
                                .ToList();
                            var modelBannerResimIds = Model.BannerResimListesi
                                .SelectMany(dil => dil.Value.Select(resim => resim.BannerResimId))
                                .Where(id => id != null)
                                .ToList();

                            var recordsToDelete = bannerTranslateResimTranslateList
                                .Where(x => !modelBannerResimIds.Contains(x.Id))
                                .ToList();

                            // Modelde olmayan veritabanı kayıtlarını sil
                            foreach (var record in recordsToDelete)
                            {
                                _context.BannerResimTranslate.Remove(record);
                            }

                            foreach (var dil in Model.BannerResimListesi.Keys)
                            {

                                foreach (var resim in Model.BannerResimListesi[dil])
                                {

                                    var bannerTranslate = _context.BannerResimTranslate.FirstOrDefault(x => x.Id == resim.BannerResimId && x.DilId == dil);

                                    var menutpi = MenuHelper.GetMenuTipi(resim.UrlTipi, Convert.ToInt32(resim.EntityId));

                                    string url = resim.Url;
                                    if (bannerTranslate != null)
                                    {

                                        if (resim.UrlTipi != MenuTipleri.Url && resim.UrlTipi != MenuTipleri.Dosyalar)
                                        {
                                            url = _context.SeoUrl.Where(p => p.EntityId == resim.EntityId & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(menutpi.seoUrlTipi)).SingleOrDefault(p => p.Diller.DilKoduId == dil).Url;
                                        }

                                        #region Resim
                                        if (resim.Resim != null)
                                        {
                                            var model = DosyaHelper.DosyaYukle(resim.Resim, "Banner", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);

                                            if (model.Result.Basarilimi == true)
                                            {
                                                bannerTranslate.Resim = model.Result.Sonuc;
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
                                            bannerTranslate.Resim = new AppDbContext().BannerResimTranslate.Find(resim.BannerResimId).Resim;
                                        }
                                        #endregion


                                        bannerTranslate.BannerAdi = resim.BannerAdi;
                                        bannerTranslate.Url = url;
                                        bannerTranslate.UrlTipi = resim.UrlTipi;
                                        bannerTranslate.EntityId = resim.EntityId;
                                        bannerTranslate.SeoUrlTipi = (SeoUrlTipleri)menutpi.seoUrlTipi;
                                        bannerTranslate.Sira = resim.Sira;
                                        bannerTranslate.DilId = dil;
                                        _context.Update(bannerTranslate);
                                    }
                                    else
                                    {
                                        var bannerResimTranslate = new BannerResimTranslate
                                        {
                                            BannerResimId = bannerResim.Id,
                                            DilId = dil,
                                            BannerAdi = resim.BannerAdi,
                                            Url = url,
                                            UrlTipi = resim.UrlTipi,
                                            EntityId = resim.EntityId,
                                            SeoUrlTipi = (SeoUrlTipleri)menutpi.seoUrlTipi,
                                            Sira = resim.Sira,
                                        };

                                        #region Resim
                                        if (resim.Resim != null)
                                        {
                                            var model = DosyaHelper.DosyaYukle(resim.Resim, "Banner", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);

                                            if (model.Result.Basarilimi == true)
                                            {
                                                bannerResimTranslate.Resim = model.Result.Sonuc;
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
                                            bannerResimTranslate.Resim = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim);
                                        }
                                        #endregion

                                        _context.BannerResimTranslate.Add(bannerResimTranslate);

                                    }
                                }
                            }
                            _context.SaveChanges();

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
                result.Mesaj = "Hata Oluştu: "+hata;
                result.Action = "AddOrUpdate";
            }
          
            return result;
        }

        public async Task<ResultViewModel> DeletePage(BannerViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.Banner.Find(Model.Id);
                    _context.Entry(model).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} Başarıyla Silindi.";

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
                            var model = _context.Banner.Find(item);
                            _context.Entry(_context.Banner.Find(item)).State = EntityState.Deleted;
                        }

                        await _context.SaveChangesAsync();

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
