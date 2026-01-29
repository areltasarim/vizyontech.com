using DocumentFormat.OpenXml.Wordprocessing;
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
    public partial class UrunSecenekleriServis : IUrunSecenekleriServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Ürün Seçenek";

        public UrunSecenekleriServis(AppDbContext _context)
        {
            this._context = _context;
        }
        public async Task<List<UrunSecenekleri>> PageList()
        {
            return (await _context.UrunSecenekleri.ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(UrunSecenekViewModel Model, string submit)
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
                    var db = new AppDbContext();

                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        var sayfaEkle = new UrunSecenekleri()
                        {
                            SecenekTipi = Model.UrunSecenek.SecenekTipi,
                            UrunSecenekleriTranslate = new List<UrunSecenekleriTranslate>(),
                        };

                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();



                        foreach (var item in Model.UrunSecenek.UrunSecenekleriTranslate)
                        {
                            var sayfaEkleTranslate = new UrunSecenekleriTranslate()
                            {
                                SecenekAdi = item.SecenekAdi,
                                DilId = item.DilId
                            };
                            sayfaEkle.UrunSecenekleriTranslate.Add(sayfaEkleTranslate);
                        }


                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        #endregion

                        #region Ürün Seçenek Değerlerini Ekleme



                        foreach (var item in Model.UrunSecenekDegerListesi)
                        {
                            var urunSecenekDeger = new UrunSecenekDegerleri()
                            {
                                UrunSecenekId = sayfaEkle.Id,
                                Sira = item.Sira
                            };
                            _context.Entry(urunSecenekDeger).State = EntityState.Added;
                            await _context.SaveChangesAsync();


                            var urunSecenekDegerTranslate = new UrunSecenekDegerleriTranslate()
                            {
                                UrunSecenekDegerId = urunSecenekDeger.Id,
                                DegerAdi = item.DegerAdi,
                                DilId = item.DilId
                            };

                            _context.Entry(urunSecenekDegerTranslate).State = EntityState.Added;
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
                        var sayfaGuncelle = _context.UrunSecenekleri.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.SecenekTipi = Model.UrunSecenek.SecenekTipi;

                        var diller = _context.Diller.ToList();

                        foreach (var item in Model.UrunSecenek.UrunSecenekleriTranslate)
                        {
                            var sayfaGuncelleTranslate = _context.UrunSecenekleriTranslate.Find(item.Id);
                            sayfaGuncelleTranslate.SecenekAdi = item.SecenekAdi;
                            sayfaGuncelleTranslate.DilId = item.DilId;
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        #endregion


                        #region Ürün Seçenek Değerleri Translate Kaydetme
                        //db.UrunSecenekDegerleri.Where(p => p.UrunSecenekId == sayfaGuncelle.Id).ToList().ForEach(p => db.UrunSecenekDegerleri.Remove(p));
                        //db.SaveChanges();

                        //foreach (var item in Model.UrunSecenekDegerListesi)
                        //{
                        //    var urunSecenekDeger = new UrunSecenekDegerleri()
                        //    {
                        //        UrunSecenekId = sayfaGuncelle.Id,
                        //        Sira = 1
                        //    };
                        //    _context.Entry(urunSecenekDeger).State = EntityState.Added;
                        //    await _context.SaveChangesAsync();


                        //    var urunSecenekDegerTranslate = new UrunSecenekDegerleriTranslate()
                        //    {
                        //        UrunSecenekDegerId = urunSecenekDeger.Id,
                        //        DegerAdi = item.DegerAdi,
                        //        DilId = item.DilId
                        //    };

                        //    _context.Entry(urunSecenekDegerTranslate).State = EntityState.Added;
                        //}
                        //await _context.SaveChangesAsync();

                        foreach (var item in Model.UrunSecenekDegerListesi)
                        {
                            if (item.SilinmeDurum == true)
                            {
                                var urunDegerSecenek = _context.UrunSecenekDegerleri.Find(item.UrunSecenekDegerId);
                                _context.Entry(urunDegerSecenek).State = EntityState.Deleted;
                                _context.SaveChanges();
                            }
                            else
                            {
                                var existingItem = await _context.UrunSecenekDegerleriTranslate
                                .FirstOrDefaultAsync(x => x.UrunSecenekDegerId == item.UrunSecenekDegerId && x.DilId == item.DilId);

                                if (existingItem != null)
                                {
                                    existingItem.DegerAdi = item.DegerAdi;
                                    existingItem.UrunSecenekDegerleri.Sira = item.Sira;
                                    _context.Entry(existingItem).State = EntityState.Modified;
                                }
                                else
                                {
                                    var urunSecenekDeger = new UrunSecenekDegerleri()
                                    {
                                        UrunSecenekId = sayfaGuncelle.Id,
                                        Sira = item.Sira,
                                    };
                                    _context.Entry(urunSecenekDeger).State = EntityState.Added;
                                    await _context.SaveChangesAsync();

                                    var urunSecenekDegerTranslate = new UrunSecenekDegerleriTranslate()
                                    {
                                        UrunSecenekDegerId = urunSecenekDeger.Id,
                                        DegerAdi = item.DegerAdi,
                                        DilId = item.DilId
                                    };

                                    _context.Entry(urunSecenekDegerTranslate).State = EntityState.Added;
                                }
                            }
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

        public async Task<ResultViewModel> DeletePage(UrunSecenekViewModel Model)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.UrunSecenekleri.Find(Model.Id);

                    _context.Entry(model).State = EntityState.Deleted;
                    _context.SaveChanges();

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
                            var model = _context.UrunSecenekleri.Find(item);
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
