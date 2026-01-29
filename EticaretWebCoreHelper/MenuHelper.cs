using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using DocumentFormat.OpenXml.InkML;
using NPOI.SS.Formula.Functions;
using System.Security.Policy;
using DocumentFormat.OpenXml.Bibliography;

namespace EticaretWebCoreHelper
{
    public static class MenuHelper
    {
        public static async Task<ResultViewModel> MenuKaydet(List<MenuYerleri> menuyerleri, string sayfaAdi, int sayfaId, int parentSayfaId, SeoUrlTipleri seoTipi, MenuTipleri menuTipi, int dilId)
        {
            var result = new ResultViewModel();

            try
            {
                var _context = new AppDbContext();
                var menuListesi = new List<Menuler>();

                // Mevcut menüleri al
                var mevcutMenuler = _context.Menuler
                    .Where(m => m.EntityId == sayfaId && m.MenuTipi == menuTipi)
                    .Include(m => m.MenulerTranslate)
                    .ToList();

                // Eğer menuyerleri boşsa, mevcut menüleri sil
                if (menuyerleri == null || !menuyerleri.Any())
                {
                    if (mevcutMenuler.Any())
                    {
                        _context.Menuler.RemoveRange(mevcutMenuler);
                        _context.SaveChanges();
                    }

                    result.Basarilimi = true;
                    result.Mesaj = "Tüm menüler kaldırıldı.";
                    return await Task.FromResult(result);
                }

                // Yeni menüleri ekleme veya güncelleme işlemleri
                foreach (var item in menuyerleri)
                {
                    var seoQuery = _context.SeoUrl
                    .Where(p => p.EntityId == sayfaId && p.EntityName == (SeoUrlTipleri)(int)seoTipi);

                    var seoUrl = seoTipi == SeoUrlTipleri.Marka
                        ? seoQuery.FirstOrDefault()
                        : seoQuery.SingleOrDefault(p => p.Diller.DilKoduId == dilId);




                    var mevcutMenu = mevcutMenuler.SingleOrDefault(m => m.MenuYeri == item);

                    if (mevcutMenu != null)
                    {
                        mevcutMenu.MenulerTranslate.ToList().ForEach(mt =>
                        {
                            mt.MenuAdi = sayfaAdi;
                            mt.Url = seoUrl?.Url;
                        });
                    }
                    else
                    {
                        int parentId = 1;
                        if (parentSayfaId > 1)
                        {
                            var menu = _context.Menuler.Where(x => x.EntityId == parentSayfaId && x.SeoUrlTipi == seoTipi).FirstOrDefault();
                            parentId = menu.Id;
                        }
                        var yeniMenu = new Menuler
                        {
                            ParentMenuId = 1,
                            MenuTipi = menuTipi,
                            EntityId = sayfaId,
                            SeoUrlTipi = seoTipi,
                            MenuKolon = 0,
                            MenuYeri = item,
                            SekmeDurumu = MenuSekmeleri.AyniSekme,
                            Sira = 99,
                            Durum = SayfaDurumlari.Aktif,
                            MenulerTranslate = new List<MenulerTranslate>
                    {
                        new MenulerTranslate
                        {
                            MenuAdi = sayfaAdi,
                            Url = seoUrl?.Url,
                            DilId = dilId,
                        }
                    }
                        };

                        _context.Menuler.Add(yeniMenu);
                    }
                }

                // Silinecek menüleri kontrol et
                var silinecekMenuler = mevcutMenuler
                    .Where(m => !menuyerleri.Contains(m.MenuYeri))
                    .ToList();

                if (silinecekMenuler.Any())
                {
                    _context.Menuler.RemoveRange(silinecekMenuler);
                }

                // Değişiklikleri kaydet
                _context.SaveChanges();

                result.Basarilimi = true;
            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Menu Güncellemede Hata Oluştu: " + hata.Message;
            }

            return await Task.FromResult(result);
        }


        public static async Task<ResultViewModel> MenuSil(int sayfaId, SeoUrlTipleri seoTipi)
        {
            var result = new ResultViewModel();

            try
            {
                var _context = new AppDbContext();

                async Task SilAltMenulerdenYukariya(int currentMenuId)
                {
                    var altMenuler = _context.Menuler.Where(p => p.ParentMenuId == currentMenuId).ToList();
                    foreach (var altMenu in altMenuler)
                    {
                        await SilAltMenulerdenYukariya(altMenu.Id);
                    }
                    var menu = _context.Menuler.FirstOrDefault(p => p.Id == currentMenuId);
                    if (menu != null)
                    {
                        _context.Entry(menu).State = EntityState.Deleted;
                    }
                }

                var menuId = _context.Menuler
                    .Where(p => p.EntityId == sayfaId && p.SeoUrlTipi == seoTipi)
                    .Select(p => p.Id)
                    .FirstOrDefault();

                if (menuId != 0) 
                {
                    await SilAltMenulerdenYukariya(menuId);
                }

                await _context.SaveChangesAsync();

                result.Basarilimi = true;
            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Menu Silmede Hata Oluştu. " + hata.Message;
            }

            return result;
        }



        public static (int entityId, int seoUrlTipi) GetMenuTipi(MenuTipleri menuTipi, int entitymenuid)
        {
            var _context = new AppDbContext();

            int entityId = 0;
            int seoUrlTipi = 0;

            switch (menuTipi)
            {
                case MenuTipleri.DinamikSayfalar:
                    var sayfa = _context.Sayfalar.SingleOrDefault(p => p.Id == entitymenuid);
                    if (sayfa != null)
                    {
                        entityId = sayfa.Id;
                        seoUrlTipi = (int)sayfa.SayfaTipi;
                    }
                    break;

                case MenuTipleri.Kategoriler:
                    var kategori = _context.Kategoriler.SingleOrDefault(p => p.Id == entitymenuid);
                    if (kategori != null)
                    {
                        entityId = kategori.Id;
                        seoUrlTipi = (int)SeoUrlTipleri.Kategori;
                    }
                    break;

                case MenuTipleri.Urunler:
                    var urun = _context.Urunler.SingleOrDefault(p => p.Id == entitymenuid);
                    if (urun != null)
                    {
                        entityId = urun.Id;
                        seoUrlTipi = (int)SeoUrlTipleri.Urun;
                    }
                    break;

                case MenuTipleri.Url:
                    entityId = 0;
                    seoUrlTipi = (int)SeoUrlTipleri.Url;
                    break;

                case MenuTipleri.Galeri:
                    var galeri = _context.FotografGalerileri.SingleOrDefault(p => p.Id == entitymenuid);
                    if (galeri != null)
                    {
                        var entityName = _context.SeoUrl.FirstOrDefault(p => p.EntityId == galeri.Id &&
                            p.EntityName == (SeoUrlTipleri)Convert.ToInt32(galeri.GaleriTipi));
                        if (entityName != null)
                        {
                            entityId = galeri.Id;
                            seoUrlTipi = (int)entityName.EntityName;
                        }
                    }
                    break;

                case MenuTipleri.EKatalog:
                    var ekatalog = _context.FotografGalerileri.SingleOrDefault(p => p.Id == entitymenuid);
                    if (ekatalog != null)
                    {
                        var entityName = _context.SeoUrl.FirstOrDefault(p => p.EntityId == ekatalog.Id &&
                            p.EntityName == (SeoUrlTipleri)Convert.ToInt32(ekatalog.GaleriTipi));
                        if (entityName != null)
                        {
                            entityId = ekatalog.Id;
                            seoUrlTipi = 310;
                        }
                    }
                    break;

                case MenuTipleri.Dosyalar:
                    entityId = entitymenuid;
                    break;

                case MenuTipleri.SabitMenu:
                    var sabitMenu = _context.SabitMenuler.SingleOrDefault(p => p.Id == entitymenuid);
                    if (sabitMenu != null)
                    {
                        var entityName = _context.SeoUrl.FirstOrDefault(p => p.EntityId == sabitMenu.Id &&
                            p.EntityName == (SeoUrlTipleri)Convert.ToInt32(sabitMenu.SayfaTipi));
                        if (entityName != null)
                        {
                            entityId = sabitMenu.Id;
                            seoUrlTipi = (int)entityName.EntityName;
                        }
                    }
                    break;

                default:
                    throw new ArgumentException("Geçersiz MenuTipi");
            }


            return (entityId, seoUrlTipi);
        }

    }
}
