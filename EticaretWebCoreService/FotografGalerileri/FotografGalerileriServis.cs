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

    public partial class FotografGalerileriServis : IFotografGalerileriServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Fotoğraf Galerisi";

        public FotografGalerileriServis(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<FotografGalerileri>> PageList()
        {
            return (await _context.FotografGalerileri.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(FotografGalerisiViewModel Model, GaleriTipleri GaleriTipi, string submit)
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
                        var sayfaEkle = new FotografGalerileri()
                        {
                            GaleriTipi = Model.GaleriTipi,
                            GaleriSayfaTipi = Model.GaleriSayfaTipi,
                            Sira = Model.Sira,
                            SilmeYetkisi = Model.SilmeYetkisi,
                            AdminSolMenu = Model.AdminSolMenu,
                            Durum = Model.Durum,
                            FotografGalerileriTranslate = new List<FotografGalerileriTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new FotografGalerileriTranslate()
                            {
                                GaleriAdi = Model.GaleriAdiCeviri[i],
                                KisaAciklama = Model.KisaAciklamaCeviri[i],
                                MetaBaslik = Model.MetaBaslikCeviri[i],
                                MetaAnahtar = Model.MetaAnahtarCeviri[i],
                                MetaAciklama = Model.MetaAciklamaCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.FotografGalerileriTranslate.Add(sayfaEkleTranslate);
                        }
                        #endregion

                        #region Kapak Resmi
                        if (Model.SayfaResmi != null)
                        {


                            string imageName = ImageHelper.ImageReplaceName(Model.SayfaResmi, Model.Resim);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "FotografGalerileri/" + imageName;
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
                            sayfaEkle.Resim = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim);
                        }
                        #endregion

                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();

                        #region Seo Url
                        for (int i = 0; i < diller.Count; i++)
                        {
                            string url = Model.GaleriAdiCeviri[i];
                            if (url == null)
                            {
                                url = "#";
                            }
                            else
                            {
                                url = Replace.UrlSeo(Model.GaleriAdiCeviri[i]);
                            }


                            foreach (var item in _context.SeoUrl.Where(p => p.Diller.DilKoduId == diller[i].Id))
                            {

                                if (url == item.Url)
                                {
                                    url = url + "-" + sayfaEkle.Id;

                                }
                            }
                            SeoTipleri seoTipi = SeoTipleri.Galeri;

                            if (GaleriTipi == GaleriTipleri.EKatalog)
                            {
                                seoTipi = SeoTipleri.EKatalog;

                            }
                            
                            var seoUrl = new SeoUrl()
                            {
                                Url = url,
                                EntityName = (SeoUrlTipleri)(int)GaleriTipi,
                                SeoTipi = seoTipi,
                                EntityId = sayfaEkle.Id,
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
                        var sayfaGuncelle = _context.FotografGalerileri.Where(p => p.Id == Model.Id).FirstOrDefault();
                        pageId = sayfaGuncelle.Id;

                        sayfaGuncelle.GaleriTipi = Model.GaleriTipi;
                        sayfaGuncelle.GaleriSayfaTipi = Model.GaleriSayfaTipi;
                        sayfaGuncelle.SilmeYetkisi = Model.SilmeYetkisi;
                        sayfaGuncelle.AdminSolMenu = Model.AdminSolMenu;
                        sayfaGuncelle.Sira = Model.Sira;
                        sayfaGuncelle.Durum = Model.Durum;



                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new FotografGalerileriTranslate()
                            {
                                GaleriAdi = Model.GaleriAdiCeviri[i],
                                KisaAciklama = Model.KisaAciklamaCeviri[i],
                                MetaBaslik = Model.MetaBaslikCeviri[i],
                                MetaAnahtar = Model.MetaAnahtarCeviri[i],
                                MetaAciklama = Model.MetaAciklamaCeviri[i],
                                DilId = diller[i].Id,
                                FotografGaleriId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }

                        var db = new AppDbContext();
                        db.FotografGalerileri.Find(Model.Id).FotografGalerileriTranslate.ToList().ForEach(p => db.FotografGalerileriTranslate.Remove(p));
                        await db.SaveChangesAsync();
                        #endregion

                        #region Kapak Resmi
                        if (Model.SayfaResmi != null)
                        {

                            string imageName = ImageHelper.ImageReplaceName(Model.SayfaResmi, Model.Resim);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "FotografGalerileri/" + imageName;
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
                            sayfaGuncelle.Resim = new AppDbContext().FotografGalerileri.Find(sayfaGuncelle.Id).Resim;
                        }
                        #endregion

                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        #region Seo Url
                        var seoUrlKontrol = _context.SeoUrl.FirstOrDefault(p => p.EntityId == sayfaGuncelle.Id & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(sayfaGuncelle.GaleriTipi))?.EntityName;

                        db.SeoUrl.Where(p => p.EntityId == sayfaGuncelle.Id & p.EntityName == seoUrlKontrol).ToList().ForEach(p => db.SeoUrl.Remove(p));
                        await db.SaveChangesAsync();


                        for (int i = 0; i < diller.Count; i++)
                        {
                            string url = Model.GaleriAdiCeviri[i];
                            if (url == null)
                            {
                                url = "#";
                            }
                            else
                            {
                                url = Replace.UrlSeo(Model.GaleriAdiCeviri[i]);
                            }


                            foreach (var item in _context.SeoUrl.Where(p => p.Diller.DilKoduId == diller[i].Id))
                            {
                                if (url == item.Url)
                                {
                                    url = url + "-" + sayfaGuncelle.Id;
                                }

                            }

                            SeoTipleri seoTipi = SeoTipleri.Galeri;

                            if (GaleriTipi == GaleriTipleri.EKatalog)
                            {
                                seoTipi = SeoTipleri.EKatalog;

                            }

                            var seoUrl = new SeoUrl()
                            {
                                Url = url,
                                EntityName = (SeoUrlTipleri)(int)GaleriTipi,
                                SeoTipi = seoTipi,
                                EntityId = sayfaGuncelle.Id,
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
                result.SayfaId = pageId;

            }

            return result;


        }

        public async Task<ResultViewModel> DeletePage(FotografGalerisiViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.FotografGalerileri.Find(Model.Id);
                    _context.Entry(model).State = EntityState.Deleted;
                    var diller = _context.Diller.ToList();
                    for (int i = 0; i < diller.Count; i++)
                    {

                        var seoUrl = _context.SeoUrl.FirstOrDefault(p => p.EntityId == Model.Id & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(model.GaleriTipi))?.Url;

                        _context.SeoUrl.Where(p => p.EntityId == model.Id & p.Url == seoUrl).ToList().ForEach(p => _context.SeoUrl.Remove(p));
                        await _context.SaveChangesAsync();

                        List<FotografGaleriResimleri> cokluresim = _context.FotografGaleriResimleri.ToList();

                        foreach (var item in cokluresim.Where(p => p.FotografGaleriId == model.Id))
                        {
                            FileInfo files = new(@"wwwroot" + item.Resim);
                            if (files.Exists)
                            {
                                files.Delete();
                            }
                        }
                    }

                    await _context.SaveChangesAsync();

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

                            var model = _context.FotografGalerileri.Find(item);


                            _context.Entry(_context.FotografGalerileri.Find(item)).State = EntityState.Deleted;

                            List<FotografGaleriResimleri> cokluresim = _context.FotografGaleriResimleri.ToList();

                            foreach (var item2 in cokluresim.Where(p => p.FotografGaleriId == item))
                            {
                                FileInfo files = new(@"wwwroot" + item2.Resim);
                                if (files.Exists)
                                {
                                    files.Delete();
                                }
                            }

                            var diller = _context.Diller.ToList();
                            for (int i = 0; i < diller.Count; i++)
                            {

                                var seoUrl = _context.SeoUrl.FirstOrDefault(p => p.EntityId == model.Id & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(model.GaleriTipi))?.Url;
                                _context.SeoUrl.Where(p => p.EntityId == model.Id & p.Url == seoUrl).ToList().ForEach(p => _context.SeoUrl.Remove(p));
                                await _context.SaveChangesAsync();
                            }
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
                        var model = _context.FotografGaleriResimleri.Find(Convert.ToInt32(item));
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

                    var model = _context.FotografGaleriResimleri.ToList().Find(p => p.Id == id);

                    _context.Entry(model).State = EntityState.Deleted;
                    FileInfo file = new(@"wwwroot" + model.Resim);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

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
    }
}
