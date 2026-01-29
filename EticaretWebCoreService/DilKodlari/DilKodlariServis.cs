using EticaretWebCoreEntity;
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

    public partial class DilKodlariServis : IDilKodlariServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Dil Kodu";

        public DilKodlariServis(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<DilKodlari>> PageList()
        {
            return (await _context.DilKodlari.ToListAsync());
        }


        public async Task<ResultViewModel> UpdatePage(DilKodlariViewModel Model, string submit)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    if (Model.Id == 0)
                    {

                        var sayfaEkle = new DilKodlari()
                        {
                            DilKodu = Model.DilKodu,
                        };

                        _context.Entry(sayfaEkle).State = EntityState.Added;

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
                        var sayfaGuncelle = _context.DilKodlari.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.DilKodu = Model.DilKodu;

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

        public async Task<ResultViewModel> DeletePage(DilKodlariViewModel Model)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.DilKodlari.Find(Model.Id);
                    _context.Entry(model).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";

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

        public async Task<ResultViewModel> DeleteAllPage(DilViewModel Model, int[] Deletes)
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
                            _context.Entry(_context.DilKodlari.Find(item)).State = EntityState.Deleted;
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
