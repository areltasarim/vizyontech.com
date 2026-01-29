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

    public partial class SayfaOzellikGruplariServis : ISayfaOzellikGruplariServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Sayfa Özellik Grubu";

        public SayfaOzellikGruplariServis(AppDbContext _context)
        {
            this._context = _context;
        }
        public async Task<List<SayfaOzellikGruplari>> PageList()
        {
            return (await _context.SayfaOzellikGruplari.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(SayfaOzellikGrupViewModel Model, string submit)
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
                        var sayfaEkle = new SayfaOzellikGruplari()
                        {
                            Sira = Model.SayfaOzellikGrup.Sira,
                            SayfaOzellikGruplariTranslate = new List<SayfaOzellikGruplariTranslate>(),
                        };

                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        foreach (var item in Model.SayfaOzellikGrup.SayfaOzellikGruplariTranslate)
                        {
                            var sayfaEkleTranslate = new SayfaOzellikGruplariTranslate()
                            {
                                GrupAdi = item.GrupAdi,
                                Aciklama = item.Aciklama,
                                DilId = item.DilId
                            };
                            sayfaEkle.SayfaOzellikGruplariTranslate.Add(sayfaEkleTranslate);
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
                        var sayfaGuncelle = _context.SayfaOzellikGruplari.Where(p => p.Id == Model.Id).FirstOrDefault();
                        pageId = sayfaGuncelle.Id;
                        sayfaGuncelle.Sira = Model.SayfaOzellikGrup.Sira;

                        var diller = _context.Diller.ToList();

                        foreach (var item in Model.SayfaOzellikGrup.SayfaOzellikGruplariTranslate)
                        {
                            var sayfaGuncelleTranslate = _context.SayfaOzellikGruplariTranslate.Find(item.Id);

                            sayfaGuncelleTranslate.GrupAdi = item.GrupAdi;
                            sayfaGuncelleTranslate.Aciklama = item.Aciklama;
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

        public async Task<ResultViewModel> DeletePage(SayfaOzellikGrupViewModel Model)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.SayfaOzellikGruplari.Find(Model.Id);
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
                            var model = _context.SayfaOzellikGruplari.Find(item);
                            _context.Entry(model).State = EntityState.Deleted;

                            await _context.SaveChangesAsync();

                            result.Basarilimi = true;
                            result.MesajDurumu = "success";
                            result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";
                        }
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
