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

    public partial class SabitMenulerServis : ISabitMenulerServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Sabit Menü";

        public SabitMenulerServis(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<SabitMenuler>> PageList()
        {
            return (await _context.SabitMenuler.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(SabitMenuViewModel Model, string submit)
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
                        var sayfaEkle = new SabitMenuler()
                        {
                            SayfaTipi = Model.SayfaTipi,
                            Sira = Model.Sira,
                            Durum = Model.Durum,
                            SabitMenulerTranslate = new List<SabitMenulerTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new SabitMenulerTranslate()
                            {
                                MenuAdi = Model.MenuAdiCeviri[i],
                                Aciklama = Model.AciklamaCeviri[i],
                                BreadcrumbAdi = Model.BreadcrumbSayfaAdiCeviri[i],
                                BreadcrumbAciklama = Model.BreadcrumbAciklamaCeviri[i],
                                Url = Model.UrlCeviri[i],
                                MetaBaslik = Model.MetaBaslikCeviri[i],
                                MetaAnahtar = Model.MetaAnahtarCeviri[i],
                                MetaAciklama = Model.MetaAciklamaCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.SabitMenulerTranslate.Add(sayfaEkleTranslate);

                        }

                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();

                        #endregion

                        #region Kapak Resmi
                        if (Model.BreadcrumbImage != null)
                        {


                            string imageName = ImageHelper.ImageReplaceName(Model.BreadcrumbImage, Model.BreadcrumbResim);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "SabitSayfalar/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            if (ResimDosyaTipleri.Contains(Model.BreadcrumbImage.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.BreadcrumbImage.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.BreadcrumbImage.CopyTo(stream);
                                }

                                sayfaEkle.BreadcrumbResim = Mappath.Remove(0, 7);
                            }

                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaEkle.Id;

                                return result;
                            }

                            if (Model.BreadcrumbImage.Length > 5242880)
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                                result.SayfaId = sayfaEkle.Id;

                                return result;
                            }
                        }

                        else
                        {
                            sayfaEkle.BreadcrumbResim = "/Content/Upload/Images/breadcumbdefault.png";
                        }
                        #endregion

                        #region Seo Url
                        for (int i = 0; i < diller.Count; i++)
                        {
                            string url = Model.UrlCeviri[i];
                            if (url == null)
                            {
                                url = "#";
                            }
                            else
                            {
                                url = Replace.UrlSeo(Model.UrlCeviri[i]);
                            }


                            foreach (var item in _context.SeoUrl.ToList())
                            {

                                if (url == item.Url)
                                {
                                    url = url + "-" + sayfaEkle.Id;

                                }
                            }

                            SeoTipleri seoTipi = SeoTipleri.Sayfa;

                            if (sayfaEkle.SayfaTipi == SabitSayfaTipleri.BizeUlasin)
                            {
                                seoTipi = SeoTipleri.BizeUlasin;
                            }

                            if (sayfaEkle.SayfaTipi == SabitSayfaTipleri.Markalar)
                            {
                                seoTipi = SeoTipleri.TumMarkalar;
                            }

                            if (sayfaEkle.SayfaTipi == SabitSayfaTipleri.Kategoriler)
                            {
                                seoTipi = SeoTipleri.TumKategoriler;
                            }

                            if (sayfaEkle.SayfaTipi == SabitSayfaTipleri.Urunler)
                            {
                                seoTipi = SeoTipleri.TumUrunler;
                            }

                            if (sayfaEkle.SayfaTipi == SabitSayfaTipleri.GaleriKategori)
                            {
                                seoTipi = SeoTipleri.GaleriKategorileri;
                            }



                            var seoUrl = new SeoUrl()
                            {
                                Url = url,
                                EntityName = (SeoUrlTipleri)Convert.ToInt32(sayfaEkle.SayfaTipi),
                                EntityId = sayfaEkle.Id,
                                SeoTipi = seoTipi,
                                DilId = diller[i].Id,
                            };
                            _context.Entry(seoUrl).State = EntityState.Added;
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
                        var sayfaGuncelle = _context.SabitMenuler.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.SayfaTipi = Model.SayfaTipi;
                        sayfaGuncelle.Sira = Model.Sira;
                        sayfaGuncelle.Durum = Model.Durum;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new SabitMenulerTranslate()
                            {
                                MenuAdi = Model.MenuAdiCeviri[i],
                                Aciklama = Model.AciklamaCeviri[i],
                                BreadcrumbAdi = Model.BreadcrumbSayfaAdiCeviri[i],
                                BreadcrumbAciklama = Model.BreadcrumbAciklamaCeviri[i],
                                Url = Model.UrlCeviri[i],
                                MetaBaslik = Model.MetaBaslikCeviri[i],
                                MetaAnahtar = Model.MetaAnahtarCeviri[i],
                                MetaAciklama = Model.MetaAciklamaCeviri[i],
                                DilId = diller[i].Id,
                                SabitMenuId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }

                        var db = new AppDbContext();
                        db.SabitMenuler.Find(Model.Id).SabitMenulerTranslate.ToList().ForEach(p => db.SabitMenulerTranslate.Remove(p));
                        await db.SaveChangesAsync();

                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        #endregion

                        #region Kapak Resim
                        if (Model.BreadcrumbImage != null)
                        {
                            string imageName = ImageHelper.ImageReplaceName(Model.BreadcrumbImage, Model.BreadcrumbResim);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "SabitSayfalar/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }

                            //string Mappath2 = ImageHelper.ImageMappath2() + "Diller/" + imageName;

                            if (ResimDosyaTipleri.Contains(Model.BreadcrumbImage.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.BreadcrumbImage.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.BreadcrumbImage.CopyTo(stream);
                                }

                                sayfaGuncelle.BreadcrumbResim = Mappath.Remove(0, 7);

                            }

                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaGuncelle.Id;

                                return result;
                            }

                            if (Model.BreadcrumbImage.Length > 5242880)
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                                result.SayfaId = sayfaGuncelle.Id;

                                return result;
                            }

                        }

                        else
                        {
                            sayfaGuncelle.BreadcrumbResim = new AppDbContext().SabitMenuler.Find(sayfaGuncelle.Id).BreadcrumbResim;
                        }
                        #endregion

                        #region Seo Url
                        var seoUrlKontrol = _context.SeoUrl.FirstOrDefault(p => p.EntityId == sayfaGuncelle.Id & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(sayfaGuncelle.SayfaTipi))?.EntityName;

                        db.SeoUrl.Where(p => p.EntityId == sayfaGuncelle.Id & p.EntityName == seoUrlKontrol).ToList().ForEach(p => db.SeoUrl.Remove(p));
                        await db.SaveChangesAsync();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            string url = Model.UrlCeviri[i];
                            if (url == null)
                            {
                                url = "#";
                            }
                            else
                            {
                                url = Replace.UrlSeo(Model.UrlCeviri[i]);
                            }


                            foreach (var item in _context.SeoUrl.ToList())
                            {
                                if (url == item.Url)
                                {
                                    url = url + "-" + sayfaGuncelle.Id;
                                }

                            }

                            SeoTipleri seoTipi = SeoTipleri.Sayfa;

                            if (sayfaGuncelle.SayfaTipi == SabitSayfaTipleri.BizeUlasin)
                            {
                                seoTipi = SeoTipleri.BizeUlasin;
                            }

                            if (sayfaGuncelle.SayfaTipi == SabitSayfaTipleri.Markalar)
                            {
                                seoTipi = SeoTipleri.TumMarkalar;
                            }

                            if (sayfaGuncelle.SayfaTipi == SabitSayfaTipleri.Kategoriler)
                            {
                                seoTipi = SeoTipleri.TumKategoriler;
                            }

                            if (sayfaGuncelle.SayfaTipi == SabitSayfaTipleri.Urunler)
                            {
                                seoTipi = SeoTipleri.TumUrunler;
                            }

                            if (sayfaGuncelle.SayfaTipi == SabitSayfaTipleri.GaleriKategori)
                            {
                                seoTipi = SeoTipleri.GaleriKategorileri;
                            }

                            var seoUrl = new SeoUrl()
                            {
                                Url = url,
                                EntityName = (SeoUrlTipleri)Convert.ToInt32(sayfaGuncelle.SayfaTipi),
                                EntityId = sayfaGuncelle.Id,
                                SeoTipi = seoTipi,
                                DilId = diller[i].Id,
                            };
                            _context.Entry(seoUrl).State = EntityState.Added;

                            var menuUrlGuncelle = _context.MenulerTranslate.Where(p => p.Menuler.EntityId == sayfaGuncelle.Id && p.Menuler.SeoUrlTipi == seoUrlKontrol && p.DilId == diller[i].Id).FirstOrDefault();
                            if (menuUrlGuncelle != null)
                            {
                                menuUrlGuncelle.Url = url;
                                _context.Entry(menuUrlGuncelle).State = EntityState.Modified;
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
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }


            return result;
        }

        public async Task<ResultViewModel> DeletePage(SabitMenuViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.SabitMenuler.Find(Model.Id);
                    _context.Entry(model).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();

                    var diller = _context.Diller.ToList();
                    for (int i = 0; i < diller.Count; i++)
                    {
                        var seoUrl = _context.SeoUrl.FirstOrDefault(p => p.EntityId == Model.Id & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(model.SayfaTipi))?.Url;

                        _context.SeoUrl.Where(p => p.EntityId == model.Id & p.Url == seoUrl).ToList().ForEach(p => _context.SeoUrl.Remove(p));
                        await _context.SaveChangesAsync();
                    }

                    var entityName = _context.SeoUrl.FirstOrDefault(p => p.EntityId == Model.Id & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(Model.SayfaTipi)).EntityName;

                    var menuSil = await MenuHelper.MenuSil(sayfaId: Model.Id, seoTipi: entityName);
                    if (menuSil.Basarilimi == false)
                    {
                        return menuSil;
                    }
                    _context.SaveChanges();
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
                            var model = _context.SabitMenuler.Find(item);

                            _context.Entry(_context.SabitMenuler.Find(item)).State = EntityState.Deleted;
                            var modelresim = _context.SabitMenuler.ToList().Where(p => p.BreadcrumbResim != "/Content/Upload/Images/breadcumbdefault.png").ToList().Find(p => p.Id == item);

                            FileInfo file = new(@"wwwroot" + modelresim?.BreadcrumbResim);
                            if (file.Exists)
                            {
                                file.Delete();
                            }


                            var seoUrl = _context.SeoUrl.FirstOrDefault(p => p.EntityId == model.Id & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(model.SayfaTipi))?.Url;
                            _context.SeoUrl.Where(p => p.EntityId == model.Id & p.Url == seoUrl).ToList().ForEach(p => _context.SeoUrl.Remove(p));
                            await _context.SaveChangesAsync();

                            var entityName = _context.SeoUrl.FirstOrDefault(p => p.EntityId == model.Id & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(model.SayfaTipi))?.EntityName;
                            _context.Menuler.Where(p => p.EntityId == item & p.SeoUrlTipi == entityName).ToList().ForEach(p => _context.Menuler.Remove(p));

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
