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

    public partial class DosyaKategorileriServis : IDosyaKategorileriServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Dosya Kategorileri";

        public DosyaKategorileriServis(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<DosyaKategorileri>> PageList()
        {
            var model = await _context.DosyaKategorileri.ToListAsync();

            return (model);
        }

        public async Task<ResultViewModel> UpdatePage(DosyaKategoriViewModel Model, string submit)
        {

            var result = new ResultViewModel();

            try
            {
                
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                   
                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        var sayfaEkle = new DosyaKategorileri()
                        {
                            Sira = Model.Sira,
                            Durum = Model.Durum,
                            DosyaKategorileriTranslate = new List<DosyaKategorileriTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new DosyaKategorileriTranslate()
                            {
                                KategoriAdi = Model.KategoriAdiCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.DosyaKategorileriTranslate.Add(sayfaEkleTranslate);

                        }

                        _context.Entry(sayfaEkle).State = EntityState.Added;
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
                        db.DosyaKategorileri.Find(Model.Id).DosyaKategorileriTranslate.ToList().ForEach(p => db.DosyaKategorileriTranslate.Remove(p));
                        db.SaveChanges();

                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.DosyaKategorileri.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.Sira = Model.Sira;
                        sayfaGuncelle.Durum = Model.Durum;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new DosyaKategorileriTranslate()
                            {
                                KategoriAdi = Model.KategoriAdiCeviri[i],
                                DilId = diller[i].Id,
                                DosyaKategoriId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }

                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
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

        public async Task<ResultViewModel> DeletePage(DosyaKategoriViewModel Model)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var model = _context.DosyaKategorileri.Find(Model.Id);
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

            int pageId = 0;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Deletes != null)
                    {
                        foreach (var item in Deletes)
                        {
                            var model = _context.DosyaKategorileri.Find(item);
                            _context.Entry(model).State = EntityState.Deleted;
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
