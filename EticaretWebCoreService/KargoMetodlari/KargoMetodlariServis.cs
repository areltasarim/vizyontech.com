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

    public partial class KargoMetodlariServis : IKargoMetodlariServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Kargo Metodu";

        public KargoMetodlariServis(AppDbContext _context)
        {
            this._context = _context;
        }
        public async Task<List<EticaretWebCoreEntity.KargoMetodlari>> PageList()
        {
            return (await _context.KargoMetodlari.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(KargoMetoduViewModel Model, string submit)
        {

            var result = new ResultViewModel();
            int pageId = 0;
            try
            {

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        var sayfaEkle = new EticaretWebCoreEntity.KargoMetodlari()
                        {
                            Fiyat = Model.Fiyat,
                            Sira = Model.Sira,
                            Durum = Model.Durum,
                            KargoMetodlariTranslate = new List<KargoMetodlariTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;


                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new KargoMetodlariTranslate()
                            {
                                KargoAdi = Model.KargoAdiCeviri[i],
                                Aciklama = Model.AciklamaCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.KargoMetodlariTranslate.Add(sayfaEkleTranslate);

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

                        pageId = sayfaEkle.Id;
                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";
                    }

                    else
                    {
                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.KargoMetodlari.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.Fiyat = Model.Fiyat;
                        sayfaGuncelle.Sira = Model.Sira;
                        sayfaGuncelle.Durum = Model.Durum;


                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new KargoMetodlariTranslate()
                            {
                                KargoAdi = Model.KargoAdiCeviri[i],
                                Aciklama = Model.AciklamaCeviri[i],
                                DilId = diller[i].Id,
                                KargoMetodId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }
                        var db = new AppDbContext();
                        db.KargoMetodlari.Find(Model.Id).KargoMetodlariTranslate.ToList().ForEach(p => db.KargoMetodlariTranslate.Remove(p));
                        await db.SaveChangesAsync();


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
                result.SayfaId = pageId;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }



            return result;

        }

        public async Task<ResultViewModel> DeletePage(KargoMetoduViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.KargoMetodlari.ToList().Find(p => p.Id == Model.Id);
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
                            var model = _context.KargoMetodlari.ToList().Find(p => p.Id == item);

                            _context.Entry(model).State = EntityState.Deleted;
                        }
                        await _context.SaveChangesAsync();

                    }

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
