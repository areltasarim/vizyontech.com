using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class SayfaFormuServis : ISayfaFormuServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "İletişim Formu";
        public SayfaFormuServis(AppDbContext _context)
        {
            this._context = _context;

        }

        public async Task<List<SayfaFormu>> PageList()
        {
            return (await _context.SayfaFormu.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(SayfaFormuViewModel Model, string submit)
        {

            var result = new ResultViewModel();

            try
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
                var sayfaEkle = new SayfaFormu()
                {
                    Tarih = DateTime.Now,
                    FirmaAdi = Model.FirmaAdi,
                    Ad = Model.Ad,
                    Soyad = Model.Soyad,
                    Telefon = Model.Telefon,
                    Email = Model.Email,
                    Mesaj = Model.Mesaj,
                    GirisTarihi = Model.GirisTarihi,
                    CikisTarihi = Model.CikisTarihi,
                    SayfaFormTipi = Model.SayfaFormTipi,
                    EntityId = Model.EntityId
                };
                #endregion

                #region Kapak Resmi
                if (Model.SayfaResmi != null)
                {


                    string imageName = ImageHelper.ImageReplaceName(Model.SayfaResmi, Model.Resim);

                    string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "SayfaFormu/" + imageName;
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
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu: " + hata.Message;

            }

            return result;

        }

        public async Task<ResultViewModel> DeletePage(SayfaFormuViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.SayfaFormu.Find(Model.Id);
                    _context.Entry(model).State = EntityState.Deleted;
                    var modelresim = _context.SayfaFormu.ToList().Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").ToList().Find(p => p.Id == Model.Id);

                    FileInfo file = new(@"wwwroot" + modelresim?.Resim);
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
                            _context.Entry(_context.SayfaFormu.Find(item)).State = EntityState.Deleted;
                            var modelresim = _context.SayfaFormu.ToList().Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").ToList().Find(p => p.Id == item);

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
