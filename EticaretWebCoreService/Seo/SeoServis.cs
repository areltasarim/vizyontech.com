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

    public partial class SeoServis : ISeoServis
    {
        private readonly AppDbContext _context;
        private readonly UnitOfWork _uow;
        public SeoServis(AppDbContext _context, UnitOfWork _uow)
        {
            this._context = _context;
            this._uow = _uow;
        }
        public async Task<ResultViewModel> SeoLinkOlustur(string sayfaAdi, int sayfaId, SeoUrlTipleri entityName, SeoTipleri seoTipi, int? dilId)
        {

            var result = new ResultViewModel();
            try
            {
                if (sayfaId > 0)
                {


                    var seoQuery = _context.SeoUrl
                    .Where(p => p.EntityId == sayfaId && p.SeoTipi == seoTipi);

                    if (seoTipi == SeoTipleri.Marka)
                    {
                        var seoUrlKontrol = seoQuery.FirstOrDefault()?.EntityName;
                        _context.SeoUrl.Where(p => p.EntityId == sayfaId & p.EntityName == seoUrlKontrol).ToList().ForEach(p => _context.SeoUrl.Remove(p));
                    }
                    else
                    {
                        var seoUrlKontrol = seoQuery
                            .FirstOrDefault(p => p.DilId == Convert.ToInt32(dilId))?.EntityName;

                        _context.SeoUrl.Where(p => p.EntityId == sayfaId & p.EntityName == seoUrlKontrol).ToList().ForEach(p => _context.SeoUrl.Remove(p));
                    }
                    _context.SaveChanges();

                }


                string url = sayfaAdi;
                if (url == null)
                {
                    url = "#";
                }
                else
                {
                    url = Replace.UrlSeo(sayfaAdi);
                }


                foreach (var item in _context.SeoUrl.ToList())
                {

                    if (url == item.Url)
                    {
                        url = url + "-" + sayfaId;

                    }
                }

                var seoUrl = new SeoUrl()
                {
                    Url = url,
                    EntityName = entityName,
                    SeoTipi = seoTipi,
                    EntityId = sayfaId,
                    DilId = dilId,
                };

                _context.Entry(seoUrl).State = EntityState.Added;
                await _context.SaveChangesAsync();


                result.Basarilimi = true;
            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Seo link eklerken hata oluştu.";
                result.SayfaId = sayfaId;

            }

            return await Task.FromResult(result);
        }

        public async Task<ResultViewModel> SeoSil(int sayfaId, SeoUrlTipleri entityName)
        {

            var _context = new AppDbContext();
            var result = new ResultViewModel();

            try
            {
                var diller = _context.Diller.ToList();

                for (int i = 0; i < diller.Count; i++)
                {
                    var seoUrl = _context.SeoUrl.FirstOrDefault(p => p.EntityId == sayfaId & p.EntityName == entityName)?.Url;
                    _context.SeoUrl.Where(p => p.EntityId == sayfaId & p.Url == seoUrl).ToList().ForEach(p => _context.SeoUrl.Remove(p));

                }
                _context.SaveChanges();
                result.Basarilimi = true;

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Seo link silerken eklerken hata oluştu.";
                result.SayfaId = sayfaId;
            }
            return await Task.FromResult(result);

        }

    }
}
