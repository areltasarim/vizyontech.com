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

    public partial class DosyalarServis : IDosyalarServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Dosya";

        public DosyalarServis(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<Dosyalar>> PageList(DosyaSayfaTipleri SayfaTipi)
        {
            return (await _context.Dosyalar.ToListAsync());
        }


        public async Task<ResultViewModel> UpdatePage(DosyaViewModel Model, string submit)
        {

            var result = new ResultViewModel();
            try
            {
               
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        var sayfaEkle = new Dosyalar()
                        {
                            DosyaKategoriId = Model.DosyaKategoriId,
                            Sira = Model.Sira,
                            DosyalarTranslate = new List<DosyalarTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new DosyalarTranslate()
                            {
                                DosyaAdi = Model.DosyaAdiCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.DosyalarTranslate.Add(sayfaEkleTranslate);

                        }

                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();

                        #endregion



                        try
                        {
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
                        catch
                        {
                            result.Basarilimi = false;
                            result.MesajDurumu = "danger";
                            result.Mesaj = "Hata Oluştu.";
                        }
                    }

                    else
                    {
                        #region Sayfa Güncelleme

                         var db = new AppDbContext();
                        db.Dosyalar.Find(Model.Id).DosyalarTranslate.ToList().ForEach(p => db.DosyalarTranslate.Remove(p));
                        await db.SaveChangesAsync();

                        var sayfaGuncelle = _context.Dosyalar.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.Sira = Model.Sira;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new DosyalarTranslate()
                            {
                                DosyaAdi = Model.DosyaAdiCeviri[i],
                                DilId = diller[i].Id,
                                DosyaId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }

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
        public async Task<ResultViewModel> DeletePage(DosyaViewModel Model)
        {
            var result = new ResultViewModel();

            var model = _context.Dosyalar.Find(Model.Id);

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    _context.Entry(model).State = EntityState.Deleted;
                    var diller = _context.Diller.ToList();
                    for (int i = 0; i < diller.Count; i++)
                    {
                        FileInfo file = new(@"wwwroot" + model.DosyalarTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == diller[i].DilKodlari.DilKodu)?.Dosya);
                        if (file.Exists)
                        {
                            file.Delete();
                        }


                        await _context.SaveChangesAsync();
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

            var sayfaTipi = "";
            int pageId = 0;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {


                    if (Deletes != null)
                    {
                        foreach (var item in Deletes)
                        {
                            var model = _context.Dosyalar.Find(item);

                            var diller = _context.Diller.ToList();
                            for (int i = 0; i < diller.Count; i++)
                            {
                                FileInfo file = new(@"wwwroot" + model.DosyalarTranslate.Where(p => p.Dosya != "#").SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == diller[i].DilKodlari.DilKodu)?.Dosya);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }

                                _context.Entry(model).State = EntityState.Deleted;
                            }

                        }
                    }
                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";
                    result.SayfaId = pageId;
                    result.SayfaUrl = sayfaTipi;

                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";
                result.SayfaId = pageId;
                result.SayfaUrl = sayfaTipi;

            }

            return result;
        }

        public async Task<ResultViewModel> FilesSortOrder(string sira)
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
                        var model = _context.DosyaGaleri.Find(Convert.ToInt32(item));
                        model.Sira = ss;
                        _context.Entry(model).State = EntityState.Modified;
                        ss++;
                    }
                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Dosyalar Sıralandı.";

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

        public async Task<ResultViewModel> FilesDelete(int id)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var model = _context.DosyaGaleri.ToList().Find(p => p.Id == id);

                    _context.Entry(model).State = EntityState.Deleted;
                    //FileInfo file = new(@"wwwroot" + model.Dosya);
                    //if (file.Exists)
                    //{
                    //    file.Delete();
                    //}

                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Dosya silindi.";
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
