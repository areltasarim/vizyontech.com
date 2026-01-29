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

    public partial class OdemeMetodlariServis : IOdemeMetodlariServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Ödeme Metodu";

        public OdemeMetodlariServis(AppDbContext _context)
        {
            this._context = _context;
        }
        public async Task<List<OdemeMetodlari>> PageList()
        {
            return (await _context.OdemeMetodlari.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(OdemeMetoduViewModel Model, string submit)
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
                        var sayfaEkle = new OdemeMetodlari()
                        {
                            SiparisDurumId = Model.SiparisDurumId,
                            Sira = Model.Sira,
                            Durum = Model.Durum,
                            OdemeMetodlariTranslate = new List<OdemeMetodlariTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;


                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new OdemeMetodlariTranslate()
                            {
                                OdemeAdi = Model.OdemeAdiCeviri[i],
                                Aciklama = Model.AciklamaCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.OdemeMetodlariTranslate.Add(sayfaEkleTranslate);

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
                        var sayfaGuncelle = _context.OdemeMetodlari.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.SiparisDurumId = Model.SiparisDurumId;
                        sayfaGuncelle.Sira = Model.Sira;
                        sayfaGuncelle.Durum = Model.Durum;


                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new OdemeMetodlariTranslate()
                            {
                                OdemeAdi = Model.OdemeAdiCeviri[i],
                                Aciklama = Model.AciklamaCeviri[i],
                                DilId = diller[i].Id,
                                OdemeMetodId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }
                        var db = new AppDbContext();
                        db.OdemeMetodlari.Find(Model.Id).OdemeMetodlariTranslate.ToList().ForEach(p => db.OdemeMetodlariTranslate.Remove(p));
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

        public async Task<ResultViewModel> DeletePage(OdemeMetoduViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.OdemeMetodlari.ToList().Find(p => p.Id == Model.Id);
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
                            var model = _context.OdemeMetodlari.ToList().Find(p => p.Id == item);

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
