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

    public partial class ModullerServis : IModullerServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Modüller";

        public ModullerServis(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<Moduller>> PageList()
        {
            return (await _context.Moduller.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(ModulViewModel Model, string submit)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        var sayfaEkle = new Moduller()
                        {
                            ModulTipi = Model.ModulTipi,
                            Sira = Model.Sira,
                            Durum = Model.Durum,
                            ModullerTranslate = new List<ModullerTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        foreach (var item in Model.Modul.ModullerTranslate)
                        {
                            var sayfaEkleTranslate = new ModullerTranslate()
                            {
                                ModulAdi = item.ModulAdi,
                             
                                DilId = item.DilId
                            };
                            sayfaEkle.ModullerTranslate.Add(sayfaEkleTranslate);
                        }

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
                        var sayfaGuncelle = _context.Moduller.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.Sira = Model.Sira;
                            sayfaGuncelle.Durum = Model.Durum;
                        var diller = _context.Diller.ToList();

                        foreach (var item in Model.Modul.ModullerTranslate)
                        {
                            var sayfaGuncelleTranslate = _context.ModullerTranslate.Find(item.Id);

                            sayfaGuncelleTranslate.ModulAdi = item.ModulAdi;
                            sayfaGuncelleTranslate.DilId = item.DilId;
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }

                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
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

        public static Task<ResultViewModel> ModulEkleGuncelle(int entityId,SayfaDurumlari durum, int sira, ModulTipleri modulTipi, bool eklemeMi)
        {

            var _context = new AppDbContext();
            var result = new ResultViewModel();
            try
            {
                if (eklemeMi == true)
                {
                    var modulEkle = new Moduller()
                    {
                        EntityId = entityId,
                        ModulTipi = modulTipi,
                        Durum = durum,
                        Sira = sira,
                    };
                    _context.Entry(modulEkle).State = EntityState.Added;
                    _context.SaveChanges();
                    result.SayfaId = modulEkle.Id;
                }
                else
                {

                    var modulGuncelle = _context.Moduller.Where(x => x.EntityId == entityId).FirstOrDefault();
                    modulGuncelle.Durum = durum;
                    modulGuncelle.Sira = sira;
                    _context.Entry(modulGuncelle).State = EntityState.Modified;
                    _context.SaveChanges();
                    result.SayfaId = modulGuncelle.Id;
                }



                result.Basarilimi = true;
            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Modül eklerken hata oluştu.";
            }

            return Task.FromResult(result);
        }

        public static Task<ResultViewModel> ModulEkleGuncelleTranslate(string modulAdi, int modulId, int dilId, SayfaDurumlari durum, int sira, bool eklemeMi)
        {

            var _context = new AppDbContext();
            var result = new ResultViewModel();
            try
            {
                if(eklemeMi == true)
                {
                    var modulEkleTranslate = new ModullerTranslate()
                    {
                        ModulId = modulId,
                        ModulAdi = modulAdi,
                        DilId = dilId,
                    };
                    _context.Entry(modulEkleTranslate).State = EntityState.Added;
                    _context.SaveChanges();
                }
                else
                {
                  
                    var modulTranslateGuncelle = _context.ModullerTranslate.Where(x=> x.ModulId == modulId && x.DilId == dilId).FirstOrDefault();
                    modulTranslateGuncelle.ModulAdi = modulAdi;
                    _context.Entry(modulTranslateGuncelle).State = EntityState.Modified;
                    _context.SaveChanges();
                }
              
                result.Basarilimi = true;
            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Modül eklerken hata oluştu.";
            }

            return Task.FromResult(result);
        }

        public async Task<ResultViewModel> DeletePage(ModulViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.Moduller.Find(Model.Id);
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
                            var model = _context.Moduller.Find(item);
                            _context.Entry(_context.Moduller.Find(item)).State = EntityState.Deleted;
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
