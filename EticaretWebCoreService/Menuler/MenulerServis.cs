using EticaretWebCoreCaching.Abstraction;
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
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class MenulerServis : IMenulerServis
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;

        private readonly string entity = "Menü";

        public MenulerServis(AppDbContext _context, ICacheService cacheService)
        {
            this._context = _context;
            _cacheService = cacheService;
        }

        public async Task<List<Menuler>> PageList()
        {
            return (await _context.Menuler.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(MenuViewModel Model, MenuTipleri MenuTipi, MenuYerleri Menukonumu,  int EntityMenuId)
        {

            var result = new ResultViewModel();
            int pageId = 0;

            var menutpi = MenuHelper.GetMenuTipi(MenuTipi, EntityMenuId);

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Model.Id == 0)
                    {
                      
                        string url = "";


                        var sayfaEkle = new Menuler()
                        {
                            ParentMenuId = Model.ParentMenuId,
                            MenuTipi = MenuTipi,
                            EntityId = menutpi.entityId,
                            SeoUrlTipi = (SeoUrlTipleri)menutpi.seoUrlTipi,
                            MenuKolon = Model.MenuKolon,
                            MenuYeri = Menukonumu,
                            SekmeDurumu = Model.SekmeDurumu,
                            Sira = 99,
                            Durum = Model.Durum,
                            MenulerTranslate = new List<MenulerTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {

                            if (MenuTipi == MenuTipleri.Url)
                            {
                                url = Model.UrlCeviri[i];

                            }
                            else
                            {
                                if (menutpi.seoUrlTipi == 100)
                                {
                                    url = "";
                                }
                                else if (MenuTipi == MenuTipleri.Dosyalar)
                                {
                                    url = _context.FotografGaleriResimleri.Where(p => p.Id == EntityMenuId).FirstOrDefault().Resim;
                                }
                                else
                                {
                                    url = _context.SeoUrl.Where(p => p.EntityId == menutpi.entityId & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(menutpi.seoUrlTipi)).SingleOrDefault(p => p.Diller.DilKoduId == diller[i].DilKoduId).Url;
                                }
                            }

                            var sayfaEkleTranslate = new MenulerTranslate()
                            {
                                MenuAdi = Model.MenuAdiCeviri[i],
                                Url = url,
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.MenulerTranslate.Add(sayfaEkleTranslate);
                        }

                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();

                        _cacheService.RemoveByPattern($"Menuler");

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";
                       
                    }

                    else
                    {
                        //int seoUrlId = _context.SeoUrl.Where(p => p.EntityId == Model.SeoUrlId).FirstOrDefault().Id;

                
                        string url = "";

                        var sayfaGuncelle = _context.Menuler.Where(p => p.Id == Model.Id).FirstOrDefault();
                        pageId = sayfaGuncelle.Id;

                        sayfaGuncelle.ParentMenuId = new AppDbContext().Menuler.Find(Model.Id).ParentMenuId;
                        sayfaGuncelle.EntityId = menutpi.entityId;
                        sayfaGuncelle.MenuKolon = Model.MenuKolon;
                        sayfaGuncelle.MenuTipi = MenuTipi;
                        sayfaGuncelle.SeoUrlTipi = (SeoUrlTipleri)menutpi.seoUrlTipi;
                        sayfaGuncelle.MenuYeri = Menukonumu;
                        sayfaGuncelle.SekmeDurumu = Model.SekmeDurumu;
                        sayfaGuncelle.Sira = new AppDbContext().Menuler.Find(Model.Id).Sira;
                        sayfaGuncelle.Durum = Model.Durum;

                        var diller = _context.Diller.ToList();


                        var db = new AppDbContext();
                        db.Menuler.Find(Model.Id).MenulerTranslate.ToList().ForEach(p => db.MenulerTranslate.Remove(p));
                        await db.SaveChangesAsync();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            if (MenuTipi == MenuTipleri.Url)
                            {
                                url = Model.UrlCeviri[i];

                            }
                            else if (MenuTipi == MenuTipleri.Dosyalar)
                            {
                                url = _context.FotografGaleriResimleri.FirstOrDefault(p => p.Id == EntityMenuId).Resim;
                            }
                            else
                            {
                                url = _context.SeoUrl.Where(p => p.EntityId == menutpi.entityId & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(menutpi.seoUrlTipi)).SingleOrDefault(p => p.Diller.DilKoduId == diller[i].DilKoduId).Url;
                            }

                            var sayfaEkleTranslate = new MenulerTranslate()
                            {
                                MenuAdi = Model.MenuAdiCeviri[i],
                                Url = url,
                                DilId = diller[i].Id,
                            };
                            sayfaGuncelle.MenulerTranslate.Add(sayfaEkleTranslate);
                        }

                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        _cacheService.RemoveByPattern($"Menuler");

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

        public async Task<ResultViewModel> DeletePage(MenuViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var model = _context.Menuler.Find(Model.Id);

                    foreach (var item in model.AltMenuler)
                    {
                        _context.Entry(item).State = EntityState.Deleted;
                    }

                    _context.Entry(model).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();

                    _cacheService.RemoveByPattern($"Menuler");

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
                            _context.Entry(_context.Menuler.Find(item)).State = EntityState.Deleted;
                        }

                        await _context.SaveChangesAsync();

                        _cacheService.RemoveByPattern($"Menuler");

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
        public static List<Menuler> MenuKonumlari(MenuYerleri? MenuYeri)
        {
            AppDbContext _context = new();
            List<Menuler> list = _context.Menuler.Where(p => p.MenuYeri == MenuYeri).OrderBy(p => p.Sira).ToList();
            return list;
        }

    }
}
