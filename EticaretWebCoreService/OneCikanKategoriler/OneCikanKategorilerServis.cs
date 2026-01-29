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

    public partial class OneCikanKategorilerServis : IOneCikanKategorilerServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Modüller";

        public OneCikanKategorilerServis(AppDbContext _context)
        {
            this._context = _context;
        }


        public async Task<ResultViewModel> UpdatePage(OneCikanKategoriViewModel Model, string submit)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Model.Id == 0)
                    {
                        #region Sayfa Güncelleme

                        var sayfaEkle = new OneCikanKategoriler()
                        {
                            ModulId = Model.ModulId,
                            OneCikanKategorilerTranslate = new List<OneCikanKategorilerTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();



                        var modulEkle = await ModullerServis.ModulEkleGuncelle(entityId: sayfaEkle.Id, durum: Model.Durum, sira: Model.Sira, modulTipi: ModulTipleri.OneCikanKategoriler, eklemeMi: true);



                        var diller = _context.Diller.ToList();
                        foreach (var item in Model.OneCikanKategori.OneCikanKategorilerTranslate)
                        {
                            var sayfaEkleTranslate = new OneCikanKategorilerTranslate()
                            {
                                ModulAdi = item.ModulAdi,
                                DilId = item.DilId
                            };
                            sayfaEkle.OneCikanKategorilerTranslate.Add(sayfaEkleTranslate);

                            await ModullerServis.ModulEkleGuncelleTranslate(modulAdi: item.ModulAdi, modulId: (int)modulEkle.SayfaId, dilId: item.DilId, durum: Model.Durum, sira: 0, eklemeMi: true);
                        }

                        await _context.SaveChangesAsync();
                        #endregion

                        #region Autocomplete
                        if (Model.SeciliKategorilerAutocomplete != null)
                        {
                            int sira = 1;
                            foreach (var item in Model.SeciliKategorilerAutocomplete)
                            {
                                var sayfaToSayfaEkle = new OneCikanKategoriToKategoriler()
                                {
                                    OneCikanKategoriId = sayfaEkle.Id,
                                    KategoriId = item,
                                    Sira = sira
                                };
                                _context.Entry(sayfaToSayfaEkle).State = EntityState.Added;
                                _context.SaveChanges();

                                sira++;
                            }
                        }

                        #endregion


                        #region Sayfa Butonlari
                        if (submit == "Kaydet")
                        {
                            result.Controller = "Moduller";
                            result.Action = "Index";
                        }
                        if (submit == "KaydetGuncelle")
                        {
                            result.Controller = "OneCikanKategoriler";
                            result.Action = "AddOrUpdate";
                            result.SayfaId = modulEkle.SayfaId;
                            result.SayfaUrl = Model.ModulId.ToString();
                        }
                        #endregion

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";

                    }
                    else
                    {
                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.OneCikanKategoriler.Where(p => p.Id == Model.Id).FirstOrDefault();

                        var modulEkle = await ModullerServis.ModulEkleGuncelle(entityId: sayfaGuncelle.Id, durum: Model.Durum, sira: Model.Sira, modulTipi: ModulTipleri.OneCikanKategoriler, eklemeMi: false);


                        var diller = _context.Diller.ToList();

                        foreach (var item in Model.OneCikanKategori.OneCikanKategorilerTranslate)
                        {
                            var sayfaGuncelleTranslate = _context.OneCikanKategorilerTranslate.Find(item.Id);

                            sayfaGuncelleTranslate.ModulAdi = item.ModulAdi;
                            sayfaGuncelleTranslate.DilId = item.DilId;
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            await ModullerServis.ModulEkleGuncelleTranslate(modulAdi: item.ModulAdi, modulId: Model.ModulId, dilId: item.DilId, durum: Model.Durum, sira: 0, eklemeMi: false);

                        }

                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        #endregion


                        #region Autocomplete
                        var db = new AppDbContext();

                        if (Model.SeciliKategorilerAutocomplete != null)
                        {
                            db.OneCikanKategoriToKategoriler.Where(p => p.OneCikanKategoriId == sayfaGuncelle.Id).ToList().ForEach(p => db.OneCikanKategoriToKategoriler.Remove(p));
                            db.SaveChanges();

                            int sira = 1; 
                            foreach (var item in Model.SeciliKategorilerAutocomplete)
                            {
                                var sayfaToSayfaEkle = new OneCikanKategoriToKategoriler()
                                {
                                    OneCikanKategoriId = sayfaGuncelle.Id,
                                    KategoriId = item,
                                    Sira = sira 
                                };
                                _context.Entry(sayfaToSayfaEkle).State = EntityState.Added;

                                sira++;
                            }
                        }
                        else
                        {
                            db.OneCikanKategoriToKategoriler.Where(p => p.OneCikanKategoriId == sayfaGuncelle.Id).ToList().ForEach(p => db.OneCikanKategoriToKategoriler.Remove(p));
                            db.SaveChanges();
                        }

                        #endregion

                        #region Sayfa Butonlari
                        if (submit == "Kaydet")
                        {
                            result.Controller = "Moduller";
                            result.Action = "Index";
                        }
                        if (submit == "KaydetGuncelle")
                        {
                            result.Controller = "OneCikanKategoriler";
                            result.Action = "AddOrUpdate";
                            result.SayfaUrl = Model.ModulId.ToString();
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

        public async Task<ResultViewModel> DeletePage(OneCikanUrunViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // OneCikanUrunToKategori tablosundan ilgili kayıtları sil
                    var onecikanUrunToKategori = _context.OneCikanUrunToKategoriler
                        .Where(x => x.OneCikanUrunId == Model.Id)
                        .ToList(); // Listeye dönüştür, çünkü IQueryable üzerinde işlem yapılamaz.

                    _context.OneCikanUrunToKategoriler.RemoveRange(onecikanUrunToKategori);
                    await _context.SaveChangesAsync();

                    var model = _context.Moduller
                        .FirstOrDefault(x => x.EntityId == Model.Id && x.ModulTipi == ModulTipleri.OneCikanKategoriler);

                    if (model != null)
                    {
                        _context.Moduller.Remove(model);
                        await _context.SaveChangesAsync();
                    }

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
    }
}
