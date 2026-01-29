using AutoMapper;
using EticaretWebCoreCaching.Abstraction;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class KuponServis : IKuponServis
    {
        private readonly AppDbContext _context;
        private UnitOfWork _uow = null;
        private readonly string entity = "Kupon";

        public KuponServis(AppDbContext _context, UnitOfWork _uow)
        {
            this._context = _context;
            this._uow = _uow;
        }

        public async Task<IEnumerable<Kuponlar>> PageList()
        {
            var model = await _uow.Repository<Kuponlar>().GetAll();


            return (model);
        }

        public async Task<ResultViewModel> UpdatePage(KuponViewModel Model, string submit)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        var sayfaEkle = new Kuponlar()
                        {
                            KuponAdi = Model.Kupon.KuponAdi,
                            Kod = Model.Kupon.Kod,
                            OranTipi = Model.Kupon.OranTipi,
                            Indirim = Model.Kupon.Indirim,
                            ToplamTutar = Model.Kupon.ToplamTutar,
                            BaslangicTarihi = Model.Kupon.BaslangicTarihi,
                            BitisTarihi = Model.Kupon.BitisTarihi,
                            Durum = Model.Kupon.Durum,
                            KuponToUrun = new List<KuponToUrun>()
                        };

                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        #endregion


                        #region Benzer Ürünler
                        if (Model.SeciliKuponAutocomplete != null)
                        {
                            foreach (var item in Model.SeciliKuponAutocomplete)
                            {
                                var kupontoUrunEkle = new KuponToUrun()
                                {
                                    KuponId = sayfaEkle.Id,
                                    UrunId = item,
                                };
                                _context.Entry(kupontoUrunEkle).State = EntityState.Added;
                                _context.SaveChanges();
                            }
                        }
                        #endregion


                        #region Sayfa Butonlari
                        if (submit == "Kaydet")
                        {
                            result.Action = "Index";
                        }
                        if (submit == "KaydetGuncelle")
                        {
                            result.Action = "AddOrUpdate";
                            result.SayfaId = Model.Id;
                        }
                        #endregion

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";

                    }

                    else
                    {
                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.Kuponlar.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.KuponAdi = Model.Kupon.KuponAdi;
                        sayfaGuncelle.Kod = Model.Kupon.Kod;
                        sayfaGuncelle.OranTipi = Model.Kupon.OranTipi;
                        sayfaGuncelle.Indirim = Model.Kupon.Indirim;
                        sayfaGuncelle.ToplamTutar = Model.Kupon.ToplamTutar;
                        sayfaGuncelle.BaslangicTarihi = Model.Kupon.BaslangicTarihi;
                        sayfaGuncelle.BitisTarihi = Model.Kupon.BitisTarihi;
                        sayfaGuncelle.Durum = Model.Kupon.Durum;
                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        _context.SaveChanges();
                        #endregion

                        var db = new AppDbContext();
                        #region Benzer Ürünler
                        if (Model.SeciliKuponAutocomplete != null)
                        {
                            db.KuponToUrun.Where(p => p.KuponId == sayfaGuncelle.Id).ToList().ForEach(p => db.KuponToUrun.Remove(p));
                            db.SaveChanges();

                            foreach (var item in Model.SeciliKuponAutocomplete)
                            {
                                var kupontoUrunEkle = new KuponToUrun()
                                {
                                    KuponId = sayfaGuncelle.Id,
                                    UrunId = item
                                };
                                _context.Entry(kupontoUrunEkle).State = EntityState.Added;
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            db.KuponToUrun.Where(p => p.KuponId == sayfaGuncelle.Id).ToList().ForEach(p => db.KuponToUrun.Remove(p));
                            db.SaveChanges();
                        }
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
                result.Mesaj = "Hata Oluştu.";
            }

            return result;


        }

        public async Task<ResultViewModel> DeletePage(KuponViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _uow.Repository<Kuponlar>().Delete(Model.Id);

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} Başarıyla Silindi.";


                    await _uow.CompleteAsync();

                    transaction.Complete();

                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu : " + hata.Message;
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
                            _context.Entry(_context.Kuponlar.Find(item)).State = EntityState.Deleted;
                        }
                        await _context.SaveChangesAsync();

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";
                    }

                    //_logServis.Bilgi($"Toplu {entity} Silindi", Convert.ToString(string.Join(",", Deletes)));

                    transaction.Complete();

                    //cacheService.RemoveByPattern("Kupon");

                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";
            }

            return result;
        }

    }
}
