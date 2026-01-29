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

    public partial class FormBasliklariServis : IFormBasliklariServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Form Başlığı";

        public FormBasliklariServis(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<FormBasliklari>> PageList(int SayfaId)
        {
            return (await _context.FormBasliklari.Where(p=> p.SayfaId == SayfaId).ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(FormBaslikViewModel Model, string submit)
        {

            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        var sayfaEkle = new FormBasliklari()
                        {
                            SayfaId = Model.SayfaId,
                            Sira = Model.Sira,
                            Durum = Model.Durum,
                            FormBasliklariTranslate = new List<FormBasliklariTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new FormBasliklariTranslate()
                            {
                                FormBasligi = Model.FormBasligiCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.FormBasliklariTranslate.Add(sayfaEkleTranslate);

                        }

                        _context.Entry(sayfaEkle).State = EntityState.Added;
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
                        var db = new AppDbContext();
                        db.FormBasliklari.Find(Model.Id).FormBasliklariTranslate.ToList().ForEach(p => db.FormBasliklariTranslate.Remove(p));
                        db.SaveChanges();

                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.FormBasliklari.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.SayfaId = Model.SayfaId;
                        sayfaGuncelle.Sira = Model.Sira;
                        sayfaGuncelle.Durum = Model.Durum;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new FormBasliklariTranslate()
                            {
                                FormBasligi = Model.FormBasligiCeviri[i],
                                DilId = diller[i].Id,
                                FormBaslikId = Model.Id
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

        public async Task<ResultViewModel> DeletePage(FormBaslikViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var model = _context.FormBasliklari.Find(Model.Id);
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
                            var model = _context.FormBasliklari.Find(item);
                            _context.Entry(model).State = EntityState.Deleted;
                        }
                    }
                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";
                    result.SayfaId = pageId;

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

    }
}
