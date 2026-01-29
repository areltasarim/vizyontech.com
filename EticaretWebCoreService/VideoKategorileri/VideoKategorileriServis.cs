using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class VideoKategorileriServis : IVideoKategorileriServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Video Kategorileri";

        public VideoKategorileriServis(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<VideoKategorileri>> PageList()
        {
            var model = await _context.VideoKategorileri.ToListAsync();

            return (model);
        }

        public async Task<ResultViewModel> UpdatePage(VideoKategoriViewModel Model, string submit)
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
                        var sayfaEkle = new VideoKategorileri()
                        {
                            Sira = Model.Sira,
                            Durum = Model.Durum,
                            VideoKategorileriTranslate = new List<VideoKategorileriTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new VideoKategorileriTranslate()
                            {
                                KategoriAdi = Model.KategoriAdiCeviri[i],
                                KisaAciklama = Model.KisaAciklamaCeviri[i],
                                MetaBaslik = Model.MetaBaslikCeviri[i],
                                MetaAnahtar = Model.MetaAnahtarCeviri[i],
                                MetaAciklama = Model.MetaAciklamaCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.VideoKategorileriTranslate.Add(sayfaEkleTranslate);

                        }

                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        #endregion

                        #region Kapak Resmi
                        if (Model.SayfaResmi != null)
                        {
                            var sayfamaxid = _context.VideoKategorileri.DefaultIfEmpty().Max(x => x == null ? 0 : x.Id) + 1;

                            string imageName = ImageHelper.ImageReplaceName(Model.SayfaResmi, Model.Resim);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "VideoKategorileri/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            if (ResimDosyaTipleri.Contains(Model.SayfaResmi.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SayfaResmi.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.SayfaResmi.CopyTo(stream);
                                }

                                sayfaEkle.Resim = Mappath.Remove(0, 7);
                            }

                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaEkle.Id;

                                return result;
                            }

                            if (Model.SayfaResmi.Length > 5242880)
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
                            sayfaEkle.Resim = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                        }
                        #endregion

                        #region Seo Url
                        for (int i = 0; i < diller.Count; i++)
                        {
                            string url = Model.KategoriAdiCeviri[i];
                            if (url == null)
                            {
                                url = "#";
                            }
                            else
                            {
                                url = Replace.UrlSeo(Model.KategoriAdiCeviri[i]);
                            }


                            foreach (var item in _context.SeoUrl.Where(p => p.Diller.DilKoduId == diller[i].Id))
                            {

                                if (url == item.Url)
                                {
                                    url = url + "-" + sayfaEkle.Id;

                                }
                            }

                            var seoUrl = new SeoUrl()
                            {
                                Url = url,
                                EntityName = SeoUrlTipleri.VideoKategorileri,
                                EntityId = sayfaEkle.Id,
                                DilId = diller[i].Id,
                            };

                            _context.Entry(seoUrl).State = EntityState.Added;

                        }
                        await _context.SaveChangesAsync();

                        #endregion

                        #region Sayfa Butonları
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
                        var db = new AppDbContext();
                        db.VideoKategorileri.Find(Model.Id).VideoKategorileriTranslate.ToList().ForEach(p => db.VideoKategorileriTranslate.Remove(p));
                        db.SaveChanges();

                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.VideoKategorileri.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.Sira = Model.Sira;
                        sayfaGuncelle.Durum = Model.Durum;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new VideoKategorileriTranslate()
                            {
                                KategoriAdi = Model.KategoriAdiCeviri[i],
                                KisaAciklama = Model.KisaAciklamaCeviri[i],
                                MetaBaslik = Model.MetaBaslikCeviri[i],
                                MetaAnahtar = Model.MetaAnahtarCeviri[i],
                                MetaAciklama = Model.MetaAciklamaCeviri[i],
                                DilId = diller[i].Id,
                                VideoKategoriId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }

                        #region Kapak Resim
                        if (Model.SayfaResmi != null)
                        {

                            string imageName = ImageHelper.ImageReplaceName(Model.SayfaResmi, Model.Resim);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "VideoKategorileri/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }

                            //string Mappath2 = ImageHelper.ImageMappath2() + "Diller/" + imageName;

                            if (ResimDosyaTipleri.Contains(Model.SayfaResmi.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SayfaResmi.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.SayfaResmi.CopyTo(stream);
                                }

                                sayfaGuncelle.Resim = Mappath.Remove(0, 7);

                            }

                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaGuncelle.Id;

                                return result;
                            }

                            if (Model.SayfaResmi.Length > 5242880)
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
                            sayfaGuncelle.Resim = new AppDbContext().VideoKategorileri.Find(sayfaGuncelle.Id).Resim;
                        }
                        #endregion

                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        #endregion

                        #region Seo Url
                        var seoUrlKontrol = _context.SeoUrl.FirstOrDefault(p => p.EntityId == sayfaGuncelle.Id & p.EntityName == SeoUrlTipleri.VideoKategorileri)?.EntityName;

                        db.SeoUrl.Where(p => p.EntityId == sayfaGuncelle.Id & p.EntityName == seoUrlKontrol).ToList().ForEach(p => db.SeoUrl.Remove(p));
                        await db.SaveChangesAsync();


                        for (int i = 0; i < diller.Count; i++)
                        {
                            string url = Model.KategoriAdiCeviri[i];
                            if (url == null)
                            {
                                url = "#";
                            }
                            else
                            {
                                url = Replace.UrlSeo(Model.KategoriAdiCeviri[i]);
                            }


                            foreach (var item in _context.SeoUrl.Where(p => p.Diller.DilKoduId == diller[i].Id))
                            {
                                if (url == item.Url)
                                {
                                    url = url + "-" + sayfaGuncelle.Id;
                                }

                            }

                            var seoUrl = new SeoUrl()
                            {
                                Url = url,
                                EntityName = SeoUrlTipleri.VideoKategorileri,
                                EntityId = sayfaGuncelle.Id,
                                DilId = diller[i].Id,
                            };
                            _context.Entry(seoUrl).State = EntityState.Added;

                            var menuUrlGuncelle = _context.MenulerTranslate.Where(p=> p.Menuler.EntityId == sayfaGuncelle.Id && p.Menuler.SeoUrlTipi == SeoUrlTipleri.VideoKategorileri && p.DilId == diller[i].Id).FirstOrDefault();
                            if (menuUrlGuncelle != null)
                            {
                                menuUrlGuncelle.Url = url;
                                _context.Entry(menuUrlGuncelle).State = EntityState.Modified;
                            }
                        }
                        await _context.SaveChangesAsync();
                        #endregion

                        #region Sayfa Butonları
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

        public async Task<ResultViewModel> DeletePage(VideoKategoriViewModel Model)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var model = _context.VideoKategorileri.Find(Model.Id);
                    _context.Entry(model).State = EntityState.Deleted;

                    var diller = _context.Diller.ToList();
                    for (int i = 0; i < diller.Count; i++)
                    {

                        var seoUrl = _context.SeoUrl.FirstOrDefault(p => p.EntityId == Model.Id & p.EntityName == SeoUrlTipleri.VideoKategorileri)?.Url;

                        _context.SeoUrl.Where(p => p.EntityId == model.Id & p.Url == seoUrl).ToList().ForEach(p => _context.SeoUrl.Remove(p));
                        await _context.SaveChangesAsync();

                    }

                    await _context.SaveChangesAsync();


                    var menuSil = _context.Menuler.Where(p=> p.EntityId == Model.Id && p.SeoUrlTipi == SeoUrlTipleri.VideoKategorileri).FirstOrDefault();
                    if (menuSil != null)
                    {
                        _context.Entry(menuSil).State = EntityState.Deleted;
                        _context.SaveChanges();
                    }

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

            int pageId = 0;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Deletes != null)
                    {
                        foreach (var item in Deletes)
                        {
                            var model = _context.VideoKategorileri.Find(item);
                            _context.Entry(model).State = EntityState.Deleted;

                            var diller = _context.Diller.ToList();
                            for (int i = 0; i < diller.Count; i++)
                            {

                                var seoUrl = _context.SeoUrl.FirstOrDefault(p => p.EntityId == model.Id & p.EntityName == SeoUrlTipleri.VideoKategorileri)?.Url;
                                _context.SeoUrl.Where(p => p.EntityId == model.Id & p.Url == seoUrl).ToList().ForEach(p => _context.SeoUrl.Remove(p));
                                await _context.SaveChangesAsync();

                                _context.Menuler.Where(p => p.EntityId == item & p.SeoUrlTipi == SeoUrlTipleri.VideoKategorileri).ToList().ForEach(p => _context.Menuler.Remove(p));

                            }
                        }

                        await _context.SaveChangesAsync();

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";
                        result.SayfaId = pageId;
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
