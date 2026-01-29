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

    public partial class EkiplerServis : IEkiplerServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Ekip";

        public EkiplerServis(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<Ekipler>> PageList()
        {
            return (await _context.Ekipler.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(EkipViewModel Model, string submit)
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
                        var sayfaEkle = new Ekipler()
                        {
                            Kategori = Model.Kategori,
                            AdSoyad = Model.AdSoyad,
                            Email = Model.Email,
                            WebSite = Model.WebSite,
                            Facebook = Model.Facebook,
                            Instagram = Model.Instagram,
                            Twitter = Model.Twitter,
                            Linkedin = Model.Linkedin,
                            Pinterest = Model.Pinterest,
                            GooglePlus = Model.GooglePlus,
                            Youtube = Model.Youtube,
                            Whatsapp = Model.Whatsapp,
                            Sira = Model.Sira,
                            Durum = Model.Durum,
                            EkiplerTranslate = new List<EkiplerTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new EkiplerTranslate()
                            {
                                Gorev = Model.GorevCeviri[i],
                                Aciklama = Model.AciklamaCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.EkiplerTranslate.Add(sayfaEkleTranslate);

                        }

                        #region Kapak Resmi
                        if (Model.EkipResim != null)
                        {

                            string imageName = ImageHelper.ImageReplaceName(Model.EkipResim, Model.Resim);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Ekipler/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            if (ResimDosyaTipleri.Contains(Model.EkipResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.EkipResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.EkipResim.CopyTo(stream);
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

                            if (Model.EkipResim.Length > 5242880)
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
                            sayfaEkle.Resim = ImageHelper.DosyaYolu(DosyaYoluTipleri.Dosya);
                        }
                        #endregion

                        #region Logo Resmi
                        if (Model.LogoResim != null)
                        {

                            string imageName = ImageHelper.ImageReplaceName(Model.LogoResim, Model.Resim);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Ekipler/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            if (ResimDosyaTipleri.Contains(Model.LogoResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.LogoResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.LogoResim.CopyTo(stream);
                                }

                                sayfaEkle.Logo = Mappath.Remove(0, 7);
                            }

                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaEkle.Id;

                                return result;
                            }

                            if (Model.LogoResim.Length > 5242880)
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
                            sayfaEkle.Logo = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                        }
                        #endregion
                        _context.Entry(sayfaEkle).State = EntityState.Added;
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
                        var sayfaGuncelle = _context.Ekipler.Where(p => p.Id == Model.Id).FirstOrDefault();
                        pageId = sayfaGuncelle.Id;

                        sayfaGuncelle.Kategori = Model.Kategori;
                        sayfaGuncelle.WebSite = Model.WebSite;
                        sayfaGuncelle.Email = Model.Email;
                        sayfaGuncelle.AdSoyad = Model.AdSoyad;
                        sayfaGuncelle.Facebook = Model.Facebook;
                        sayfaGuncelle.Instagram = Model.Instagram;
                        sayfaGuncelle.Twitter = Model.Twitter;
                        sayfaGuncelle.Linkedin = Model.Linkedin;
                        sayfaGuncelle.Pinterest = Model.Pinterest;
                        sayfaGuncelle.GooglePlus = Model.GooglePlus;
                        sayfaGuncelle.Youtube = Model.Youtube;
                        sayfaGuncelle.Whatsapp = Model.Whatsapp;
                        sayfaGuncelle.Sira = Model.Sira;
                        sayfaGuncelle.Durum = Model.Durum;


                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new EkiplerTranslate()
                            {
                                Gorev = Model.GorevCeviri[i],
                                Aciklama = Model.AciklamaCeviri[i],
                                DilId = diller[i].Id,
                                EkipId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }

                        var db = new AppDbContext();
                        db.Ekipler.Find(Model.Id).EkiplerTranslate.ToList().ForEach(p => db.EkiplerTranslate.Remove(p));
                        await db.SaveChangesAsync();

                        #region Kapak Resim
                        if (Model.EkipResim != null)
                        {
                            string imageName = ImageHelper.ImageReplaceName(Model.EkipResim, Model.Resim);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Ekipler/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }

                            //string Mappath2 = ImageHelper.ImageMappath2() + "Diller/" + imageName;

                            if (ResimDosyaTipleri.Contains(Model.EkipResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.EkipResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.EkipResim.CopyTo(stream);
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

                            if (Model.EkipResim.Length > 5242880)
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
                            sayfaGuncelle.Resim = new AppDbContext().Ekipler.Find(sayfaGuncelle.Id).Resim;
                        }
                        #endregion

                        #region Logo
                        if (Model.LogoResim != null)
                        {
                            string imageName = ImageHelper.ImageReplaceName(Model.LogoResim, Model.Resim);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Ekipler/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }

                            //string Mappath2 = ImageHelper.ImageMappath2() + "Diller/" + imageName;

                            if (ResimDosyaTipleri.Contains(Model.LogoResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.LogoResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.LogoResim.CopyTo(stream);
                                }

                                sayfaGuncelle.Logo = Mappath.Remove(0, 7);

                            }

                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaGuncelle.Id;

                                return result;
                            }

                            if (Model.LogoResim.Length > 5242880)
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
                            sayfaGuncelle.Logo = new AppDbContext().Ekipler.Find(sayfaGuncelle.Id).Logo;
                        }
                        #endregion
                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;

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

        public async Task<ResultViewModel> DeletePage(EkipViewModel Model)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var model = _context.Ekipler.Find(Model.Id);
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
                            var model = _context.Ekipler.Find(item);

                            _context.Entry(_context.Ekipler.Find(item)).State = EntityState.Deleted;
                            var modelresim = _context.Ekipler.ToList().Where(p => p.Resim != "/Content/Upload/Images/Ekipler/resimyok.png").ToList().Find(p => p.Id == item);

                            FileInfo file = new(@"wwwroot" + modelresim?.Resim);
                            if (file.Exists)
                            {
                                file.Delete();
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

    }
}
