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

    public partial class AdresBilgileriServis : IAdresBilgileriServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Adres Bilgisi";

        public AdresBilgileriServis(AppDbContext _context)
        {
            this._context = _context;
        }



        public async Task<List<AdresBilgileri>> PageList()
        {
            return (await _context.AdresBilgileri.ToListAsync());
        }


        public async Task<ResultViewModel> UpdatePage(AdresBilgileriViewModel Model, string submit)
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
                        var sayfaEkle = new AdresBilgileri()
                        {
                            SiteAyarId = Model.SiteAyarId,
                            Sira = Model.Sira,
                            AdresBilgileriTranslate = new List<AdresBilgileriTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new AdresBilgileriTranslate()
                            {
                                AdresBasligi = Model.AdresBasligiCeviri[i],
                                Telefon = Model.TelefonCeviri[i],
                                Faks = Model.FaksCeviri[i],
                                Gsm = Model.GsmCeviri[i],
                                Email = Model.EmailCeviri[i],
                                Adres = Model.AdresCeviri[i],
                                Harita = Model.HaritaCeviri[i],
                                HaritaLink = Model.HaritaLinkCeviri[i],
                                CalismaSaatlari = Model.CalismaSaatlariCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.AdresBilgileriTranslate.Add(sayfaEkleTranslate);

                        }
                        #endregion

                        #region Kapak Resmi
                        if (Model.MagazaResim != null)
                        {


                            string imageName = ImageHelper.ImageReplaceName(Model.MagazaResim, Model.Resim);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Magazalar/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            if (ResimDosyaTipleri.Contains(Model.MagazaResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.MagazaResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.MagazaResim.CopyTo(stream);
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

                            if (Model.MagazaResim.Length > 5242880)
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
                        var sayfaGuncelle = _context.AdresBilgileri.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.SiteAyarId = Model.SiteAyarId;
                        sayfaGuncelle.Sira = Model.Sira;


                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new AdresBilgileriTranslate()
                            {
                                AdresBasligi = Model.AdresBasligiCeviri[i],
                                Telefon = Model.TelefonCeviri[i],
                                Faks = Model.FaksCeviri[i],
                                Gsm = Model.GsmCeviri[i],
                                Email = Model.EmailCeviri[i],
                                Adres = Model.AdresCeviri[i],
                                Harita = Model.HaritaCeviri[i],
                                HaritaLink = Model.HaritaLinkCeviri[i],
                                CalismaSaatlari = Model.CalismaSaatlariCeviri[i],
                                DilId = diller[i].Id,
                                AdresBilgiId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }

                        var db = new AppDbContext();
                        db.AdresBilgileri.Find(Model.Id).AdresBilgileriTranslate.ToList().ForEach(p => db.AdresBilgileriTranslate.Remove(p));
                        await db.SaveChangesAsync();
                        #endregion

                        #region Kapak Resim
                        if (Model.MagazaResim != null)
                        {
                           
                            string imageName = ImageHelper.ImageReplaceName(Model.MagazaResim, Model.Resim);

                            string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Magazalar/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }

                            //string Mappath2 = ImageHelper.ImageMappath2() + "Magazalar/" + imageName;

                            if (ResimDosyaTipleri.Contains(Model.MagazaResim.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.MagazaResim.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.MagazaResim.CopyTo(stream);
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

                            if (Model.MagazaResim.Length > 5242880)
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
                            sayfaGuncelle.Resim = new AppDbContext().AdresBilgileri.Find(sayfaGuncelle.Id).Resim;
                        }
                        #endregion

                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
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

            }



            return result;


        }

        public async Task<ResultViewModel> DeletePage(AdresBilgileriViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.AdresBilgileri.Find(Model.Id);
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

        public async Task<ResultViewModel> DeleteAllPage(AdresBilgileriViewModel Model, int[] Deletes)
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
                            _context.Entry(_context.AdresBilgileri.Find(item)).State = EntityState.Deleted;

                        }
                    }

                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";

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
