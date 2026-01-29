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

    public partial class SiteAyarlariServis : ISiteAyarlariServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Site Ayarları";

        public SiteAyarlariServis(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<SiteAyarlari>> PageList()
        {
            return (await _context.SiteAyarlari.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(SiteAyariViewModel Model, string submit)
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
                        var sayfaEkle = new SiteAyarlari()
                        {
                            FirmaAdi = Model.FirmaAdi,
                            AktifDilId = Model.AktifDilId,
                            ParaBirimId = Model.ParaBirimId,
                            Facebook = Model.Facebook,
                            Instagram = Model.Instagram,
                            Twitter = Model.Twitter,
                            Linkedin = Model.Linkedin,
                            Pinterest = Model.Pinterest,
                            GooglePlus = Model.GooglePlus,
                            Youtube = Model.Youtube,
                            Whatsapp = Model.Whatsapp,
                            BodyKod = Model.BodyKod,
                            HeaderKod = Model.HeaderKod,
                            FooterKod = Model.FooterKod,
                            PopupDurum = Model.PopupDurum,
                            MailTipi = Model.MailTipi,
                            ExchangeVersiyon = Model.ExchangeVersiyon,
                            EmailHost = Model.EmailHost,
                            EmailAdresi = Model.EmailAdresi,
                            EmailSifre = Model.EmailSifre,
                            EmailPort = Model.EmailPort,
                            EmailSSL = Model.EmailSSL,
                            MailBaslik = Model.MailBaslik,
                            MailKonu = Model.MailKonu,
                            MailGonderildiMesaji = Model.MailGonderildiMesaji,
                            GonderilecekMail = Model.GonderilecekMail,
                            SinirsiKategoriDurum = Model.SinirsiKategoriDurum,
                            SiteAyarlariTranslate = new List<SiteAyarlariTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;



                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new SiteAyarlariTranslate()
                            {
                                MetaBaslik = Model.MetaBaslikCeviri[i],
                                MetaAnahtar = Model.MetaAnahtarCeviri[i],
                                MetaAciklama = Model.MetaAciklamaCeviri[i],
                                HeaderAciklama = Model.HeaderAciklamaCeviri[i],
                                FooterAciklama = Model.FooterAciklamaCeviri[i],
                                Popup = Model.PopupCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.SiteAyarlariTranslate.Add(sayfaEkleTranslate);

                        }

                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        #region Ust Logo Resmi

                        
                        if (Model.UstLogoResim != null)
                        {




                            string imageName = ImageHelper.ImageReplaceName(Model.UstLogoResim,Model.UstLogo);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Logo/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            if (ResimDosyaTipleri.Contains(Model.UstLogoResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UstLogoResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.UstLogoResim.CopyTo(stream);
                                }

                                sayfaEkle.UstLogo = Mappath.Remove(0, 7);
                            }
                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaEkle.Id;

                                return result;
                            }

                            if (Model.UstLogoResim.Length > 5242880)
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
                            sayfaEkle.UstLogo = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                        }
                        #endregion

                        #region Mobil Logo Resmi
                        if (Model.MobilLogoResim != null)
                        {

                            string imageName = ImageHelper.ImageReplaceName(Model.MobilLogoResim, Model.MobilLogo);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Logo/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            if (ResimDosyaTipleri.Contains(Model.MobilLogoResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UstLogoResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.MobilLogoResim.CopyTo(stream);
                                }

                                sayfaEkle.MobilLogo = Mappath.Remove(0, 7);
                            }
                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";

                                return result;
                            }

                            if (Model.MobilLogoResim.Length > 5242880)
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";

                                return result;
                            }
                        }

                        else
                        {
                            sayfaEkle.MobilLogo = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                        }
                        #endregion

                        #region Footer Logo Resmi
                        if (Model.FooterLogoResim != null)
                        {
                            string imageName = ImageHelper.ImageReplaceName(Model.FooterLogoResim,Model.FooterLogo);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Logo/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            if (ResimDosyaTipleri.Contains(Model.FooterLogoResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UstLogoResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.FooterLogoResim.CopyTo(stream);
                                }

                                sayfaEkle.FooterLogo = Mappath.Remove(0, 7);
                            }
                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";

                                return result;
                            }

                            if (Model.FooterLogoResim.Length > 5242880)
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";

                                return result;
                            }
                        }

                        else
                        {
                            sayfaEkle.FooterLogo = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                        }
                        #endregion

                        #region Favicon Resmi
                        if (Model.FaviconResim != null)
                        {
                            string imageName = ImageHelper.ImageReplaceName(Model.FaviconResim, Model.Favicon);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Logo/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            if (ResimDosyaTipleri.Contains(Model.FaviconResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UstLogoResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.FaviconResim.CopyTo(stream);
                                }

                                sayfaEkle.Favicon = Mappath.Remove(0, 7);
                            }
                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";

                                return result;
                            }

                            if (Model.FaviconResim.Length > 5242880)
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";

                                return result;
                            }
                        }

                        else
                        {
                            sayfaEkle.Favicon = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                        }
                        #endregion

                        #region Mail Logo Resim
                        if (Model.MailLogoResim != null)
                        {
                            string imageName = ImageHelper.ImageReplaceName(Model.MailLogoResim, Model.MailLogo);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Logo/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            if (ResimDosyaTipleri.Contains(Model.MailLogoResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UstLogoResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.MailLogoResim.CopyTo(stream);
                                }

                                sayfaEkle.MailLogo = Mappath.Remove(0, 7);
                            }
                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";

                                return result;
                            }

                            if (Model.MailLogoResim.Length > 5242880)
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";

                                return result;
                            }
                        }

                        else
                        {
                            sayfaEkle.Favicon = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                        }
                        #endregion

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
                        var sayfaGuncelle = _context.SiteAyarlari.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.AktifDilId = Model.AktifDilId;
                        sayfaGuncelle.ParaBirimId = Model.ParaBirimId;
                        sayfaGuncelle.FirmaAdi = Model.FirmaAdi;
                        sayfaGuncelle.Facebook = Model.Facebook;
                        sayfaGuncelle.Instagram = Model.Instagram;
                        sayfaGuncelle.Twitter = Model.Twitter;
                        sayfaGuncelle.Linkedin = Model.Linkedin;
                        sayfaGuncelle.Pinterest = Model.Pinterest;
                        sayfaGuncelle.GooglePlus = Model.GooglePlus;
                        sayfaGuncelle.Youtube = Model.Youtube;
                        sayfaGuncelle.Whatsapp = Model.Whatsapp;
                        sayfaGuncelle.BodyKod = Model.BodyKod;
                        sayfaGuncelle.HeaderKod = Model.HeaderKod;
                        sayfaGuncelle.FooterKod = Model.FooterKod;
                        sayfaGuncelle.PopupDurum = Model.PopupDurum;
                        sayfaGuncelle.MailTipi = Model.MailTipi;
                        sayfaGuncelle.ExchangeVersiyon = Model.ExchangeVersiyon;
                        sayfaGuncelle.EmailHost = Model.EmailHost;
                        sayfaGuncelle.EmailAdresi = Model.EmailAdresi;
                        sayfaGuncelle.EmailSifre = Model.EmailSifre;
                        sayfaGuncelle.EmailPort = Model.EmailPort;
                        sayfaGuncelle.EmailSSL = Model.EmailSSL;
                        sayfaGuncelle.MailBaslik = Model.MailBaslik;
                        sayfaGuncelle.MailKonu = Model.MailKonu;
                        sayfaGuncelle.MailGonderildiMesaji = Model.MailGonderildiMesaji;
                        sayfaGuncelle.GonderilecekMail = Model.GonderilecekMail;
                        sayfaGuncelle.SinirsiKategoriDurum = Model.SinirsiKategoriDurum;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new SiteAyarlariTranslate()
                            {
                                MetaBaslik = Model.MetaBaslikCeviri[i],
                                MetaAnahtar = Model.MetaAnahtarCeviri[i],
                                MetaAciklama = Model.MetaAciklamaCeviri[i],
                                HeaderAciklama = Model.HeaderAciklamaCeviri[i],
                                FooterAciklama = Model.FooterAciklamaCeviri[i],
                                Popup = Model.PopupCeviri[i],
                                DilId = diller[i].Id,
                                SiteAyarId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }

                        var db = new AppDbContext();
                        db.SiteAyarlari.Find(Model.Id).SiteAyarlariTranslate.ToList().ForEach(p => db.SiteAyarlariTranslate.Remove(p));
                        await db.SaveChangesAsync();

                        #region Ust Logo Resim

                       
                        if (Model.UstLogoResim != null)
                        {

                            string imageName = ImageHelper.ImageReplaceName(Model.UstLogoResim,Model.UstLogo);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Logo/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            //string Mappath2 = ImageHelper.ImageMappath2() + "SiteAyarlari/" + imageName;


                            if (ResimDosyaTipleri.Contains(Model.UstLogoResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UstLogoResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.UstLogoResim.CopyTo(stream);
                                }

                                sayfaGuncelle.UstLogo = Mappath.Remove(0, 7);

                            }
                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaGuncelle.Id;

                                return result;
                            }

                            if (Model.UstLogoResim.Length > 5242880)
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
                            sayfaGuncelle.UstLogo = new AppDbContext().SiteAyarlari.Find(sayfaGuncelle.Id).UstLogo;
                        }
                        #endregion
                        #region Mobil Logo Resim

                        if (Model.MobilLogoResim != null)
                        {
                            string imageName = ImageHelper.ImageReplaceName(Model.MobilLogoResim, Model.MobilLogo);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Logo/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            //string Mappath2 = ImageHelper.ImageMappath2() + "SiteAyarlari/" + imageName;

                            if (ResimDosyaTipleri.Contains(Model.MobilLogoResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UstLogoResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.MobilLogoResim.CopyTo(stream);
                                }

                                sayfaGuncelle.MobilLogo = Mappath.Remove(0, 7);
                            }
                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaGuncelle.Id;

                                return result;
                            }

                            if (Model.MobilLogoResim.Length > 5242880)
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
                            sayfaGuncelle.MobilLogo = new AppDbContext().SiteAyarlari.Find(sayfaGuncelle.Id).MobilLogo;
                        }
                        #endregion
                        #region Footer Logo Resim

                        if (Model.FooterLogoResim != null)
                        {
                            string imageName = ImageHelper.ImageReplaceName(Model.FooterLogoResim, Model.FooterLogo);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Logo/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            //string Mappath2 = ImageHelper.ImageMappath2() + "SiteAyarlari/" + imageName;

                            if (ResimDosyaTipleri.Contains(Model.FooterLogoResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UstLogoResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.FooterLogoResim.CopyTo(stream);
                                }

                                sayfaGuncelle.FooterLogo = Mappath.Remove(0, 7);
                            }
                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaGuncelle.Id;

                                return result;
                            }

                            if (Model.FooterLogoResim.Length > 5242880)
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
                            sayfaGuncelle.FooterLogo = new AppDbContext().SiteAyarlari.Find(sayfaGuncelle.Id).FooterLogo;
                        }
                        #endregion
                        #region Favicon Resim

                        if (Model.FaviconResim != null)
                        {
                            string imageName = ImageHelper.ImageReplaceName(Model.FaviconResim, Model.Favicon);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Logo/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            //string Mappath2 = ImageHelper.ImageMappath2() + "SiteAyarlari/" + imageName;

                            if (ResimDosyaTipleri.Contains(Model.FaviconResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UstLogoResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.FaviconResim.CopyTo(stream);
                                }

                                sayfaGuncelle.Favicon = Mappath.Remove(0, 7);
                            }
                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaGuncelle.Id;

                                return result;
                            }

                            if (Model.FaviconResim.Length > 5242880)
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
                            sayfaGuncelle.Favicon = new AppDbContext().SiteAyarlari.Find(sayfaGuncelle.Id).Favicon;
                        }
                        #endregion

                        #region Mail Logo Resim

                        if (Model.MailLogoResim != null)
                        {
                            string imageName = ImageHelper.ImageReplaceName(Model.MailLogoResim, Model.MailLogo);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Logo/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            //string Mappath2 = ImageHelper.ImageMappath2() + "SiteAyarlari/" + imageName;

                            if (ResimDosyaTipleri.Contains(Model.MailLogoResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UstLogoResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.MailLogoResim.CopyTo(stream);
                                }

                                sayfaGuncelle.MailLogo = Mappath.Remove(0, 7);
                            }
                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaGuncelle.Id;

                                return result;
                            }

                            if (Model.MailLogoResim.Length > 5242880)
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
                            sayfaGuncelle.MailLogo = new AppDbContext().SiteAyarlari.Find(sayfaGuncelle.Id).MailLogo;
                        }
                        #endregion
                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
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

        public async Task<ResultViewModel> DeletePage(SiteAyariViewModel Model)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var model = _context.SiteAyarlari.Find(Model.Id);
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
                            _context.Entry(_context.SiteAyarlari.Find(item)).State = EntityState.Deleted;
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
