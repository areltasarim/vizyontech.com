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

    public partial class AdresBilgileriTelefonlarServis : IAdresBilgileriTelefonlarServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Telefonlar";

        public AdresBilgileriTelefonlarServis(AppDbContext _context)
        {
            this._context = _context;
        }



        public async Task<List<AdresBilgileriTelefonlar>> PageList()
        {
            return (await _context.AdresBilgileriTelefonlar.ToListAsync());
        }


        public async Task<ResultViewModel> UpdatePage(AdresBilgileriTelefonlarViewModel Model, string submit)
        {

            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Model.Id == 0)
                    {

                        #region Sayfa Ekleme
                        var sayfaEkle = new AdresBilgileriTelefonlar()
                        {
                            AdresBilgiId = Model.AdresBilgiId,
                            Sira = Model.Sira,
                            AdresBilgileriTelefonlarTranslate = new List<AdresBilgileriTelefonlarTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new AdresBilgileriTelefonlarTranslate()
                            {
                                Telefon = Model.TelefonCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.AdresBilgileriTelefonlarTranslate.Add(sayfaEkleTranslate);

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
                        var sayfaGuncelle = _context.AdresBilgileriTelefonlar.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.AdresBilgiId = Model.AdresBilgiId;
                        sayfaGuncelle.Sira = Model.Sira;


                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new AdresBilgileriTelefonlarTranslate()
                            {
                                Telefon = Model.TelefonCeviri[i],
                                DilId = diller[i].Id,
                                AdresBilgileriTelefonId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }

                        var db = new AppDbContext();
                        db.AdresBilgileriTelefonlar.Find(Model.Id).AdresBilgileriTelefonlarTranslate.ToList().ForEach(p => db.AdresBilgileriTelefonlarTranslate.Remove(p));
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

        public async Task<ResultViewModel> DeletePage(AdresBilgileriTelefonlarViewModel Model)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var model = _context.AdresBilgileriTelefonlar.Find(Model.Id);
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

        public async Task<ResultViewModel> DeleteAllPage(AdresBilgileriTelefonlarViewModel Model, int[] Deletes)
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
                            _context.Entry(_context.AdresBilgileriTelefonlar.Find(item)).State = EntityState.Deleted;
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
