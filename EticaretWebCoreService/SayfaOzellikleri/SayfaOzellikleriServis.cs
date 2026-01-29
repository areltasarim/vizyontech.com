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

    public partial class SayfaOzellikleriServis : ISayfaOzellikleriServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Sayfa Özellik";

        public SayfaOzellikleriServis(AppDbContext _context)
        {
            this._context = _context;
        }
        public async Task<List<SayfaOzellikleri>> PageList()
        {
            return (await _context.SayfaOzellikleri.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(SayfaOzellikViewModel Model, string submit)
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
                        var sayfaEkle = new SayfaOzellikleri()
                        {
                            SayfaOzellikGrupId = Model.SayfaOzellik.SayfaOzellikGrupId,
                            Sira = Model.SayfaOzellik.Sira,
                            SayfaOzellikleriTranslate = new List<SayfaOzellikleriTranslate>(),
                        };

                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        foreach (var item in Model.SayfaOzellik.SayfaOzellikleriTranslate)
                        {
                            var sayfaEkleTranslate = new SayfaOzellikleriTranslate()
                            {
                                OzellikAdi = item.OzellikAdi,
                                DilId = item.DilId
                            };
                            sayfaEkle.SayfaOzellikleriTranslate.Add(sayfaEkleTranslate);
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

                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.SayfaOzellikleri.Where(p => p.Id == Model.Id).FirstOrDefault();
                        pageId = sayfaGuncelle.Id;
                        sayfaGuncelle.SayfaOzellikGrupId = Model.SayfaOzellik.SayfaOzellikGrupId;
                        sayfaGuncelle.Sira = Model.SayfaOzellik.Sira;

                        var diller = _context.Diller.ToList();

                        foreach (var item in Model.SayfaOzellik.SayfaOzellikleriTranslate)
                        {
                            var sayfaGuncelleTranslate = _context.SayfaOzellikleriTranslate.Find(item.Id);

                            sayfaGuncelleTranslate.OzellikAdi = item.OzellikAdi;
                            sayfaGuncelleTranslate.DilId = item.DilId;
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }

                        var db = new AppDbContext();


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
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu : " + hata.Message;
                result.SayfaId = pageId;

            }

            return result;

        }

        public async Task<ResultViewModel> DeletePage(SayfaOzellikViewModel Model)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.SayfaOzellikleri.Find(Model.Id);

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
                            var model = _context.SayfaOzellikleri.Find(item);

                            _context.Entry(model).State = EntityState.Deleted;

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
