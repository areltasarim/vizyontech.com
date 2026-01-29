using EticaretWebCoreCaching.Abstraction;
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

    public partial class OneCikanUrunlerServis : IOneCikanUrunlerServis
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;

        private readonly string entity = "Modüller";

        public OneCikanUrunlerServis(AppDbContext _context, ICacheService cacheService)
        {
            this._context = _context;
            _cacheService = cacheService;
        }


        public async Task<ResultViewModel> UpdatePage(OneCikanUrunViewModel Model, string submit)
        {

            var result = new ResultViewModel();
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



                        #region Sayfa Güncelleme
                        var sayfaEkle = new OneCikanUrunler()
                        {
                            ModulId = Model.ModulId,
                            BannerDurumu = Model.OneCikanUrun.BannerDurumu,
                            BannerUrl = Model.OneCikanUrun.BannerUrl,
                            ColumnDesktop = Model.OneCikanUrun.ColumnDesktop,
                            ColumnMobil = Model.OneCikanUrun.ColumnMobil,
                            OneCikanUrunlerTranslate = new List<OneCikanUrunlerTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        #region Resim
                        if (Model.SayfaResmi != null)
                        {

                            var model = DosyaHelper.DosyaYukle(Model.SayfaResmi, "Banner", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);

                            if (model.Result.Basarilimi == true)
                            {
                                Model.Banner = model.Result.Sonuc;
                            }
                            else
                            {
                                result.Basarilimi = result.Basarilimi;
                                result.MesajDurumu = result.MesajDurumu;
                                result.Mesaj = result.Mesaj;

                                return result;
                            }
                        }
                        else
                        {
                            Model.Banner = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim);
                        }
                        #endregion


                        await _context.SaveChangesAsync();



                        var modulEkle = await ModullerServis.ModulEkleGuncelle(entityId: sayfaEkle.Id, durum: Model.Durum, sira: Model.Sira, modulTipi: Model.ModulTipi, eklemeMi: true);



                        var diller = _context.Diller.ToList();
                        foreach (var item in Model.OneCikanUrun.OneCikanUrunlerTranslate)
                        {
                            var sayfaEkleTranslate = new OneCikanUrunlerTranslate()
                            {
                                ModulAdi = item.ModulAdi,
                                DilId = item.DilId
                            };
                            sayfaEkle.OneCikanUrunlerTranslate.Add(sayfaEkleTranslate);

                            await ModullerServis.ModulEkleGuncelleTranslate(modulAdi: item.ModulAdi, modulId: (int)modulEkle.SayfaId, dilId: item.DilId, durum: Model.Durum, sira: Model.Sira, eklemeMi: true);
                        }

                        await _context.SaveChangesAsync();
                        #endregion

                        #region Autocomplete Ürünler
                        if (Model.SeciliUrunlerAutocomplete != null)
                        {
                            int sira = 1; 
                            foreach (var item in Model.SeciliUrunlerAutocomplete)
                            {
                                var sayfaToSayfaEkle = new OneCikanUrunToUrunler()
                                {
                                    OneCikanUrunId = sayfaEkle.Id,
                                    UrunId = item,
                                    Sira = sira
                                };
                                _context.Entry(sayfaToSayfaEkle).State = EntityState.Added;
                                sira++;
                            }
                        }
                        #endregion

                        #region Autocomplete Kategoriler
                        if (Model.SeciliKategorilerAutocomplete != null)
                        {
                            int sira = 1;
                            foreach (var item in Model.SeciliKategorilerAutocomplete)
                            {
                                var sayfaToSayfaEkle = new OneCikanUrunToKategoriler()
                                {
                                    OneCikanUrunId = sayfaEkle.Id,
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
                            result.Controller = "OneCikanUrunler";
                            result.Action = "AddOrUpdate";
                            result.SayfaId = modulEkle.SayfaId;
                            result.SayfaUrl = Model.ModulTipi.ToString();
                        }
                        #endregion

                        _cacheService.RemoveByPattern($"OneCikanUrunToKategori");
                        _cacheService.RemoveByPattern($"OneCikanUrunler");
                        _cacheService.RemoveByPattern($"ModullerListesi");
                        
                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";

                    }
                    else
                    {
                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.OneCikanUrunler.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.BannerDurumu = Model.OneCikanUrun.BannerDurumu;
                        sayfaGuncelle.BannerUrl = Model.OneCikanUrun.BannerUrl;
                        sayfaGuncelle.ColumnDesktop = Model.OneCikanUrun.ColumnDesktop;
                        sayfaGuncelle.ColumnMobil = Model.OneCikanUrun.ColumnMobil;


                        var modulEkle = await ModullerServis.ModulEkleGuncelle(entityId: sayfaGuncelle.Id, durum: Model.Durum, sira: Model.Sira, modulTipi: Model.ModulTipi, eklemeMi: false);


                        var diller = _context.Diller.ToList();

                        foreach (var item in Model.OneCikanUrun.OneCikanUrunlerTranslate)
                        {
                            var sayfaGuncelleTranslate = _context.OneCikanUrunlerTranslate.Find(item.Id);

                            sayfaGuncelleTranslate.ModulAdi = item.ModulAdi;
                            sayfaGuncelleTranslate.DilId = item.DilId;
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            await ModullerServis.ModulEkleGuncelleTranslate(modulAdi: item.ModulAdi, modulId: Model.ModulId, dilId: item.DilId, durum: Model.Durum, sira: Model.Sira, eklemeMi: false);

                        }

                        #region Resim
                        if (Model.SayfaResmi != null)
                        {

                            var model = DosyaHelper.DosyaYukle(Model.SayfaResmi, "Banner", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);

                            if (model.Result.Basarilimi == true)
                            {
                                sayfaGuncelle.Banner = model.Result.Sonuc;
                            }
                            else
                            {
                                result.Basarilimi = result.Basarilimi;
                                result.MesajDurumu = result.MesajDurumu;
                                result.Mesaj = result.Mesaj;

                                return result;
                            }
                        }

                        else
                        {
                            sayfaGuncelle.Banner = new AppDbContext().OneCikanUrunler.Find(Model.Id).Banner;
                        }
                        #endregion

                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        _context.SaveChanges();
                        #endregion


                        #region Autocomplete Ürünler
                        var db = new AppDbContext();

                        if (Model.SeciliUrunlerAutocomplete != null)
                        {
                            db.OneCikanUrunToUrunler.Where(p => p.OneCikanUrunId == sayfaGuncelle.Id).ToList().ForEach(p => db.OneCikanUrunToUrunler.Remove(p));
                            db.SaveChanges();

                            int sira = 1;
                            foreach (var item in Model.SeciliUrunlerAutocomplete)
                            {
                                var sayfaToSayfaEkle = new OneCikanUrunToUrunler()
                                {
                                    OneCikanUrunId = sayfaGuncelle.Id,
                                    UrunId = item,
                                    Sira = sira
                                };
                                _context.Entry(sayfaToSayfaEkle).State = EntityState.Added;
                                _context.SaveChanges();
                                sira++;
                            }
                        }
                        else
                        {
                            db.OneCikanUrunToUrunler.Where(p => p.OneCikanUrunId == sayfaGuncelle.Id).ToList().ForEach(p => db.OneCikanUrunToUrunler.Remove(p));
                            db.SaveChanges();
                        }

                        #endregion

                        #region Autocomplete Kategoriler
                        if (Model.SeciliKategorilerAutocomplete != null)
                        {
                            db.OneCikanUrunToKategoriler.Where(p => p.OneCikanUrunId == sayfaGuncelle.Id).ToList().ForEach(p => db.OneCikanUrunToKategoriler.Remove(p));
                            db.SaveChanges();

                            int sira = 1;
                            foreach (var item in Model.SeciliKategorilerAutocomplete)
                            {
                                var sayfaToSayfaEkle = new OneCikanUrunToKategoriler()
                                {
                                    OneCikanUrunId = sayfaGuncelle.Id,
                                    KategoriId = item,
                                    Sira = sira
                                };
                                _context.Entry(sayfaToSayfaEkle).State = EntityState.Added;
                                _context.SaveChanges();
                                sira++;

                            }
                        }
                        else
                        {
                            db.OneCikanUrunToKategoriler.Where(p => p.OneCikanUrunId == sayfaGuncelle.Id).ToList().ForEach(p => db.OneCikanUrunToKategoriler.Remove(p));
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
                            result.Controller = "OneCikanUrunler";
                            result.Action = "AddOrUpdate";
                            result.SayfaUrl = Model.ModulTipi.ToString();
                            result.SayfaId = sayfaGuncelle.Id;
                        }
                        #endregion

                        _cacheService.RemoveByPattern($"OneCikanUrunToKategori");
                        _cacheService.RemoveByPattern($"OneCikanUrunler");
                        _cacheService.RemoveByPattern($"ModullerListesi");

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

                    // OneCikanUrunToUrun tablosundan ilgili kayıtları sil
                    var onecikanUrunToUrun = _context.OneCikanUrunToUrunler
                        .Where(x => x.OneCikanUrunId == Model.Id)
                        .ToList();

                    _context.OneCikanUrunToUrunler.RemoveRange(onecikanUrunToUrun);
                    await _context.SaveChangesAsync();

                    // onecikanUrunResimleri tablosundan ilgili kayıtları sil
                    var onecikanUrunResimleri = _context.OneCikanUrunResimleri
                        .Where(x => x.OneCikanUrunId == Model.Id)
                        .ToList();

                    _context.OneCikanUrunResimleri.RemoveRange(onecikanUrunResimleri);
                    await _context.SaveChangesAsync();

                    // OneCikanUrun tablosundan ilgili kaydı sil
                    var onecikanUrun = _context.OneCikanUrunler
                        .FirstOrDefault(x => x.Id == Model.Id);

                    if (onecikanUrun != null)
                    {
                        _context.OneCikanUrunler.Remove(onecikanUrun);
                        await _context.SaveChangesAsync();
                    }

                    // Moduller tablosundan ilgili kaydı sil
                    var model = _context.Moduller
                        .FirstOrDefault(x => x.EntityId == Model.Id);

                    if (model != null)
                    {
                        _context.Moduller.Remove(model);
                        await _context.SaveChangesAsync();
                    }

                    _cacheService.RemoveByPattern($"OneCikanUrunToKategori");
                    _cacheService.RemoveByPattern($"OneCikanUrunler");
                    _cacheService.RemoveByPattern($"ModullerListesi");

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
        public async Task<ResultViewModel> ImageSortOrder(string sira)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    string sira_ = sira.Replace("sira[]=", "");
                    string[] p = sira_.Split('&');
                    int ss = 0;
                    foreach (string item in p)
                    {
                        var model = _context.OneCikanUrunResimleri.Find(Convert.ToInt32(item));
                        model.Sira = ss;
                        _context.Entry(model).State = EntityState.Modified;
                        ss++;
                    }
                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Resimler Sıralandı.";

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

        public async Task<ResultViewModel> ImageDelete(int id)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var model = _context.OneCikanUrunResimleri.ToList().Find(p => p.Id == id);

                    _context.Entry(model).State = EntityState.Deleted;

                    FileInfo file = new(@"wwwroot" + model.Resim);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Resim silindi.";

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
