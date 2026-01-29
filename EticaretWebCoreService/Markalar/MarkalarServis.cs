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

    public partial class MarkalarServis : IMarkalarServis
    {
        private readonly AppDbContext _context;
        private readonly LogsServis _logServis;
        private UnitOfWork _uow = null;
        private readonly string entity = "Marka";
        private readonly ICacheService cacheService;
        private readonly SeoServis _seoServis;

        //public MarkalarServis(AppDbContext _context, LogsServis _logServis)
        //{
        //    _uow = new UnitOfWork();
        //    this._context = _context;
        //    this._logServis = _logServis;
        //}
        public MarkalarServis(UnitOfWork uow, ICacheService cacheService, SeoServis _seoServis)
        {
            _uow = uow;
            this.cacheService = cacheService;
            this._seoServis = _seoServis;
        }

        public async Task<IEnumerable<Markalar>> PageList()
        {
            var model = await _uow.Repository<Markalar>().GetAll();


            return (model);
        }

        public async Task<ResultViewModel> UpdatePage(MarkaViewModel Model, string submit)
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
                        #region Sayfa Ekleme
                        await _uow.Repository<Markalar>().Add(Model);
                        #endregion

                        #region Seo Url
                        var seoUrl = await _seoServis.SeoLinkOlustur(sayfaAdi: Model.MarkaAdi, sayfaId: Model.Id, entityName: SeoUrlTipleri.Marka, seoTipi: SeoTipleri.Marka, null);
                        if (seoUrl.Basarilimi == false)
                        {
                            return seoUrl;
                        }
                        var menuEkle = await MenuHelper.MenuKaydet(Model.MenuYerleri, sayfaAdi: Model.MarkaAdi, sayfaId: Model.Id, parentSayfaId: 1, seoTipi: SeoUrlTipleri.Marka, MenuTipleri.Marka, dilId: 1);

                        #endregion

                        #region Resim
                        if (Model.SayfaResmi != null)
                        {

                            var model = DosyaHelper.DosyaYukle(Model.SayfaResmi, "Markalar", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);

                            if (model.Result.Basarilimi == true)
                            {
                                Model.Resim = model.Result.Sonuc;
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
                            Model.Resim = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim);
                        }
                        #endregion
                        await _uow.CompleteAsync();
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

                        //_logServis.Bilgi("Marka Eklendi", Model.Id.ToString());
                    }

                    else
                    {
                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _uow.Repository<Markalar>().GetById(Model.Id).Result;
                        sayfaGuncelle.MarkaAdi = Model.MarkaAdi;
                        sayfaGuncelle.Sira = Model.Sira;
                        sayfaGuncelle.Durum = Model.Durum;
                        #endregion

                        #region Seo Url


                        var seoUrl = await _seoServis.SeoLinkOlustur(sayfaAdi: Model.MarkaAdi, sayfaId: sayfaGuncelle.Id,entityName: SeoUrlTipleri.Marka, seoTipi: SeoTipleri.Marka, dilId: null);
                        if (seoUrl.Basarilimi == false)
                        {
                            return seoUrl;
                        }
                        var menuGuncelle = await MenuHelper.MenuKaydet(Model.MenuYerleri, sayfaAdi: Model.MarkaAdi, sayfaId: Model.Id, parentSayfaId: 1, seoTipi: SeoUrlTipleri.Marka, MenuTipleri.Marka, dilId: 1);

                        #endregion

                        #region Resim
                        if (Model.SayfaResmi != null)
                        {

                            var model = DosyaHelper.DosyaYukle(Model.SayfaResmi, "Markalar", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);

                            if (model.Result.Basarilimi == true)
                            {
                                sayfaGuncelle.Resim = model.Result.Sonuc;
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
                            sayfaGuncelle.Resim = new AppDbContext().Markalar.Find(Model.Id).Resim;
                        }
                        #endregion
                        _uow.Repository<Markalar>().Update(sayfaGuncelle);
                        await _uow.CompleteAsync();
                        //_context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        //await _context.SaveChangesAsync();

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

                        //_logServis.Bilgi("Marka Güncellendi", sayfaGuncelle.Id.ToString());

                    }
                    transaction.Complete();

                    //cacheService.RemoveByPattern("Marka");
                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";
                //await _logServis.Hata(hata);
            }

            return result;


        }

        public async Task<ResultViewModel> DeletePage(MarkaViewModel Model)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _uow.Repository<Markalar>().Delete(Model.Id);
                    //_context.Entry(model).State = EntityState.Deleted;

                    var modelresim = _uow.Repository<Markalar>().GetAll().Result.Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").ToList().Find(p => p.Id == Model.Id);

                    //var modelresim = _context.Markalar.ToList().Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").ToList().Find(p => p.Id == Model.Id);

                    FileInfo file = new(@"wwwroot" + modelresim?.Resim);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    //await _context.SaveChangesAsync();
                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} Başarıyla Silindi.";


                    var menuSil = await MenuHelper.MenuSil(sayfaId: Model.Id, seoTipi: SeoUrlTipleri.Marka);
                    if (menuSil.Basarilimi == false)
                    {
                        return menuSil;
                    }
                    _context.SaveChanges();


                    //_logServis.Bilgi($"{entity} Silindi", Model.Id.ToString());

                    await _uow.CompleteAsync();

                    transaction.Complete();

                    //cacheService.RemoveByPattern("Marka");

                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu : " + hata.Message;
                //await _logServis.Hata(hata);
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
                            _context.Entry(_context.Markalar.Find(item)).State = EntityState.Deleted;
                            var modelresim = _context.Markalar.ToList().Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").ToList().Find(p => p.Id == item);

                            FileInfo file = new(@"wwwroot" + modelresim?.Resim);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                        }
                        await _context.SaveChangesAsync();

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";
                    }

                    //_logServis.Bilgi($"Toplu {entity} Silindi", Convert.ToString(string.Join(",", Deletes)));

                    transaction.Complete();

                    //cacheService.RemoveByPattern("Marka");

                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";
                await _logServis.Hata(hata);
            }

            return result;
        }

    }
}
