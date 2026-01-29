using EticaretWebCoreCaching.Abstraction;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.WebPages.Html;
using X.PagedList;

namespace vizyontech.com.com.Controllers
{
    [AllowAnonymous]

    public class UrunlerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly HelperServis _helperServis;
        private readonly ICacheService _cacheService;

        public UrunlerController(AppDbContext _context, HelperServis _helperServis, ICacheService cacheService)
        {
            this._context = _context;
            this._helperServis = _helperServis;
            _cacheService = cacheService;
        }


        //Sınırsız Kategori Listeleme
        public IActionResult Kategoriler(string url)
        {
            var kategoriId = _context.SeoUrl.FirstOrDefault(p => p.Url == url)?.EntityId;

            var dil = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

            var model = _context.KategorilerTranslate.Where(p => p.Diller.DilKodlari.DilKodu == dil && p.Kategoriler.ParentKategoriId == kategoriId && p.Kategoriler.Durum == SayfaDurumlari.Aktif).Where(p => p.Id != 1).OrderBy(p=> p.Kategoriler.Sira).ToList().ToPagedList(1, 100);
           
            return View(model);
        }

        //Tüm Kategorileri Listeleme
        public IActionResult TumKategoriler()
        {
            return View();

        }
        //Tüm Kategorileri Listeleme

        //Tüm Ürünleri Listeleme
        public IActionResult TumUrunler()
        {
            return View();
        }
        //Tüm Ürünleri Listeleme

        //Kategori Idsine Göre Ürünleri Listeleme

        public class KategoriDTO
        {
            public int Id { get; set; }
            public string KategoriAdi { get; set; }
            public int Sira { get; set; }
            public string SeoUrl { get; set; }
            public List<KategoriDTO> AltKategoriler { get; set; }
        }

        public class UrunDTO
        {
            public int Id { get; set; }
            public string UrunAdi { get; set; }
            public string UrunKodu { get; set; }
            public decimal Fiyat { get; set; }
            public string MarkaAdi { get; set; }
            public string Ozellik { get; set; }
            public string KisaAciklama { get; set; }
            public string ResimUrl { get; set; } // varsa 1. resmi
            public int Sira { get; set; }
        }


        private KategoriDTO MapKategori(Kategoriler kategori, string aktifDil)
        {
            var seoUrl = _context.SeoUrl
                .FirstOrDefault(p => p.EntityId == kategori.Id && p.SeoTipi == SeoTipleri.Kategori)?.Url ?? "";

            return new KategoriDTO
            {
                Id = kategori.Id,
                KategoriAdi = kategori.KategorilerTranslate
                    ?.FirstOrDefault(t => t.Diller.DilKodlari.DilKodu == aktifDil)?.KategoriAdi ?? "",
                Sira = kategori.Sira ?? 0,
                SeoUrl = seoUrl,
                AltKategoriler = kategori.AltKategoriler?
                    .Where(k => k.Durum == SayfaDurumlari.Aktif)
                    .Select(k => MapKategori(k, aktifDil))
                    .ToList()
            };
        }
        private List<UrunDTO> MapUrunListesi(List<Urunler> urunler, string aktifDil)
        {
            return urunler.Select(x => new UrunDTO
            {
                Id = x.Id,
                UrunAdi = x.UrunlerTranslate
                    ?.FirstOrDefault(t => t.Diller.DilKodlari.DilKodu == aktifDil)?.UrunAdi ?? "",
                UrunKodu = x.UrunKodu,
                Ozellik = x.UrunlerTranslate
                    ?.FirstOrDefault(t => t.Diller.DilKodlari.DilKodu == aktifDil)?.Ozellik ?? "",
                Fiyat = x.ListeFiyat,
                KisaAciklama = x.UrunlerTranslate
                    ?.FirstOrDefault(t => t.Diller.DilKodlari.DilKodu == aktifDil)?.KisaAciklama ?? "",
                ResimUrl = x.UrunResimleri?.FirstOrDefault()?.Resim ?? "",
                MarkaAdi = x.Markalar?.MarkaAdi,
     
                Sira = x.Sira
            }).ToList();
        }




        public async Task<IActionResult> Index(string url, int sayfa = 1, string siralama = "son")
        {
            KategoriDTO model = null;
            IPagedList<UrunDTO> urunler = null;

            var siteAyariHelper = await _helperServis.GetSiteAyari();
            var dil = await _helperServis.GetAktifDil();
            string aktifDilKodu = dil.DilKodlari.DilKodu;

            try
            {
                string kategoriKey = $"KategoriDetay-{url}";
                model = await _cacheService.GetAsync<KategoriDTO>(kategoriKey);

                if (model == null)
                {
                    var kategoriId = _context.SeoUrl
                        .FirstOrDefault(p => p.Url == url && p.SeoTipi == SeoTipleri.Kategori)?.EntityId;

                    var kategori = _context.Kategoriler
                        .Include(k => k.KategorilerTranslate)
                        .Include(k => k.AltKategoriler)
                            .ThenInclude(k => k.KategorilerTranslate)
                        .Where(p => p.Id == kategoriId && p.Durum == SayfaDurumlari.Aktif)
                        .SingleOrDefault();

                    if (kategori == null)
                        return RedirectToAction("Error", "Home");

                    model = MapKategori(kategori, aktifDilKodu);
                    await _cacheService.SetAsync(kategoriKey, model, 360000);
                }

                string urunKey = siteAyariHelper.SiteAyarlari.SinirsiKategoriDurum == SayfaDurumlari.Pasif
                    ? $"KategoriUrunlerSinirsiz-{url}-Sayfa-{sayfa}-Siralama-{siralama}"
                    : $"KategoriUrunler-{url}-Sayfa-{sayfa}-Siralama-{siralama}";

                var cachedUrunList = await _cacheService.GetAsync<List<UrunDTO>>(urunKey);

                if (cachedUrunList == null)
                {
                    var kategoriEntity = await _context.Kategoriler
                        .Include(k => k.AltKategoriler)
                        .FirstOrDefaultAsync(k => k.Id == model.Id);

                    var urunListesi = Populate(kategoriEntity)
                        .Where(p => p.Durum == SayfaDurumlari.Aktif)
                        .ToList();

                    if (siteAyariHelper.SiteAyarlari.SinirsiKategoriDurum == SayfaDurumlari.Pasif)
                        urunListesi = urunListesi.DistinctBy(x => x.Id).ToList();

                    var urunDTOs = MapUrunListesi(urunListesi, aktifDilKodu);
                    
                    // Sıralama uygula
                    urunDTOs = siralama switch
                    {
                        "fiyat-artan" => urunDTOs.OrderBy(p => p.Fiyat).ToList(),
                        "fiyat-azalan" => urunDTOs.OrderByDescending(p => p.Fiyat).ToList(),
                        _ => urunDTOs.OrderByDescending(p => p.Id).ToList() // son eklenenler (ID'ye göre azalan)
                    };

                    await _cacheService.SetAsync(urunKey, urunDTOs, 360000);
                    cachedUrunList = urunDTOs;
                }

                urunler = cachedUrunList.ToPagedList(sayfa, siteAyariHelper.SiteAyarlari.SinirsiKategoriDurum == SayfaDurumlari.Pasif ? 20 : 21);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.Siralama = siralama;
            ViewData["Urunler"] = urunler;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreProducts(string url, int sayfa = 1, string siralama = "son")
        {
            try
            {
                var siteAyariHelper = await _helperServis.GetSiteAyari();
                var dil = await _helperServis.GetAktifDil();
                string aktifDilKodu = dil.DilKodlari.DilKodu;

                string kategoriKey = $"KategoriDetay-{url}";
                var model = await _cacheService.GetAsync<KategoriDTO>(kategoriKey);

                if (model == null)
                {
                    var kategoriId = _context.SeoUrl
                        .FirstOrDefault(p => p.Url == url && p.SeoTipi == SeoTipleri.Kategori)?.EntityId;

                    var kategori = _context.Kategoriler
                        .Include(k => k.KategorilerTranslate)
                        .Include(k => k.AltKategoriler)
                            .ThenInclude(k => k.KategorilerTranslate)
                        .Where(p => p.Id == kategoriId && p.Durum == SayfaDurumlari.Aktif)
                        .SingleOrDefault();

                    if (kategori == null)
                        return Json(new { success = false, message = "Kategori bulunamadı" });

                    model = MapKategori(kategori, aktifDilKodu);
                    await _cacheService.SetAsync(kategoriKey, model, 360000);
                }

                string urunKey = siteAyariHelper.SiteAyarlari.SinirsiKategoriDurum == SayfaDurumlari.Pasif
                    ? $"KategoriUrunlerSinirsiz-{url}-Sayfa-{sayfa}-Siralama-{siralama}"
                    : $"KategoriUrunler-{url}-Sayfa-{sayfa}-Siralama-{siralama}";

                var cachedUrunList = await _cacheService.GetAsync<List<UrunDTO>>(urunKey);

                if (cachedUrunList == null)
                {
                    var kategoriEntity = await _context.Kategoriler
                        .Include(k => k.AltKategoriler)
                        .FirstOrDefaultAsync(k => k.Id == model.Id);

                    var urunListesi = Populate(kategoriEntity)
                        .Where(p => p.Durum == SayfaDurumlari.Aktif)
                        .ToList();

                    if (siteAyariHelper.SiteAyarlari.SinirsiKategoriDurum == SayfaDurumlari.Pasif)
                        urunListesi = urunListesi.DistinctBy(x => x.Id).ToList();

                    var urunDTOs = MapUrunListesi(urunListesi, aktifDilKodu);
                    
                    // Sıralama uygula
                    urunDTOs = siralama switch
                    {
                        "fiyat-artan" => urunDTOs.OrderBy(p => p.Fiyat).ToList(),
                        "fiyat-azalan" => urunDTOs.OrderByDescending(p => p.Fiyat).ToList(),
                        _ => urunDTOs.OrderByDescending(p => p.Id).ToList() // son eklenenler (ID'ye göre azalan)
                    };

                    await _cacheService.SetAsync(urunKey, urunDTOs, 360000);
                    cachedUrunList = urunDTOs;
                }

                var urunler = cachedUrunList.ToPagedList(sayfa, siteAyariHelper.SiteAyarlari.SinirsiKategoriDurum == SayfaDurumlari.Pasif ? 20 : 21);

                var products = new List<object>();
                foreach (var u in urunler)
                {
                    var seoUrlResult = await _helperServis.GetSeoUrl(u.Id, SeoUrlTipleri.Urun);
                    var seoUrl = seoUrlResult?.Url ?? "";
                    var urunResimFirstResult = _helperServis.GetUrunResimFirst(u.Id, UrunResimKategorileri.UrunResim);
                    var urunResimFirst = urunResimFirstResult?.Resim ?? "";
                    var urunListeFiyatDoviz = await _helperServis.GetPriceAsync(u.Fiyat, FiyatTipleri.ListeFiyat, ParaBirimi.USD, dovizDurum: false);
                    var urunListeFiyatTl = await _helperServis.GetPriceAsync(u.Fiyat, FiyatTipleri.ListeFiyat, ParaBirimi.USD, dovizDurum: true);

                    products.Add(new
                    {
                        id = u.Id,
                        urunAdi = u.UrunAdi,
                        urunKodu = u.UrunKodu,
                        markaAdi = u.MarkaAdi,
                        fiyat = u.Fiyat,
                        fiyatFormatted = urunListeFiyatDoviz.Format(true),
                        fiyatTlFormatted = urunListeFiyatTl.Format(true),
                        ozellik = u.Ozellik,
                        resimUrl = urunResimFirst,
                        seoUrl = seoUrl
                    });
                }

                return Json(new
                {
                    success = true,
                    hasMore = urunler.HasNextPage,
                    products = products
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }




        public IList<Urunler> Populate(Kategoriler kategori)
        {
            var urunler = new List<Urunler>();
            var visitedKategoriIds = new HashSet<int>();

            var urunToKategoriList = _context.UrunToKategori
                .Include(x => x.Urunler)
                .ToList();

            PopulateSub(kategori, ref urunler, visitedKategoriIds, urunToKategoriList);

            return urunler;
        }

        private void PopulateSub(Kategoriler kategori, ref List<Urunler> urunler, HashSet<int> visited, List<UrunToKategori> urunToKategoriList)
        {
            if (kategori == null || visited.Contains(kategori.Id))
                return;

            visited.Add(kategori.Id);

            urunler.AddRange(
                urunToKategoriList
                    .Where(p => p.KategoriId == kategori.Id)
                    .Select(p => p.Urunler)
            );

            foreach (var alt in kategori.AltKategoriler)
            {
                PopulateSub(alt, ref urunler, visited, urunToKategoriList);
            }
        }



        public IActionResult Detay(int Id, string UrunKodu)
        {
            ViewData["UrunSecenekleri"] = _context.UrunSecenekleri
                .ToList()
                .AsQueryable()
                .Select(p => new SelectListItem()
                {
                    Text = p.UrunSecenekleriTranslate
                           .SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR")
                           .SecenekAdi,
                    Value = p.Id.ToString()
                });

            if (!string.IsNullOrEmpty(UrunKodu))
            {
                string duzeltilmisUrunKodu = UrunKodu.Replace("-", " ");
                var model = _context.Urunler.FirstOrDefault(x => x.UrunKodu == duzeltilmisUrunKodu);
                return View(model);
            }
            else
            {
                var model = _context.Urunler.Find(Id);
                return View(model);
            }
        }


        [AllowAnonymous]
        public async Task<PartialViewResult> _HizliGoruntule(int urunId)
        {

            var model = await _helperServis.GetUrun(urunId);

            return PartialView("~/Views/Urunler/_HizliGoruntule.cshtml", model);
        }
        [Route("aramasonucu")]
        public IActionResult Arama(string keyword)
        {

            if(string.IsNullOrEmpty(keyword))
            {
                return RedirectToAction("Index", "Home");
            }

            var dil = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            var tmpmodel = _context.Urunler.ToList();
            var model=tmpmodel.Where(ContainsKeyword(dil, keyword)).ToList();
            if (model.Count == 0)
            {
                ViewData["AramaSonucu"] = "Sonuç Bulunamadı...";
            }

            return View(model);
        }

        public static Func<Urunler, bool> ContainsKeyword(string dil, string keyword)
        {
            return x => x.UrunlerTranslate.Any(p => p.Diller != null &&
                                                      p.Diller.DilKodlari != null &&
                                                      p.Diller.DilKodlari.DilKodu == dil &&
                                                      ((p.UrunAdi != null && p.UrunAdi.ToLower().Contains(keyword.ToLower().Trim())) ||
                                                      (p.Urunler.UrunKodu != null && p.Urunler.UrunKodu.ToLower().Contains(keyword.ToLower().Trim())) ||
                                                       (p.KisaAciklama != null && HtmlToText(p.KisaAciklama).ToLower().Contains(keyword.ToLower().Trim()))));
        }

        public static string HtmlToText(string text)
        {
            string result = "";
            if (!string.IsNullOrEmpty(text))
            {
                text = Regex.Replace(text, @"&\w+;", match =>
                {
                    string entity = match.Value;
                    switch (entity)
                    {
                        case "&quot;": return "\"";
                        case "&amp;": return "&";
                        case "&lt;": return "<";
                        case "&gt;": return ">";
                        case "&nbsp;": return " ";
                        case "&iexcl;": return "¡";
                        case "&cent;": return "¢";
                        case "&pound;": return "£";
                        case "&curren;": return "¤";
                        case "&yen;": return "¥";
                        case "&brvbar;": return "¦";
                        case "&sect;": return "§";
                        case "&uml;": return "¨";
                        case "&copy;": return "©";
                        case "&ordf;": return "ª";
                        case "&laquo;": return "«";
                        case "&not;": return "¬";
                        case "&shy;": return "­";
                        case "&reg;": return "®";
                        case "&macr;": return "¯";
                        case "&deg;": return "°";
                        case "&plusmn;": return "±";
                        case "&sup2;": return "²";
                        case "&sup3;": return "³";
                        case "&acute;": return "´";
                        case "&micro;": return "µ";
                        case "&para;": return "¶";
                        case "&middot;": return "·";
                        case "&cedil;": return "¸";
                        case "&sup1;": return "¹";
                        case "&ordm;": return "º";
                        case "&raquo;": return "»";
                        case "&frac14;": return "¼";
                        case "&frac12;": return "½";
                        case "&frac34;": return "¾";
                        case "&iquest;": return "¿";
                        case "&Agrave;": return "À";
                        case "&Aacute;": return "Á";
                        case "&Acirc;": return "Â";
                        case "&Atilde;": return "Ã";
                        case "&Auml;": return "Ä";
                        case "&Aring;": return "Å";
                        case "&AElig;": return "Æ";
                        case "&Ccedil;": return "Ç";
                        case "&Egrave;": return "È";
                        case "&Eacute;": return "É";
                        case "&Ecirc;": return "Ê";
                        case "&Euml;": return "Ë";
                        case "&Igrave;": return "Ì";
                        case "&Iacute;": return "Í";
                        case "&Icirc;": return "Î";
                        case "&Iuml;": return "Ï";
                        case "&ETH;": return "Ð";
                        case "&Ntilde;": return "Ñ";
                        case "&Ograve;": return "Ò";
                        case "&Oacute;": return "Ó";
                        case "&Ocirc;": return "Ô";
                        case "&Otilde;": return "Õ";
                        case "&Ouml;": return "Ö";
                        case "&times;": return "×";
                        case "&Oslash;": return "Ø";
                        case "&Ugrave;": return "Ù";
                        case "&Uacute;": return "Ú";
                        case "&Ucirc;": return "Û";
                        case "&Uuml;": return "Ü";
                        case "&Yacute;": return "Ý";
                        case "&THORN;": return "Þ";
                        case "&szlig;": return "ß";
                        case "&agrave;": return "à";
                        case "&aacute;": return "á";
                        case "&acirc;": return "â";
                        case "&atilde;": return "ã";
                        case "&auml;": return "ä";
                        case "&aring;": return "å";
                        case "&aelig;": return "æ";
                        case "&ccedil;": return "ç";
                        case "&egrave;": return "è";
                        case "&eacute;": return "é";
                        case "&ecirc;": return "ê";
                        case "&euml;": return "ë";
                        case "&igrave;": return "ì";
                        case "&iacute;": return "í";
                        case "&icirc;": return "î";
                        case "&iuml;": return "ï";
                        case "&eth;": return "ð";
                        case "&ntilde;": return "ñ";
                        case "&ograve;": return "ò";
                        case "&oacute;": return "ó";
                        case "&ocirc;": return "ô";
                        case "&otilde;": return "õ";
                        case "&ouml;": return "ö";
                        case "&divide;": return "÷";
                        case "&oslash;": return "ø";
                        case "&ugrave;": return "ù";
                        case "&uacute;": return "ú";
                        case "&ucirc;": return "û";
                        case "&uuml;": return "ü";
                        case "&yacute;": return "ý";
                        case "&thorn;": return "þ";
                        case "&yuml;": return "ÿ";
                        // Diğer HTML encoding sembollerini buraya ekleyebilirsiniz
                        default: return entity;
                    }
                });
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(text);
                result=document.DocumentNode.InnerText;
            }
            return result;
        }
    }
}
