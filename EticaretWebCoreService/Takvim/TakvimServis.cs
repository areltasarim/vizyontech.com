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

    public partial class TakvimServis : ITakvimServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Takvim";

        public TakvimServis(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<Takvim>> PageList()
        {
            return (await _context.Takvim.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(TakvimViewModel Model, string submit)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        var sayfaEkle = new Takvim()
                        {
                            Renk = Model.Renk,
                            BaslangicTarihi = Model.BaslangicTarihi,
                            BitisTarihi = Model.BitisTarihi,
                            Durum = Model.Durum,
                            TakvimTranslate = new List<TakvimTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new TakvimTranslate()
                            {
                                Baslik = Model.BaslikCeviri[i],
                                Aciklama = Model.AciklamaCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.TakvimTranslate.Add(sayfaEkleTranslate);

                        }

                        _context.Entry(sayfaEkle).State = EntityState.Added;
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
                        var sayfaGuncelle = _context.Takvim.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.Renk = Model.Renk;
                        sayfaGuncelle.BaslangicTarihi = Model.BaslangicTarihi;
                        sayfaGuncelle.BitisTarihi = Model.BitisTarihi;
                        sayfaGuncelle.Durum = Model.Durum;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new TakvimTranslate()
                            {
                                Baslik = Model.BaslikCeviri[i],
                                Aciklama = Model.AciklamaCeviri[i],
                                DilId = diller[i].Id,
                                TakvimId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }

                        var db = new AppDbContext();
                        db.Takvim.Find(Model.Id).TakvimTranslate.ToList().ForEach(p => db.TakvimTranslate.Remove(p));
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
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }

   

            return result;
        }

        public async Task<ResultViewModel> DeletePage(TakvimViewModel Model)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.Takvim.Find(Model.Id);
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
                            var model = _context.Takvim.Find(item);

                            _context.Entry(_context.Takvim.Find(item)).State = EntityState.Deleted;
                            await _context.SaveChangesAsync();
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
